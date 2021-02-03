using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Avatar.App.Api.Extensions;
using Avatar.App.Authentication.Extensions;
using Avatar.App.Casting;
using Avatar.App.Casting.Extensions;
using Avatar.App.SharedKernel;
using Avatar.App.SharedKernel.Settings;
using Avatar.App.Core.Security;
using Avatar.App.Core.Security.Impl;
using Avatar.App.Core.Services;
using Avatar.App.Core.Services.Impl;
using Avatar.App.Final.Extensions;
using Avatar.App.Infrastructure.Extensions;
using Avatar.App.Infrastructure.FileStorage.Interfaces;
using Avatar.App.Infrastructure.FileStorage.Services;
using Avatar.App.Infrastructure.Models.Casting;
using Avatar.App.Infrastructure.Models.Semifinal;
using Avatar.App.Infrastructure.Repositories;
using Avatar.App.Schedulers;
using Avatar.App.Schedulers.Extensions;
using Avatar.App.Semifinal.Extensions;
using Avatar.App.SharedKernel.Interfaces;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Avatar.App.Api
{
    public class Startup
    {
        private const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMemoryCache();

            services.AddMvcCore().AddRazorViewEngine();

            services.AddAvatarAppHttpContextAccessor();

            AddJwtAuthentication(services);

            AddSwagger(services);

            AddSettings(services);

            AddRepositories(services);

            AddServices(services);

            AddSemifinalServices(services);

            AddFireBaseMessaging(services);

            services.AddCronSchedulers(ServiceLifetime.Scoped, typeof(ICronInvocable).Assembly, typeof(ICastingComponent).Assembly);
            services.AddSemifinalComponent();
            services.AddFinalComponent(Configuration);
            services.AddInfrastructure(Configuration);
            services.AddAuthenticationComponent();
            services.AddCastingComponent();
        }

        private void AddJwtAuthentication(IServiceCollection services)
        {
            var signingSecurityKey = Configuration["SigningSecurityKey"];
            var signingKey = new SigningSymmetricKey(signingSecurityKey);
            services.AddSingleton<IJwtSigningEncodingKey>(signingKey);

            const string jwtSchemeName = "JwtBearer";
            var signingDecodingKey = (IJwtSigningDecodingKey)signingKey;
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = jwtSchemeName;
                options.DefaultChallengeScheme = jwtSchemeName;
            }).AddJwtBearer(jwtSchemeName, jwtBearerOptions =>
            {
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingDecodingKey.GetKey(),
                    ValidateIssuer = true,
                    ValidIssuer = "AvatarApp",
                    ValidateAudience = true,
                    ValidAudience = "AvatarAppClient",
                    ValidateLifetime = false
                };
            });
        }

        private static void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "AvatarApp",
                    Version = "v1",
                    Description = "ASP.NET Core Web API"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        private void AddSettings(IServiceCollection services)
        {
            services.Configure<EmailSettings>(Configuration.GetSection("Email.Settings"));
            services.Configure<GeneralEmailSettings>(Configuration.GetSection("GeneralEmail.Settings"));
            services.Configure<AvatarAppSettings>(Configuration.GetSection("Avatar.App.Settings"));
            services.Configure<EnvironmentConfig>(Configuration);
        }
        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IRepository<VideoDb>, VideoRepository>();
            services.AddScoped<IRepository<UserDb>, UserRepository>();
            services.AddScoped<IRepository<WatchedVideoDb>, WatchedVideoRepository>();
            services.AddScoped<IRepository<LikedVideoDb>, LikedVideoRepository>();
            services.AddScoped<IRepository<SemifinalistDb>, SemifinalistRepository>();
            services.AddScoped<IBattleRepository, BattleRepository>();
            services.AddScoped<IRepository<BattleSemifinalistDb>, BattleSemifinalistRepository>();
            services.AddScoped<IRepository<BattleVoteDb>, BattleVoteRepository>();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<SmtpClient>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IStorageService, LocalStorageService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IVideoService, VideoService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAnalyticsService, AnalyticsService>();
        }

        private static void AddSemifinalServices(IServiceCollection services)
        {
            services.AddScoped<ISemifinalistService, SemifinalistService>();
            services.AddScoped<IBattleService, BattleService>();
            services.AddScoped<IBattleVoteService, BattleVoteService>();
        }

        private void AddCorsPolicy(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins("http://web.xce-factor.ru", "http://192.168.1.2:8080").AllowAnyHeader().AllowAnyMethod();
                    });
            });
        }

        private void AddFireBaseMessaging(IServiceCollection services)
        {
            services.AddSingleton(service =>
            {
                var path = Configuration.GetSection("Firebase.Settings")["PathToCredentials"];
                FirebaseApp app;
                try
                {
                    app = FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.FromFile(path)
                    }, "FBApp");
                }
                catch (Exception ex)
                {
                    Logger.Log.LogWarning("Firebase messaging has already started or not working" + ex);
                    app = FirebaseApp.GetInstance("FBApp");
                }

                var messagingInstance = FirebaseMessaging.GetMessaging(app);
                return messagingInstance;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime applicationLifetime, ILoggerFactory loggerFactory)
        {

            RegisterLogger(env, loggerFactory, applicationLifetime);

            UseHttpContext(app);

            EnableSwagger(app);

            app.UseCronSchedulers(Configuration, typeof(ICronInvocable).Assembly);
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(MyAllowSpecificOrigins);

            app.UseHttpContext();

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        #region Private Configure Methods

        private static void RegisterLogger(IHostEnvironment env, ILoggerFactory loggerFactory, IHostApplicationLifetime applicationLifetime)
        {
            var log4NetProviderOptions = new Log4NetProviderOptions("log4net.config");

            loggerFactory.AddLog4Net(log4NetProviderOptions);
            Logger.RegisterLogger(loggerFactory.CreateLogger("LOGGER"));

            applicationLifetime.ApplicationStarted.Register(
                () =>
                {
                    Logger.Log.LogInformation("Service started");
                    Logger.Log.LogInformation($"Settings {env.EnvironmentName}");
                });
        }
        
        private static void EnableSwagger(IApplicationBuilder app)
        {
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Avatar App V1");
                c.RoutePrefix = "swagger/avatar";
            });
        }

        private static void UseHttpContext(IApplicationBuilder app)
        {
            MyHttpContext.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
        }

        #endregion
    }
}
