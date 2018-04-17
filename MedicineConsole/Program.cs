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
				
				Doctor doctor = new Doctor("Иванова", "Людмила", "Ивановна", connection);
				doctor.Save(connection);

				Patient patient = new Patient("Карапузов", "Иван", "Андреевич");
				patient.Save(connection);

				Problem problem = new Problem("Не могу больше держаться", new DateTime(2018, 4, 17), patient, doctor, connection);
				problem.Save();
				Console.WriteLine(problem);

				Complaint complaint1 = new Complaint("Мне больно", problem, connection);
				complaint1.Save();

				Complaint complaint2 = new Complaint("Очень", problem, connection);
				complaint2.Save();

				Diagnosis diagnosis = new Diagnosis("Простуда", problem, doctor, connection);
				diagnosis.Save();
				Console.WriteLine(diagnosis);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			Console.ReadKey();
		}
	}
}
