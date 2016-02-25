using Pertagas.IPL.Domain;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Pertagas.IPL.DataAccess.DAO
{
    public class IncomeSourceDao
    {
        public List<IncomeSourceDomain> GetAllIncomeSources()
        {
            SQLiteCommand command = new SQLiteCommand("select * from income_source", DatabaseManager.SQLiteConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            List<IncomeSourceDomain> incomeSources = new List<IncomeSourceDomain>();
            while (reader.Read())
            {
                IncomeSourceDomain incomeSource = new IncomeSourceDomain();
                incomeSource.Id = Convert.ToInt32(reader["id"]);
                incomeSource.Description = reader["description"] != null ? Convert.ToString(reader["description"]) : null;

                incomeSources.Add(incomeSource);
            }

            return incomeSources;
        }

        public IncomeSourceDomain Save(IncomeSourceDomain incomeSource)
        {
            SQLiteCommand command = new SQLiteCommand("insert into income_source (description) values (@description)", DatabaseManager.SQLiteConnection);
            command.Parameters.Add(new SQLiteParameter("description", incomeSource.Description));
            command.ExecuteNonQuery();

            return GetIncomeSourceByDescription(incomeSource.Description);
        }

        public IncomeSourceDomain Update(IncomeSourceDomain incomeSource)
        {
            SQLiteCommand command = new SQLiteCommand("update income_source set description=@description where id=@incomesourceid", DatabaseManager.SQLiteConnection);
            command.Parameters.Add(new SQLiteParameter("description", incomeSource.Description));
            command.Parameters.Add(new SQLiteParameter("incomesourceid", incomeSource.Id));
            command.ExecuteNonQuery();

            return GetIncomeSourceByDescription(incomeSource.Description);
        }

        public void Delete(IncomeSourceDomain incomeSource)
        {
            SQLiteCommand command = new SQLiteCommand("delete from income_source where id=@incomesourceid", DatabaseManager.SQLiteConnection);
            command.Parameters.Add(new SQLiteParameter("incomesourceid", incomeSource.Id));
            command.ExecuteNonQuery();
        }

        public IncomeSourceDomain GetIncomeSourceByDescription(string description)
        {
            SQLiteCommand command = new SQLiteCommand("select * from income_source where description=@description", DatabaseManager.SQLiteConnection);
            command.Parameters.Add(new SQLiteParameter("description", description));
            SQLiteDataReader reader = command.ExecuteReader();

            IncomeSourceDomain incomeSource = null;
            while (reader.Read())
            {
                incomeSource = new IncomeSourceDomain();
                incomeSource.Id = Convert.ToInt32(reader["id"]);
                incomeSource.Description = reader["description"] != null ? Convert.ToString(reader["description"]) : null;

                break;
            }

            return incomeSource;
        }
    }
}
