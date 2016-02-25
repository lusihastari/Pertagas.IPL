using Pertagas.IPL.DataAccess.DAO;
using Pertagas.IPL.Domain;
using System.Collections.Generic;

namespace Pertagas.IPL.Logic
{
    public class ClusterLogic
    {
        public List<ClusterDomain> GetAllCluster()
        {
            return DaoFactory.ClusterDao.GetAllClusters();
        }

        public ClusterDomain AddCluster(string clusterName)
        {
            ClusterDomain newCluster = new ClusterDomain();
            newCluster.ClusterName = clusterName;

            return DaoFactory.ClusterDao.Save(newCluster);
        }

        public ClusterDomain UpdateCluster(ClusterDomain cluster)
        {
            return DaoFactory.ClusterDao.Update(cluster);
        }

        public bool DeleteCluster(ClusterDomain cluster, out string errorMessage)
        {
            errorMessage = null;

            int count = DaoFactory.IncomeClusterDao.GetIncomeClusterCountByCluster(cluster);
            if (count > 0)
            {
                errorMessage = "Cluster tidak dapat dihapus karena Cluster '" + cluster.ClusterName + "' digunakan pada data pendapatan cluster!";
                return false;
            }

            DaoFactory.ClusterDao.Delete(cluster);
            return true;
        }
    }
}
