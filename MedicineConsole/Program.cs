using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Write("Имя пользователя: ");
			string user = Console.ReadLine();
			Console.Write("Пароль: ");
			try
			{
				MongoConnection connection = new MongoConnection("ds229008.mlab.com", "29008", user, Console.ReadLine());
				connection.Connect();
				Console.Clear();

				Association association = new Association();
				association.GetById("5ad286a65f734429d4f80dc7", connection);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			Console.ReadKey();
		}
	}
}
