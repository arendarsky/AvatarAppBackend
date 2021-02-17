using System;
using Avatar.App.Communications.Settings;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using MailKit.Net.Smtp;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Avatar.App.Communications.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommunicationsComponent(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(typeof(AvatarAppHttpContext).Assembly);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.Configure<EmailSettings>(configuration.GetSection("Email.Settings"));
            services.AddScoped<ICommunicationsComponent, CommunicationsComponent>();
            services.AddScoped<SmtpClient>();
            services.AddFireBaseMessaging(configuration);
        }

        public static void UseCommunicationsComponent(this IApplicationBuilder app)
        {
            AvatarAppHttpContext.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
        }

        private static void AddFireBaseMessaging(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(service =>
            {
                var path = configuration.GetSection("Firebase.Settings")["PathToCredentials"];
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
    }
}
