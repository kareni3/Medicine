using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenEHR
{
	public class Doctor : SqlEntity, IEhrObject
	{
		public string Lastname { get; set; }
		public string Firstname { get; set; }
		public string Middlename { get; set; }
		public string Username { get; set; }
		private string password;

		public Doctor()
		{
			TableName = "Doctor";
			Connection = new SqlConnection();

			Username = "";
			password = "";
		}

		public Doctor(string lastname, string firstname, string middlename, SqlConnection connection) : this()
		{
			Lastname = lastname;
			Firstname = firstname;
			Middlename = middlename;
			Connection = connection;
		}

		public void SignUp(string username, string password, string confirm)
		{
			if (password == confirm)
			{
				Username = username;
				this.password = password;
				Save();
			}
			else
				throw new Exception("Пароли не совпадают");
		}

		public void ChangePassword(string oldPassword, string newPassword, string confirm)
		{
			if (oldPassword == password && newPassword == confirm)
			{
				password = newPassword;
				Save();
			}
			else
				throw new Exception("Вы ввели неверный пароль");
		}

		public bool SignIn(string username, string password, SqlConnection connection)
		{
			Connection = connection;
			SqlCommand command = Connection.GetCommand();
			command.CommandText = "sign_in";
			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.AddWithValue("@username", username);
			command.Parameters.AddWithValue("@password", password);
			SqlParameter id = command.Parameters.AddWithValue("@id", 0);
			id.Direction = ParameterDirection.Output;
			command.ExecuteNonQuery();
			Id = Convert.ToInt32(id.Value);
			if (Id == 0)
			{
				Id = null;
				return false;
			}
			else
			{
				GetById(Id);
				connection.CurrentDoctor = this;
				return true;
			}
		}

		public override void GetById(int? id)
		{
			SqlCommand command = Connection.GetCommand();
			command.CommandText = "SELECT * FROM Doctor WHERE DoctorID = @id";
			command.Parameters.AddWithValue("@id", id);
			SqlDataReader reader = command.ExecuteReader();
			if (reader.HasRows)
			{
				reader.Read();
				Id = id;
				Lastname = reader.GetString(1);
				Firstname = reader.GetString(2);
				Middlename = reader.GetString(3);
				Username = reader.GetString(4);
				password = reader.GetString(5);
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
				if(String.IsNullOrWhiteSpace(Username))
				{
					command.Parameters.AddWithValue("@username", DBNull.Value);
					command.Parameters.AddWithValue("@password", DBNull.Value);
				}
				else
				{
					command.Parameters.AddWithValue("@username", Username);
					command.Parameters.AddWithValue("@password", password);
				}
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
				command.Parameters.AddWithValue("@username", Username);
				command.ExecuteNonQuery();
			}
		}

		public override string ToString()
		{
			return $"Врач: {{\n\tФамилия: {Lastname},\n\tИмя: {Firstname},\n\tОтчество: {Middlename}\n}}";
		}
	}
}
