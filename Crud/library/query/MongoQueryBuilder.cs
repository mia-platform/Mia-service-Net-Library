using System.Collections;
using System.Collections.Generic;
using Crud.library.query.Extensions;
using Microsoft.AspNetCore.Http.Extensions;
using Crud.library.query.Extensions;

namespace Crud.library.query
{
    public class MongoQueryBuilder
    {
        private readonly Dictionary<string, object> _query;

        public MongoQueryBuilder Equals(string key, object value)
        {
            _query.AddEqualsExpression(key, value);
            return this;
        }
        
        public MongoQueryBuilder NotEquals(string key, object value)
        {
            _query.AddNotEqualsExpression(key, value);
            return this;
        }
        
        public MongoQueryBuilder Greater(string key, object value)
        {
            _query.AddGreaterExpression(key, value);
            return this;
        }
        
        public MongoQueryBuilder GreaterOrEquals(string key, object value)
        {
            _query.AddGreaterOrEqualsExpression(key, value);
            return this;
        }
        
        public MongoQueryBuilder Less(string key, object value)
        {
            _query.AddLessExpression(key, value);
            return this;
        }

        public MongoQueryBuilder LessOrEquals(string key, object value)
        {
            _query.AddLessOrEqualsExpression(key, value);
            return this;
        }

        public MongoQueryBuilder In(string key, IList value)
        {
            _query.AddInExpression(key, value);
            return this;
        }
        
        public MongoQueryBuilder NotIn(string key, IList value)
        {
            _query.AddNotInExpression(key, value);
            return this;
        }

        public MongoQueryBuilder And(IEnumerable<MongoQueryBuilder> queryBuilders)
        {
            _query.AddAndExpression(queryBuilders);
            return this;
        }
        
        public MongoQueryBuilder Or(IEnumerable<MongoQueryBuilder> queryBuilders)
        {
            _query.AddOrExpression(queryBuilders);
            return this;
        }
        
        public MongoQueryBuilder Nor(IEnumerable<MongoQueryBuilder> queryBuilders)
        {
            _query.AddNorExpression(queryBuilders);
            return this;
        }
        
        public MongoQueryBuilder Not(MongoQueryBuilder queryBuilder)
        {
            _query.AddNotExpression(queryBuilder);
            return this;
        }

        public MongoQueryBuilder()
        {
            _query = new Dictionary<string, object>();
        }

        public Dictionary<string, object> Build()
        {
            return _query;
        }
    }
}
