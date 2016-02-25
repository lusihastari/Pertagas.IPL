using Pertagas.IPL.Common;
using Pertagas.IPL.DataAccess.DAO;
using Pertagas.IPL.Domain;
using System;
using System.Collections.Generic;

namespace Pertagas.IPL.Logic
{
    public class UserLogic
    {
        public bool Login(string username, string password, out string errorMessage)
        {
            errorMessage = null;

            UserDomain user = DaoFactory.UserDao.GetUserByUsername(username);
            if (user == null)
            {
                errorMessage = "Login gagal. Username tidak ditemukan!";
                return false;
            }

            if (!SecurityManager.VerifyMd5Hash(password, user.Password))
            {
                errorMessage = "Login gagal. Password salah!";
                return false;
            }

            SecurityManager.CurrentUser = user;
            return true;
        }

        public UserDomain AddUser(string firstName, string lastName, string userName, string password, out string message)
        {
            message = String.Empty;
            UserDomain user = DaoFactory.UserDao.GetUserByUsername(userName);
            if (user != null)
            {
                message = "Nama Login sudah terpakai! Gunakan nama lain.";
                return null;
            }

            UserDomain newUser = new UserDomain();
            newUser.FirstName = firstName;
            newUser.Lastname = lastName;
            newUser.Username = userName;
            newUser.Password = SecurityManager.GetMd5Hash(password);

            return DaoFactory.UserDao.Save(newUser);            
        }

        public UserDomain UpdateUser(UserDomain user, bool passwordChanged)
        {
            if (passwordChanged)
            {
                user.Password = SecurityManager.GetMd5Hash(user.Password);
            }

            DaoFactory.UserDao.Update(user);

            return user;
        }

        public bool DeleteUser(UserDomain user, out string errorMessage)
        {
            errorMessage = null;
            if (user.Username.Trim().ToLower() == "admin")
            {
                errorMessage = "Administrator tidak dapat dihapus dari sistem!";
                return false;
            }

            DaoFactory.UserDao.Delete(user);
            return true;
        }

        public List<UserDomain> GetAllUsers()
        {
            return DaoFactory.UserDao.GetAllUsers();
        }
    }
}
