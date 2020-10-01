using Decorators;
using Environment;
using Microsoft.AspNetCore.Mvc;

namespace CustomPlugin.obj
{
    public abstract class CpStatusController : CpController
    {
        protected CpStatusController(
            MiaEnvConfiguration miaEnvConfiguration,
            ServiceClientFactory serviceClientFactory,
            DecoratorResponseFactory decoratorResponseFactory) :
            base(miaEnvConfiguration, serviceClientFactory, decoratorResponseFactory)
        {
        }

        [HttpGet]
        [Route("/-/healthz")]
        public CpStatusBody Healthz()
        {
            return HealthinessHandler();
        }

        [HttpGet]
        [Route("/-/ready")]
        public CpStatusBody Ready()
        {
            return ReadinessHandler();
        }

        [HttpGet]
        [Route("/-/check-up")]
        public CpStatusBody CheckUp()
        {
            return CheckUpHandler();
        }

        protected virtual CpStatusBody HealthinessHandler()
        {
            return CpStatus.Ok();
        }

        protected virtual CpStatusBody ReadinessHandler()
        {
            return CpStatus.Ok();
        }
        
        protected virtual CpStatusBody CheckUpHandler()
        {
            return CpStatus.Ok();
        }
    }
}
