using Logging.Entities;

 namespace Logging
{
    public class IncomingRequestLog
    {
        public Http Http { get; set; }
        public Url Url  { get; set; }
        public UserAgent UserAgent  { get; set; }
        public Host Host  { get; set; }
    }
}