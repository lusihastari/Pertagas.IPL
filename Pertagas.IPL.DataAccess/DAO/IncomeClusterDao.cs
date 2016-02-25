using Pertagas.IPL.Domain;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Pertagas.IPL.DataAccess.DAO
{
    public class IncomeClusterDao
    {
        public IncomeClusterDomain Save(IncomeClusterDomain incomeCluster)
        {
            SQLiteCommand command = new SQLiteCommand(
                "insert into income_cluster (cluster_id, occupant_name, address_block, address_number, phone_number, month, year, amount) " +
            "values (@clusterid, @occupantname, @addressblock, @addressnumber, @phonenumber, @month, @year, @amount)", DatabaseManager.SQLiteConnection);
            command.Parameters.Add(new SQLiteParameter("clusterid", incomeCluster.ClusterId));
            command.Parameters.Add(new SQLiteParameter("occupantname", incomeCluster.OccupantName));
            command.Parameters.Add(new SQLiteParameter("addressblock", incomeCluster.AddressBlock));
            command.Parameters.Add(new SQLiteParameter("addressnumber", incomeCluster.AddressNumber));
            command.Parameters.Add(new SQLiteParameter("phonenumber", incomeCluster.PhoneNumber));
            command.Parameters.Add(new SQLiteParameter("month", incomeCluster.Month));
            command.Parameters.Add(new SQLiteParameter("year", incomeCluster.Year));
            command.Parameters.Add(new SQLiteParameter("amount", incomeCluster.Amount));
            command.ExecuteNonQuery();

            command = new SQLiteCommand("select last_insert_rowid()", DatabaseManager.SQLiteConnection);
            Int64 id = (Int64)command.ExecuteScalar();
            incomeCluster.Id = (int)id;

            return incomeCluster;
        }

        public List<IncomeClusterDomain> GetAllIncomeClusters()
        {
            SQLiteCommand command = new SQLiteCommand("select * from income_cluster", DatabaseManager.SQLiteConnection);            
            SQLiteDataReader reader = command.ExecuteReader();

            List<IncomeClusterDomain> clusterIncomes = new List<IncomeClusterDomain>();
            while (reader.Read())
            {
                IncomeClusterDomain clusterIncome = new IncomeClusterDomain();
                clusterIncome.Id = Convert.ToInt32(reader["id"]);
                clusterIncome.ClusterId = Convert.ToInt32(reader["cluster_id"]);
                clusterIncome.AddressBlock = reader["address_block"] != null ? reader["address_block"].ToString() : null;
                clusterIncome.AddressNumber = reader["address_number"] != null ? reader["address_number"].ToString() : null;
                clusterIncome.PhoneNumber = reader["phone_number"] != null ? reader["phone_number"].ToString() : null;
                clusterIncome.OccupantName = reader["occupant_name"] != null ? reader["occupant_name"].ToString() : null;
                clusterIncome.Month = Convert.ToInt32(reader["month"]);
                clusterIncome.Year = Convert.ToInt32(reader["year"]);
                clusterIncome.Amount = Convert.ToDouble(reader["amount"]);

                clusterIncomes.Add(clusterIncome);
            }

            return clusterIncomes;
        }

        public void Delete(IncomeClusterDomain incomeCluster)
        {
            SQLiteCommand command = new SQLiteCommand("delete from income_cluster where id=@clusterincomeid", DatabaseManager.SQLiteConnection);
            command.Parameters.Add(new SQLiteParameter("clusterincomeid", incomeCluster.Id));
            command.ExecuteNonQuery();
        }

        public IncomeClusterDomain Update(IncomeClusterDomain incomeCluster)
        {
            SQLiteCommand command = new SQLiteCommand(
                "update income_cluster set cluster_id=@clusterid, occupant_name=@occupantname, address_block=@addressblock, address_number=@addressnumber, " +
                "phone_number=@phonenumber, month=@month, year=@year, amount=@amount where id=@clusterincomeid", DatabaseManager.SQLiteConnection);
            command.Parameters.Add(new SQLiteParameter("clusterincomeid", incomeCluster.Id));
            command.Parameters.Add(new SQLiteParameter("clusterid", incomeCluster.ClusterId));
            command.Parameters.Add(new SQLiteParameter("occupantname", incomeCluster.OccupantName));
            command.Parameters.Add(new SQLiteParameter("addressblock", incomeCluster.AddressBlock));
            command.Parameters.Add(new SQLiteParameter("addressnumber", incomeCluster.AddressNumber));
            command.Parameters.Add(new SQLiteParameter("phonenumber", incomeCluster.PhoneNumber));
            command.Parameters.Add(new SQLiteParameter("month", incomeCluster.Month));
            command.Parameters.Add(new SQLiteParameter("year", incomeCluster.Year));
            command.Parameters.Add(new SQLiteParameter("amount", incomeCluster.Amount));
            command.ExecuteNonQuery();

            return incomeCluster;
        }

        public int GetIncomeClusterCountByCluster(ClusterDomain cluster)
        {
            SQLiteCommand command = new SQLiteCommand("select count(p.Id) from income_cluster p where p.cluster_id=@clusterid", DatabaseManager.SQLiteConnection);
            command.Parameters.Add(new SQLiteParameter("clusterid", cluster.Id));

            int incomeClusterCount = Convert.ToInt32(command.ExecuteScalar());

            return incomeClusterCount;
        }

        public List<IncomeClusterDomain> GetIncomeClusters(ClusterDomain cluster, int fromMonthIndex, int fromYear, int toMonthIndex, int toYear)
        {
            SQLiteCommand command = new SQLiteCommand("select * from income_cluster where cluster_id=@clusterid and (month >= @frommonthindex and month <= @tomonthindex) and (year >= @fromyear and year <= @toyear)", DatabaseManager.SQLiteConnection);
            command.Parameters.Add(new SQLiteParameter("clusterid", cluster.Id));
            command.Parameters.Add(new SQLiteParameter("frommonthindex", fromMonthIndex));
            command.Parameters.Add(new SQLiteParameter("tomonthindex", toMonthIndex));
            command.Parameters.Add(new SQLiteParameter("fromyear", fromYear));
            command.Parameters.Add(new SQLiteParameter("toyear", toYear));
            SQLiteDataReader reader = command.ExecuteReader();

            List<IncomeClusterDomain> clusterIncomes = new List<IncomeClusterDomain>();
            while (reader.Read())
            {
                IncomeClusterDomain clusterIncome = new IncomeClusterDomain();
                clusterIncome.Id = Convert.ToInt32(reader["id"]);
                clusterIncome.ClusterId = Convert.ToInt32(reader["cluster_id"]);
                clusterIncome.AddressBlock = reader["address_block"] != null ? reader["address_block"].ToString() : null;
                clusterIncome.AddressNumber = reader["address_number"] != null ? reader["address_number"].ToString() : null;
                clusterIncome.PhoneNumber = reader["phone_number"] != null ? reader["phone_number"].ToString() : null;
                clusterIncome.OccupantName = reader["occupant_name"] != null ? reader["occupant_name"].ToString() : null;
                clusterIncome.Month = Convert.ToInt32(reader["month"]);
                clusterIncome.Year = Convert.ToInt32(reader["year"]);
                clusterIncome.Amount = Convert.ToDouble(reader["amount"]);

                clusterIncomes.Add(clusterIncome);
            }

            return clusterIncomes;
        }

        public List<IncomeClusterDomain> GetIncomeClusters(ClusterDomain cluster, int fromMonthIndex, int toMonthIndex, int year)
        {
            SQLiteCommand command = new SQLiteCommand("select * from income_cluster where cluster_id=@clusterid and (month >= @frommonthindex and month <= @tomonthindex) and year=@year", DatabaseManager.SQLiteConnection);
            command.Parameters.Add(new SQLiteParameter("clusterid", cluster.Id));
            command.Parameters.Add(new SQLiteParameter("frommonthindex", fromMonthIndex));
            command.Parameters.Add(new SQLiteParameter("tomonthindex", toMonthIndex));
            command.Parameters.Add(new SQLiteParameter("year", year));
            SQLiteDataReader reader = command.ExecuteReader();

            List<IncomeClusterDomain> clusterIncomes = new List<IncomeClusterDomain>();
            while (reader.Read())
            {
                IncomeClusterDomain clusterIncome = new IncomeClusterDomain();
                clusterIncome.Id = Convert.ToInt32(reader["id"]);
                clusterIncome.ClusterId = Convert.ToInt32(reader["cluster_id"]);
                clusterIncome.AddressBlock = reader["address_block"] != null ? reader["address_block"].ToString() : null;
                clusterIncome.AddressNumber = reader["address_number"] != null ? reader["address_number"].ToString() : null;
                clusterIncome.PhoneNumber = reader["phone_number"] != null ? reader["phone_number"].ToString() : null;
                clusterIncome.OccupantName = reader["occupant_name"] != null ? reader["occupant_name"].ToString() : null;
                clusterIncome.Month = Convert.ToInt32(reader["month"]);
                clusterIncome.Year = Convert.ToInt32(reader["year"]);
                clusterIncome.Amount = Convert.ToDouble(reader["amount"]);

                clusterIncomes.Add(clusterIncome);
            }

            return clusterIncomes;
        }

        public List<IncomeClusterDomain> GetIncomeClusters(int fromMonthIndex, int toMonthIndex, int year)
        {
            SQLiteCommand command = new SQLiteCommand("select * from income_cluster where (month >= @frommonthindex and month <= @tomonthindex) and year=@year", DatabaseManager.SQLiteConnection);            
            command.Parameters.Add(new SQLiteParameter("frommonthindex", fromMonthIndex));
            command.Parameters.Add(new SQLiteParameter("tomonthindex", toMonthIndex));
            command.Parameters.Add(new SQLiteParameter("year", year));
            SQLiteDataReader reader = command.ExecuteReader();

            List<IncomeClusterDomain> clusterIncomes = new List<IncomeClusterDomain>();
            while (reader.Read())
            {
                IncomeClusterDomain clusterIncome = new IncomeClusterDomain();
                clusterIncome.Id = Convert.ToInt32(reader["id"]);
                clusterIncome.ClusterId = Convert.ToInt32(reader["cluster_id"]);
                clusterIncome.AddressBlock = reader["address_block"] != null ? reader["address_block"].ToString() : null;
                clusterIncome.AddressNumber = reader["address_number"] != null ? reader["address_number"].ToString() : null;
                clusterIncome.PhoneNumber = reader["phone_number"] != null ? reader["phone_number"].ToString() : null;
                clusterIncome.OccupantName = reader["occupant_name"] != null ? reader["occupant_name"].ToString() : null;
                clusterIncome.Month = Convert.ToInt32(reader["month"]);
                clusterIncome.Year = Convert.ToInt32(reader["year"]);
                clusterIncome.Amount = Convert.ToDouble(reader["amount"]);

                clusterIncomes.Add(clusterIncome);
            }

            return clusterIncomes;
        }
    }
}
