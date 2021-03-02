using MiaServiceDotNetLibrary.Decorators;
using MiaServiceDotNetLibrary.Environment;
using Microsoft.AspNetCore.Mvc;

namespace MiaServiceDotNetLibrary
{
    [ApiController]
    [Route("[controller]")]
    public abstract class ServiceController : ControllerBase
    {
        protected readonly MiaEnvsConfigurations MiaEnvsConfigurations;
        protected readonly ServiceClientFactory ServiceClientFactory;
        protected readonly DecoratorResponseFactory DecoratorResponseFactory;

        public ServiceController(MiaEnvsConfigurations miaEnvsConfigurations, ServiceClientFactory serviceClientFactory,
            DecoratorResponseFactory decoratorResponseFactory)
        {
            MiaEnvsConfigurations = miaEnvsConfigurations;
            ServiceClientFactory = serviceClientFactory;
            DecoratorResponseFactory = decoratorResponseFactory;
        }
    }
}
