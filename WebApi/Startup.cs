using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NLog.Extensions.Logging;
using NLog.Web;
using Nodester.Data.Contexts;
using Nodester.Data.Models;
using Nodester.Data.Settings;
using Nodester.Hubs;
using Nodester.Services;
using Swashbuckle.AspNetCore.Swagger;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Nodester.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private IHostingEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<TokenSettings>(Configuration.GetSection("TokenSettings"));

            services.AddMemoryCache();

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
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
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
                });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            services.AddServices(Configuration);

            services.AddCors();
            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = Environment.IsDevelopment() || Environment.IsStaging();
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {Title = "Nodester API", Version = "v1"});

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "Header",
                    Type = "Token"
                });
                c.AddSecurityRequirement(security);
            });

            services.AddLogging(logging =>
            {
                logging.AddConsole();
                logging.AddNLog();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            NodesterDBContext nodesterContext, IServiceProvider provider)
        {
            
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            
            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            if (env.IsStaging() || env.IsProduction())
            {
                app.UseHttpsRedirection();
                app.UseHsts();                
            }

            var domains = Configuration.GetSection("CorsSettings:AllowedHosts").Get<string>().Split(',');
            app.UseCors(
                builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins(domains).AllowCredentials());                

            env.ConfigureNLog("nlog.config");
            app.UseAuthentication();

            app.UseWebSockets();
            app.UseSignalR(routes =>
            {
                routes.MapHub<GraphHub>("/graphHub");
                routes.MapHub<TerminalHub>("/terminalHub");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nodester API V1"); });

            app.UseMvc();

            NodeCache.CacheNodeData(provider, Configuration.GetValue<string>("ProjectName"));

            nodesterContext.Database.EnsureCreated();
        }
    }
}