using System.Collections.Generic;
using Crud.library.enums;
using Crud.library.query;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using NFluent;
using NUnit.Framework;

namespace Crud.Tests
{
    public class CrudQueryBuilderTest
    {
        [Test]
        public void TestMongoQuery()
        {
            var qb = new CrudQueryBuilder();
            var mongoQueryOperations = new Dictionary<MongoOperator, object> {[MongoOperator.GreaterThan] = 42, [MongoOperator.NotIn]= new List<string>{"baz", "bam"}};
            var mongoQuery = new Dictionary<string, Dictionary<MongoOperator, object>> {["foo"] = mongoQueryOperations};
            qb.MongoQuery(mongoQuery);
            var result = qb.GetQuery().Find(p => p.Key == "_q");
            Check.That(result.Value).IsEqualTo(@"{""foo"":{""$gt"":42,""$nin"":[""baz"",""bam""]}}");
        } 
        
        [Test]
        public void TestState()
        {
            var qb = new CrudQueryBuilder();
            qb.State(State.Draft);
            var result = qb.GetQuery().Find(p => p.Key == "_s");
            Check.That(result.Value).IsEqualTo(State.Draft.Value);
        }
    }
}