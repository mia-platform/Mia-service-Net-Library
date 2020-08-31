namespace Crud.library.enums
{
    public class State
    {
        private State(string value) { Value = value; }

        public string Value { get; set; }

        public static State Pub => new State("PUBLIC");
        public static State Draft => new State("DRAFT");
        public static State Trash => new State("TRASH");
        public static State Deleted => new State("DELETED");
        public static State ToPush => new State("TO_PUSH");
        public static State ToPub => new State("TO_PUBLIC");
        public static State ToDraft => new State("TO_DRAFT");
        public static State ToTrash => new State("TO_TRASH");
        public static State ToDeleted => new State("TO_DELETED");
    }
}