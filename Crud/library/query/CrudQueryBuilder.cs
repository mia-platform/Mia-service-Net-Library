using System;
using System.Collections.Generic;
using Crud.library.enums;
using Crud.library.query.Extensions;
using Microsoft.AspNetCore.Http.Extensions;

namespace Crud.library.query
{
    public class CrudQueryBuilder
    {
        private readonly Dictionary<string, string> _query;

        public CrudQueryBuilder()
        {
            _query = new Dictionary<string, string>();
        }

        public CrudQueryBuilder Id(string value)
        {
            _query.AddIdParam(value);
            return this;
        }

        public CrudQueryBuilder CreatorId(string value)
        {
            _query.AddCreatorIdParam(value);
            return this;
        }

        public CrudQueryBuilder CreatedAt(string value)
        {
            _query.AddCreatedAtParam(value);
            return this;
        }

        public CrudQueryBuilder UpdaterId(string value)
        {
            _query.AddUpdaterIdParam(value);
            return this;
        }

        public CrudQueryBuilder UpdatedAt(string value)
        {
            _query.AddUpdatedAtParam(value);
            return this;
        }

        public CrudQueryBuilder MongoQuery(MongoQueryBuilder mongoQueryBuilder)
        {
            _query.AddMongoQueryParam(mongoQueryBuilder.Build());
            return this;
        }

        public CrudQueryBuilder State(State value)
        {
            _query.AddStateParam(value);
            return this;
        }

        public CrudQueryBuilder Properties(List<string> value)
        {
            _query.AddPropertiesParam(value);
            return this;
        }

        public CrudQueryBuilder Limit(int value)
        {
            _query.AddLimitParam(value);
            return this;
        }

        public CrudQueryBuilder Skip(int value)
        {
            _query.AddSkipParam(value);
            return this;
        }

        public CrudQueryBuilder Sort(string value)
        {
            _query.AddSortParam(value);
            return this;
        }

        public CrudQueryBuilder Param(string key, string value)
        {
            _query.Add(key, value);
            return this;
        }

        public Dictionary<string, string> Build()
        {
            return _query;
        }

        public override string ToString()
        {
            return new QueryBuilder(_query).ToQueryString().ToString();
        }
    }
}
