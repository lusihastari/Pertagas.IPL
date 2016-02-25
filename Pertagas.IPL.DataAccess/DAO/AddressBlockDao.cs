using Pertagas.IPL.Domain;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Pertagas.IPL.DataAccess.DAO
{
    public class AddressBlockDao
    {
        public List<AddressBlockDomain> GetAllAddressBlocks()
        {
            SQLiteCommand command = new SQLiteCommand("select * from address_block", DatabaseManager.SQLiteConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            List<AddressBlockDomain> addressBlocks = new List<AddressBlockDomain>();
            while (reader.Read())
            {
                string block = reader["block"] != null ? reader["block"].ToString() : String.Empty;
                addressBlocks.Add(new AddressBlockDomain() { Id = Convert.ToInt32(reader["id"]), Block = block });
            }

            return addressBlocks;
        }

        public AddressBlockDomain Save(string block)
        {
            SQLiteCommand command = new SQLiteCommand("insert into address_block (block) values (@block)", DatabaseManager.SQLiteConnection);
            command.Parameters.Add(new SQLiteParameter("block", block));
            command.ExecuteNonQuery();

            AddressBlockDomain addressBlock = new AddressBlockDomain();
            addressBlock.Block = block;

            command = new SQLiteCommand("select last_insert_rowid()", DatabaseManager.SQLiteConnection);
            Int64 id = (Int64)command.ExecuteScalar();
            addressBlock.Id = (int)id;

            return addressBlock;
        }

        public void Update(AddressBlockDomain addressBlock)
        {
            SQLiteCommand command = new SQLiteCommand("update address_block set block=@block where id=@id", DatabaseManager.SQLiteConnection);
            command.Parameters.Add(new SQLiteParameter("block", addressBlock.Block));
            command.Parameters.Add(new SQLiteParameter("id", addressBlock.Id));
            command.ExecuteNonQuery();
        }

        public void Delete(string block)
        {
            SQLiteCommand command = new SQLiteCommand("delete from address_block where block=@block", DatabaseManager.SQLiteConnection);
            command.Parameters.Add(new SQLiteParameter("block", block));
            command.ExecuteNonQuery();
        }
    }
}
