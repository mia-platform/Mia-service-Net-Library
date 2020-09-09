using Decorators.Constants;

namespace Decorators.PreDecorators
{
    public class LeaveOriginalRequestUnmodified : DecoratorResponse
    {
        public LeaveOriginalRequestUnmodified() : base(DecoratorConstants.LeaveOriginalUnchangedStatusCode, DecoratorConstants.DefaultHeaders, null)
        {
        }
    }
}
