using Decorators.Constants;
using MiaServiceDotNetLibrary.Decorators;

namespace Decorators.PreDecorators
{
    public class ChangeOriginalRequest : DecoratorResponse
    {
        public ChangeOriginalRequest(DecoratorRequest newRequest) : base(DecoratorConstants.ChangeOriginalStatusCode, DecoratorConstants.DefaultHeaders, newRequest.ToExpandoObject())
        {
        }
    }
}
