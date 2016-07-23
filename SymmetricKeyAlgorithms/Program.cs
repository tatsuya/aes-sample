﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SymmetricKeyAlgorithms
{
    static class Program
    {
        [SuppressMessage("Microsoft.Usage", "CA2202")]
        static string Encrypt(SymmetricAlgorithm sa, string s)
        {
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, sa.CreateEncryptor(sa.Key, sa.IV), CryptoStreamMode.Write))
                {
                    var bytes = Encoding.ASCII.GetBytes(s);
                    cs.Write(bytes, 0, bytes.Length);
                    cs.FlushFinalBlock();
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2202")]
        static string Decrypt(SymmetricAlgorithm sa, string encrypted)
        {
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, sa.CreateDecryptor(sa.Key, sa.IV), CryptoStreamMode.Write))
                {
                    var bytes = Convert.FromBase64String(encrypted);
                    cs.Write(bytes, 0, bytes.Length);
                    cs.FlushFinalBlock();
                    return Encoding.ASCII.GetString(ms.ToArray());
                }
            }
        }

        static void Main()
        {
            const string original = "Here comes a string to encrypt.";
            string encrypted;
            string decrypted;

            // AesCryptoServiceProvider below can be replaced by:
            // RijndaelManaged
            // DESCryptoServiceProvider
            // RC2CryptoServiceProvider
            // TripleDESCryptoServiceProvider
            // https://msdn.microsoft.com/en-us/library/system.security.cryptography.symmetricalgorithm.aspx
            using (var aes = new AesCryptoServiceProvider())
            {
                encrypted = Encrypt(aes, original);
                decrypted = Decrypt(aes, encrypted);
            }

            Console.WriteLine($"Encrypted: {encrypted}");
            Console.WriteLine($"Decrypted: {decrypted}");
        }
    }
}
