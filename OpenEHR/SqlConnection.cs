using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenEHR
{
	public class SqlConnection
	{
		System.Data.SqlClient.SqlConnection Connection;
		public string Server { get; set; }
		public string Port { get; set; }
		public string User { get; set; }
		public string Password { private get; set; }
		public Doctor CurrentDoctor { get; internal set; }

		private bool connected;

		public SqlConnection()
		{
			connected = false;
		}

		public SqlConnection(string server, string port, string user, string password) : this()
		{
			Server = server;
			Port = port;
			User = user;
			Password = password;
		}

		public void Connect()
		{
			try
			{
				Connection = new System.Data.SqlClient.SqlConnection(String.Format(ConfigurationManager.ConnectionStrings["openehr"].ConnectionString, Server, Port, User, Password));
				Connection.Open();
				connected = true;
			}
			catch
			{
				connected = false;
				throw;
			}
		}

		internal System.Data.SqlClient.SqlCommand GetCommand()
		{
			if (!connected)
				throw new Exception("Не передан экземпляр SqlConnection");
			return new System.Data.SqlClient.SqlCommand() { Connection = Connection };
		}
	}
}
