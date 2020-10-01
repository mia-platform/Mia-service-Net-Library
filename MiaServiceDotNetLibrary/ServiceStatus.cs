namespace MiaServiceDotNetLibrary
{
    public class ServiceStatus
    {
        private const string OkStatus = "OK";
        private const string KoStatus = "KO";
        
        public static ServiceStatusBody Ok()
        {
            return new ServiceStatusBody
            {
                Status = OkStatus
            };
        }
        
        public static ServiceStatusBody Ko()
        {
            return new ServiceStatusBody
            {
                Status = KoStatus
            };
        }
    }
}
