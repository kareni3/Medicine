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
				Medicament medicament = new Medicament("Терафлю", connection);
				medicament.Save();
				Console.WriteLine(medicament._id);
				Console.WriteLine(medicament.Name);
				medicament.Name = "Мезим";
				medicament.Save();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			Console.ReadKey();
		}
	}
}
