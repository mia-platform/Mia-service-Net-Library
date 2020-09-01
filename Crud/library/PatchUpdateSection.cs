using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Crud.library.enums;
using Newtonsoft.Json.Linq;

namespace Crud.library
{
    public class PatchUpdateSection : Dictionary<PatchCodingKey, Dictionary<string, object>>
    {
        
    }
}