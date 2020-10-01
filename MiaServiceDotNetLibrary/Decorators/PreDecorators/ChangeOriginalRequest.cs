using MiaServiceDotNetLibrary.Decorators.Constants;

namespace MiaServiceDotNetLibrary.Decorators.PreDecorators
{
    public class ChangeOriginalRequest : DecoratorResponse
    {
        public ChangeOriginalRequest(DecoratorRequest newRequest) : base(DecoratorConstants.ChangeOriginalStatusCode, DecoratorConstants.DefaultHeaders, newRequest.ToExpandoObject())
        {
        }
    }
}
