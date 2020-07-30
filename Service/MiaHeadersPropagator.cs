using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Service.Environment;

namespace Service
{
    public class MiaHeadersPropagator
    {
        private readonly Dictionary<string, string> _headers = new Dictionary<string, string>();

        public Dictionary<string, string> Headers => _headers;

        public MiaHeadersPropagator(IHeaderDictionary headers, MiaEnvConfiguration miaEnvConfiguration)
        {
            Headers[miaEnvConfiguration.USERID_HEADER_KEY] = headers[miaEnvConfiguration.USERID_HEADER_KEY];
            Headers[miaEnvConfiguration.GROUPS_HEADER_KEY] = headers[miaEnvConfiguration.GROUPS_HEADER_KEY];
            Headers[miaEnvConfiguration.CLIENTTYPE_HEADER_KEY] = headers[miaEnvConfiguration.CLIENTTYPE_HEADER_KEY];
            Headers[miaEnvConfiguration.BACKOFFICE_HEADER_KEY] = headers[miaEnvConfiguration.BACKOFFICE_HEADER_KEY];
        }
    }
}