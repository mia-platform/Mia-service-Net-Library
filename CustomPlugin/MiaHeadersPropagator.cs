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
            Headers[_envConfig.USERID_HEADER_KEY] = headers[_envConfig.USERID_HEADER_KEY];
            Headers[_envConfig.GROUPS_HEADER_KEY] = headers[_envConfig.GROUPS_HEADER_KEY];
            Headers[_envConfig.CLIENTTYPE_HEADER_KEY] = headers[_envConfig.CLIENTTYPE_HEADER_KEY];
            Headers[_envConfig.BACKOFFICE_HEADER_KEY] = headers[_envConfig.BACKOFFICE_HEADER_KEY];
        }

        public string GetUserId()
        {
            return Headers[_envConfig.USERID_HEADER_KEY];
        }

        public string GetGroups()
        {
            return Headers[_envConfig.GROUPS_HEADER_KEY];
        }

        public string GetClientType()
        {
            return Headers[_envConfig.CLIENTTYPE_HEADER_KEY];
        }

        public bool IsFromBackOffice()
        {
            bool result;
            try
            {
                result = bool.Parse(Headers[_envConfig.BACKOFFICE_HEADER_KEY]);
            }
            catch (Exception e)
            {
                result = false;
            }

            return result;
        }
    }
}
