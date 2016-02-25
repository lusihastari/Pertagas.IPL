using Pertagas.IPL.DataAccess;
using System;

namespace Pertagas.IPL.Logic
{
    public static class LogicFactory
    {
        private static UserLogic s_userLogic;
        private static ClusterLogic s_clusterLogic;
        private static IncomeSourceLogic s_incomeSourceLogic;
        private static IncomeLogic s_incomeLogic;
        private static ExpenseLogic s_expenseLogic;
        private static AccountingLogic s_accountingLogic;
        private static AddressBlockLogic s_addressBlockLogic;

        public static UserLogic UserLogic
        {
            get
            {
                if (s_userLogic == null)
                {
                    s_userLogic = new UserLogic();
                }
                return s_userLogic;
            }
        }

        public static ClusterLogic ClusterLogic
        {
            get
            {
                if (s_clusterLogic == null)
                {
                    s_clusterLogic = new ClusterLogic();
                }
                return s_clusterLogic;
            }
        }

        public static IncomeSourceLogic IncomeSourceLogic
        {
            get
            {
                if (s_incomeSourceLogic == null)
                {
                    s_incomeSourceLogic = new IncomeSourceLogic();
                }
                return s_incomeSourceLogic;
            }
        }

        public static IncomeLogic IncomeLogic
        {
            get
            {
                if (s_incomeLogic == null)
                {
                    s_incomeLogic = new IncomeLogic();
                }
                return s_incomeLogic;
            }
        }

        public static ExpenseLogic ExpenseLogic
        {
            get
            {
                if (s_expenseLogic == null)
                {
                    s_expenseLogic = new ExpenseLogic();
                }
                return s_expenseLogic;
            }
        }

        public static AccountingLogic AccountingLogic
        {
            get
            {
                if (s_accountingLogic == null)
                {
                    s_accountingLogic = new AccountingLogic();
                }
                return s_accountingLogic;
            }
        }

        public static AddressBlockLogic AddressBlockLogic
        {
            get
            {
                if (s_addressBlockLogic == null)
                {
                    s_addressBlockLogic = new AddressBlockLogic();
                }
                return s_addressBlockLogic;
            }
        }

        public static void Initialize(string databaseFilePath)
        {
            DatabaseManager.OpenConnection(databaseFilePath);
            try
            {
                DatabaseUpdater.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Terminate()
        {
            DatabaseManager.CloseConnection();
        }
    }
}
