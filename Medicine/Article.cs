using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine
{
	public class Article : MongoEntity, IMedicineObject
	{
		public string Name { get; set; }
		public string Link { get; set; }
		public string Extract { get; set; }

		public Article()
		{
			collectionName = "Article";
			Connection = new MongoConnection();
		}

		public Article(string name, string link, string extract, MongoConnection connection) : this()
		{
			Connection = connection;
			Name = name;
			Link = link;
			Extract = extract;
		}

		public override void GetById(ObjectId id)
		{
			Collection = Connection.GetCollection(collectionName);
			var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
			var document = Collection.Find(filter).First();
			_id = id;
			Name = document.GetValue("Name").AsString;
			Link = document.GetValue("Link").AsString;
			Extract = document.GetValue("Extract").AsString;
		}

		public void GetByName(string name, MongoConnection connection)
		{
			Connection = connection;
			Collection = Connection.GetCollection(collectionName);
			var filter = Builders<BsonDocument>.Filter.Eq("Name", name);
			var document = Collection.Find(filter).First();
			_id = document.GetValue("_id").AsObjectId;
			Name = name;
			Link = document.GetValue("Link").AsString;
			Extract = document.GetValue("Extract").AsString;
		}

		public void GetByLink(string link, MongoConnection connection)
		{
			Connection = connection;
			Collection = Connection.GetCollection(collectionName);
			var filter = Builders<BsonDocument>.Filter.Eq("Link", link);
			var document = Collection.Find(filter).First();
			_id = document.GetValue("_id").AsObjectId;
			Name = document.GetValue("Name").AsString;
			Link = link;
			Extract = document.GetValue("Extract").AsString;
		}

		public override void Save()
		{
			Collection = Connection.GetCollection(collectionName);
			if (_id.CompareTo(new ObjectId()) == 0)
			{
				var document = new BsonDocument()
				{
					{ "Name", Name },
					{ "Link", Link },
					{ "Extract", Extract }
				};
				Collection.InsertOne(document);
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
				Collection.UpdateOne(filter, update);
			}
		}
	}
}
