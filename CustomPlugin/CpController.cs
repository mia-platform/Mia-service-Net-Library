using System;
using Decorators;
using Environment;
using Microsoft.AspNetCore.Mvc;

namespace CustomPlugin
{
    [ApiController]
    [Route("[controller]")]
    public abstract class CpController : ControllerBase
    {
        protected readonly MiaEnvConfiguration MiaEnvConfiguration;
        protected readonly ServiceClientFactory ServiceClientFactory;
        protected readonly DecoratorResponseFactory DecoratorResponseFactory;

        public CpController(MiaEnvConfiguration miaEnvConfiguration, ServiceClientFactory serviceClientFactory,
            DecoratorResponseFactory decoratorResponseFactory)
        {
            MiaEnvConfiguration = miaEnvConfiguration;
            ServiceClientFactory = serviceClientFactory;
            DecoratorResponseFactory = decoratorResponseFactory;
        }


        [HttpGet]
        [Route("/-/healthz")]
        public virtual CpStatusBody Healthz()
        {
            return CpStatus.Ok();
        }

        [HttpGet]
        [Route("/-/ready")]
        public virtual CpStatusBody Ready()
        {
            return CpStatus.Ok();
        }

        [HttpGet]
        [Route("/-/check-up")]
        public virtual CpStatusBody CheckUp()
        {
            return CpStatus.Ok();
        }
    }
}
