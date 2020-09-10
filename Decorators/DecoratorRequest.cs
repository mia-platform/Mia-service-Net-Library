using System.Collections.Generic;
using System.Dynamic;

namespace Decorators
{
    public class DecoratorRequest
    {
        public string Method { get; set; }
        public string Path { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public IDictionary<string, string> Query { get; set; }
        public ExpandoObject Body { get; set; }

        public ExpandoObject ToExpandoObject()
        {
            dynamic result = new ExpandoObject();
            result.method = Method;
            result.path = Path;
            result.headers = Headers;
            result.query = Query;
            result.body = Body;
            return result;
        }
    }
}
