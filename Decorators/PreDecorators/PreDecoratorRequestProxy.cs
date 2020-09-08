using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

namespace Decorators.PreDecorators
{
    public class PreDecoratorRequestProxy
    {
        private DecoratorRequest _request;

        public PreDecoratorRequestProxy(DecoratorRequest originalRequest)
        {
            _request = originalRequest;
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
            return new PreDecoratorRequest()
            {
                Method = _request.Method,
                Path = _request.Path,
                Headers = _request.Headers,
                Query = _request.Query,
                Body = _request.Body,
            };
        }
    }
}