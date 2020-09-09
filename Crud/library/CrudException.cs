using System;

namespace Crud.library
{
    public class CrudException : Exception
    {
        public CrudException(string message) : base("CRUD client error: " + message)
        {
        }
    }
}
