using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.InteropServices;
using MiaServiceDotNetLibrary.Decorators.PostDecorators;
using MiaServiceDotNetLibrary.Decorators.PreDecorators;

namespace MiaServiceDotNetLibrary.Decorators
{
    public class DecoratorResponseFactory
    {
        public DecoratorResponse MakePreDecoratorResponse(PreDecoratorRequest preDecoratorRequest)
        {
            if (preDecoratorRequest == null)
            {
                return new LeaveOriginalRequestUnmodified();
            }

            return new ChangeOriginalRequest(preDecoratorRequest);
        }

        public DecoratorResponse MakePostDecoratorResponse(PostDecoratorRequest postDecoratorRequest)
        {
            if (postDecoratorRequest == null)
            {
                return new LeaveOriginalResponseUnmodified();
            }

            return new ChangeOriginalResponse(postDecoratorRequest.Response);
        }

        public DecoratorResponse AbortChain(int finalStatusCode, [Optional]IDictionary<string, string> finalHeaders,
            [Optional]ExpandoObject finalBody)
        {
            return new AbortChainResponse(finalStatusCode, finalHeaders ?? new Dictionary<string, string>(), finalBody ?? new ExpandoObject());
        }
    }
}
