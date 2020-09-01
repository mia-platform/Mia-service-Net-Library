using System.Collections.Generic;
using Crud.library.enums;
using Newtonsoft.Json;

namespace Crud.library.query.Extensions
{
    public static class CrudQueryExtensionMethods
    {
        public static void AddIdParam(
            this List<KeyValuePair<string, string>> query, string id)
        {
            query.Add(new KeyValuePair<string, string>("_id", id));
        }      
        
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
        
        public static void AddPropertiesParam(
            this List<KeyValuePair<string, string>> query, List<string> value)
        {
            var properties = JsonConvert.SerializeObject(value);
            query.Add(new KeyValuePair<string, string>(Parameters.Properties.Value, properties));
        } 
        
        public static void AddLimitParam(
            this List<KeyValuePair<string, string>> query, int value)
        {
            query.Add(new KeyValuePair<string, string>(Parameters.Limit.Value, value.ToString()));
        } 
        
        public static void AddSkipParam(
            this List<KeyValuePair<string, string>> query, int value)
        {
            query.Add(new KeyValuePair<string, string>(Parameters.Skip.Value, value.ToString()));
        } 
        
        public static void AddSortParam(
            this List<KeyValuePair<string, string>> query, string value)
        {
            query.Add(new KeyValuePair<string, string>(Parameters.Sort.Value, value));
        } 
    }
}
