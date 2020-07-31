using System;
using CustomPlugin.Environment;
using Microsoft.AspNetCore.Mvc;

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