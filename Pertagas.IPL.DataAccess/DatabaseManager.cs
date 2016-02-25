using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace Pertagas.IPL.DataAccess
{
    public static class DatabaseManager
    {
        private static System.Data.SQLite.SQLiteConnection s_sqliteConnection;

        public static SQLiteConnection SQLiteConnection
        {
            get { return s_sqliteConnection; }
        }

        public static void OpenConnection(string databaseFilePath)
        {
            s_sqliteConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;", databaseFilePath));
            s_sqliteConnection.Open();
        }

        public static void CloseConnection()
        {
            if (s_sqliteConnection != null)
            {
                s_sqliteConnection.Close();
            }
        }
    }
}
