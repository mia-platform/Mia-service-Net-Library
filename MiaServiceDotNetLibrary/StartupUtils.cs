using System;
using System.Threading.Tasks;
using MiaServiceDotNetLibrary.Decorators;
using MiaServiceDotNetLibrary.Environment;
using MiaServiceDotNetLibrary.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using log4net;

namespace MiaServiceDotNetLibrary
{
    public class StartupUtils
    {
        private const string SwaggerDocumentName = "json";
        private const string SwaggerDocsPrefix = "documentation";

        public static void ConfigureMiaLibraryServices(IServiceCollection services, IConfiguration configuration)
        {

            /* 
            * The line below is necessary to invoke log4net and to load its configuration, 
            * without this line every logging functionality will be disabled.
            * Follow this link to better understand how log4net configuration works: 
            * https://logging.apache.org/log4net/release/manual/configuration.html
            */
            LogManager.GetLogger(typeof(Logger));

            var miaEnvConfiguration = new MiaEnvConfiguration();
            configuration.Bind(miaEnvConfiguration);
            ConfigValidator.ValidateConfig(miaEnvConfiguration);

            var serviceClientFactory = new ServiceClientFactory(miaEnvConfiguration);
            var decoratorResponseFactory = new DecoratorResponseFactory();

            SetupJsonSerializer();

            services.AddSingleton(miaEnvConfiguration);
            services.AddSingleton(serviceClientFactory);
            services.AddSingleton(decoratorResponseFactory);
        }

        public static void ConfigureDocs(IServiceCollection services, string title = "Custom Service", string description = "Custom Service API")
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(SwaggerDocumentName, new OpenApiInfo
                {
                    Title = title,
                    Description = description
                });
            });
        }  
        
        public static void ConfigureDocs(IServiceCollection services, OpenApiInfo apiInfo)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(SwaggerDocumentName, apiInfo);
            });
        }

        public static void UseSwagger(IApplicationBuilder app, string apiName = "API")
        {
            app.UseSwagger(c => { c.RouteTemplate = $"/{SwaggerDocsPrefix}/{{documentName}}/swagger.json"; });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/{SwaggerDocsPrefix}/{SwaggerDocumentName}/swagger.json", apiName);
                c.RoutePrefix = SwaggerDocsPrefix;
            });

            app.UseSwagger(c => { c.RouteTemplate = $"/{SwaggerDocsPrefix}/{{documentName}}"; });
        }

        private static void SetupJsonSerializer()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public static Func<HttpContext, Func<Task>, Task> RouteInjections(IApplicationBuilder app)
        {
            return async (context, next) =>
            {
                var miaEnvConfiguration =
                    (MiaEnvConfiguration) app.ApplicationServices.GetService(typeof(MiaEnvConfiguration));

                var miaHeadersPropagator = new MiaHeadersPropagator(context.Request.Headers, miaEnvConfiguration);
                ServiceClientFactory.SetMiaHeaders(miaHeadersPropagator);
                context.Items.Add("MiaHeadersPropagator", miaHeadersPropagator);

                await next.Invoke();
            };
        }
    }
}
