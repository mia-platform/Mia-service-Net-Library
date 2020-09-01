using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Crud.library.enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Crud.library.query.Extensions
{
    public static class CrudQueryExtensionMethods
    {
        public static void AddCreatorIdParam(
            this List<KeyValuePair<string, string>> query, string value)
        {
            query.Add(new KeyValuePair<string, string>("creatorId", value));
        } 
        
        public static void AddCreatedAtParam(
            this List<KeyValuePair<string, string>> query, string value)
        {
            query.Add(new KeyValuePair<string, string>("createdAt", value));
        } 
        
        public static void AddUpdaterIdParam(
            this List<KeyValuePair<string, string>> query, string value)
        {
            query.Add(new KeyValuePair<string, string>("updaterId", value));
        }


        public static void AddUpdatedAtParam(
            this List<KeyValuePair<string, string>> query, string value)
        {
            query.Add(new KeyValuePair<string, string>("updatedAt", value));
        }

        public static void AddMongoQueryParam(
            this List<KeyValuePair<string, string>> query, Dictionary<string, Dictionary<MongoOperator, object>> value)
        {
            var mongoQuery = JsonConvert.SerializeObject(value);
            query.Add(new KeyValuePair<string, string>(Parameters.Query.Value, mongoQuery));
        }

        public static void AddStateParam(
            this List<KeyValuePair<string, string>> query, State state)
        {
            query.Add(new KeyValuePair<string, string>(Parameters.State.Value, state.Value));
        } 
    }
}