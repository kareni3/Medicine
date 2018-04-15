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
		protected IMongoCollection<BsonDocument> collection { get; set; }
		public string CollectionName { get; protected set; }
		public ObjectId _id { get; protected set; }
	}
}
