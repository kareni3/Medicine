using MongoDB.Bson;
using MongoDB.Driver;
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
				Patient patient = new Patient("Карапузов", "Иван", "Андреевич", connection);
				patient.Save();

				Console.WriteLine(patient._id);
				Console.WriteLine(patient.Lastname);
				Console.WriteLine(patient.Firstname);
				Console.WriteLine(patient.Middlename);

				patient.GetByName("Карапузов", "Иван", "Андреевич", connection);
				patient.Middlename = "Михайлович";
				patient.Save();
				
				Console.WriteLine(patient._id);
				Console.WriteLine(patient.Lastname);
				Console.WriteLine(patient.Firstname);
				Console.WriteLine(patient.Middlename);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			Console.ReadKey();
		}
	}
}
