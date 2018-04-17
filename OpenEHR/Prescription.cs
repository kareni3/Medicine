﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenEHR
{
	public class Prescription : SqlEntity
	{
		public Medicament Medicament { get; set; }
		public Diagnosis Diagnosis { get; set; }

		public Prescription()
		{
			tableName = "Prescription";
			Connection = new SqlConnection();
		}

		public Prescription(Medicament medicament, Diagnosis diagnosis, SqlConnection connection) : this()
		{
			Connection = connection;
			Medicament = medicament;
			Diagnosis = diagnosis;
		}

		public override void GetById(int? id)
		{
			SqlCommand command = Connection.GetCommand();
			command.CommandText = "SELECT * FROM Prescription WHERE PrescriptionID = @id";
			command.Parameters.AddWithValue("@id", id);
			SqlDataReader reader = command.ExecuteReader();
			if (reader.HasRows)
			{
				reader.Read();
				Id = id;
				Medicament = new Medicament();
				Medicament.GetById(reader.GetInt32(1));
				Diagnosis = new Diagnosis();
				Diagnosis.GetById(reader.GetInt32(2));
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
				command.Parameters.AddWithValue("@medicamentid", Medicament.Id);
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
				command.Parameters.AddWithValue("@medicamentid", Medicament.Id);
				command.ExecuteNonQuery();
			}
		}

		public override string ToString()
		{
			return $"Рецепт: {{\n\t{Diagnosis.ToString()},\n\t{Medicament.ToString()}\n}}";
		}
	}
}
