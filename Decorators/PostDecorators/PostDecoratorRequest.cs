using System.Collections.Generic;
using System.Dynamic;
using Environment;

namespace Decorators.PostDecorators
{
    public class PostDecoratorRequest
    {
        public DecoratorRequest Request { get; set; }
        public DecoratorResponse Response { get; set; }

        public PostDecoratorRequestProxy ChangeOriginalResponse()
        {
            var copy = (PostDecoratorRequest) MemberwiseClone();
            copy.Response = new DecoratorResponse(
                Response.StatusCode,
                new Dictionary<string, string>(Response.Headers),
                CloneResponseBody());
            
            return new PostDecoratorRequestProxy(copy);
        }
        
        public PostDecoratorRequest LeaveOriginalResponseUnmodified()
        {
            return null;
        }


        private ExpandoObject CloneResponseBody()
        {
            dynamic newBody = new ExpandoObject();

            foreach (var kvp in Response.Body)
            {
                ((IDictionary<string, object>) newBody).Add(kvp);
            }

            return newBody;
        }
        
        public string GetUserId(MiaEnvConfiguration config)
        {
            return Request.Headers[config.USERID_HEADER_KEY];
        }

        public string GetGroups(MiaEnvConfiguration config)
        {
            return Request.Headers[config.GROUPS_HEADER_KEY];
        }

        public string GetClientType(MiaEnvConfiguration config)
        {
            return Request.Headers[config.CLIENTTYPE_HEADER_KEY];
        }

        public string IsFromBackOffice(MiaEnvConfiguration config)
        {
            return Request.Headers[config.BACKOFFICE_HEADER_KEY];
        }
    }
}
