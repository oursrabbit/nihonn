using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Nihonn.Model
{
    public class UserInfo
    {
        public static String CollectionName = "userinfo";

        private static IMongoCollection<UserInfo> _collation;
        private static bool connected = false;

        public static IMongoCollection<UserInfo> DBCollation
        {
            get {
                if (connected == false)
                {
                    _collation = MongoDBHelper.Database.GetCollection<UserInfo>(CollectionName);
                }
                return _collation;
            }
        }

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("schoolid")]
        public string SchoolId { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

		[BsonElement("usertype")]
		public string UserType { get; set; }

		[BsonElement("name")]
		public string Name { get; set; }
	}
}
