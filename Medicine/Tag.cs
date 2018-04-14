using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine
{
	class Tag
	{
		public ObjectId _id { get; }
		string Content { get; set; }

		public void Save()
		{

		}
	}
}
