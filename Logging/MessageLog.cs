namespace Logging
{
    public class MessageLog
    {
        public int Level { get; set; }
        public long Time { get; set; }
        public long ReqId { get; set; }
        public string Msg { get; set; }
        public object LogProperties { get; set; }
    }
}
