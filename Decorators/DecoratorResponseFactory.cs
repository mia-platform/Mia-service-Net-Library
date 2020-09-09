using System.Collections;
using System.Collections.Generic;
using Decorators.PreDecorators;

namespace Decorators
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

        public DecoratorResponse AbortChain(int finalStatusCode, IDictionary<string, string> finalHeaders = null, object finalBody = null)
        {
            return new AbortChainResponse(finalStatusCode, finalHeaders, finalBody);
        }
    }
}
