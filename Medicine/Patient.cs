using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine
{
	class Patient : MedicineObject
	{
		public string Lastname { get; set; }
		public string Firstname { get; set; }
		public string Middlename { get; set; }
	}
}
