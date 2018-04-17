﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenEHR
{
	public class Medicament : SqlEntity, IEhrObject
	{
		public string Name { get; set; }

		public Medicament()
		{
			TableName = "Medicament";
			Connection = new SqlConnection();
		}

		public Medicament(string name) : this()
		{
			Name = name;
		}

		public override void GetById(int? id)
		{
			SqlCommand command = Connection.GetCommand();
			command.CommandText = "SELECT * FROM Medicament WHERE MedicamentID = @id";
			command.Parameters.AddWithValue("@id", id);
			SqlDataReader reader = command.ExecuteReader();
			if(reader.HasRows)
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
			command.CommandText = "SELECT * FROM Medicament WHERE Name = @name";
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
				command.CommandText = "insert_" + TableName;
				command.Parameters.AddWithValue("@name", Name);
				SqlParameter id = command.Parameters.AddWithValue("@id", 0);
				id.Direction = ParameterDirection.Output;
				command.ExecuteNonQuery();
				Id = Convert.ToInt32(id.Value);
			}
			else
			{
				command.CommandText = "update_" + TableName;
				command.Parameters.AddWithValue("@id", Id);
				command.Parameters.AddWithValue("@name", Name);
				command.ExecuteNonQuery();
			}
		}

		public override string ToString()
		{
			return $"Лекарство: {{\n\tНазвание: {Name}\n}}";
		}
	}
}
