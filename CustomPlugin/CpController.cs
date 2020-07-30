using System;
using Microsoft.AspNetCore.Mvc;
using Service.Environment;

namespace CustomPlugin
{
    [ApiController]
    [Route("[controller]")]
    public class CpController : ControllerBase
    {
        protected readonly MiaEnvConfiguration MiaEnvConfiguration;
        protected readonly ServiceClientFactory ServiceClientFactory;

        public CpController(MiaEnvConfiguration miaEnvConfiguration, ServiceClientFactory serviceClientFactory)
        {
            MiaEnvConfiguration = miaEnvConfiguration;
            ServiceClientFactory = serviceClientFactory;
        }
    }
}