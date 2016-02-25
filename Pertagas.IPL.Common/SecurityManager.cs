
using Pertagas.IPL.Domain;
using System;
using System.Security.Cryptography;
using System.Text;
namespace Pertagas.IPL.Common
{
    public static class SecurityManager
    {
        private static UserDomain s_currentUser;

        public static UserDomain CurrentUser
        {
            get { return s_currentUser; }
            set { s_currentUser = value; }
        }

        public static bool VerifyMd5Hash(string plainText, string chiperText)
        {
            // Hash the plain text.
            string hashOfInput = GetMd5Hash(plainText);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashOfInput, chiperText))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetMd5Hash(string input)
        {
            MD5 md5Hash = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}
