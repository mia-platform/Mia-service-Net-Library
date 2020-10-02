namespace MiaServiceDotNetLibrary.Crud.library.enums
{
    public class Parameters
    {
        private Parameters(string value) { Value = value; }

        public string Value { get; set; }

        public static Parameters Query => new Parameters("_q");
        public static Parameters Properties => new Parameters("_p");
        public static Parameters State => new Parameters("_st");
        public static Parameters Sort => new Parameters("_s");
        public static Parameters Limit => new Parameters("_l");
        public static Parameters Skip => new Parameters("_sk");
    }
}
