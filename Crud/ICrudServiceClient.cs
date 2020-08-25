using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Crud
{
    public interface ICrudServiceClient
    {
        public Task<Stream> RetrieveAll<T>();
    }
}