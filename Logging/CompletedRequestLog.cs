using Logging.Entities;

 namespace Logging
{
    public class CompletedRequestLog
    {
        public Http Http { get; set; }
        public Url Url { get; set; }
        public UserAgent UserAgent { get; set; }
        public Host Host { get; set; }
        public double ResponseTime { get; set; }
    }
}