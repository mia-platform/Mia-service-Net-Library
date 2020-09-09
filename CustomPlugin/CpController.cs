using System;
using Decorators;
using Environment;
using Microsoft.AspNetCore.Mvc;

namespace CustomPlugin
{
    [ApiController]
    [Route("[controller]")]
    public class CpController : ControllerBase
    {
        protected readonly MiaEnvConfiguration MiaEnvConfiguration;
        protected readonly ServiceClientFactory ServiceClientFactory;
        protected readonly IDecoratorResponseFactory DecoratorResponseFactory;

        public CpController(MiaEnvConfiguration miaEnvConfiguration, ServiceClientFactory serviceClientFactory, IDecoratorResponseFactory decoratorResponseFactory)
        {
            MiaEnvConfiguration = miaEnvConfiguration;
            ServiceClientFactory = serviceClientFactory;
            DecoratorResponseFactory = decoratorResponseFactory;
        }
    }
}
