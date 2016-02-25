using Pertagas.IPL.DataAccess.DAO;
using Pertagas.IPL.Domain;
using System.Collections.Generic;

namespace Pertagas.IPL.Logic
{
    public class IncomeSourceLogic
    {
        public List<IncomeSourceDomain> GetAllIncomeSources()
        {
            return DaoFactory.IncomeSourceDao.GetAllIncomeSources();
        }

        public IncomeSourceDomain AddIncomeSource(string description)
        {
            IncomeSourceDomain newIncomeSource = new IncomeSourceDomain();
            newIncomeSource.Description = description;

            return DaoFactory.IncomeSourceDao.Save(newIncomeSource);
        }

        public IncomeSourceDomain UpdateIncomeSource(IncomeSourceDomain incomeSource)
        {
            return DaoFactory.IncomeSourceDao.Update(incomeSource);
        }

        public bool DeleteIncomeSource(IncomeSourceDomain incomeSource, out string errorMessage)
        {
            errorMessage = null;

            int count = DaoFactory.IncomeDao.GetIncomeCountByIncomeSource(incomeSource);
            if (count > 0)
            {
                errorMessage = "Jenis pendapatan tidak dapat dihapus karena jenis pendapatan '" + incomeSource.Description + "' digunakan pada data pendapatan!";
                return false;
            }

            DaoFactory.IncomeSourceDao.Delete(incomeSource);
            return true;
        }
    }
}
