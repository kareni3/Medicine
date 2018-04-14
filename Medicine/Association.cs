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
		public List<Tag> TagList { get; set; }
		public Doctor Doctor { get; set; }
		public List<MedicineObject> MedicineObjectList { get; set; }
		public List<Change> ChangeList { get; set; }
	}
}
