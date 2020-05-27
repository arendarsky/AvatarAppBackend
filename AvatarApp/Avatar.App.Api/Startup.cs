using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Avatar.App.Core.Entities;
using Avatar.App.Infrastructure;
using Avatar.App.SharedKernel;
using Avatar.App.SharedKernel.Settings;
using Avatar.App.Core.Security;
using Avatar.App.Core.Security.Impl;
using Avatar.App.Core.Services;
using Avatar.App.Core.Services.Impl;
using Avatar.App.Infrastructure.FileStorage.Interfaces;
using Avatar.App.Infrastructure.FileStorage.Services;
using Avatar.App.Infrastructure.Repositories;
using Avatar.App.SharedKernel.Interfaces;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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

            AddJwtAuthentication(services);

            AddDbConnection(services);

            AddSwagger(services);

            AddSettings(services);

            AddRepositories(services);

            AddServices(services);

            AddCorsPolicy(services);

            AddFireBaseMessaging(services);


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime applicationLifetime, ILoggerFactory loggerFactory)
        {

            RegisterLogger(env, loggerFactory, applicationLifetime);

            UseHttpContext(app);

            EnableSwagger(app);
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(MyAllowSpecificOrigins);

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

        #region Private ConfigureServices Methods

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

        private void AddDbConnection(IServiceCollection services)
        {
            var connection = Configuration["DB_CONNECTION"];
            services.AddDbContextPool<AvatarAppContext>(options =>
                options.UseSqlServer(connection, b => b.MigrationsAssembly("Avatar.App.Infrastructure")));
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
            services.Configure<AvatarAppSettings>(Configuration.GetSection("Avatar.App.Settings"));
            services.Configure<EnvironmentConfig>(Configuration);
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<SmtpClient>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IStorageService, LocalStorageService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IVideoService, VideoService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IRepository<Video>, VideoRepository>();
            services.AddScoped<IRepository<User>, UserRepository>();
            services.AddScoped<IRepository<WatchedVideo>, WatchedVideoRepository>();
            services.AddScoped<IRepository<LikedVideo>, LikedVideoRepository>();
            services.AddScoped<IRepository<Semifinalist>, SemifinalistRepository>();
        }

        private void AddCorsPolicy(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins(Configuration["WebSiteUrl"], "http://web.xce-factor.ru", "http://192.168.1.2:8080").AllowAnyHeader().AllowAnyMethod();
                    });
            });
        }

        private void AddFireBaseMessaging(IServiceCollection services)
        {
            services.AddSingleton(service =>
            {
                var path = Configuration.GetSection("FireBase.Settings")["PathToCredentials"];
                FirebaseApp app;
                try
                {
                    app = FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.FromFile(path)
                    }, "FBApp");
                }
                catch (Exception)
                {
                    app = FirebaseApp.GetInstance("FBApp");
                }

                var messagingInstance = FirebaseMessaging.GetMessaging(app);
                return messagingInstance;
            });
        }

        #endregion

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
