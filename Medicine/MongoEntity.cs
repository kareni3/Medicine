using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine
{
	public abstract class MongoEntity
	{
		internal IMongoCollection<BsonDocument> Collection { get; set; }
		public ObjectId _id { get; protected set; }
		public MongoConnection Connection { get; set; }
		protected string collectionName;
	}
}
