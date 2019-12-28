using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Converters;
using Nodegem.Data.Contexts;
using Nodegem.Data.Models;
using Nodegem.Data.Settings;
using Nodegem.Services;
using Nodegem.Services.Hubs;
using Nodegem.WebApi.Extensions;

namespace Nodegem.WebApi
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment { get; }
        private bool HostFrontEnd => Configuration.GetValue("SelfHosted", false);
        private bool UseReverseProxy => Configuration.GetValue("ReverseProxy", false);

        private bool UsingPostgres => !string.IsNullOrEmpty(Configuration.GetConnectionString("nodegemDb")) &&
                                      !string.IsNullOrEmpty(Configuration.GetConnectionString("keysDb"));

        private string NodegemConnectionString => UsingPostgres
            ? Configuration.GetConnectionString("nodegemDb")
            : $"Data Source={Environment.ContentRootPath}/NodegemDatabase.db";

        private string KeysConnectionString => UsingPostgres
            ? Configuration.GetConnectionString("keysDb")
            : $"Data Source={Environment.ContentRootPath}/KeysDatabase.db";

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<TokenSettings>(Configuration.GetSection("TokenSettings"));
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<AppSettings>(a => { a.HostFrontEnd = HostFrontEnd; });

            services.AddHttpContextAccessor();
            services.AddDistributedMemoryCache();

            if (UsingPostgres)
            {
                Console.WriteLine("Using Postgres database...");
                services.AddEntityFrameworkNpgsql()
                    .AddDbContext<NodegemContext>(options =>
                    {
                        options.UseNpgsql(NodegemConnectionString,
                            b => b.MigrationsAssembly("Nodegem.WebApi"));
                    })
                    .AddDbContext<KeysContext>(options =>
                    {
                        options.UseNpgsql(KeysConnectionString,
                            b => b.MigrationsAssembly("Nodegem.WebApi"));
                    });
            }
            else
            {
                Console.WriteLine("Using Sqlite database...");
                services.AddEntityFrameworkSqlite()
                    .AddDbContext<NodegemContext>(options =>
                    {
                        options.UseSqlite(NodegemConnectionString,
                            b => b.MigrationsAssembly("Nodegem.WebApi"));
                    })
                    .AddDbContext<KeysContext>(options =>
                    {
                        options.UseSqlite(KeysConnectionString,
                            b => b.MigrationsAssembly("Nodegem.WebApi"));
                    });
            }

            services.AddDataProtection()
                .SetApplicationName(Configuration["AppSettings:AppName"])
                .PersistKeysToDbContext<KeysContext>();

            if (UsingPostgres)
            {
                services.AddHealthChecks()
                    .AddSqlite($"Data Source={Environment.ContentRootPath}/NodegemDatabase.db", name: "NodegemDb")
                    .AddSqlite($"Data Source={Environment.ContentRootPath}/KeysDatabase.db", name: "KeysDb");
            }
            else
            {
                services.AddHealthChecks()
                    .AddNpgSql(Configuration.GetConnectionString("nodegemDb"), name: "NodegemDb")
                    .AddNpgSql(Configuration.GetConnectionString("keysDb"), name: "KeysDb");
            }

            services.AddIdentity<ApplicationUser, Role>()
                .AddEntityFrameworkStores<NodegemContext>()
                .AddDefaultTokenProviders()
                .AddUserStore<UserStore<ApplicationUser, Role, NodegemContext, Guid>>()
                .AddRoleStore<RoleStore<Role, NodegemContext, Guid>>();

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

            var authentication = services.AddAuthentication(options =>
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
                });

            var googleAuth = Configuration.GetSection("Authentication:Google")?.GetChildren();
            if (googleAuth != null && googleAuth.Any())
            {
                authentication.AddGoogle(options =>
                {
                    options.ClientId = Configuration["Authentication:Google:ClientId"];
                    options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                    options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
                });
            }
            else
            {
                Console.WriteLine("No Google authorization configuration was setup.");
            }

            var githubAuth = Configuration.GetSection("Authentication:GitHub")?.GetChildren();
            if (githubAuth != null && githubAuth.Any())
            {
                authentication.AddGitHub(options =>
                {
                    options.ClientId = Configuration["Authentication:GitHub:ClientId"];
                    options.ClientSecret = Configuration["Authentication:GitHub:ClientSecret"];
                });
            }
            else
            {
                Console.WriteLine("No GitHub authorization configuration was setup.");
            }

            var mailConfig = Configuration.GetSection("MailConfiguration").Get<MailConfigurationSettings>();
            if (mailConfig != null)
            {
                services.Configure<AppSettings>(settings => settings.IsUsingEmail = true);
                services.AddEmailService(mailConfig, "EmailTemplates");
            }
            else
            {
                Console.WriteLine("No mail configuration was setup.");
            }

            services.AddServices();

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes;
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
                        .WithOrigins(domains)
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .SetPreflightMaxAge(TimeSpan.FromMinutes(60));
                });
            });

            services.AddSignalR(options =>
                {
                    options.EnableDetailedErrors = Environment.IsDevelopment() || Environment.IsStaging();
                })
                .AddNewtonsoftJsonProtocol();

            services.AddRouting();

            if (HostFrontEnd)
            {
                services.AddControllersWithViews();
                services.AddSpaStaticFiles(configuration => { configuration.RootPath = "wwwroot"; });
            }

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
            }

            if (env.IsStaging() || env.IsProduction())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            if (HostFrontEnd)
            {
                app.UseStaticFiles();
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseCors("AppCors");

            if (UseReverseProxy)
            {
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseResponseCompression();
            app.UseResponseCaching();

            app.UseEndpoints(routes =>
            {
                routes.MapControllers();
                routes.MapHub<GraphHub>("/graphHub");
                routes.MapHub<TerminalHub>("/terminalHub");
                routes.MapHealthChecks("/health");

                if (HostFrontEnd)
                {
                    routes.MapControllerRoute(
                        name: "default",
                        pattern: "{controller}/{action=Index}/{id?}");
                }
            });

            if (HostFrontEnd)
            {
                app.UseSpa(spa => { spa.Options.SourcePath = "wwwroot"; });
            }

            NodeCache.CacheNodeData(provider);
        }
    }
}