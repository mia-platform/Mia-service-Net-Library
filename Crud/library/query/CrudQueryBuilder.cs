using System.Collections.Generic;
using Crud.library.enums;
using Crud.library.query.Extensions;
using Microsoft.AspNetCore.Http.Extensions;

namespace Crud.library.query
{
    public class CrudQueryBuilder
    {
        private List<KeyValuePair<string, string>> _query;
        
        public CrudQueryBuilder()
        {
            _query = new List<KeyValuePair<string, string>>();
        }

        public void MongoQuery(  Dictionary<string, Dictionary<MongoOperator, object>> value)
        {
            _query.AddMongoQueryParam(value);
        }
        public void State(State value)
        {
            _query.AddStateParam(value);
        }

        public List<KeyValuePair<string, string>> GetQuery()
        {
            return _query;
        } 
        public override string ToString()
        {
            return new QueryBuilder(_query).ToQueryString().ToString();
        }
        
    }
}