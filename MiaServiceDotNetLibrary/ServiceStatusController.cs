using MiaServiceDotNetLibrary.Decorators;
using MiaServiceDotNetLibrary.Environment;
using Microsoft.AspNetCore.Mvc;

namespace MiaServiceDotNetLibrary
{
    public abstract class ServiceStatusController : ServiceController
    {
        protected ServiceStatusController(
            MiaEnvConfiguration miaEnvConfiguration,
            ServiceClientFactory serviceClientFactory,
            DecoratorResponseFactory decoratorResponseFactory) :
            base(miaEnvConfiguration, serviceClientFactory, decoratorResponseFactory)
        {
        }

        [HttpGet]
        [Route("/-/healthz")]
        public ServiceStatusBody Healthz()
        {
            return HealthinessHandler();
        }

        [HttpGet]
        [Route("/-/ready")]
        public ServiceStatusBody Ready()
        {
            return ReadinessHandler();
        }

        [HttpGet]
        [Route("/-/check-up")]
        public ServiceStatusBody CheckUp()
        {
            return CheckUpHandler();
        }

        protected virtual ServiceStatusBody HealthinessHandler()
        {
            return ServiceStatus.Ok();
        }

        protected virtual ServiceStatusBody ReadinessHandler()
        {
            return ServiceStatus.Ok();
        }
        
        protected virtual ServiceStatusBody CheckUpHandler()
        {
            return ServiceStatus.Ok();
        }
    }
}
