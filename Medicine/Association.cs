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
	public class Association : MongoEntity
	{
		public string Description { get; set; }
		public List<Tag> Tags { get; set; }
		public Doctor Doctor { get; set; }
		public List<IMedicineObject> MedicineObjects { get; set; }
		public List<Change> Changes { get; private set; }
		
		

		public Association()
		{
			collectionName = "Association";
			Tags = new List<Tag>();
			MedicineObjects = new List<IMedicineObject>();
			Changes = new List<Change>();
		}

		public Association(Doctor doctor, string description, MongoConnection connection) : this()
		{
			Doctor = doctor;
			Description = description;
			Connection = connection;
		}

		public void Save(MongoConnection connection)
		{
			Collection = connection.GetCollection(collectionName);
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
				foreach(IMedicineObject medicineObject in MedicineObjects)
				{
					document.GetElement("MedicineObjects").Value.AsBsonArray.Add(new MongoDBRef((medicineObject as MongoEntity).Collection.CollectionNamespace.CollectionName, (medicineObject as MongoEntity)._id).ToBsonDocument());
				}
				Change change = new Change();
				document.GetElement("Changes").Value.AsBsonArray.Add(new BsonDocument
				{
					{ "ChangeTime", change.ChangeTime },
					{ "Content", change.Content }
				});
				Collection.InsertOne(document);
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

		public void GetById(string id, MongoConnection connection)
		{
			Connection = connection;
			GetById(new ObjectId(id));
		}

		private void GetById(ObjectId id, MongoConnection connection)
		{
			Connection = connection;
			GetById(id);
		}

		public void GetById(ObjectId id)
		{
			if (Connection == null)
				throw new Exception("Передайте экземпляр MongoConnection");
			Collection = Connection.GetCollection(collectionName);
			var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
			var document = Collection.Find(filter).First();
			_id = id;

			Description = document.GetValue("Description").AsString;

			Doctor = new Doctor();
			Doctor.GetById(document.GetValue("Doctor").AsBsonDocument.GetValue("$id").AsObjectId, Connection);

			foreach (BsonDocument doc in document.GetValue("Tags").AsBsonArray)
			{
				Tag tag = new Tag();
				tag.GetById(doc.GetValue("$id").AsObjectId, Connection);
				Tags.Add(tag);
			}

			foreach (BsonDocument doc in document.GetValue("MedicineObjects").AsBsonArray)
			{
				switch (doc.GetValue("$ref").AsString)
				{
					case "Doctor":
						Doctor doctor = new Doctor();
						doctor.GetById(doc.GetValue("$id").AsObjectId, Connection);
						MedicineObjects.Add(doctor);
						break;
					case "Medicament":
						Medicament medicament = new Medicament();
						medicament.GetById(doc.GetValue("$id").AsObjectId, Connection);
						MedicineObjects.Add(medicament);
						break;
					case "Patient":
						Patient patient = new Patient();
						patient.GetById(doc.GetValue("$id").AsObjectId, Connection);
						MedicineObjects.Add(patient);
						break;
					case "Article":
						Article article = new Article();
						article.GetById(doc.GetValue("$id").AsObjectId, Connection);
						MedicineObjects.Add(article);
						break;
				}
			}

			foreach (BsonDocument doc in document.GetValue("Changes").AsBsonArray)
			{
				Change change = new Change();
				change.ChangeTime = doc.GetValue("ChangeTime").ToUniversalTime();
				change.Content = doc.GetValue("Content").AsString;
				Changes.Add(change);
			}
		}

		public void Save()
		{
			Save(Connection);
		}
	}
}
