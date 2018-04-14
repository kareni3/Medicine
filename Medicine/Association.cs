using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine
{
	class Association
	{
		public ObjectId _id { get; private set; }
		public string Description { get; set; }
		public List<Tag> Tags { get; set; }
		public Doctor Doctor { get; set; }
		public List<MedicineObject> MedicineObjects { get; set; }
		public List<Change> Changes { get; private set; }

		public MongoConnection Connection { get; set; }
		private IMongoCollection<BsonDocument> collection;

		public Association()
		{
			Tags = new List<Tag>();
			MedicineObjects = new List<MedicineObject>();
			Changes = new List<Change>();
		}

		public Association(Doctor doctor, string description, MongoConnection connection)
		{
			Tags = new List<Tag>();
			MedicineObjects = new List<MedicineObject>();
			Changes = new List<Change>();

			Doctor = doctor;
			Description = description;
			Connection = connection;
		}

		public void Save(MongoConnection connection)
		{
			collection = connection.GetCollection(Collection.Association);
			if (_id.CompareTo(new ObjectId()) == 0)
			{
				var document = new BsonDocument()
				{
					{ "Description", Description },
					{ "Doctor", new MongoDBRef("Doctor", Doctor._id).ToBsonDocument() },
					{ "Tags", new BsonArray() },
					{ "MedicineObjects", new BsonArray() },
					{ "Changes", new BsonArray() }
				};
				foreach(Tag tag in Tags)
				{
					document.GetElement("Tags").Value.AsBsonArray.Add(new MongoDBRef("Tag", tag._id).ToBsonDocument());
				}
				foreach(MedicineObject medicineObject in MedicineObjects)
				{
					if(medicineObject is Doctor)
						document.GetElement("MedicineObjects").Value.AsBsonArray.Add(new MongoDBRef("Doctor", (medicineObject as Doctor)._id).ToBsonDocument());
					if (medicineObject is Article)
						document.GetElement("MedicineObjects").Value.AsBsonArray.Add(new MongoDBRef("Article", (medicineObject as Article)._id).ToBsonDocument());
					if (medicineObject is Medicament)
						document.GetElement("MedicineObjects").Value.AsBsonArray.Add(new MongoDBRef("Medicament", (medicineObject as Medicament)._id).ToBsonDocument());
					if (medicineObject is Patient)
						document.GetElement("MedicineObjects").Value.AsBsonArray.Add(new MongoDBRef("Patient", (medicineObject as Patient)._id).ToBsonDocument());
				}
				Change change = new Change();
				document.GetElement("Changes").Value.AsBsonArray.Add(new BsonDocument
				{
					{ "ChangeTime", change.ChangeTime },
					{ "Content", change.Content }
				});
				collection.InsertOne(document);
				_id = document.GetValue("_id").AsObjectId;
			}
			else
			{
				var filter = Builders<BsonDocument>.Filter.Eq("_id", _id);
				/*var update = Builders<BsonDocument>.Update.Combine(
					Builders<BsonDocument>.Update.Set("Name", Name),
					Builders<BsonDocument>.Update.Set("Link", Link),
					Builders<BsonDocument>.Update.Set("Extract", Extract)
				);
				collection.UpdateOne(filter, update);*/
			}
		}

		public void Save()
		{
			Save(Connection);
		}
	}
}
