using System.Collections.Generic;
using Crud.library.enums;
using Crud.library.query;
using Crud.Tests.utils;
using Newtonsoft.Json;
using NFluent;
using NUnit.Framework;

namespace Crud.Tests
{
    public class CrudQueryBuilderTest
    {
        private CrudQueryBuilder _qb;

        [SetUp]
        public void Init()
        {
            _qb = new CrudQueryBuilder();
        }

        [Test]
        public void TestId()
        {
            var query = _qb.Id("foo").Build();
            var result = query["_id"];
            Check.That(result).IsEqualTo("foo");
        }
        
        [Test]
        public void TestCreatorId()
        {
            var query = _qb.CreatorId("foo").Build();
            var result = query["creatorId"];
            Check.That(result).IsEqualTo("foo");
        }
        
        [Test]
        public void TestCreatedAt()
        {
            var query = _qb.CreatedAt("foo").Build();
            var result = query["createdAt"];
            Check.That(result).IsEqualTo("foo");
        }
        
        [Test]
        public void TestUpdaterId()
        {
            var query = _qb.UpdaterId("foo").Build();
            var result = query["updaterId"];
            Check.That(result).IsEqualTo("foo");
        }
        
        [Test]
        public void TestUpdatedAt()
        {
            var query = _qb.UpdatedAt("foo").Build();
            var result = query["updatedAt"];
            Check.That(result).IsEqualTo("foo");
        }

        /*[Test]
        public void TestMongoQuery()
        {
            var mongoQueryOperations = new Dictionary<MongoOperator, object>
                {[MongoOperator.GreaterThan] = 42, [MongoOperator.NotIn] = new List<string> {"baz", "bam"}};
            var mongoQuery = new Dictionary<string, Dictionary<MongoOperator, object>> {["foo"] = mongoQueryOperations};
            var query = _qb.MongoQuery(mongoQuery).Build();
            var result = query["_q"];
            Check.That(result).IsEqualTo(@"{""foo"":{""$gt"":42,""$nin"":[""baz"",""bam""]}}");
        }*/

        [Test]
        public void TestState()
        {
            var query = _qb.State(State.Draft).Build();
            var result = query["_st"];
            Check.That(result).IsEqualTo(State.Draft.Value);
        }

        [Test]
        public void TestProperties()
        {
            var properties = new List<string> {"foo", "bar", "baz"};
            var query = _qb.Properties(properties).Build();
            var result = query["_p"];
            Check.That(result).IsEqualTo(@"[""foo"",""bar"",""baz""]");
        }

        [Test]
        public void TestLimit()
        {
            var query = _qb.Limit(42).Build();
            var result = query["_l"];
            Check.That(result).IsEqualTo("42");
        }

        [Test]
        public void TestSkip()
        {
            var query = _qb.Skip(42).Build();
            var result = query["_sk"];
            Check.That(result).IsEqualTo("42");
        }

        [Test]
        public void TestSort()
        {
            var query = _qb.Sort("foo").Build();
            var result = query["_s"];
            Check.That(result).IsEqualTo("foo");
        }

        [Test]
        public void TestMultipleParams()
        {
            var mongoQueryOperations = new Dictionary<MongoOperator, object>
                {[MongoOperator.GreaterThan] = 42, [MongoOperator.NotIn] = new List<string> {"baz", "bam"}};
            //var mongoQuery = new Dictionary<string, Dictionary<MongoOperator, object>> {["foo"] = mongoQueryOperations};
            var greaterThanQuery = new MongoQueryBuilder().Greater("foo", 42);
            var notInQuery = new MongoQueryBuilder().NotIn("foo", new List<string> {"baz", "bam"});
            var mongoQuery = new MongoQueryBuilder().And(new List<MongoQueryBuilder>{greaterThanQuery, notInQuery});
            
            var query = _qb
                .Id("012345")
                .CreatorId("12")
                .CreatedAt("34")
                .UpdaterId("56")
                .UpdatedAt("78")
                .MongoQuery(mongoQuery)
                .State(State.Trash)
                .Properties(new List<string> {"foo", "bar"})
                .Limit(42)
                .Skip(10)
                .Sort("bam")
                .Build();

            Check.That(query["_id"]).IsEqualTo("012345");
            Check.That(query["creatorId"]).IsEqualTo("12");
            Check.That(query["createdAt"]).IsEqualTo("34");
            Check.That(query["updaterId"]).IsEqualTo("56");
            Check.That(query["updatedAt"]).IsEqualTo("78");

            Check.That(query["_q"])
                .IsEqualTo(@"{""$and"":[{""foo"":{""$gt"":42.0}},{""foo"":{""$nin"":[""baz"",""bam""]}}]}");
            Check.That(query["_st"]).IsEqualTo(State.Trash.Value);
            Check.That(query["_p"]).IsEqualTo(@"[""foo"",""bar""]");
            Check.That(query["_l"]).IsEqualTo("42");
            Check.That(query["_sk"]).IsEqualTo("10");
            Check.That(query["_s"]).IsEqualTo("bam");
        }
        
        [Test]
        public void TestCustomParam()
        {
            var query = _qb.Param("foo", "bar").Build();
            var result = query["foo"];
            Check.That(result).IsEqualTo("bar");
        }
    }
}
