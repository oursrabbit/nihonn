using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataCollectionASP.Model
{
    public class TableInfo
    {
        public static String CollectionName = "tableinfo";

        private static IMongoCollection<TableInfo> _collation;
        private static bool connected = false;

        public static IMongoCollection<TableInfo> DBCollation
        {
            get
            {
                if (connected == false)
                {
                    _collation = MongoDBHelper.Database.GetCollection<TableInfo>(CollectionName);
                }
                return _collation;
            }
        }

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("name")]
        public string TableName { get; set; }

        [BsonElement("sheet")]
        public string SheetName { get; set; }

        [BsonElement("cols")]
        public List<ObjectId> ColIds { get; set; }

        private List<ColInfo>? _cols = null;

        [BsonIgnore]
        public List<ColInfo> Cols
        {
            get
            {
                if (_cols == null)
                {
                    _cols = new List<ColInfo>();
                    foreach (var colid in ColIds)
                    {
                        var filter = Builders<ColInfo>.Filter.Eq("_id", colid);
                        var col = ColInfo.DBCollation.Find(filter).FirstOrDefault();
                        if (col != null)
                            _cols.Add(col);
                    }
                }
                return _cols;
            }

            set
            {
                _cols = value;
            }
        }
    }
}
