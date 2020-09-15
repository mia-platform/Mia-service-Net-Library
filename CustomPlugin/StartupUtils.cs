using System;
using System.Threading.Tasks;
using Decorators;
using Environment;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CustomPlugin
{
    public class StartupUtils
    {
        public static void ConfigureCpServices(IServiceCollection services, IConfiguration configuration)
        {
            var miaEnvConfiguration = new MiaEnvConfiguration();
            configuration.Bind(miaEnvConfiguration);
            ConfigValidator.ValidateConfig(miaEnvConfiguration);

            var serviceClientFactory = new ServiceClientFactory(miaEnvConfiguration);
            var decoratorResponseFactory = new DecoratorResponseFactory();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            services.AddSingleton(miaEnvConfiguration);
            services.AddSingleton(serviceClientFactory);
            services.AddSingleton(decoratorResponseFactory);
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
