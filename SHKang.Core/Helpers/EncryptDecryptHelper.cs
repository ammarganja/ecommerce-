namespace SHKang.Core.Helpers
{
    #region Namespaces
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Mail;
    using System.Security.Cryptography; 
    #endregion
    
    public static class EncryptDecryptHelper
    {
        #region Variables
        #endregion

        #region Encrption/Decryption functions

        /// <summary>
        /// Encrypts the specified string text.
        /// </summary>
        /// <param name="strText">The string text.</param>
        /// <returns>
        /// Convert string to base64 string
        /// </returns>
        public static string Encrypt(string strText)
        {
            byte[] byKey = { };
            byte[] IV =
            {
                18,52,86,120,144,171,205,239
            };
            byKey = System.Text.Encoding.UTF8.GetBytes("VirtFiit");
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = System.Text.Encoding.UTF8.GetBytes(strText);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            string encryptedString = Convert.ToBase64String(ms.ToArray());
            encryptedString = encryptedString.Replace("/", "*");
            encryptedString = encryptedString.Replace("\\", "#");
            encryptedString = encryptedString.Replace("&", "$");
            encryptedString = encryptedString.Replace("+", "_");
            return encryptedString;
        }

        /// <summary>
        /// Decrypts the specified string to decrypt.
        /// </summary>
        /// <param name="stringToDecrypt">The string to decrypt.</param>
        /// <returns>
        /// Convert base64 string to string
        /// </returns>
        public static string Decrypt(string stringToDecrypt)
        {
            stringToDecrypt = stringToDecrypt.Replace("*", "/");
            stringToDecrypt = stringToDecrypt.Replace("#", "\\");
            stringToDecrypt = stringToDecrypt.Replace("$", "&");
            stringToDecrypt = stringToDecrypt.Replace("_", "+");

            byte[] inputByteArray = new byte[stringToDecrypt.Length];
            byte[] byKey = { };
            byte[] IV =
            {
            18,52,86,120,144,171,205,239
            };
            byKey = System.Text.Encoding.UTF8.GetBytes("VirtFiit");
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByteArray = Convert.FromBase64String(stringToDecrypt);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            return encoding.GetString(ms.ToArray());
        }

        #endregion


    }
}
