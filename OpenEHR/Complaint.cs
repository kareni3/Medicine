using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenEHR
{
	public class Complaint : SqlEntity, IEhrObject
	{
		public Problem Problem { get; set; }
		public string Content { get; set; }

		public Complaint()
		{
			TableName = "Complaint";
			Connection = new SqlConnection();
		}

		public Complaint(string content, Problem problem, SqlConnection connection) : this()
		{
			Connection = connection;
			Content = content;
			Problem = problem;
		}

		public override void GetById(int? id)
		{
			SqlCommand command = Connection.GetCommand();
			command.CommandText = "SELECT * FROM Complaint WHERE ComplaintID = @id";
			command.Parameters.AddWithValue("@id", id);
			SqlDataReader reader = command.ExecuteReader();
			if (reader.HasRows)
			{
				reader.Read();
				Id = id;
				Problem = new Problem();
				Problem.GetById(reader.GetInt32(1));
				Content = reader.GetString(2);
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
				command.Parameters.AddWithValue("@problemid", Problem.Id);
				command.Parameters.AddWithValue("@content", Content);
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
				command.Parameters.AddWithValue("@problemid", Problem.Id);
				command.Parameters.AddWithValue("@content", Content);
				command.ExecuteNonQuery();
			}
		}

		public override string ToString()
		{
			return $"Жалоба: {{\n\t{Problem.ToString()},\n\tСодержание: {Content}\n}}";
		}
	}
}
