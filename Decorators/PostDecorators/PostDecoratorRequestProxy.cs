using System.Collections.Generic;

namespace Decorators.PostDecorators
{
    public class PostDecoratorRequestProxy
    {
        private readonly PostDecoratorRequest _request;

        public PostDecoratorRequestProxy(PostDecoratorRequest request)
        {
            _request = request;
        }

        public PostDecoratorRequestProxy StatusCode(int statusCode)
        {
            _request.Response.StatusCode = statusCode;
            return this;
        }
        
        public PostDecoratorRequestProxy Body(IDictionary<string, string> headers)
        {
            _request.Response.Headers = headers;
            return this;
        }

        public PostDecoratorRequest Change()
        {
            return _request;
        }
    }
}
