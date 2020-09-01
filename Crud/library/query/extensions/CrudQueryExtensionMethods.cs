using System.Collections.Generic;
using Crud.library.enums;
using Newtonsoft.Json;

namespace Crud.library.query.Extensions
{
    public static class CrudQueryExtensionMethods
    {
        public static void AddIdParam(
            this Dictionary<string, string> query, string id)
        {
            query.Add("_id", id);
        }      
        
        public static void AddCreatorIdParam(
            this Dictionary<string, string> query, string value)
        {
            query.Add("creatorId", value);
        } 
        
        public static void AddCreatedAtParam(
            this Dictionary<string, string> query, string value)
        {
            query.Add("createdAt", value);
        } 
        
        public static void AddUpdaterIdParam(
            this Dictionary<string, string> query, string value)
        {
            query.Add("updaterId", value);
        }


        public static void AddUpdatedAtParam(
            this Dictionary<string, string> query, string value)
        {
            query.Add("updatedAt", value);
        }

        public static void AddMongoQueryParam(
            this Dictionary<string, string> query, Dictionary<string, Dictionary<MongoOperator, object>> value)
        {
            var mongoQuery = JsonConvert.SerializeObject(value);
            query.Add(Parameters.Query.Value, mongoQuery);
        }

        public static void AddStateParam(
            this Dictionary<string, string> query, State state)
        {
            query.Add(Parameters.State.Value, state.Value);
        } 
        
        public static void AddPropertiesParam(
            this Dictionary<string, string> query, IEnumerable<string> value)
        {
            var properties = JsonConvert.SerializeObject(value);
            query.Add(Parameters.Properties.Value, properties);
        } 
        
        public static void AddLimitParam(
            this Dictionary<string, string> query, int value)
        {
            query.Add(Parameters.Limit.Value, value.ToString());
        } 
        
        public static void AddSkipParam(
            this Dictionary<string, string> query, int value)
        {
            query.Add(Parameters.Skip.Value, value.ToString());
        } 
        
        public static void AddSortParam(
            this Dictionary<string, string> query, string value)
        {
            query.Add(Parameters.Sort.Value, value);
        } 
    }
}
