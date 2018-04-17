using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenEHR
{
	public class Diagnosis : SqlEntity
	{
		public string Content { get; set; }
		public Problem Problem { get; set; }
		public Doctor Doctor { get; set; }

		public Diagnosis()
		{
			tableName = "Diagnosis";
			Connection = new SqlConnection();
		}

		public Diagnosis(string content, Problem problem, Doctor doctor, SqlConnection connection) : this()
		{
			Connection = connection;
			Content = content;
			Problem = problem;
			Doctor = doctor;
		}

		public override void GetById(int? id)
		{
			SqlCommand command = Connection.GetCommand();
			command.CommandText = "SELECT * FROM Diagnosis WHERE DiagnosisID = @id";
			command.Parameters.AddWithValue("@id", id);
			SqlDataReader reader = command.ExecuteReader();
			if (reader.HasRows)
			{
				reader.Read();
				Id = id;
				Problem = new Problem();
				Problem.GetById(reader.GetInt32(1));
				Doctor = new Doctor();
				Doctor.GetById(reader.GetInt32(2));
				Content = reader.GetString(3);
			}
			reader.Close();
		}

		public override void Save()
		{
			SqlCommand command = Connection.GetCommand();
			if (Id == null)
			{
				command.CommandText = "insert_" + tableName;
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@problemid", Problem.Id);
				command.Parameters.AddWithValue("@doctorid", Doctor.Id);
				command.Parameters.AddWithValue("@content", Content);
				SqlParameter id = command.Parameters.AddWithValue("@id", 0);
				id.Direction = ParameterDirection.Output;
				command.ExecuteNonQuery();
				Id = Convert.ToInt32(id.Value);
			}
			else
			{
				command.CommandText = "update_" + tableName;
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@id", Id);
				command.Parameters.AddWithValue("@problemid", Problem.Id);
				command.Parameters.AddWithValue("@doctorid", Doctor.Id);
				command.Parameters.AddWithValue("@content", Content);
				command.ExecuteNonQuery();
			}
		}

		public override string ToString()
		{
			return $"Диагноз: {{\n\t{Problem.ToString()},\n\t{Doctor.ToString()},\n\tСодержание: {Content}\n}}";
		}
	}
}
