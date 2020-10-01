using Logging.Entities;

namespace Logging
{
    public class IncomingRequestLog
    {
        public int Level { get; set; }
        public long Time { get; set; }
        public long ReqId { get; set; }
        public HttpIncoming Http { get; set; }
        public Url Url { get; set; }
        public Host Host { get; set; }
    }
}