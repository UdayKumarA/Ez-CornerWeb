
using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace EvolutyzCorner.UI.Web.Models
{
    public class Decript
    {

        public string Decryption(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }


        public string DecGetMD5(string str)
        {

            Random rand = new Random();
            byte[] key = new byte[16];
            byte[] iv = new byte[16];
            for (int i = 0; i < 16; i++) key[i] = (byte)rand.Next(256);
            for (int i = 0; i < 16; i++) iv[i] = (byte)rand.Next(256);
            byte[] data = Encoding.ASCII.GetBytes(str);
            byte[] encrypted = Encrypt(data, key, iv);
            byte[] decrypted = Decrypt(encrypted, key, iv);
            string password2 = Encoding.ASCII.GetString(encrypted);
            string password3 = Encoding.ASCII.GetString(decrypted);
            return password3;

        }
       
        public string GetMD5(string str)
        {

            Random rand = new Random();
            byte[] key = new byte[16];
            byte[] iv = new byte[16];
            for (int i = 0; i < 16; i++) key[i] = (byte)rand.Next(256);
            for (int i = 0; i < 16; i++) iv[i] = (byte)rand.Next(256);
            byte[] data = Encoding.ASCII.GetBytes(str);
            byte[] encrypted = Encrypt(data, key, iv);
            byte[] decrypted = Decrypt(encrypted, key, iv);
            string password2 = Encoding.ASCII.GetString(encrypted);
            string password3 = Encoding.ASCII.GetString(decrypted);
            return password2;

        }
        static byte[] Encrypt(byte[] data, byte[] key, byte[] iv)
        {
            using (Aes algo = Aes.Create())
            {
                using (ICryptoTransform encryptor = algo.CreateEncryptor(key, iv))
                {
                    return Crypt(data, encryptor);
                }
            }
        }
        static byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
        {
            using (Aes algo = Aes.Create())
            {
                using (ICryptoTransform decryptor = algo.CreateDecryptor(key, iv))
                {
                    return Crypt(data, decryptor);
                }
            }
        }

        static byte[] Crypt(byte[] data, ICryptoTransform cryptor)
        {
            var ms = new MemoryStream();
            using (Stream cs = new CryptoStream(ms, cryptor, CryptoStreamMode.Write))
            {
                cs.Write(data, 0, data.Length);
            }
            return ms.ToArray();
        }











    }
}