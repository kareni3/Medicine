using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine
{
	public class Medicament : MongoEntity, IMedicineObject
	{
		public string Name { get; set; }

		private string collectionName = "Medicament";

		public Medicament() { }

		public Medicament(string name, MongoConnection connection)
		{
			Connection = connection;
			Name = name;
		}

		public void GetById(ObjectId id, MongoConnection connection)
		{
			Connection = connection;
			Collection = Connection.GetCollection(collectionName);
			var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
			var document = Collection.Find(filter).First();
			_id = id;
			Name = document.GetValue("Name").AsString;
		}

		public void GetByName(string name, MongoConnection connection)
		{
			Connection = connection;
			Collection = Connection.GetCollection(collectionName);
			var filter = Builders<BsonDocument>.Filter.Eq("Name", name);
			var document = Collection.Find(filter).First();
			_id = document.GetValue("_id").AsObjectId;
			Name = name;
		}

		public void Save(MongoConnection connection)
		{
			Collection = connection.GetCollection(collectionName);
			if (_id.CompareTo(new ObjectId()) == 0)
			{
				var document = new BsonDocument()
				{
					{ "Name", Name }
				};
				Collection.InsertOne(document);
				_id = document.GetValue("_id").AsObjectId;
			}
			else
			{
				var filter = Builders<BsonDocument>.Filter.Eq("_id", _id);
				var update = Builders<BsonDocument>.Update.Set("Name", Name);
				Collection.UpdateOne(filter, update);
			}
		}

		public void Save()
		{
			Save(Connection);
		}
	}
}
