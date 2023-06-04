using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataCollectionASP.Model
{
	public class RowInfo
	{
		public static String CollectionName = "rowinfo";

		private static IMongoCollection<RowInfo> _collation;
		private static bool connected = false;

		public static IMongoCollection<RowInfo> DBCollation
		{
			get
			{
				if (connected == false)
				{
					_collation = MongoDBHelper.Database.GetCollection<RowInfo>(CollectionName);
				}
				return _collation;
			}
		}

		[BsonId]
		public ObjectId Id { get; set; }

		[BsonElement("userid")]
		public ObjectId UserID { get; set; }

		private UserInfo? _user = null;

		[BsonIgnore]
		public UserInfo User
		{
			get
			{
				if (_user == null)
				{
					var filter = Builders<UserInfo>.Filter.Eq("_id", UserID);
					_user = UserInfo.DBCollation.Find(filter).FirstOrDefault() ?? new UserInfo();
				}
				return _user;
			}

			set
			{
				_user = value;
			}
		}

		[BsonElement("value")]
		public List<String> Value { get; set; }

		[BsonElement("table")]
		public ObjectId TableId { get; set; }

		private TableInfo? _table = null;

		[BsonIgnore]
		public TableInfo Table
		{
			get
			{
				if (_table == null)
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
