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
        public Task<List<T>> RetrieveAll<T>();
        public Task<T> RetrieveById<T>(string id);
    }
}