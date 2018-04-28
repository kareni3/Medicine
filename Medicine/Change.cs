using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine
{
	public class Change
	{
		public DateTime ChangeTime { get; set; }
		public string Content { get; set; }

		public Change()
		{
            ChangeTime = DateTime.Now;
            Content = "Ассоциация создана";
		}

        public override string ToString()
        {
            return $"Изменение: {{ Время изменения: {ChangeTime}, Содержание: \"{Content}\" }}";
        }
    }
}
