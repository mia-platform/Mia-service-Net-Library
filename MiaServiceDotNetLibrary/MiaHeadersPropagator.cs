using System;
using System.Collections.Generic;
using MiaServiceDotNetLibrary.Environment;
using Microsoft.AspNetCore.Http;

namespace MiaServiceDotNetLibrary
{
    public class MiaHeadersPropagator
    {
        private readonly Dictionary<string, string> _headers = new Dictionary<string, string>();
        internal Dictionary<string, string> Headers => _headers;
        private MiaEnvsConfigurations _envsConfigurations;

        public MiaHeadersPropagator(IHeaderDictionary headers, MiaEnvsConfigurations envsConfigurations)
        {
            _envsConfigurations = envsConfigurations;
            Headers[_envsConfigurations.USERID_HEADER_KEY] = headers[_envsConfigurations.USERID_HEADER_KEY];
            Headers[_envsConfigurations.USER_PROPERTIES_HEADER_KEY] = headers[_envsConfigurations.USER_PROPERTIES_HEADER_KEY];
            Headers[_envsConfigurations.GROUPS_HEADER_KEY] = headers[_envsConfigurations.GROUPS_HEADER_KEY];
            Headers[_envsConfigurations.CLIENTTYPE_HEADER_KEY] = headers[_envsConfigurations.CLIENTTYPE_HEADER_KEY];
            Headers[_envsConfigurations.BACKOFFICE_HEADER_KEY] = headers[_envsConfigurations.BACKOFFICE_HEADER_KEY];
        }

        public string GetUserId()
        {
            return Headers[_envsConfigurations.USERID_HEADER_KEY];
        }

        public string GetUserProperties()
        {
            return Headers[_envsConfigurations.USER_PROPERTIES_HEADER_KEY];
        }

        public string GetGroups()
        {
            return Headers[_envsConfigurations.GROUPS_HEADER_KEY];
        }

        public string GetClientType()
        {
            return Headers[_envsConfigurations.CLIENTTYPE_HEADER_KEY];
        }

        public bool IsFromBackOffice()
        {
            bool result;
            try
            {
                result = bool.Parse(Headers[_envsConfigurations.BACKOFFICE_HEADER_KEY]);
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }
    }
}
