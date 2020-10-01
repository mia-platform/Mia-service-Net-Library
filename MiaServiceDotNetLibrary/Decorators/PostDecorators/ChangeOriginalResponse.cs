using Decorators.Constants;
using MiaServiceDotNetLibrary.Decorators;

namespace Decorators.PostDecorators
{
    public class ChangeOriginalResponse : DecoratorResponse
    {
        public ChangeOriginalResponse(DecoratorResponse newResponse) : base(DecoratorConstants.ChangeOriginalStatusCode, DecoratorConstants.DefaultHeaders, newResponse.ToExpandoObject())
        {
        }
    }
}
