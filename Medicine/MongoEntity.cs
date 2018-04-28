using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine
{
	public abstract class MongoEntity
	{
		internal IMongoCollection<BsonDocument> Collection { get; set; }
		public ObjectId _id { get; protected set; }
		public MongoConnection Connection { get; set; }
		protected string collectionName;

		public void GetById(ObjectId id, MongoConnection connection)
		{
			Connection = connection;
			GetById(id);
		}
		public void GetById(string id, MongoConnection connection)
		{
			Connection = connection;
			GetById(id);
		}
		public void GetById(string id)
		{
			GetById(new ObjectId(id));
		}
		public abstract void GetById(ObjectId id);
		public void Save(MongoConnection connection)
		{
			Connection = connection;
			Save();
		}
		public abstract void Save();
	}
}
