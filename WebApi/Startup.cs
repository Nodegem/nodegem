using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Converters;
using Nodester.Data.Contexts;
using Nodester.Data.Models;
using Nodester.Data.Settings;
using Nodester.Services;
using Nodester.Services.Hubs;

namespace Nodester.WebApi
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataProtection();

            services.AddHealthChecks()
                .AddNpgSql(Configuration.GetConnectionString("nodesterDb"), name: "NodesterDB");
            services.Configure<TokenSettings>(Configuration.GetSection("TokenSettings"));

            services.AddHttpContextAccessor();
            services.AddDistributedMemoryCache();

            services.AddEntityFrameworkNpgsql()
                .AddDbContext<NodesterDBContext>(options =>
                {
                    options.UseNpgsql(Configuration.GetConnectionString("nodesterDb"),
                        b => b.MigrationsAssembly("Nodester.WebApi"));
                });

            services.AddIdentity<ApplicationUser, Role>()
                .AddEntityFrameworkStores<NodesterDBContext>()
                .AddDefaultTokenProviders()
                .AddUserStore<UserStore<ApplicationUser, Role, NodesterDBContext, Guid>>()
                .AddRoleStore<RoleStore<Role, NodesterDBContext, Guid>>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;

                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidAudience = Configuration.GetValue<string>("TokenSettings:Audience"),
                        ValidIssuer = Configuration.GetValue<string>("TokenSettings:Issuer"),
                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(Configuration.GetValue<string>("TokenSettings:Key"))),
                        ClockSkew = TimeSpan.Zero
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            if (!string.IsNullOrEmpty(accessToken))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                })
                .AddGoogle(options =>
                {
                    options.SaveTokens = true;
                    options.ClientId = Configuration["Authentication:Google:ClientId"];
                    options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                    options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
                })
                .AddGitHub(options =>
                {
                    options.ClientId = Configuration["Authentication:GitHub:ClientId"];
                    options.ClientSecret = Configuration["Authentication:GitHub:ClientSecret"];
                });

            services.AddServices();

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
            });

            services.AddResponseCaching(options => { options.UseCaseSensitivePaths = true; });

            var domains = Configuration.GetSection("CorsSettings:AllowedHosts").Get<string>()
                .Replace(" ", "")
                .Split(',');

            services.AddCors(options =>
            {
                options.AddPolicy("AppCors", builder =>
                {
                    builder
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .WithOrigins(domains)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .SetPreflightMaxAge(TimeSpan.FromMinutes(60));
                });
            });

            services.AddSignalR(options =>
                {
                    options.EnableDetailedErrors = Environment.IsDevelopment() || Environment.IsStaging();
                    options.KeepAliveInterval = TimeSpan.FromSeconds(25);
                })
                .AddNewtonsoftJsonProtocol();

            services.AddRouting();

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            NodesterDBContext nodesterContext, IServiceProvider provider)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
                nodesterContext.Database.Migrate();
            }

            if (env.IsStaging() || env.IsProduction())
            {
                app.UseHttpsRedirection();
                app.UseHsts();
            }

            app.UseRouting();

            app.UseCors("AppCors");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseResponseCompression();
            app.UseResponseCaching();

            app.UseEndpoints(routes =>
            {
                routes.MapControllers();
                routes.MapHub<GraphHub>("/graphHub");
                routes.MapHub<TerminalHub>("/terminalHub");
                routes.MapHealthChecks("/health").RequireAuthorization();
            });

            NodeCache.CacheNodeData(provider);

            nodesterContext.Database.EnsureCreated();
        }
    }
}