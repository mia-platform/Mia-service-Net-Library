using System.Collections.Generic;
using System.Dynamic;

namespace Decorators
{
    public class DecoratorResponse
    {
        public int StatusCode { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public object Body { get; set; }
    }
}