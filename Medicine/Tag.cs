using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine
{
	class Tag
	{
		public ObjectId _id { get; private set; }
		public string Content { get; set; }

		public MongoConnection Connection;
		private IMongoCollection<BsonDocument> collection;

		public Tag() { }

		public Tag(string content, MongoConnection connection)
		{
			Content = content;
		}

		public void GetById(ObjectId id, MongoConnection connection)
		{
			Connection = connection;
			collection = Connection.GetCollection(Collection.Tag);
			var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
			var document = collection.Find(filter).First();
			_id = id;
			Content = document.GetValue("Content").AsString;
		}

		public void GetByContent(string content, MongoConnection connection)
		{
			Connection = connection;
			collection = Connection.GetCollection(Collection.Tag);
			var filter = Builders<BsonDocument>.Filter.Eq("Content", content);
			var document = collection.Find(filter).First();
			_id = document.GetValue("_id").AsObjectId;
			Content = content;
		}

		public void Save(MongoConnection connection)
		{
			collection = connection.GetCollection(Collection.Tag);
			if (_id.CompareTo(new ObjectId()) == 0)
			{
				var document = new BsonDocument()
				{
					{ "Content", Content }
				};
				collection.InsertOne(document);
				_id = document.GetValue("_id").AsObjectId;
			}
			else
			{
				var filter = Builders<BsonDocument>.Filter.Eq("_id", _id);
				var update = Builders<BsonDocument>.Update.Set("Content", Content);
				collection.UpdateOne(filter, update);
			}
		}

		public void Save()
		{
			Save(Connection);
		}
	}
}
