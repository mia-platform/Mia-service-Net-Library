using System.Collections.Generic;
using System.Dynamic;
using Environment;

namespace Decorators.PreDecorators
{
    public class PreDecoratorRequest : DecoratorRequest
    {
        public PreDecoratorRequestProxy ChangeOriginalRequest()
        {
            var copy = (PreDecoratorRequest) MemberwiseClone();
            copy.Headers = new Dictionary<string, string>(Headers);
            copy.Query = new Dictionary<string, string>(Query);
            copy.Body = CloneRequestBody();
            return new PreDecoratorRequestProxy(copy);
        }

        public PreDecoratorRequest LeaveOriginalRequestUnmodified()
        {
            return null;
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

        public string GetUserId(MiaEnvConfiguration config)
        {
            return Headers[config.USERID_HEADER_KEY];
        }

        public string GetGroups(MiaEnvConfiguration config)
        {
            return Headers[config.GROUPS_HEADER_KEY];
        }

        public string GetClientType(MiaEnvConfiguration config)
        {
            return Headers[config.CLIENTTYPE_HEADER_KEY];
        }

        public string IsFromBackOffice(MiaEnvConfiguration config)
        {
            return Headers[config.BACKOFFICE_HEADER_KEY];
        }
    }
}
