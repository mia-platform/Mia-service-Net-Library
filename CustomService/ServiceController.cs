using Decorators;
using Environment;
using Microsoft.AspNetCore.Mvc;

namespace CustomService
{
    [ApiController]
    [Route("[controller]")]
    public abstract class ServiceController : ControllerBase
    {
        protected readonly MiaEnvConfiguration MiaEnvConfiguration;
        protected readonly ServiceClientFactory ServiceClientFactory;
        protected readonly DecoratorResponseFactory DecoratorResponseFactory;

        public ServiceController(MiaEnvConfiguration miaEnvConfiguration, ServiceClientFactory serviceClientFactory,
            DecoratorResponseFactory decoratorResponseFactory)
        {
            MiaEnvConfiguration = miaEnvConfiguration;
            ServiceClientFactory = serviceClientFactory;
            DecoratorResponseFactory = decoratorResponseFactory;
        }
    }
}