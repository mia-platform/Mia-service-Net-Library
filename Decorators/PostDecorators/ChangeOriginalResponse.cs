using System.Collections.Generic;
using System.Dynamic;

namespace Decorators.PostDecorators
{
    public class ChangeOriginalResponse : DecoratorResponse
    {
        public ChangeOriginalResponse(int statusCode, IDictionary<string, string> headers, ExpandoObject body) : base(statusCode, headers, body)
        {
        }
    }
}
