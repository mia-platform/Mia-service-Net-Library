namespace Logging
{
    public class MessageLog
    {
        public int level { get; set; }
        public long time { get; set; }
        public long reqId { get; set; }
        public string msg { get; set; }
        public object LogProperties { get; set; }
    }
}