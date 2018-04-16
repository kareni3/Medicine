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
		
		public Medicament()
		{
			collectionName = "Medicament";
		}

		public Medicament(string name, MongoConnection connection) : this()
		{
			Connection = connection;
			Name = name;
		}

		public override void GetById(ObjectId id)
		{
			if (Connection == null)
				throw new Exception("Не передан экземпляр MongoConnection");
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

		public override void Save()
		{
			if (Connection == null)
				throw new Exception("Не передан экземпляр MongoConnection");
			Collection = Connection.GetCollection(collectionName);
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
	}
}
