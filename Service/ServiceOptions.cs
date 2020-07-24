namespace Service
{
    public class ServiceOptions : InitServiceOptions
    {
        public ReturnAs ReturnAs { get; set; } = ReturnAs.JSON;
        public int[] AllowedStatusCodes { get; set; } = new int[] {200, 201, 202};
        public bool IsMiaHeaderInjected { get; set; } = true;
    }
}