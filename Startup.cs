using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using IotTest.MessageHandlers;
using WebSocketManager;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using IotTest.Services;

namespace IotTest
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .AddJsonOptions(options =>
                    {
                        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    });

            services.AddWebSocketManager();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Version = "v1",
                        Title = "IotTest API",
                        Description = "A sample API for testing Swashbuckle",
                        TermsOfService = "Some terms ..."
                    }
                );

                c.DescribeAllEnumsAsStrings();
            });

            services.AddSingleton<IRfidReader, RfidReader>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseWebSockets();

            app.UseSwagger(); 
            
            app.UseSwaggerUI(c =>
            {
                //c.RoutePrefix = ""; // serve the UI at root
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
            });

            app.UseMvcWithDefaultRoute();

            app.MapWebSocketManager("/notifications", serviceProvider.GetService<NotificationsMessageHandler>());

            var rfidReader = serviceProvider.GetService<IRfidReader>();
            var notificationsMessageHandler = serviceProvider.GetService<NotificationsMessageHandler>();
            rfidReader.WhenTagRead.Subscribe(e => {
                var (a, b, c, d) = e.Tag;
                var tag = $"{a},{b},{c},{d}";
                notificationsMessageHandler.InvokeClientMethodToAllAsync("rfidRead", tag);
            });
            rfidReader.Start();
        }
    }
}
