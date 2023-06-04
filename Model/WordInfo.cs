using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;

namespace Nihonn.Model
{
    public class WordInfo
    {
        public static String CollectionName = "words";

        private static IMongoCollection<WordInfo> _collation;
        private static bool connected = false;

        public static IMongoCollection<WordInfo> DBCollation
        {
            get
            {
                if (connected == false)
                {
                    _collation = MongoDBHelper.Database.GetCollection<WordInfo>(CollectionName);
                }
                return _collation;
            }
        }

        public static List<WordInfo> GetNewWordsFromDB()
        {
            if(WordInfo.DBCollation.AsQueryable().Count(t=>t.MemoTime == 0) == 0)
                return new List<WordInfo>();
            return WordInfo.DBCollation.AsQueryable()
                            .Where(t => t.MemoTime == 0)
                            .OrderBy(t => t.Book)
                            .GroupBy(t => t.Book)
                            .First()
                            .OrderBy(t => t.Lesson)
                            .GroupBy(t => t.Lesson)
                            .First()
                            .OrderBy(t => t.Sentence)
                            .ToList();
        }

		public static List<WordInfo> GetTodayMemoWordsFromDB()
		{
			return WordInfo.DBCollation.AsQueryable()
				.Where(t => t.MemoTime != 0 && t.LastMemo < DateTime.Now)
				.ToList();
		}

		public static List<WordInfo> GetAllWordsFromDB()
		{
            return WordInfo.DBCollation.AsQueryable().ToList();
		}

		public static void ListToJson(List<WordInfo> words, ISession session)
        {

            for (var i = 0; i < words.Count; i++)
            {
                words[i].SessionName = "Word_" + i;
                session.SetString(words[i].SessionName, words[i].ToJson());
            }
        }

        public static List<WordInfo> JsonToList(ISession session)
        {
            var Words = new List<WordInfo>();
            var i = 0;
            var json = session.GetString("Word_" + i);
            while (json != null)
            {
                Words.Add(BsonSerializer.Deserialize<WordInfo>(json));
                i++;
                json = session.GetString("Word_" + i);
            }
            return Words;
        }

        public static WordInfo? GetWord(ISession session)
        {
            var json = session.GetString("Word");

            if (json != null && json != "")
            {
                return BsonSerializer.Deserialize<WordInfo>(json);
            }
            else
            {
                var Words = WordInfo.JsonToList(session);
                var UnCheckedWords = Words.Where(t => t.CorrectlyMemo == false).ToList();

                if (UnCheckedWords.Count == 0)
                {
                    return null;
                }

                var word = UnCheckedWords[(new Random(DateTime.Now.Second).Next(0, UnCheckedWords.Count))];
                session.SetString("Word", word.ToJson());
                return word;
            }
        }

        public void Update()
        {
            this.LastMemo = DateTime.Now;
            this.MemoTime += 1;
            this.SessionName = "";
            this.CorrectlyMemo = false;
            this.ErrorInMemo = false;

            if (MemoTime > Program.TimeLine.Count)
            {
                this.LastMemo = DateTime.Now.AddDays(20);
            }
            else
            {
                this.LastMemo = DateTime.Now.AddDays(Program.TimeLine[MemoTime - 1]);
            }

            var filter = Builders<WordInfo>.Filter.Eq(x => x.Id, this.Id);
            WordInfo.DBCollation.DeleteOne(filter);
            WordInfo.DBCollation.InsertOne(this);
        }

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("kanji")]
        public string Kanji { get; set; }

        [BsonElement("hiragana")]
        public string Hiragana { get; set; }

        [BsonElement("katakana")]
        public string Katakana { get; set; }

        [BsonElement("lastmemo")]
        public DateTime LastMemo { get; set; }

        [BsonElement("memotime")]
        public int MemoTime { get; set; }

        [BsonElement("trans")]
        public string Translate { get; set; }

        [BsonElement("book")]
        public int Book { get; set; }

        [BsonElement("lesson")]
        public int Lesson { get; set; }

        [BsonElement("sentence")]
        public int Sentence { get; set; }


        public string Audiofile
        {
            get
            {
                return Book + "-" + Lesson + "-" + Sentence;
            }
        }

        public string SessionName { get; set; }

        public bool CorrectlyMemo { get; set; } // 是否正确背诵过
        public bool ErrorInMemo { get; set; } // 背诵过程中是否出错
    }
}
