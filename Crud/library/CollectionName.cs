using System;

namespace Crud.library
{
    public class CollectionName : Attribute
    {
        public virtual string Value { get; }

        public CollectionName(string value)
        {
            Value = value;
        }
    }
}
