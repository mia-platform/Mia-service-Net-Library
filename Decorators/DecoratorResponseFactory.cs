using Decorators.PreDecorators;

namespace Decorators
{
    public class DecoratorResponseFactory : IDecoratorResponseFactory
    {
        public DecoratorResponse MakePreDecoratorResponse(PreDecoratorRequest preDecoratorRequest)
        {
            if (preDecoratorRequest == null)
            {
                return new LeaveOriginalRequestUnmodified();
            }
            return new ChangeOriginalRequest(preDecoratorRequest);
        }
    }
}
