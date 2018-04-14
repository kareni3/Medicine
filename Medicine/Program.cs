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
				Doctor doctor = new Doctor("Иванова", "Людмила", "Ивановна", connection);
				doctor.Save();

				Console.WriteLine(doctor._id);
				Console.WriteLine(doctor.Lastname);
				Console.WriteLine(doctor.Firstname);
				Console.WriteLine(doctor.Middlename);

				doctor.GetByName("Иванова", "Людмила", "Ивановна", connection);
				doctor.Middlename = "Петровна";
				doctor.Save();
				
				Console.WriteLine(doctor._id);
				Console.WriteLine(doctor.Lastname);
				Console.WriteLine(doctor.Firstname);
				Console.WriteLine(doctor.Middlename);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			Console.ReadKey();
		}
	}
}
