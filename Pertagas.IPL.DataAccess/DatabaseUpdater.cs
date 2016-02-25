using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace Pertagas.IPL.DataAccess
{
    public static class DatabaseUpdater
    {
        public static void Update()
        {
            SQLiteCommand command = new SQLiteCommand("select count(*) from sqlite_master where name='address_block' and type='table'", DatabaseManager.SQLiteConnection);
            object result = command.ExecuteScalar();

            bool tableAddressBlockExist = result != null && Convert.ToInt32(result) == 1;
            if (!tableAddressBlockExist)
            {
                command = new SQLiteCommand("CREATE TABLE \"address_block\" (\"id\" INTEGER PRIMARY KEY NOT NULL UNIQUE, \"block\" TEXT);", DatabaseManager.SQLiteConnection);
                command.ExecuteNonQuery();

                string[] addressBlocks = new string[] { "PN1", "PN2", "PN3", "PN4", "PN5", "PN6", "PNR", "AG1", "AG3", "AG5", "AG7", "AG9", "AG11" };
                foreach (string block in addressBlocks)
                {
                    command = new SQLiteCommand("insert into address_block (block) values (@block)", DatabaseManager.SQLiteConnection);
                    command.Parameters.Add(new SQLiteParameter("block", block));
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
