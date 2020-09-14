using System;

namespace Crud.library
{
    public sealed class CollectionName : Attribute
    {
        public string Value { get; }

        public CollectionName(string value)
        {
            Value = value;
        }
    }
}
