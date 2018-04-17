using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenEHR
{
	public class Patient : SqlEntity, IEhrObject
	{
		public string Lastname { get; set; }
		public string Firstname { get; set; }
		public string Middlename { get; set; }

		public Patient()
		{
			TableName = "Patient";
			Connection = new SqlConnection();
		}

		public Patient(string lastname, string firstname, string middlename) : this()
		{
			Lastname = lastname;
			Firstname = firstname;
			Middlename = middlename;
		}

		public override void GetById(int? id)
		{
			SqlCommand command = Connection.GetCommand();
			command.CommandText = "SELECT * FROM Patient WHERE PatientID = @id";
			command.Parameters.AddWithValue("@id", id);
			SqlDataReader reader = command.ExecuteReader();
			if (reader.HasRows)
			{
				reader.Read();
				Id = id;
				Lastname = reader.GetString(1);
				Firstname = reader.GetString(2);
				Middlename = reader.GetString(3);
			}
			reader.Close();
		}

		public void GetByName(string lastname, string firstname, string middlename, SqlConnection connection)
		{
			Connection = connection;
			SqlCommand command = Connection.GetCommand();
			command.CommandText = "SELECT * FROM Patient WHERE Lastname = @lastname AND Firstname = @firstname AND Middlename = @middlename";
			command.Parameters.AddWithValue("@lastname", lastname);
			command.Parameters.AddWithValue("@firstname", firstname);
			command.Parameters.AddWithValue("@middlename", middlename);
			SqlDataReader reader = command.ExecuteReader();
			if (reader.HasRows)
			{
				reader.Read();
				Id = reader.GetInt32(0);
				Lastname = lastname;
				Firstname = firstname;
				Middlename = middlename;
			}
			reader.Close();
		}

		public override void Save()
		{
			SqlCommand command = Connection.GetCommand();
			if (Id == null)
			{
				command.CommandText = "insert_" + TableName;
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@lastname", Lastname);
				command.Parameters.AddWithValue("@firstname", Firstname);
				command.Parameters.AddWithValue("@middlename", Middlename);
				SqlParameter id = command.Parameters.AddWithValue("@id", 0);
				id.Direction = ParameterDirection.Output;
				command.ExecuteNonQuery();
				Id = Convert.ToInt32(id.Value);
			}
			else
			{
				command.CommandText = "update_" + TableName;
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@id", Id);
				command.Parameters.AddWithValue("@lastname", Lastname);
				command.Parameters.AddWithValue("@firstname", Firstname);
				command.Parameters.AddWithValue("@middlename", Middlename);
				command.ExecuteNonQuery();
			}
		}

		public override string ToString()
		{
			return $"Пациент: {{\n\tФамилия: {Lastname},\n\tИмя: {Firstname},\n\tОтчество: {Middlename}\n}}";
		}
	}
}
