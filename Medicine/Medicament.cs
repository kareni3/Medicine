using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine
{
	class Medicament : MedicineObject
	{
		public ObjectId _id { get; private set; }
		public string Name { get; set; }

		public MongoConnection Connection;
		private IMongoCollection<BsonDocument> collection;

		public Medicament() { }

		public Medicament(string name, MongoConnection connection)
		{
			Connection = connection;
			Name = name;
		}

		public void GetById(ObjectId id, MongoConnection connection)
		{
			Connection = connection;
			collection = Connection.GetCollection(Collection.Medicament);
			var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
			var document = collection.Find(filter).First();
			_id = id;
			Name = document.GetValue("Name").AsString;
		}

		public void GetByName(string name, MongoConnection connection)
		{
			Connection = connection;
			collection = Connection.GetCollection(Collection.Medicament);
			var filter = Builders<BsonDocument>.Filter.Eq("Name", name);
			var document = collection.Find(filter).First();
			_id = document.GetValue("_id").AsObjectId;
			Name = name;
		}

		public void Save(MongoConnection connection)
		{
			collection = connection.GetCollection(Collection.Medicament);
			if (_id.CompareTo(new ObjectId()) == 0)
			{
				var document = new BsonDocument()
				{
					{ "Name", Name }
				};
				collection.InsertOne(document);
				_id = document.GetValue("_id").AsObjectId;
			}
			else
			{
				var filter = Builders<BsonDocument>.Filter.Eq("_id", _id);
				var update = Builders<BsonDocument>.Update.Set("Name", Name);
				collection.UpdateOne(filter, update);
			}
		}

		public void Save()
		{
			Save(Connection);
		}
	}
}
