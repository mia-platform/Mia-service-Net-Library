using Decorators.Constants;

namespace Decorators.PreDecorators
{
    public class ChangeOriginalRequest : DecoratorResponse
    {
        public ChangeOriginalRequest(DecoratorRequest body) : base(DecoratorConstants.ChangeOriginalStatusCode, DecoratorConstants.DefaultHeaders, body.ToExpandoObject())
        {
        }
    }
}
