using System.Collections.Generic;
using System.Dynamic;
using Environment;

namespace Decorators.PreDecorators
{
    public class PreDecoratorRequest : DecoratorRequest
    {
        public PreDecoratorRequestProxy ChangeOriginalRequest()
        {
            var newRequest = (PreDecoratorRequest) MemberwiseClone();
            newRequest.Headers = new Dictionary<string, string>(Headers);
            newRequest.Query = new Dictionary<string, string>(Query);
            newRequest.Body = CloneRequestBody();
            return new PreDecoratorRequestProxy(new PreDecoratorRequest
            {
                Method = newRequest.Method,
                Path = newRequest.Path,
                Headers = newRequest.Headers,
                Query = newRequest.Query,
                Body = newRequest.Body
            });
        }

        private ExpandoObject CloneRequestBody()
        {
            dynamic newBody = new ExpandoObject();

            foreach (var kvp in Body)
            {
                ((IDictionary<string, object>) newBody).Add(kvp);
            }

            return newBody;
        }

        public string GetOriginalRequestPath()
        {
            return Path;
        }

        public string GetOriginalRequestMethod()
        {
            return Method;
        }

        public IDictionary<string, string> GetOriginalRequestHeaders()
        {
            return Headers;
        }

        public IDictionary<string, string> GetOriginalRequestQuery()
        {
            return Query;
        }

        public ExpandoObject GetOriginalRequestBody()
        {
            return Body;
        }

        public string GetUserId(MiaEnvConfiguration config)
        {
            return Headers[config.UserIdHeaderKey];
        }

        public string GetGroups(MiaEnvConfiguration config)
        {
            return Headers[config.GroupsHeaderKey];
        }

        public string GetClientType(MiaEnvConfiguration config)
        {
            return Headers[config.ClientTypeHeaderKey];
        }

        public string IsFromBackOffice(MiaEnvConfiguration config)
        {
            return Headers[config.BackOfficeHeaderKey];
        }
    }
}
