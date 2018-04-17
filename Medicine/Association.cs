using MongoDB.Bson;
using MongoDB.Driver;
using OpenEHR;
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
		private List<BsonDocument> medicineObjects;
		public List<Change> Changes { get; private set; }

		SqlConnection sqlConnection;
		
		public Association(SqlConnection connection)
		{
			sqlConnection = connection;
			collectionName = "Association";
			Tags = new List<Tag>();
			medicineObjects = new List<BsonDocument>();
			Changes = new List<Change>();
			Doctor = sqlConnection.CurrentDoctor;
		}

		public void AddMedicineObject(object medicineObject)
		{
			if (medicineObject is IMedicineObject)
			{
				var document = new BsonDocument
				{
					{"db", "medicine" },
					{ "ref", new MongoDBRef((medicineObject as MongoEntity).Collection.CollectionNamespace.CollectionName, (medicineObject as MongoEntity)._id).ToBsonDocument() }
				};
			}
			if (medicineObject is IEhrObject)
			{
				var document = new BsonDocument
				{
					{"db", "openehr" },
					{ "ref", new BsonDocument
					{
						{"table", (medicineObject as SqlEntity).TableName },
						{ "id", (medicineObject as SqlEntity).Id }
					} }
				};
			}
		}

		public Association(string description, MongoConnection connection, SqlConnection sqlConnection) : this(sqlConnection)
		{
			Description = description;
			Connection = connection;
		}

		public override void Save()
		{
			Collection = Connection.GetCollection(collectionName);
			if (_id.CompareTo(new ObjectId()) == 0)
			{
				var document = new BsonDocument()
				{
					{ "Description", Description },
					{ "Doctor", Doctor.Id },
					{ "Tags", new BsonArray() },
					{ "MedicineObjects", new BsonArray() },
					{ "Changes", new BsonArray() }
				};
				foreach(Tag tag in Tags)
				{
					document.GetElement("Tags").Value.AsBsonArray.Add(new MongoDBRef("Tag", tag._id).ToBsonDocument());
				}
				foreach(IMedicineObject medicineObject in medicineObjects)
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
				//
			}
		}

		public override void GetById(ObjectId id)
		{
			Collection = Connection.GetCollection(collectionName);
			var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
			var document = Collection.Find(filter).First();
			_id = id;

			Description = document.GetValue("Description").AsString;

			Doctor = new Doctor();
			Doctor.GetById(document.GetValue("Doctor").AsInt32, sqlConnection);

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
					/*case "Doctor":
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
						break;*/
					case "Article":
						Article article = new Article();
						article.GetById(doc.GetValue("$id").AsObjectId, Connection);
						AddMedicineObject(article);
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
	}
}
