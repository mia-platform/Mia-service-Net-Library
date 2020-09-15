using Logging.Entities;

 namespace Logging
{
    public class CompletedRequestLog
    {
        public int level { get; set; }
        public long time { get; set; }
        public long reqId { get; set; }
        public Http http { get; set; }
        public Url url { get; set; }
        public UserAgent userAgent { get; set; }
        public Host host { get; set; }
        public decimal responseTime { get; set; }
    }
}