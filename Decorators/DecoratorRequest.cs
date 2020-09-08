using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc;

namespace Decorators
{
    public class DecoratorRequest
    {
        public string Method { get; set; }
        public string Path { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public IDictionary<string, string> Query { get; set; }
        public ExpandoObject Body { get; set; }
    }
}