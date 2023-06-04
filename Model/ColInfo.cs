using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using NPOI.SS.UserModel;

namespace DataCollectionASP.Model
{
    public class ColInfo
    {
        public static String CollectionName = "colinfo";

        private static IMongoCollection<ColInfo> _collation;
        private static bool connected = false;

        public static IMongoCollection<ColInfo> DBCollation
        {
            get
            {
                if (connected == false)
                {
                    _collation = MongoDBHelper.Database.GetCollection<ColInfo>(CollectionName);
                }
                return _collation;
            }
        }

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("name")]
        public string ColName { get; set; }

        [BsonElement("type")]
        public string ColType { get; set; }

        [BsonElement("momo")]
        public string ColMemo { get; set; }

        [BsonElement("table")]
        public ObjectId TableId { get; set; }

        private TableInfo? _table = null;

        [BsonIgnore]
        public TableInfo Table
        {
            get
            {
                if(_table == null)
                {
                    var filter = Builders<TableInfo>.Filter.Eq("_id", TableId);
                    _table = TableInfo.DBCollation.Find(filter).FirstOrDefault() ?? new TableInfo();
                }
                return _table;
            }

            set
            {
                _table = value;
            }
        }
    }
}
