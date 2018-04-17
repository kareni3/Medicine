using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenEHR
{
	public abstract class SqlEntity
	{
		public int? Id { get; protected set; }
		public SqlConnection Connection { get; set; }
		public string TableName;

		public void GetById(int? id, SqlConnection connection)
		{
			Connection = connection;
			GetById(id);
		}
		public abstract void GetById(int? id);
		public void Save(SqlConnection connection)
		{
			Connection = connection;
			Save();
		}
		public abstract void Save();
	}
}
