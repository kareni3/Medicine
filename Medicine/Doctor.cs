using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine
{
	class Doctor : MedicineObject
	{
		public ObjectId _id { get; }
		public string Lastname { get; set; }
		public string Firstname { get; set; }
		public string Middlename { get; set; }
	}
}
