using Pertagas.IPL.Domain;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Pertagas.IPL.DataAccess.DAO
{
    public class ClusterDao
    {
        public List<ClusterDomain> GetAllClusters()
        {
            SQLiteCommand command = new SQLiteCommand("select * from cluster", DatabaseManager.SQLiteConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            List<ClusterDomain> clusters = new List<ClusterDomain>();
            while (reader.Read())
            {
                ClusterDomain cluster = new ClusterDomain();
                cluster.Id = Convert.ToInt32(reader["id"]);
                cluster.ClusterName = reader["cluster_name"] != null ? Convert.ToString(reader["cluster_name"]) : null;

                clusters.Add(cluster);
            }

            return clusters;
        }

        public ClusterDomain Save(ClusterDomain cluster)
        {
            SQLiteCommand command = new SQLiteCommand("insert into cluster (cluster_name) values (@clustername)", DatabaseManager.SQLiteConnection);
            command.Parameters.Add(new SQLiteParameter("clustername", cluster.ClusterName));
            command.ExecuteNonQuery();

            command = new SQLiteCommand("select last_insert_rowid()", DatabaseManager.SQLiteConnection);
            Int64 id = (Int64)command.ExecuteScalar();
            cluster.Id = (int)id;

            return cluster;
        }

        public ClusterDomain Update(ClusterDomain cluster)
        {
            SQLiteCommand command = new SQLiteCommand("update cluster set cluster_name=@clustername where id=@clusterid", DatabaseManager.SQLiteConnection);
            command.Parameters.Add(new SQLiteParameter("clustername", cluster.ClusterName));
            command.Parameters.Add(new SQLiteParameter("clusterid", cluster.Id));
            command.ExecuteNonQuery();

            return cluster;
        }

        public void Delete(ClusterDomain cluster)
        {
            SQLiteCommand command = new SQLiteCommand("delete from cluster where id=@clusterid", DatabaseManager.SQLiteConnection);            
            command.Parameters.Add(new SQLiteParameter("clusterid", cluster.Id));
            command.ExecuteNonQuery();
        }
    }
}
