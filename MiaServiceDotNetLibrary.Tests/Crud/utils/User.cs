using MiaServiceDotNetLibrary.Crud.library;

namespace MiaServiceDotNetLibrary.Tests.Crud.utils
{
    [CollectionName("users")]
    internal class User
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Status { get; set; }

        public User()
        {
        }

        public User(int id, string firstname, string lastname, string status)
        {
            Id = id;
            Firstname = firstname;
            Lastname = lastname;
            Status = status;
        }
    }
}
