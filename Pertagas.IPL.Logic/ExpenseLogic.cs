using Pertagas.IPL.DataAccess.DAO;
using Pertagas.IPL.Domain;
using System.Collections.Generic;

namespace Pertagas.IPL.Logic
{
    public class ExpenseLogic
    {
        public List<ExpenseDomain> GetAllExpenses()
        {
            return DaoFactory.ExpenseDao.GetAllExpenses();
        }

        public ExpenseDomain AddExpense(string description, int month, int year, double amount)
        {
            ExpenseDomain expense = new ExpenseDomain();
            expense.Description = description;
            expense.Month = month;
            expense.Year = year;
            expense.Amount = amount;

            return DaoFactory.ExpenseDao.Save(expense);
        }

        public ExpenseDomain UpdateExpense(ExpenseDomain expense)
        {
            return DaoFactory.ExpenseDao.Update(expense);
        }

        public void DeleteExpense(ExpenseDomain expense)
        {
            DaoFactory.ExpenseDao.Delete(expense);
        }
    }
}
