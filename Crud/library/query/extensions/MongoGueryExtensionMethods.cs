using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Crud.library.enums;
using Newtonsoft.Json;

namespace Crud.library.query.Extensions
{
    public static class MongoQueryExtensionMethods
    {
        public static void AddEqualsExpression(
            this Dictionary<string, object> query, string key, object value)
        {
            query[key] = value;
        }

        public static void AddNotEqualsExpression(
            this Dictionary<string, object> query, string key, object value)
        {
            var queryOperation = new Dictionary<string, object> {{MongoOperator.NotEquals.Value, value}};
            query[key] = queryOperation;
        }

        public static void AddGreaterExpression(
            this Dictionary<string, object> query, string key, object value)
        {
            var queryOperation = new Dictionary<string, object> {{MongoOperator.GreaterThan.Value, value}};
            query[key] = queryOperation;
        }

        public static void AddGreaterOrEqualsExpression(
            this Dictionary<string, object> query, string key, object value)
        {
            var queryOperation = new Dictionary<string, object> {{MongoOperator.GreaterThanEquals.Value, value}};
            query[key] = queryOperation;
        }

        public static void AddLessExpression(
            this Dictionary<string, object> query, string key, object value)
        {
            var queryOperation = new Dictionary<string, object> {{MongoOperator.LessThan.Value, value}};
            query[key] = queryOperation;
        }

        public static void AddLessOrEqualsExpression(
            this Dictionary<string, object> query, string key, object value)
        {
            var queryOperation = new Dictionary<string, object> {{MongoOperator.LessThanEquals.Value, value}};
            query[key] = queryOperation;
        }

        public static void AddInExpression(
            this Dictionary<string, object> query, string key, IList value)
        {
            var queryOperation = new Dictionary<string, IList> {{MongoOperator.In.Value, value}};
            query[key] = queryOperation;
        }

        public static void AddNotInExpression(
            this Dictionary<string, object> query, string key, IList value)
        {
            var queryOperation = new Dictionary<string, IList> {{MongoOperator.NotIn.Value, value}};
            query[key] = queryOperation;
        }

        public static void AddAndExpression(
            this Dictionary<string, object> query, IEnumerable<MongoQueryBuilder> queryBuilders)
        {
            var mongoQueryBuilders = queryBuilders.ToList();
            var queries = mongoQueryBuilders.Select(qb => qb.Build()).ToList();
            query[MongoOperator.And.Value] = queries;
        }
        
        public static void AddOrExpression(
            this Dictionary<string, object> query, IEnumerable<MongoQueryBuilder> queryBuilders)
        {
            var mongoQueryBuilders = queryBuilders.ToList();
            var queries = mongoQueryBuilders.Select(qb => qb.Build()).ToList();
            query[MongoOperator.Or.Value] = queries;
        }
        
        public static void AddNorExpression(
            this Dictionary<string, object> query, IEnumerable<MongoQueryBuilder> queryBuilders)
        {
            var mongoQueryBuilders = queryBuilders.ToList();
            var queries = mongoQueryBuilders.Select(qb => qb.Build()).ToList();
            query[MongoOperator.Nor.Value] = queries;
        }
        
        public static void AddNotExpression(
            this Dictionary<string, object> query, MongoQueryBuilder queryBuilder)
        {
            var builtQuery = queryBuilder.Build();
            query[MongoOperator.Not.Value] = builtQuery;
        }
    }
}