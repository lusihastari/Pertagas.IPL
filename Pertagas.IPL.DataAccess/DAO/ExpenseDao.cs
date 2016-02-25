using Pertagas.IPL.Domain;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Pertagas.IPL.DataAccess.DAO
{
    public class ExpenseDao
    {
        public List<ExpenseDomain> GetAllExpenses()
        {
            SQLiteCommand command = new SQLiteCommand("select * from expense", DatabaseManager.SQLiteConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            List<ExpenseDomain> expenses = new List<ExpenseDomain>();

            while (reader.Read())
            {
                ExpenseDomain expense = new ExpenseDomain();

                expense.Id = Convert.ToInt32(reader["id"]);
                expense.Description = reader["description"] != null ? reader["description"].ToString() : null;
                expense.Month = Convert.ToInt32(reader["month"]);
                expense.Year = reader["year"] != null ? Convert.ToInt32(reader["year"]) : 0;
                expense.Amount = reader["amount"] != null ? Convert.ToDouble(reader["amount"]) : 0;
                expenses.Add(expense);
            }

            return expenses;
        }

        public ExpenseDomain Save(ExpenseDomain expense)
        {
            SQLiteCommand command = new SQLiteCommand("insert into expense (description, month, year, amount) values (@description, @month, @year, @amount)", DatabaseManager.SQLiteConnection);
            command.Parameters.Add(new SQLiteParameter("description", expense.Description));
            command.Parameters.Add(new SQLiteParameter("month", expense.Month));
            command.Parameters.Add(new SQLiteParameter("year", expense.Year));
            command.Parameters.Add(new SQLiteParameter("amount", expense.Amount));
            command.ExecuteNonQuery();

            command = new SQLiteCommand("select last_insert_rowid()", DatabaseManager.SQLiteConnection);
            Int64 id = (Int64)command.ExecuteScalar();
            expense.Id = (int)id;

            return expense;
        }

        public void Delete(ExpenseDomain expense)
        {
            SQLiteCommand command = new SQLiteCommand("delete from expense where id=@expenseid", DatabaseManager.SQLiteConnection);
            command.Parameters.Add(new SQLiteParameter("expenseid", expense.Id));

            command.ExecuteNonQuery();
        }

        public ExpenseDomain Update(ExpenseDomain expense)
        {
            SQLiteCommand command = new SQLiteCommand("update expense set description=@description, month=@month, year=@year, amount=@amount where id=@expenseid", DatabaseManager.SQLiteConnection);
            command.Parameters.Add(new SQLiteParameter("expenseid", expense.Id));
            command.Parameters.Add(new SQLiteParameter("description", expense.Description));
            command.Parameters.Add(new SQLiteParameter("month", expense.Month));
            command.Parameters.Add(new SQLiteParameter("year", expense.Year));
            command.Parameters.Add(new SQLiteParameter("amount", expense.Amount));
            
            command.ExecuteNonQuery();

            return expense;
        }

        public List<ExpenseDomain> GetExpenses(int fromMonthIndex, int toMonthIndex, int year)
        {
            SQLiteCommand command = new SQLiteCommand("select * from expense where (month >= @frommonthindex and month <= @tomonthindex) and year=@year", DatabaseManager.SQLiteConnection);
            command.Parameters.Add(new SQLiteParameter("frommonthindex", fromMonthIndex));
            command.Parameters.Add(new SQLiteParameter("tomonthindex", toMonthIndex));
            command.Parameters.Add(new SQLiteParameter("year", year));
            SQLiteDataReader reader = command.ExecuteReader();

            List<ExpenseDomain> expenses = new List<ExpenseDomain>();

            while (reader.Read())
            {
                ExpenseDomain expense = new ExpenseDomain();

                expense.Id = Convert.ToInt32(reader["id"]);
                expense.Description = reader["description"] != null ? reader["description"].ToString() : null;
                expense.Month = Convert.ToInt32(reader["month"]);
                expense.Year = reader["year"] != null ? Convert.ToInt32(reader["year"]) : 0;
                expense.Amount = reader["amount"] != null ? Convert.ToDouble(reader["amount"]) : 0;
                expenses.Add(expense);
            }

            return expenses;
        }
    }
}
