namespace MiaServiceDotNetLibrary.Crud.library.enums
{
    public class MongoOperator
    {
        private MongoOperator(string value) { Value = value; }

        public string Value { get; set; }

        public static MongoOperator And => new MongoOperator(@"$and");
        public static MongoOperator Or => new MongoOperator(@"$or");
        public static MongoOperator GreaterThan => new MongoOperator(@"$gt");
        public static MongoOperator GreaterThanEquals => new MongoOperator(@"$gte");
        public static MongoOperator LessThan => new MongoOperator(@"$lt");
        public static MongoOperator LessThanEquals => new MongoOperator(@"$lte");
        public static MongoOperator In => new MongoOperator(@"$in");
        public static MongoOperator NotIn => new MongoOperator(@"$nin");
        public static MongoOperator NotEquals => new MongoOperator(@"$ne");
        public static MongoOperator Not => new MongoOperator(@"$not");
        public static MongoOperator Nor => new MongoOperator(@"$nor");

        public override string ToString()
        {
            return Value;
        }
    }
}
