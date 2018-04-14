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
				Article article = new Article();
				article.GetByName("Дивертикулярная болезнь толстой кишки в практике участкового терапевта", connection);
				Console.WriteLine(article._id);
				Console.WriteLine(article.Name);
				Console.WriteLine(article.Link);
				Console.WriteLine(article.Extract);
				article.Link = article.Link.Remove(article.Link.Length - 1);
				article.Save();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			Console.ReadKey();
		}
	}
}
