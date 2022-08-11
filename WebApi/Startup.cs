﻿using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Reutberg;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.Swagger;
using TradingPlaces.Core;
using TradingPlaces.WebApi.Filters;
using TradingPlaces.WebApi.Helpers;
using TradingPlaces.WebApi.Services;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace TradingPlaces.WebApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSwaggerGen(options => {
                    options.SwaggerDoc("v1", new Info() { Title = "Homework.WebApi", Version = "v1" });
                    options.DescribeAllEnumsAsStrings();
                })
                .AddMvc(options => {
                    options.Filters.Add(new ExceptionFilter());
                    options.Filters.Add(new ProducesAttribute("application/json"));
                })
                .AddJsonOptions(options => {
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });
                        
            services.AddHostedService<StrategyManagementService>();
            services.AddSingleton<IHostedServiceAccessor<IStrategyManagementService>, HostedServiceAccessor<IStrategyManagementService>>();
            services.AddSingleton<IRepository, InMemoryRepository>();
            services.AddSingleton<IStrategyEvaluator, StrategyEvaluator>();
            services.AddSingleton<IReutbergService, ReutbergService>();            
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var logEventLevel = LogEventLevel.Debug;
            var applicationName = "Homework";

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(logEventLevel)
                .WriteTo.Console()
                .CreateLogger();

            Log.Information("Starting...");

            loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(options =>
                {
                    options.RouteTemplate = "swagger/api/{documentname}/swagger.json";
                })
                .UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/api/v1/swagger.json", $"{applicationName} V1");
                    options.RoutePrefix = "swagger/api";
                })
                .UseMvc();
        }
    }
}
