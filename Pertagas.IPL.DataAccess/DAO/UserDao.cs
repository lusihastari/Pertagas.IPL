using Pertagas.IPL.Domain;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Pertagas.IPL.DataAccess.DAO
{
    public class UserDao
    {        
        public List<UserDomain> GetAllUsers()
        {
            SQLiteCommand command = new SQLiteCommand("select * from user", DatabaseManager.SQLiteConnection);            
            SQLiteDataReader reader = command.ExecuteReader();

            List<UserDomain> users = new List<UserDomain>();
            while (reader.Read())
            {
                UserDomain user = new UserDomain();
                user.Id = Convert.ToInt32(reader["id"]);
                user.Username = reader["user_name"] != null ? Convert.ToString(reader["user_name"]) : null;
                user.Password = reader["password"] != null ? Convert.ToString(reader["password"]) : null;
                user.FirstName = reader["first_name"] != null ? Convert.ToString(reader["first_name"]) : null;
                user.Lastname = reader["last_name"] != null ? Convert.ToString(reader["last_name"]) : null;

                users.Add(user);
            }

            return users;
        }

        public UserDomain GetUserByUsername(string username)
        {
            SQLiteCommand command = new SQLiteCommand("select * from user where upper(user_name)=@username", DatabaseManager.SQLiteConnection);
            command.Parameters.Add(new SQLiteParameter("username", username.ToUpper()));
            SQLiteDataReader reader = command.ExecuteReader();

            UserDomain user = null;
            while (reader.Read())
            {
                user = new UserDomain();
                user.Id = Convert.ToInt32(reader["id"]);
                user.Username = reader["user_name"] != null ? Convert.ToString(reader["user_name"]) : null;
                user.Password = reader["password"] != null ? Convert.ToString(reader["password"]) : null;
                user.FirstName = reader["first_name"] != null ? Convert.ToString(reader["first_name"]) : null;
                user.Lastname = reader["last_name"] != null ? Convert.ToString(reader["last_name"]) : null;

                break;
            }

            return user;
        }

        public void Update(UserDomain user)
        {
            SQLiteCommand command = new SQLiteCommand("update user set first_name=@firstname, last_name=@lastname, password=@password where id=@user_id", DatabaseManager.SQLiteConnection);
            command.Parameters.Add(new SQLiteParameter("user_id", user.Id));
            command.Parameters.Add(new SQLiteParameter("firstname", user.FirstName));
            command.Parameters.Add(new SQLiteParameter("lastname", user.Lastname));
            command.Parameters.Add(new SQLiteParameter("password", user.Password));
            command.ExecuteNonQuery();        
        }

        public void Delete(UserDomain user)
        {
            SQLiteCommand command = new SQLiteCommand("delete from user where id=@user_id", DatabaseManager.SQLiteConnection);
            command.Parameters.Add(new SQLiteParameter("user_id", user.Id));
            command.ExecuteNonQuery();
        }

        public UserDomain Save(UserDomain user)
        {
            SQLiteCommand command = new SQLiteCommand(String.Format("insert into user (first_name, last_name, user_name, password) values ('{0}', '{1}', '{2}', '{3}')",
                user.FirstName, user.Lastname, user.Username, user.Password), DatabaseManager.SQLiteConnection);
            command.ExecuteNonQuery();

            return GetUserByUsername(user.Username);
        }
    }
}
