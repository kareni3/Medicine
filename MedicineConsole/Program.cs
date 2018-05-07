using Medicine;
using OpenEHR;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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

				SqlConnection sqlConnection = new SqlConnection("ELISEY", /*"1433",*/ sqlUser, sqlPassword);
				sqlConnection.Connect();

				Console.Clear();

                #region Заполнение БД
                Doctor doctor = new Doctor("Иванова", "Людмила", "Ивановна", sqlConnection);
                doctor.Save(sqlConnection);

                doctor.SignUp("doctor", "hello", "hello");
                doctor.SignIn("doctor", "hello", sqlConnection);

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

				Article article1 = new Article("Повышенная вязкость крови – один из факторов развития кардиоваскулярных осложнений у больных с истинной полицитемией",
												"https://research-journal.org/medical/povyshennaya-vyazkost-krovi-odin-iz-faktorov-razvitiya-kardiovaskulyarnyx-oslozhnenij-u-bolnyx-s-istinnoj-policitemiej/",
												"Разнообразие клинических проявлений и высокий процент развития осложнений, отличающихся по характеру и тяжести, у больных эритремией продолжает создавать трудности как в лечении, так и в повышении качества жизни больных [6, С. 32]. Изменение реологических свойств и повышение вязкости крови, по результатам наших исследований, оказывают большее влияние на сердечно-сосудистую систему с развитием целого ряда синдромов и симптомов – гипертоническая болезнь, ишемическая болезнь сердца, симптоматическая артериальная гипертензия в 53,4% случаев. Возрастной пик заболеваемости, приходящий на пациентов среднего и пожилого возраста, делает их более уязвимыми в отношении развития тромботических осложнений. В рамках нашего исследования истинной полицитемии более подвержены мужчины  от 40 до 70 лет.", 
												mongoConnection);
				Article article2 = new Article("Центральная гемодинамика, тиреоидный статус и дисфункции эндотелия у больных артериальной гипертонией в условиях высокогорья",
											   "https://research-journal.org/medical/centralnaya-gemodinamika-tireoidnyj-status-i-disfunkcii-endoteliya-u-bolnyx-arterialnoj-gipertoniej-v-usloviyax-vysokogorya/",
											   "Установлено, что снижение продукции NO с одновременным снижением концентрации тиреоидных гормонов вызывают нарушения интракардиальной гемодинамики, изменения структурно-функционального состояния ЛЖ и утяжеляют течение артериальной гипертонии.",
											   mongoConnection);
				Article article3 = new Article("Сочетанные изменения экспрессии генов cstb и acap3 при симптоматической эпилепсии и болезни паркинсона",
											   "https://research-journal.org/medical/sochetannye-izmeneniya-ekspressii-genov-cstb-i-acap3-pri-simptomaticheskoj-epilepsii-i-bolezni-parkinsona/",
											   "Кроме того, у женщин наблюдалось снижение уровня мРНК гена CSTB при эпилепсии(примерно в 3 раза) и при болезни Паркинсона(примерно в 2.5 раза).Полученные данные указывают на возможное участие исследованных генов в патогенезе симптоматической эпилепсии и болезни Паркинсона.",
											   mongoConnection);
				Article article4 = new Article("Синтезируемый эндотелием вазодилататор оксид азота (NO) оказывает существенное влияние на многие фундаментальные процессы в организме человека: стабилизацию венозного и артериального давления, общего периферического сосудистого сопротивления (ОПСС), объема и вязкости циркулирующей крови и распределение крови в сосудах.",
											   "https://research-journal.org/medical/centralnaya-gemodinamika-tireoidnyj-status-i-disfunkcii-endoteliya-u-bolnyx-arterialnoj-gipertoniej-v-usloviyax-vysokogorya/",
											   "Кроме того, у женщин наблюдалось снижение уровня мРНК гена CSTB при эпилепсии(примерно в 3 раза) и при болезни Паркинсона(примерно в 2.5 раза).Полученные данные указывают на возможное участие исследованных генов в патогенезе симптоматической эпилепсии и болезни Паркинсона.",
											   mongoConnection);
				Article article5 = new Article("Полученные данные указывают на возможное участие исследованных генов в патогенезе симптоматической эпилепсии и болезни Паркинсона.",
											   "https://research-journal.org/medical/sochetannye-izmeneniya-ekspressii-genov-cstb-i-acap3-pri-simptomaticheskoj-epilepsii-i-bolezni-parkinsona/",
											   "Кроме того, у женщин наблюдалось снижение уровня мРНК гена CSTB при эпилепсии(примерно в 3 раза) и при болезни Паркинсона(примерно в 2.5 раза).Полученные данные указывают на возможное участие исследованных генов в патогенезе симптоматической эпилепсии и болезни Паркинсона.",
											   mongoConnection);
				Tag tag1 = new Tag("Карапузов", mongoConnection);
				Tag tag2 = new Tag("Эритремия", mongoConnection);
				Tag tag3 = new Tag("Гипертония", mongoConnection);
				Tag tag4 = new Tag("Карапузовдержись", mongoConnection);
				Tag tag5 = new Tag("Гены", mongoConnection);
				Tag tag6 = new Tag("Оксид азота", mongoConnection);

                article1.Save();
                article2.Save();
                article3.Save();
                article4.Save();
                article5.Save();

                tag1.Save();
                tag2.Save();
                tag3.Save();
                tag4.Save();
                tag5.Save();
                tag6.Save();

                Association association1 = new Association("Ассоциация с наибольшим количеством тегов", mongoConnection, sqlConnection);
				association1.Tags.AddRange(new Tag[]
				{
					tag1,
					tag2,
					tag3
				});
                association1.Article = article1;
				association1.MedicineObjects.AddRange(new IEhrObject[]
				{
					patient,
					problem,
					medicament2,
					medicament3
				});
				association1.Save();

				Association association2 = new Association("Ассоциация про Карапузова", mongoConnection, sqlConnection);
				association2.Tags.AddRange(new Tag[]
				{
					tag1,
					tag3
				});
                association2.Article = article2;
				association2.MedicineObjects.AddRange(new IEhrObject[]
				{
					patient,
					medicament
				});
				association2.Save();

				Association association3 = new Association("Ассоциация с наименьшим количеством тегов", mongoConnection, sqlConnection);
				association3.Tags.AddRange(new Tag[]
				{
					tag1,
					tag4
				});
                association3.Article = article3;
				association3.MedicineObjects.AddRange(new IEhrObject[]
				{
					medicament,
					medicament3
				});
				association3.Save();
                #endregion

                #region Поиск по тегам
                Tag findTag1 = new Tag();
                Tag findTag2 = new Tag();
                Tag findTag3 = new Tag();
                Tag findTag4 = new Tag();
                Tag findTag5 = new Tag();

                try
                {
                    Console.Write("Введите тег: ");
                    findTag1.GetByContent(Console.ReadLine(), mongoConnection); //"Карапузов"
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                try
                {
                    Console.Write("Введите тег: ");
                    findTag2.GetByContent(Console.ReadLine(), mongoConnection); //"Эритремия"
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                try
                {
                    Console.Write("Введите тег: ");
                    findTag3.GetByContent(Console.ReadLine(), mongoConnection); //"Гипертония"
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                try
                {
                    Console.Write("Введите тег: ");
                    findTag4.GetByContent(Console.ReadLine(), mongoConnection); //"Грипп"
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.WriteLine();

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                List<KeyValuePair<int, Association>> associations = Association.GetAssociationListByTag(new Tag[]
                {
                    findTag1,
                    findTag2,
                    findTag3,
                    findTag4
                }, sqlConnection, mongoConnection);
                stopwatch.Stop();

                foreach(KeyValuePair<int, Association> association in associations)
                {
                    Console.WriteLine(association.Value);
                    Console.WriteLine("Число совпадений: " + association.Key);
                    Console.WriteLine();
                }
                Console.WriteLine("Затрачено времени: " + stopwatch.Elapsed);
                #endregion
            }
            catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			Console.ReadKey();
		}
	}
}
