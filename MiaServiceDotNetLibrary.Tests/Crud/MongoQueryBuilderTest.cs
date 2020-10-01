using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Crud.library.query;
using MiaServiceDotNetLibrary.Tests.Crud.utils;
using NFluent;
using NUnit.Framework;

namespace MiaServiceDotNetLibrary.Tests.Crud
{
    public class MongoQueryBuilderTest
    {
        private MongoQueryBuilder _qb;

        [SetUp]
        public void Init()
        {
            _qb = new MongoQueryBuilder();
        }

        [Test]
        public void TestEquals()
        {
            var query = _qb.Equals("foo", "bar").Build();
            var result = query["foo"];
            Check.That(result).IsEqualTo("bar");
        }

        [Test]
        public void TestEqualsObject()
        {
            var user = new User {Firstname = "bar"};
            var query = _qb.Equals("foo", user).Build();
            var result = (User) query["foo"];
            Check.That(result.Firstname).IsEqualTo("bar");
        }

        [Test]
        public void TestNotEquals()
        {
            var query = _qb.NotEquals("foo", "bar").Build();
            var result = (Dictionary<string, object>) query["foo"];
            Check.That(result["$ne"]).IsEqualTo("bar");
        }

        [Test]
        public void TestNotEqualsObject()
        {
            var user = new User {Firstname = "bar"};
            var query = _qb.NotEquals("foo", user).Build();
            var result = (Dictionary<string, object>) query["foo"];
            Check.That(((User) result["$ne"]).Firstname).IsEqualTo("bar");
        }

        [Test]
        public void TestGreater()
        {
            var query = _qb.Greater("foo", 42).Build();
            var result = (Dictionary<string, object>) query["foo"];
            Check.That(result["$gt"]).IsEqualTo(42);
        }

        [Test]
        public void TestGreaterOrEquals()
        {
            var query = _qb.GreaterOrEquals("foo", 42).Build();
            var result = (Dictionary<string, object>) query["foo"];
            Check.That(result["$gte"]).IsEqualTo(42);
        }

        [Test]
        public void TestLess()
        {
            var query = _qb.Less("foo", 42).Build();
            var result = (Dictionary<string, object>) query["foo"];
            Check.That(result["$lt"]).IsEqualTo(42);
        }

        [Test]
        public void TestLessOrEquals()
        {
            var query = _qb.LessOrEquals("foo", 42).Build();
            var result = (Dictionary<string, object>) query["foo"];
            Check.That(result["$lte"]).IsEqualTo(42);
        }

        [Test]
        public void TestIn()
        {
            var values = new List<string> {"bar", "baz", "bam"};
            var query = _qb.In("foo", values).Build();
            var result = (Dictionary<string, IList>) query["foo"];
            Check.That(result["$in"]).IsEqualTo(values);
        }

        [Test]
        public void TestNotIn()
        {
            var values = new List<string> {"bar", "baz", "bam"};
            var query = _qb.NotIn("foo", values).Build();
            var result = (Dictionary<string, IList>) query["foo"];
            Check.That(result["$nin"]).IsEqualTo(values);
        }

        [Test]
        public void TestAnd()
        {
            var values = new List<string> {"bar", "baz", "bam"};
            var qb1 = new MongoQueryBuilder().In("foo", values);
            var qb2 = new MongoQueryBuilder().NotEquals("foo", "bar");
            var queryBuilders = new List<MongoQueryBuilder> {qb1, qb2};

            var globalQuery = _qb.And(queryBuilders).Build();
            var queries = (List<Dictionary<string, object>>) globalQuery["$and"];
            var firstQueryOperation = (Dictionary<string, IList>) queries[0]["foo"];
            var secondQueryOperation = (Dictionary<string, object>) queries[1]["foo"];

            Check.That(firstQueryOperation.Keys.First()).IsEqualTo("$in");
            Check.That(firstQueryOperation["$in"]).IsEqualTo(values);
            Check.That(secondQueryOperation.Keys.First()).IsEqualTo("$ne");
            Check.That(secondQueryOperation["$ne"]).IsEqualTo("bar");
        }

        [Test]
        public void TestOr()
        {
            var values = new List<string> {"bar", "baz", "bam"};
            var qb1 = new MongoQueryBuilder().NotIn("foo", values);
            var qb2 = new MongoQueryBuilder().Equals("foo", "bum");
            var queryBuilders = new List<MongoQueryBuilder> {qb1, qb2};

            var globalQuery = _qb.Or(queryBuilders).Build();
            var queries = (List<Dictionary<string, object>>) globalQuery["$or"];
            var firstQueryOperation = (Dictionary<string, IList>) queries[0]["foo"];
            var secondQueryOperation = (string) queries[1]["foo"];

            Check.That(firstQueryOperation.Keys.First()).IsEqualTo("$nin");
            Check.That(firstQueryOperation["$nin"]).IsEqualTo(values);
            Check.That(secondQueryOperation).IsEqualTo("bum");
        }

        [Test]
        public void TestNor()
        {
            var values = new List<string> {"bar", "baz", "bam"};
            var qb1 = new MongoQueryBuilder().In("foo", values);
            var qb2 = new MongoQueryBuilder().NotEquals("bar", 42);
            var queryBuilders = new List<MongoQueryBuilder> {qb1, qb2};

            var globalQuery = _qb.Nor(queryBuilders).Build();
            var queries = (List<Dictionary<string, object>>) globalQuery["$nor"];
            var firstQueryOperation = (Dictionary<string, IList>) queries[0]["foo"];
            var secondQueryOperation = (Dictionary<string, object>) queries[1]["bar"];

            Check.That(firstQueryOperation.Keys.First()).IsEqualTo("$in");
            Check.That(firstQueryOperation["$in"]).IsEqualTo(values);
            Check.That(secondQueryOperation.Keys.First()).IsEqualTo("$ne");
            Check.That(secondQueryOperation["$ne"]).IsEqualTo(42);
        }

        [Test]
        public void TestNot()
        {
            var queryToNegate = new MongoQueryBuilder().GreaterOrEquals("foo", 42);
            var query = _qb.Not(queryToNegate);

            var result = (Dictionary<string, object>) query.Build()["$not"];
            var innerQuery = (Dictionary<string, object>) result["foo"];

            Check.That(innerQuery["$gte"]).IsEqualTo(42);
        }
    }
}
