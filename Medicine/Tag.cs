using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine
{
	public class Tag : MongoEntity
	{
		public string Content { get; set; }

		public Tag()
		{
			collectionName = "Tag";
		}

		public Tag(string content, MongoConnection connection) : this()
		{
			Connection = connection;
			Content = content;
		}

		public void GetById(ObjectId id, MongoConnection connection)
		{
			Connection = connection;
			Collection = Connection.GetCollection(collectionName);
			var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
			var document = Collection.Find(filter).First();
			_id = id;
			Content = document.GetValue("Content").AsString;
		}

		public void GetByContent(string content, MongoConnection connection)
		{
			Connection = connection;
			Collection = Connection.GetCollection(collectionName);
			var filter = Builders<BsonDocument>.Filter.Eq("Content", content);
			var document = Collection.Find(filter).First();
			_id = document.GetValue("_id").AsObjectId;
			Content = content;
		}

		public void Save(MongoConnection connection)
		{
			Collection = connection.GetCollection(collectionName);
			if (_id.CompareTo(new ObjectId()) == 0)
			{
				var document = new BsonDocument()
				{
					{ "Content", Content }
				};
				Collection.InsertOne(document);
				_id = document.GetValue("_id").AsObjectId;
			}
			else
			{
				var filter = Builders<BsonDocument>.Filter.Eq("_id", _id);
				var update = Builders<BsonDocument>.Update.Set("Content", Content);
				Collection.UpdateOne(filter, update);
			}
		}

		public void Save()
		{
			Save(Connection);
		}
	}
}
