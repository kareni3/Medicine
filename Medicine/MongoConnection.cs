using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine
{
	public class MongoConnection
	{
		MongoClient mongoClient;
		IMongoDatabase database;
		public string Server { get; set; }
		public string Port { get; set; }
		public string User { get; set; }
		public string Password { private get; set; }

		private bool connected;

		public MongoConnection()
		{
			connected = false;
		}

		public MongoConnection(string server, string port, string user, string password) : this()
		{
			Server = server;
			Port = port;
			User = user;
			Password = password;
		}

		public void Connect()
		{
			try
			{
				mongoClient = new MongoClient(String.Format(ConfigurationManager.ConnectionStrings["medicine"].ConnectionString, User, Password, Server, Port));
				database = mongoClient.GetDatabase("medicine");
				connected = true;
			}
			catch
			{
				connected = false;
				throw;
			}
		}

		internal IMongoCollection<BsonDocument> GetCollection(string name)
		{
			if (!connected)
				throw new Exception("Не передан экземпляр MongoConnection");
			return database.GetCollection<BsonDocument>(name);
		}
	}
}
