using System.Collections.Generic;

namespace MiaServiceDotNetLibrary.Decorators.Constants
{
    public static class DecoratorConstants
    {
        public const int ChangeOriginalStatusCode = 200;
        public const int LeaveOriginalUnchangedStatusCode = 204;
        public const int AbortChainStatusCode = 418;
        public const string ContentTypeHeaderKey = "Content-Type";
        public const string ContentTypeHeaderValue = "application/json; charset=utf-8";
        public static readonly Dictionary<string, string> DefaultHeaders = new Dictionary<string, string>() {{
            ContentTypeHeaderKey, ContentTypeHeaderValue
        }};
    }
}
