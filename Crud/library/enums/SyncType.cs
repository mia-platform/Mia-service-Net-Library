namespace Crud.library.enums
{
    public class SyncType
    {
        private SyncType(int value)
        {
            Value = value;
        }

        public int Value { get; set; }

        public static SyncType SyncWayPullPush => new SyncType(0);
        public static SyncType SyncWayPull => new SyncType(1);
        public static SyncType SyncWayPush => new SyncType(2);
    }
}