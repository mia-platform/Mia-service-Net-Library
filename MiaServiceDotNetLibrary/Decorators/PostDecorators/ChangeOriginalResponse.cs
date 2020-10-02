using MiaServiceDotNetLibrary.Decorators.Constants;

namespace MiaServiceDotNetLibrary.Decorators.PostDecorators
{
    public class ChangeOriginalResponse : DecoratorResponse
    {
        public ChangeOriginalResponse(DecoratorResponse newResponse) : base(DecoratorConstants.ChangeOriginalStatusCode, DecoratorConstants.DefaultHeaders, newResponse.ToExpandoObject())
        {
        }
    }
}
