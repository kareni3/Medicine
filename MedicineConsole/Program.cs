using Medicine;
using OpenEHR;
using System;

namespace MedicineConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Write("Имя пользователя SQL Server: ");
			string sqlUser = Console.ReadLine();
			Console.Write("Пароль SQL Server: ");
			string sqlPassword = Console.ReadLine();

			Console.Write("Имя пользователя MongoDB: ");
			string mongoUser = Console.ReadLine();
			Console.Write("Пароль MongoDB: ");
			string mongoPassword = Console.ReadLine();
			try
			{
				MongoConnection mongoConnection = new MongoConnection("ds229008.mlab.com", "29008", mongoUser, mongoPassword);
				mongoConnection.Connect();

				SqlConnection sqlConnection = new SqlConnection("tcp:medic.database.windows.net", "1433", sqlUser, sqlPassword);
				sqlConnection.Connect();

				Console.Clear();
				
				Doctor doctor = new Doctor("Иванова", "Людмила", "Ивановна", sqlConnection);
				doctor.Save(sqlConnection);

				Patient patient = new Patient("Карапузов", "Иван", "Андреевич");
				patient.Save(sqlConnection);

				Problem problem = new Problem("Не могу больше держаться", new DateTime(2018, 4, 17), patient, doctor, sqlConnection);
				problem.Save();
				Console.WriteLine(problem);

				Complaint complaint1 = new Complaint("Мне больно", problem, sqlConnection);
				complaint1.Save();

				Complaint complaint2 = new Complaint("Очень", problem, sqlConnection);
				complaint2.Save();

				Diagnosis diagnosis = new Diagnosis("Простуда", problem, doctor, sqlConnection);
				diagnosis.Save();
				Console.WriteLine(diagnosis);

				Archetype pulse = new Archetype("Пульс");
				pulse.Save(sqlConnection);

				Archetype pressure = new Archetype("Давление");
				pressure.Save(sqlConnection);

				Console.WriteLine(pressure);

				Symptom symptom1 = new Symptom(diagnosis, pulse, "60", sqlConnection);
				symptom1.Save();

				Symptom symptom2 = new Symptom(diagnosis, pressure, "120/80", sqlConnection);
				symptom2.Save();

				Console.WriteLine(symptom1);
				Console.WriteLine(symptom2);

				Medicament medicament = new Medicament("Мезим");
				medicament.Save(sqlConnection);

				Medicament medicament2 = new Medicament("Терафлю");
				medicament2.Save(sqlConnection);

				Medicament medicament3 = new Medicament("Нурофен");
				medicament3.Save(sqlConnection);

				Prescription prescription = new Prescription(medicament, diagnosis, sqlConnection);
				prescription.Save();

				Prescription prescription2 = new Prescription(medicament2, diagnosis, sqlConnection);
				prescription2.Save();

				Prescription prescription3 = new Prescription(medicament3, diagnosis, sqlConnection);
				prescription3.Save();

				Console.WriteLine(prescription3);

				Article article1 = new Article(@"Повышенная вязкость крови – один из факторов
										        развития кардиоваскулярных осложнений у больных
												с истинной полицитемией",
												"https://research-journal.org/medical/povyshennaya-vyazkost-krovi-odin-iz-faktorov-razvitiya-kardiovaskulyarnyx-oslozhnenij-u-bolnyx-s-istinnoj-policitemiej/",
												"Разнообразие клинических проявлений и высокий процент развития осложнений, отличающихся по характеру и тяжести, у больных эритремией продолжает создавать трудности как в лечении, так и в повышении качества жизни больных [6, С. 32]. Изменение реологических свойств и повышение вязкости крови, по результатам наших исследований, оказывают большее влияние на сердечно-сосудистую систему с развитием целого ряда синдромов и симптомов – гипертоническая болезнь, ишемическая болезнь сердца, симптоматическая артериальная гипертензия в 53,4% случаев. Возрастной пик заболеваемости, приходящий на пациентов среднего и пожилого возраста, делает их более уязвимыми в отношении развития тромботических осложнений. В рамках нашего исследования истинной полицитемии более подвержены мужчины  от 40 до 70 лет.", 
												mongoConnection);
				Article article2 = new Article(@"Центральная гемодинамика, тиреоидный статус и 
											   дисфункции эндотелия у больных артериальной 
											   гипертонией в условиях высокогорья",
											   "https://research-journal.org/medical/centralnaya-gemodinamika-tireoidnyj-status-i-disfunkcii-endoteliya-u-bolnyx-arterialnoj-gipertoniej-v-usloviyax-vysokogorya/",
											   "Установлено, что снижение продукции NO с одновременным снижением концентрации тиреоидных гормонов вызывают нарушения интракардиальной гемодинамики, изменения структурно-функционального состояния ЛЖ и утяжеляют течение артериальной гипертонии.",
											   mongoConnection);
				Article article3 = new Article(@"Сочетанные изменения экспрессии генов cstb и acap3
											   при симптоматической эпилепсии и болезни паркинсона",
											   "https://research-journal.org/medical/sochetannye-izmeneniya-ekspressii-genov-cstb-i-acap3-pri-simptomaticheskoj-epilepsii-i-bolezni-parkinsona/",
											   "Кроме того, у женщин наблюдалось снижение уровня мРНК гена CSTB при эпилепсии(примерно в 3 раза) и при болезни Паркинсона(примерно в 2.5 раза).Полученные данные указывают на возможное участие исследованных генов в патогенезе симптоматической эпилепсии и болезни Паркинсона.",
											   mongoConnection);
				Tag tag1 = new Tag("Карапузов", mongoConnection);
				Tag tag2 = new Tag("Эритремия", mongoConnection);
				Tag tag3 = new Tag("Гипертония", mongoConnection);
				Tag tag4 = new Tag("Карапузовдержись", mongoConnection);

				Association association1 = new Association("Ассоциация с наибольшим количеством тегов", mongoConnection);
				association1.Tags.AddRange(new Tag[]
				{
					tag1,
					tag2,
					tag3
				});
				association1.MedicineObjects.Add()

				Association association2 = new Association("Ассоциация про Карапузова", mongoConnection);
				association2.Tags.AddRange(new Tag[]
				{
					tag1,
					tag3
				});

				Association association3 = new Association("Ассоциация с наименьшим количеством тегов", mongoConnection);
				association3.Tags.AddRange(new Tag[]
				{
					tag1,
					tag4
				});
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			Console.ReadKey();
		}
	}
}
