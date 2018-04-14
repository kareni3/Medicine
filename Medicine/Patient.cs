using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine
{
	class Patient : MedicineObject
	{
		public ObjectId _id { get; private set; }
		public string Lastname { get; set; }
		public string Firstname { get; set; }
		public string Middlename { get; set; }

		public MongoConnection Connection;
		private IMongoCollection<BsonDocument> collection;

		public Patient() { }

		public Patient(string lastname, string firstname, string middlename, MongoConnection connection)
		{
			Connection = connection;
			Lastname = lastname;
			Firstname = firstname;
			Middlename = middlename;
		}

		public void GetById(ObjectId id, MongoConnection connection)
		{
			Connection = connection;
			collection = Connection.GetCollection(Collection.Patient);
			var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
			var document = collection.Find(filter).First();
			_id = id;
			Lastname = document.GetValue("Lastname").AsString;
			Firstname = document.GetValue("Firstname").AsString;
			Middlename = document.GetValue("Middlename").AsString;
		}

		//Плохой способ, потому что имя не является уникальным идентификатором
		public void GetByName(string lastname, string firstname, string middlename, MongoConnection connection)
		{
			Connection = connection;
			collection = Connection.GetCollection(Collection.Patient);
			var filter = Builders<BsonDocument>.Filter.And(new List<FilterDefinition<BsonDocument>>()
			{
				Builders<BsonDocument>.Filter.Eq("Lastname", lastname),
				Builders<BsonDocument>.Filter.Eq("Firstname", firstname),
				Builders<BsonDocument>.Filter.Eq("Middlename", middlename)
			});
			var document = collection.Find(filter).First();
			_id = document.GetValue("_id").AsObjectId;
			Lastname = lastname;
			Firstname = firstname;
			Middlename = middlename;
		}

		public void Save(MongoConnection connection)
		{
			collection = connection.GetCollection(Collection.Patient);
			if (_id.CompareTo(new ObjectId()) == 0)
			{
				var document = new BsonDocument()
				{
					{ "Lastname", Lastname },
					{ "Firstname", Firstname },
					{ "Middlename", Middlename }
				};
				collection.InsertOne(document);
				_id = document.GetValue("_id").AsObjectId;
			}
			else
			{
				var filter = Builders<BsonDocument>.Filter.Eq("_id", _id);
				var update = Builders<BsonDocument>.Update.Combine(
					Builders<BsonDocument>.Update.Set("Lastname", Lastname),
					Builders<BsonDocument>.Update.Set("Firstname", Firstname),
					Builders<BsonDocument>.Update.Set("Middlename", Middlename)
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
