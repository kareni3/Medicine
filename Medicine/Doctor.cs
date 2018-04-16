using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine
{
	public class Doctor : MongoEntity, IMedicineObject
	{
		public string Lastname { get; set; }
		public string Firstname { get; set; }
		public string Middlename { get; set; }
		
		

		public Doctor()
		{
			collectionName = "Doctor";
		}

		public Doctor(string lastname, string firstname, string middlename, MongoConnection connection) : this()
		{
			Connection = connection;
			Lastname = lastname;
			Firstname = firstname;
			Middlename = middlename;
		}

		public override void GetById(ObjectId id)
		{
			Collection = Connection.GetCollection(collectionName);
			var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
			var document = Collection.Find(filter).First();
			_id = id;
			Lastname = document.GetValue("Lastname").AsString;
			Firstname = document.GetValue("Firstname").AsString;
			Middlename = document.GetValue("Middlename").AsString;
		}

		//Плохой способ, потому что имя не является уникальным идентификатором
		public void GetByName(string lastname, string firstname, string middlename, MongoConnection connection)
		{
			Connection = connection;
			Collection = Connection.GetCollection(collectionName);
			var filter = Builders<BsonDocument>.Filter.And(new List<FilterDefinition<BsonDocument>>()
			{
				Builders<BsonDocument>.Filter.Eq("Lastname", lastname),
				Builders<BsonDocument>.Filter.Eq("Firstname", firstname),
				Builders<BsonDocument>.Filter.Eq("Middlename", middlename)
			});
			var document = Collection.Find(filter).First();
			_id = document.GetValue("_id").AsObjectId;
			Lastname = lastname;
			Firstname = firstname;
			Middlename = middlename;
		}

		public override void Save()
		{
			Collection = Connection.GetCollection(collectionName);
			if (_id.CompareTo(new ObjectId()) == 0)
			{
				var document = new BsonDocument()
				{
					{ "Lastname", Lastname },
					{ "Firstname", Firstname },
					{ "Middlename", Middlename }
				};
				Collection.InsertOne(document);
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
				Collection.UpdateOne(filter, update);
			}
		}
	}
}
