using DataCollectionASP.Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System.Security.Claims;

namespace DataCollectionASP
{

    public class MongoDBHelper
    {
        private static IMongoDatabase _database;
        private static bool connected = false;
        public static String ErrorMessage = "";
        public static UserInfo CurrentUser;

        public static IMongoDatabase Database
        {
            get
            {
                if (connected == false)
                {
                    ConnnectionDatabase();
                }
                return _database;
            }
        }

        private static void ConnnectionDatabase ()
        {
            try
            {
                var configuration = WebApplication.CreateBuilder().Configuration;
                var connectionString = configuration.GetConnectionString("MongoDB");
                var databaseName = configuration.GetValue<string>("DatabaseName");
                var client = new MongoClient(connectionString);

                _database = client.GetDatabase(databaseName);
                ErrorMessage = "";
                connected = true;

#if DEBUG
				MongoDBHelper.CurrentUser = UserInfo.DBCollation.AsQueryable<UserInfo>().First(t => t.SchoolId == "1535");
#endif
			}
			catch (Exception e)
            {
                ErrorMessage = e.Message;
                connected = false;
            }
        }

        public static async Task<bool> VerifyUserAsync  (string schoolid, string password) 
        {
            // 查询数据库中是否存在该用户
            var users = UserInfo.DBCollation;
            var user =  await users.Find(u => u.SchoolId == schoolid && u.Password == password).SingleOrDefaultAsync();
            if (user != null)
            {
                CurrentUser = user;
                return true;
            }
            return false;
        }
    }
}
