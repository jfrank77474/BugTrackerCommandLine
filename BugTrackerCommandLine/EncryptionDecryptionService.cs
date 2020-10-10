using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BugTrackerCommandLine
{
    // This is based on the tutorial from 
    // https://www.c-sharpcorner.com/article/encryption-and-decryption-using-a-symmetric-key-in-c-sharp/
    // for futher study
    public class EncryptionDecryptionService
    {
        public static string Encrypt(string key, string text)
        {
            // Creating a 16byte array
            byte[] iv = new byte[16];
            byte[] array;

            // Create the Aes object for use
            using (Aes aes = Aes.Create())
            {
                // setting aes Key to string key 
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                // Create an encryptor using the key and byte array IV
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                // Look into why nested usings and not just
                // MemoryStream memoryStream = new MemoryStream();
                // CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write);
                // StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream);
                // streamWriter.Write(text);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(text);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        // Same questions from Encrypt
        public static string Decrypt(string key, string cipher)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipher);

            // Find out difference between using from Encrypt to this one
            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using MemoryStream memoryStream = new MemoryStream(buffer);
            using CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read);
            using StreamReader streamReader = new StreamReader((Stream)cryptoStream);
            return streamReader.ReadToEnd();
        }
    }
}
