﻿using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine
{
	public class Article : MedicineObject
	{
		public ObjectId _id { get; private set; }
		public string Name { get; set; }
		public string Link { get; set; }
		public string Extract { get; set; }

		public MongoConnection Connection { get; set; }
		private IMongoCollection<BsonDocument> collection;

		public Article() { }

		public Article(string name, string link, string extract, MongoConnection connection)
		{
			Connection = connection;
			Name = name;
			Link = link;
			Extract = extract;
		}

		public void GetById(ObjectId id, MongoConnection connection)
		{
			Connection = connection;
			collection = Connection.GetCollection(Collection.Article);
			var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
			var document = collection.Find(filter).First();
			_id = id;
			Name = document.GetValue("Name").AsString;
			Link = document.GetValue("Link").AsString;
			Extract = document.GetValue("Extract").AsString;
		}

		public void GetByName(string name, MongoConnection connection)
		{
			Connection = connection;
			collection = Connection.GetCollection(Collection.Article);
			var filter = Builders<BsonDocument>.Filter.Eq("Name", name);
			var document = collection.Find(filter).First();
			_id = document.GetValue("_id").AsObjectId;
			Name = name;
			Link = document.GetValue("Link").AsString;
			Extract = document.GetValue("Extract").AsString;
		}

		public void GetByLink(string link, MongoConnection connection)
		{
			Connection = connection;
			collection = Connection.GetCollection(Collection.Article);
			var filter = Builders<BsonDocument>.Filter.Eq("Link", link);
			var document = collection.Find(filter).First();
			_id = document.GetValue("_id").AsObjectId;
			Name = document.GetValue("Name").AsString;
			Link = link;
			Extract = document.GetValue("Extract").AsString;
		}

		public void Save(MongoConnection connection)
		{
			collection = connection.GetCollection(Collection.Article);
			if (_id.CompareTo(new ObjectId()) == 0)
			{
				var document = new BsonDocument()
				{
					{ "Name", Name },
					{ "Link", Link },
					{ "Extract", Extract }
				};
				collection.InsertOne(document);
				_id = document.GetValue("_id").AsObjectId;
			}
			else
			{
				var filter = Builders<BsonDocument>.Filter.Eq("_id", _id);
				var update = Builders<BsonDocument>.Update.Combine(
					Builders<BsonDocument>.Update.Set("Name", Name),
					Builders<BsonDocument>.Update.Set("Link", Link),
					Builders<BsonDocument>.Update.Set("Extract", Extract)
				);
				collection.UpdateOne(filter, update);
			}
		}

		public void Save()
		{
			Save(Connection);
		}
	}
}
