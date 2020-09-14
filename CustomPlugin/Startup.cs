using System;
using System.Threading.Tasks;
using Decorators;
using Environment;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CustomPlugin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            var miaEnvConfiguration = new MiaEnvConfiguration();
            Configuration.Bind(miaEnvConfiguration);
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Example API",
                    Description = "A simple example ASP.NET Core Web API",
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseHttpsRedirection();

            app.Use(RouteInjections(app));

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Example API V1"); 
                c.RoutePrefix = "documentations";
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private static Func<HttpContext, Func<Task>, Task> RouteInjections(IApplicationBuilder app)
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
