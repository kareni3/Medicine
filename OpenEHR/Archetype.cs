using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenEHR
{
	class Archetype : SqlEntity
	{
		public string Name { get; set; }

		public Archetype()
		{
			tableName = "Archetype";
			Connection = new SqlConnection();
		}

		public Archetype(string name) : this()
		{
			Name = name;
		}

		public override void GetById(int? id)
		{
			SqlCommand command = Connection.GetCommand();
			command.CommandText = "SELECT * FROM Archetype WHERE ArchetypeID = @id";
			command.Parameters.AddWithValue("@id", id);
			SqlDataReader reader = command.ExecuteReader();
			if (reader.HasRows)
			{
				reader.Read();
				Id = id;
				Name = reader.GetString(1);
			}
			reader.Close();
		}

		public void GetByName(string name, SqlConnection connection)
		{
			Connection = connection;
			SqlCommand command = connection.GetCommand();
			command.CommandText = "SELECT * FROM Archetype WHERE Name = @name";
			command.Parameters.AddWithValue("@name", name);
			SqlDataReader reader = command.ExecuteReader();
			if (reader.HasRows)
			{
				reader.Read();
				Id = reader.GetInt32(0);
				Name = name;
			}
			reader.Close();
		}

		public override void Save()
		{
			SqlCommand command = Connection.GetCommand();
			command.CommandType = CommandType.StoredProcedure;
			if (Id == null)
			{
				command.CommandText = "insert_" + tableName;
				command.Parameters.AddWithValue("@name", Name);
				SqlParameter id = command.Parameters.AddWithValue("@id", 0);
				id.Direction = ParameterDirection.Output;
				command.ExecuteNonQuery();
				Id = Convert.ToInt32(id.Value);
			}
			else
			{
				command.CommandText = "update_" + tableName;
				command.Parameters.AddWithValue("@id", Id);
				command.Parameters.AddWithValue("@name", Name);
				command.ExecuteNonQuery();
			}
		}

		public override string ToString()
		{
			return $"Архетип: {{\n\tНазвание: {Name}\n}}";
		}
	}
}
