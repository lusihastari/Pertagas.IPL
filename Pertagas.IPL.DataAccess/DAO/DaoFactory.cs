
namespace Pertagas.IPL.DataAccess.DAO
{
    public static class DaoFactory
    {
        private static UserDao s_userDao;
        private static ClusterDao s_clusterDao;
        private static IncomeSourceDao s_incomeSourceDao;
        private static IncomeClusterDao s_incomeClusterDao;
        private static IncomeDao s_incomeDao;
        private static ExpenseDao s_expenseDao;
        private static AddressBlockDao s_addressBlockDao;

        public static UserDao UserDao
        {
            get
            {
                if (s_userDao == null)
                {
                    s_userDao = new UserDao();
                }

                return s_userDao;
            }
        }


        public static ClusterDao ClusterDao
        {
            get
            {
                if (s_clusterDao == null)
                {
                    s_clusterDao = new ClusterDao();
                }

                return s_clusterDao;
            }
        }

        public static IncomeSourceDao IncomeSourceDao
        {
            get
            {
                if (s_incomeSourceDao == null)
                {
                    s_incomeSourceDao = new IncomeSourceDao();
                }

                return s_incomeSourceDao;
            }
        }

        public static IncomeClusterDao IncomeClusterDao
        {
            get
            {
                if (s_incomeClusterDao == null)
                {
                    s_incomeClusterDao = new IncomeClusterDao();
                }

                return s_incomeClusterDao;
            }
        }

        public static IncomeDao IncomeDao
        {
            get
            {
                if (s_incomeDao == null)
                {
                    s_incomeDao = new IncomeDao();
                }

                return s_incomeDao;
            }
        }

        public static ExpenseDao ExpenseDao
        {
            get
            {
                if (s_expenseDao == null)
                {
                    s_expenseDao = new ExpenseDao();
                }

                return s_expenseDao;
            }
        }

        public static AddressBlockDao AddressBlockDao
        {
            get
            {
                if (s_addressBlockDao == null)
                {
                    s_addressBlockDao = new AddressBlockDao();
                }

                return s_addressBlockDao;
            }
        }
    }
}
