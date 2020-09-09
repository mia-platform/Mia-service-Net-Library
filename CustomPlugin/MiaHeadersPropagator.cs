using System;
using System.Collections.Generic;
using Environment;
using Microsoft.AspNetCore.Http;

namespace CustomPlugin
{
    public class MiaHeadersPropagator
    {
        private readonly Dictionary<string, string> _headers = new Dictionary<string, string>();
        internal Dictionary<string, string> Headers => _headers;
        private MiaEnvConfiguration _envConfig;

        public MiaHeadersPropagator(IHeaderDictionary headers, MiaEnvConfiguration envConfig)
        {
            _envConfig = envConfig;
            Headers[_envConfig.UserIdHeaderKey] = headers[_envConfig.UserIdHeaderKey];
            Headers[_envConfig.GroupsHeaderKey] = headers[_envConfig.GroupsHeaderKey];
            Headers[_envConfig.ClientTypeHeaderKey] = headers[_envConfig.ClientTypeHeaderKey];
            Headers[_envConfig.BackOfficeHeaderKey] = headers[_envConfig.BackOfficeHeaderKey];
        }

        public string GetUserId()
        {
            return Headers[_envConfig.UserIdHeaderKey];
        }

        public string GetGroups()
        {
            return Headers[_envConfig.GroupsHeaderKey];
        }

        public string GetClientType()
        {
            return Headers[_envConfig.ClientTypeHeaderKey];
        }

        public bool IsFromBackOffice()
        {
            bool result;
            try
            {
                result = bool.Parse(Headers[_envConfig.BackOfficeHeaderKey]);
            }
            catch (Exception e)
            {
                result = false;
            }

            return result;
        }
    }
}
