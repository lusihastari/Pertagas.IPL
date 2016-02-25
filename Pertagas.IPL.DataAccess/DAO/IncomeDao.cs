using Pertagas.IPL.Domain;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Pertagas.IPL.DataAccess.DAO
{
    public class IncomeDao
    {
        public IncomeDomain Save(IncomeDomain income)
        {
            SQLiteCommand command = new SQLiteCommand("insert into income (income_source_id, amount, month, year) values " +
            "(@incomesourceid, @amount, @month, @year)", DatabaseManager.SQLiteConnection);
            command.Parameters.Add(new SQLiteParameter("incomesourceid", income.IncomeSourceId));
            command.Parameters.Add(new SQLiteParameter("amount", income.Amount));
            command.Parameters.Add(new SQLiteParameter("month", income.Month));
            command.Parameters.Add(new SQLiteParameter("year", income.Year));
            command.ExecuteNonQuery();

            command = new SQLiteCommand("select last_insert_rowid()", DatabaseManager.SQLiteConnection);
            Int64 id = (Int64)command.ExecuteScalar();            
            income.Id = (int)id;

            return income;
        }

        public IncomeDomain Update(IncomeDomain income)
        {
            SQLiteCommand command = new SQLiteCommand("update income set income_source_id=@incomesourceid, month=@month, year=@year, amount=@amount where id=@incomeid", DatabaseManager.SQLiteConnection);
            command.Parameters.Add(new SQLiteParameter("incomeid", income.Id));
            command.Parameters.Add(new SQLiteParameter("incomesourceid", income.IncomeSourceId));
            command.Parameters.Add(new SQLiteParameter("amount", income.Amount));
            command.Parameters.Add(new SQLiteParameter("month", income.Month));
            command.Parameters.Add(new SQLiteParameter("year", income.Year));

            command.ExecuteNonQuery();

            return income;
        }

        public void Delete(IncomeDomain income)
        {
            SQLiteCommand command = new SQLiteCommand("delete from income where id=@incomeid", DatabaseManager.SQLiteConnection);
            command.Parameters.Add(new SQLiteParameter("incomeid", income.Id));

            command.ExecuteNonQuery();
        }

        public List<IncomeDomain> GetAllIncomes()
        {
            SQLiteCommand command = new SQLiteCommand("select * from income", DatabaseManager.SQLiteConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            List<IncomeDomain> incomes = new List<IncomeDomain>();
            while(reader.Read())
            {
                IncomeDomain income = new IncomeDomain();
                income.Id = Convert.ToInt32(reader["id"]);
                income.IncomeSourceId = reader["income_source_id"] != null ? Convert.ToInt32(reader["income_source_id"]) : 0;
                income.Amount = Convert.ToDouble(reader["amount"]);
                income.Month = reader["month"] != null ? Convert.ToInt32(reader["month"]) : 0;
                income.Year = reader["year"] != null ? Convert.ToInt32(reader["year"]) : 0;

                incomes.Add(income);
            }

            return incomes;
        }

        public int GetIncomeCountByIncomeSource(IncomeSourceDomain incomeSource)
        {
            SQLiteCommand command = new SQLiteCommand("select count(p.Id) from income p where p.income_source_id=@incomesourceid", DatabaseManager.SQLiteConnection);
            command.Parameters.Add(new SQLiteParameter("incomesourceid", incomeSource.Id));

            int incomeCount = Convert.ToInt32(command.ExecuteScalar());

            return incomeCount;
        }

        public List<IncomeDomain> GetIncomes(int fromMonthIndex, int toMonthIndex, int year)
        {
            SQLiteCommand command = new SQLiteCommand("select * from income where (month >= @frommonthindex and month <= @tomonthindex) and year=@year ", DatabaseManager.SQLiteConnection);
            command.Parameters.Add(new SQLiteParameter("frommonthindex", fromMonthIndex));
            command.Parameters.Add(new SQLiteParameter("tomonthindex", toMonthIndex));
            command.Parameters.Add(new SQLiteParameter("year", year));
            SQLiteDataReader reader = command.ExecuteReader();

            List<IncomeDomain> incomes = new List<IncomeDomain>();
            while (reader.Read())
            {
                IncomeDomain income = new IncomeDomain();
                income.Id = Convert.ToInt32(reader["id"]);
                income.IncomeSourceId = reader["income_source_id"] != null ? Convert.ToInt32(reader["income_source_id"]) : 0;
                income.Amount = Convert.ToDouble(reader["amount"]);
                income.Month = reader["month"] != null ? Convert.ToInt32(reader["month"]) : 0;
                income.Year = reader["year"] != null ? Convert.ToInt32(reader["year"]) : 0;

                incomes.Add(income);
            }

            return incomes;
        }
    }
}
