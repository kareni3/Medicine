using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenEHR
{
	public class Problem : SqlEntity
	{
		public string Content { get; set; }
		public DateTime ProblemDate { get; set; }
		public Patient Patient { get; set; }
		public Doctor Doctor { get; set; }

		public Problem()
		{
			tableName = "Problem";
			Connection = new SqlConnection();
		}

		public Problem(string content, DateTime problemDate, Patient patient, Doctor doctor, SqlConnection connection) : this()
		{
			Connection = connection;
			Content = content;
			ProblemDate = problemDate;
			Patient = patient;
			Doctor = doctor;
		}

		public override void GetById(int? id)
		{
			SqlCommand command = Connection.GetCommand();
			command.CommandText = "SELECT * FROM Problem WHERE ProblemID = @id";
			command.Parameters.AddWithValue("@id", id);
			SqlDataReader reader = command.ExecuteReader();
			if (reader.HasRows)
			{
				reader.Read();
				Id = id;
				Patient = new Patient();
				Patient.GetById(reader.GetInt32(1));
				Doctor = new Doctor();
				Doctor.GetById(reader.GetInt32(2));
				Content = reader.GetString(3);
				ProblemDate = reader.GetDateTime(4);
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
				command.Parameters.AddWithValue("@patientid", Patient.Id);
				command.Parameters.AddWithValue("@doctorid", Doctor.Id);
				command.Parameters.AddWithValue("@content", Content);
				command.Parameters.AddWithValue("@problemdate", ProblemDate);
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
				command.Parameters.AddWithValue("@patientid", Patient.Id);
				command.Parameters.AddWithValue("@doctorid", Doctor.Id);
				command.Parameters.AddWithValue("@content", Content);
				command.Parameters.AddWithValue("@problemdate", ProblemDate);
				command.ExecuteNonQuery();
			}
		}

		public override string ToString()
		{
			return $"Проблема: {{\n\t{Patient.ToString()},\n\t{Doctor.ToString()},\n\tСодержание: {Content},\n\tДата: {ProblemDate}\n}}";
		}
	}
}
