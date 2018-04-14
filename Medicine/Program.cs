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
			string password = Console.ReadLine();
			try
			{
				MongoConnection connection = new MongoConnection("ds229008.mlab.com", "29008", user, password);
				Console.Clear();
				connection.Connect();
				var collection = connection.GetCollection(Collection.Tag);
				var document = collection.Find(new BsonDocument()).FirstOrDefault();
				Console.WriteLine(document.ToString());
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			Console.ReadKey();
		}
	}
}
