using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine
{
	class Article : MedicineObject
	{
		public string Name { get; set; }
		public string Link { get; set; }
		public string Extract { get; set; }
	}
}
