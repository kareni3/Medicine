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

				Archetype pulse = new Archetype("Пульс");
				pulse.Save(connection);

				Archetype pressure = new Archetype("Давление");
				pressure.Save(connection);

				Console.WriteLine(pressure);

				Symptom symptom1 = new Symptom(diagnosis, pulse, "60", connection);
				symptom1.Save();

				Symptom symptom2 = new Symptom(diagnosis, pressure, "120/80", connection);
				symptom2.Save();

				Console.WriteLine(symptom1);
				Console.WriteLine(symptom2);

				Medicament medicament = new Medicament("Мезим");
				medicament.Save(connection);

				Medicament medicament2 = new Medicament("Терафлю");
				medicament2.Save(connection);

				Medicament medicament3 = new Medicament("Нурофен");
				medicament3.Save(connection);

				Prescription prescription = new Prescription(medicament, diagnosis, connection);
				prescription.Save();

				Prescription prescription2 = new Prescription(medicament2, diagnosis, connection);
				prescription2.Save();

				Prescription prescription3 = new Prescription(medicament3, diagnosis, connection);
				prescription3.Save();

				Console.WriteLine(prescription3);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			Console.ReadKey();
		}
	}
}
