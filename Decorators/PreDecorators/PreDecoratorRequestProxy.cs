using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

namespace Decorators.PreDecorators
{
    public class PreDecoratorRequestProxy
    {
        private readonly PreDecoratorRequest _request;

        public PreDecoratorRequestProxy(PreDecoratorRequest request)
        {
            _request = request;
        }

        public PreDecoratorRequestProxy Method(string method)
        {
            _request.Method = method;
            return this;
        }

        public PreDecoratorRequestProxy Path(string path)
        {
            _request.Path = path;
            return this;
        }

        public PreDecoratorRequestProxy Headers(IDictionary<string, string> headers)
        {
            _request.Headers = headers;
            return this;
        }

        public PreDecoratorRequestProxy Query(IDictionary<string, string> query)
        {
            _request.Query = query;
            return this;
        }

        public PreDecoratorRequestProxy Body(ExpandoObject body)
        {
            _request.Body = body;
            return this;
        }

        public PreDecoratorRequest Change()
        {
            return _request;
        }
    }
}
