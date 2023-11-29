using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System;
namespace JetDevsPrcatical.Common
{

    public static class PasswordHelper
    {
        public static string GetSHA256Password(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);

                // Convert the byte array to a hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        public static string GenerateRandomPassword()
        {
            const int passwordLength = 12; // Define your desired password length
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+-=";

            var random = new Random();
            var passwordBuilder = new StringBuilder();

            for (int i = 0; i < passwordLength; i++)
            {
                int index = random.Next(validChars.Length);
                passwordBuilder.Append(validChars[index]);
            }

            return passwordBuilder.ToString();
        }
    }
}