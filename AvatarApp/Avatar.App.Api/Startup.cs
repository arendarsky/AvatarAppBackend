using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Avatar.App.Context;
using Avatar.App.Entities;
using Avatar.App.Entities.Settings;
using Avatar.App.Service.Security;
using Avatar.App.Service.Security.Impl;
using Avatar.App.Service.Services;
using Avatar.App.Service.Services.Impl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
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

            var signingSecurityKey = Configuration["SigningSecurityKey"];
            var signingKey = new SigningSymmetricKey(signingSecurityKey);
            services.AddSingleton<IJwtSigningEncodingKey>(signingKey);

            const string jwtSchemeName = "JwtBearer";
            var signingDecodingKey = (IJwtSigningDecodingKey) signingKey;
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
            var connection = Configuration["DB_CONNECTION"];
            services.AddDbContext<AvatarAppContext>(options =>
                options.UseSqlServer(connection, b => b.MigrationsAssembly("Avatar.App.Context")));
            services.Configure<EmailSettings>(Configuration.GetSection("Email.Settings"));
            services.Configure<EnvironmentConfig>(Configuration);
            var credentials = new StorageCredentials(Configuration.GetSection("AzureBlob.Settings")["AccountName"],
                Configuration.GetSection("AzureBlob.Settings")["AccountKey"]);
            services.AddScoped<CloudStorageAccount>(s => new CloudStorageAccount(credentials, true));
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IStorageService, LocalStorageService>();
            services.AddScoped<IVideoService, VideoService>();
            services.AddScoped<IMessageService, MessageService>();

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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime applicationLifetime, ILoggerFactory loggerFactory)
        {

            var log4NetProviderOptions = new Log4NetProviderOptions("log4net.config");

            loggerFactory.AddLog4Net(log4NetProviderOptions);
            Logger.RegisterLogger(loggerFactory.CreateLogger("LOGGER"));

            applicationLifetime.ApplicationStarted.Register(
                () =>
                {
                    Logger.Log.LogInformation("������ �������");
                    Logger.Log.LogInformation($"��������� {env.EnvironmentName}");
                });

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Avatar App V1");
                c.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
