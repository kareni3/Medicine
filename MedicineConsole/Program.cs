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

				Doctor doctorUser = new Doctor("Синкевич", "Елисей", "Вячеславович", connection);
				doctorUser.SignUp("selisej", "123456", "123456");

				Doctor selisej = new Doctor();
				if (selisej.SignIn("selisej", "123456", connection))
					Console.WriteLine("Вход выполнен успешно");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			Console.ReadKey();
		}
	}
}
