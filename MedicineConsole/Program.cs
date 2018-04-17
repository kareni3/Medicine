using Medicine;
using OpenEHR;
using System;

namespace MedicineConsole
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
				SqlConnection connection = new SqlConnection("tcp:medic.database.windows.net", "1433", user, Console.ReadLine());
				connection.Connect();
				Console.Clear();
				
				Patient patient = new Patient("Иванова", "Людмила", "Ивановна");
				patient.Save(connection);

				Patient patient2 = new Patient();
				patient2.GetById(patient.Id, connection);
				patient2.Lastname = "Петрова";
				patient2.Save();

				Patient patient3 = new Patient();
				patient3.GetByName("Петрова", "Людмила", "Ивановна", connection);
				Console.WriteLine(patient3);

				Medicament medicament = new Medicament("Терафлю");
				medicament.Save(connection);

				Medicament medicament2 = new Medicament();
				medicament2.GetById(medicament.Id, connection);
				medicament2.Name = "Нурофен";
				medicament2.Save();
				
				Medicament medicament3 = new Medicament();
				medicament3.GetByName("Нурофен", connection);
				Console.WriteLine(medicament3);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			Console.ReadKey();
		}
	}
}
