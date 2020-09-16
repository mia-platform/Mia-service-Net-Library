namespace CustomPlugin
{
    public class CpStatus
    {
        private const string OkStatus = "OK";
        private const string KoStatus = "KO";
        
        public static CpStatusBody Ok()
        {
            return new CpStatusBody
            {
                Status = OkStatus
            };
        }
        
        public static CpStatusBody Ko()
        {
            return new CpStatusBody
            {
                Status = KoStatus
            };
        }
    }
}
