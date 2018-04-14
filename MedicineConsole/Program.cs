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

				Doctor doctor = new Doctor();
				doctor.GetByName("Иванова", "Людмила", "Петровна", connection);

				Patient patient = new Patient();
				patient.GetByName("Карапузов", "Иван", "Иванович", connection);

				Medicament medicament = new Medicament();
				medicament.GetByName("Мезим", connection);

				Tag tag1 = new Tag();
				tag1.GetByContent("Отит", connection);

				Tag tag2 = new Tag();
				tag2.GetByContent("Карапузов", connection);

				Tag tag3 = new Tag();
				tag3.GetByContent("Карапузовдержись", connection);

				Tag tag4 = new Tag();
				tag4.GetByContent("Сложный случай", connection);

				Association association = new Association(doctor, "Карапузов молодец", connection);
				association.Tags.AddRange(new Tag[]
				{
					tag1,
					tag2,
					tag3,
					tag4
				});
				association.MedicineObjects.AddRange(new MedicineObject[]
				{
					article,
					medicament,
					patient
				});

				association.Save();
				Console.WriteLine("Success");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			Console.ReadKey();
		}
	}
}
