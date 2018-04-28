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
        public Article Article { get; set; }
		public List<IEhrObject> MedicineObjects { get; set; }
		public List<Change> Changes { get; private set; }

		SqlConnection sqlConnection;
		
		public Association(SqlConnection connection)
		{
			sqlConnection = connection;
			collectionName = "Association";
			Tags = new List<Tag>();
			MedicineObjects = new List<IEhrObject>();
			Changes = new List<Change>();
			Doctor = sqlConnection.CurrentDoctor;
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
                    { "Article", new MongoDBRef("Article", Article._id).ToBsonDocument() },
					{ "Tags", new BsonArray() },
					{ "MedicineObjects", new BsonArray() },
					{ "Changes", new BsonArray() }
				};
				foreach(Tag tag in Tags)
				{
					document.GetElement("Tags").Value.AsBsonArray.Add(new MongoDBRef("Tag", tag._id).ToBsonDocument());
				}
				foreach(IEhrObject medicineObject in MedicineObjects)
				{
                    document.GetElement("MedicineObjects").Value.AsBsonArray.Add(new BsonDocument
                    {
                        { "Table", (medicineObject as SqlEntity).TableName },
                        { "Id", (medicineObject as SqlEntity).Id }
                    });
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
            
            Article = new Article();
            Article.GetById(document.GetValue("Article").AsBsonDocument.GetValue("$id").AsObjectId, Connection);

            foreach (BsonDocument doc in document.GetValue("Tags").AsBsonArray)
			{
				Tag tag = new Tag();
				tag.GetById(doc.GetValue("$id").AsObjectId, Connection);
				Tags.Add(tag);
			}

			foreach (BsonDocument doc in document.GetValue("MedicineObjects").AsBsonArray)
			{
				switch (doc.GetValue("Table").AsString)
				{
                    case "Complaint":
                        Complaint complaint = new Complaint();
                        complaint.GetById(doc.GetValue("Id").AsInt32, sqlConnection);
                        MedicineObjects.Add(complaint);
                        break;
                    case "Diagnosis":
                        Diagnosis diagnosis = new Diagnosis();
                        diagnosis.GetById(doc.GetValue("Id").AsInt32, sqlConnection);
                        MedicineObjects.Add(diagnosis);
                        break;
                    case "Doctor":
						Doctor doctor = new Doctor();
						doctor.GetById(doc.GetValue("Id").AsInt32, sqlConnection);
						MedicineObjects.Add(doctor);
						break;
					case "Medicament":
						Medicament medicament = new Medicament();
						medicament.GetById(doc.GetValue("Id").AsInt32, sqlConnection);
						MedicineObjects.Add(medicament);
						break;
					case "Patient":
						Patient patient = new Patient();
						patient.GetById(doc.GetValue("Id").AsInt32, sqlConnection);
                        MedicineObjects.Add(patient);
						break;
                    case "Problem":
                        Problem problem = new Problem();
                        problem.GetById(doc.GetValue("Id").AsInt32, sqlConnection);
                        MedicineObjects.Add(problem);
                        break;
                    case "Symptom":
                        Symptom symptom = new Symptom();
                        symptom.GetById(doc.GetValue("Id").AsInt32, sqlConnection);
                        MedicineObjects.Add(symptom);
                        break;
				}
			}

			foreach (BsonDocument doc in document.GetValue("Changes").AsBsonArray)
			{
				Change change = new Change()
				{
					ChangeTime = doc.GetValue("ChangeTime").ToUniversalTime(),
					Content = doc.GetValue("Content").AsString
				};
				Changes.Add(change);
			}
		}

        public static List<KeyValuePair<int, Association>> GetAssociationListByTag(IEnumerable<Tag> findTagList, SqlConnection sqlConnection, MongoConnection mongoConnection)
        {
            List<KeyValuePair<int, Association>> result = new List<KeyValuePair<int, Association>>();
            var collection = mongoConnection.GetCollection("Association");
            var documents = collection.Find(new BsonDocument()).ToList();
            foreach(BsonDocument document in documents)
            {
                int counter = 0;
                foreach (BsonDocument tag in document.GetValue("Tags").AsBsonArray)
                {
                    foreach(Tag findTag in findTagList)
                    {
                        if (findTag._id.CompareTo(tag.GetValue("$id").AsObjectId) == 0)
                        {
                            counter++;
                            break;
                        }
                    }
                }
                Association association = new Association(sqlConnection);
                association.GetById(document.GetValue("_id").AsObjectId, mongoConnection);
                result.Add(new KeyValuePair<int, Association>(counter, association));
            }
            return result;
        }

        public override string ToString()
        {
            string result = "Ассоциация: { ";
            result += $"Описание: \"{Description}\", ";
            result += $"{Doctor.ToString()}, ";
            result += $"{Article.ToString()}, ";
            result += "Теги: { ";
            foreach(Tag tag in Tags)
            {
                result += $"{tag}, ";
            }
            result = result.Remove(result.Length - 2);
            result += " }, ";

            result += "Медицинские объекты: { ";
            foreach (IEhrObject medicineObject in MedicineObjects)
            {
                result += $"{medicineObject.ToString()}, ";
            }
            result = result.Remove(result.Length - 2);
            result += " }, ";

            result += "Изменения: { ";
            foreach (Change change in Changes)
            {
                result += $"{change.ToString()}, ";
            }
            result = result.Remove(result.Length - 2);
            result += " } }";
            return result;
        }
    }
}
