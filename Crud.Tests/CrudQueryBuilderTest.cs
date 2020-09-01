using System.Collections.Generic;
using Crud.library.enums;
using Crud.library.query;
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
            var result = query.Find(p => p.Key == "_id");
            Check.That(result.Value).IsEqualTo("foo");
        }
        
        [Test]
        public void TestCreatorId()
        {
            var query = _qb.CreatorId("foo").Build();
            var result = query.Find(p => p.Key == "creatorId");
            Check.That(result.Value).IsEqualTo("foo");
        }
        
        [Test]
        public void TestCreatedAt()
        {
            var query = _qb.CreatedAt("foo").Build();
            var result = query.Find(p => p.Key == "createdAt");
            Check.That(result.Value).IsEqualTo("foo");
        }
        
        [Test]
        public void TestUpdaterId()
        {
            var query = _qb.UpdaterId("foo").Build();
            var result = query.Find(p => p.Key == "updaterId");
            Check.That(result.Value).IsEqualTo("foo");
        }
        
        [Test]
        public void TestUpdatadAt()
        {
            var query = _qb.UpdatedAt("foo").Build();
            var result = query.Find(p => p.Key == "updatedAt");
            Check.That(result.Value).IsEqualTo("foo");
        }

        [Test]
        public void TestMongoQuery()
        {
            var mongoQueryOperations = new Dictionary<MongoOperator, object>
                {[MongoOperator.GreaterThan] = 42, [MongoOperator.NotIn] = new List<string> {"baz", "bam"}};
            var mongoQuery = new Dictionary<string, Dictionary<MongoOperator, object>> {["foo"] = mongoQueryOperations};
            var query = _qb.MongoQuery(mongoQuery).Build();
            var result = query.Find(p => p.Key == "_q");
            Check.That(result.Value).IsEqualTo(@"{""foo"":{""$gt"":42,""$nin"":[""baz"",""bam""]}}");
        }

        [Test]
        public void TestState()
        {
            var query = _qb.State(State.Draft).Build();
            var result = query.Find(p => p.Key == "_st");
            Check.That(result.Value).IsEqualTo(State.Draft.Value);
        }

        [Test]
        public void TestProperties()
        {
            var properties = new List<string> {"foo", "bar", "baz"};
            var query = _qb.Properties(properties).Build();
            var result = query.Find(p => p.Key == "_p");
            Check.That(result.Value).IsEqualTo(@"[""foo"",""bar"",""baz""]");
        }

        [Test]
        public void TestLimit()
        {
            var query = _qb.Limit(42).Build();
            var result = query.Find(p => p.Key == "_l");
            Check.That(result.Value).IsEqualTo("42");
        }

        [Test]
        public void TestSkip()
        {
            var query = _qb.Skip(42).Build();
            var result = query.Find(p => p.Key == "_sk");
            Check.That(result.Value).IsEqualTo("42");
        }

        [Test]
        public void TestSort()
        {
            var query = _qb.Sort("foo").Build();
            var result = query.Find(p => p.Key == "_s");
            Check.That(result.Value).IsEqualTo("foo");
        }

        [Test]
        public void TestMultipleParams()
        {
            var mongoQueryOperations = new Dictionary<MongoOperator, object>
                {[MongoOperator.GreaterThan] = 42, [MongoOperator.NotIn] = new List<string> {"baz", "bam"}};
            var mongoQuery = new Dictionary<string, Dictionary<MongoOperator, object>> {["foo"] = mongoQueryOperations};

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

            Check.That(query.Find(p => p.Key == "_id").Value).IsEqualTo("012345");
            Check.That(query.Find(p => p.Key == "creatorId").Value).IsEqualTo("12");
            Check.That(query.Find(p => p.Key == "createdAt").Value).IsEqualTo("34");
            Check.That(query.Find(p => p.Key == "updaterId").Value).IsEqualTo("56");
            Check.That(query.Find(p => p.Key == "updatedAt").Value).IsEqualTo("78");

            Check.That(query.Find(p => p.Key == "_q").Value)
                .IsEqualTo(@"{""foo"":{""$gt"":42,""$nin"":[""baz"",""bam""]}}");
            Check.That(query.Find(p => p.Key == "_st").Value).IsEqualTo(State.Trash.Value);
            Check.That(query.Find(p => p.Key == "_p").Value).IsEqualTo(@"[""foo"",""bar""]");
            Check.That(query.Find(p => p.Key == "_l").Value).IsEqualTo("42");
            Check.That(query.Find(p => p.Key == "_sk").Value).IsEqualTo("10");
            Check.That(query.Find(p => p.Key == "_s").Value).IsEqualTo("bam");
        }
    }
}
