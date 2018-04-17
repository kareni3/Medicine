using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenEHR
{
	public class Symptom : SqlEntity
	{
		public Diagnosis Diagnosis { get; set; }
		public Archetype Archetype { get; set; }
		public string ArchetypeValue { get; set; }

		public Symptom()
		{
			tableName = "Symptom";
			Connection = new SqlConnection();
		}

		public Symptom(Diagnosis diagnosis, Archetype archetype, string archetypeValue, SqlConnection connection) : this()
		{
			Connection = connection;
			Diagnosis = diagnosis;
			Archetype = archetype;
			ArchetypeValue = archetypeValue;
		}

		public override void GetById(int? id)
		{
			SqlCommand command = Connection.GetCommand();
			command.CommandText = "SELECT * FROM Symptom WHERE SymptomID = @id";
			command.Parameters.AddWithValue("@id", id);
			SqlDataReader reader = command.ExecuteReader();
			if (reader.HasRows)
			{
				reader.Read();
				Id = id;
				Diagnosis = new Diagnosis();
				Diagnosis.GetById(reader.GetInt32(1));
				Archetype = new Archetype();
				Archetype.GetById(reader.GetInt32(2));
				ArchetypeValue = reader.GetString(3);
			}
			reader.Close();
		}

		public override void Save()
		{
			SqlCommand command = Connection.GetCommand();
			if (Id == null)
			{
				command.CommandText = "insert_" + tableName;
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@diagnosisid", Diagnosis.Id);
				command.Parameters.AddWithValue("@archetypeid", Archetype.Id);
				command.Parameters.AddWithValue("@archetypevalue", ArchetypeValue);
				SqlParameter id = command.Parameters.AddWithValue("@id", 0);
				id.Direction = ParameterDirection.Output;
				command.ExecuteNonQuery();
				Id = Convert.ToInt32(id.Value);
			}
			else
			{
				command.CommandText = "update_" + tableName;
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@id", Id);
				command.Parameters.AddWithValue("@diagnosisid", Diagnosis.Id);
				command.Parameters.AddWithValue("@archetypeid", Archetype.Id);
				command.Parameters.AddWithValue("@archetypevalue", ArchetypeValue);
				command.ExecuteNonQuery();
			}
		}

		public override string ToString()
		{
			return $"Симптом: {{\n\t{Diagnosis.ToString()},\n\t{Archetype.ToString()},\n\tЗначение архетипа: {ArchetypeValue}\n}}";
		}
	}
}
