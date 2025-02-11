using System;
using System.IO;
using System.Security.Cryptography;

namespace Redbox.Core
{
    internal static class ByteArrayExtensions
    {
        private static readonly byte[] m_keyValue = new Guid("{776DA6AF-3033-43ee-B379-2D4F28B5F1FC}").ToByteArray();
        private static readonly byte[] m_initialVector = new Guid("{F375D7E0-4572-4518-9C2F-E8F022F42AA7}").ToByteArray();

        public static void WriteToFile(this byte[] buffer, string fileName)
        {
            if (buffer == null || fileName == null)
                return;
            File.WriteAllBytes(fileName, buffer);
        }

        public static byte[] Encrypt(this byte[] inputArray)
        {
            ICryptoTransform encryptor = new TripleDESCryptoServiceProvider().CreateEncryptor(ByteArrayExtensions.m_keyValue, ByteArrayExtensions.m_initialVector);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write);
                cryptoStream.Write(inputArray, 0, inputArray.Length);
                cryptoStream.FlushFinalBlock();
                return memoryStream.ToArray();
            }
        }

        public static byte[] Decrypt(this byte[] inputArray)
        {
            ICryptoTransform decryptor = new TripleDESCryptoServiceProvider().CreateDecryptor(ByteArrayExtensions.m_keyValue, ByteArrayExtensions.m_initialVector);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Write);
                cryptoStream.Write(inputArray, 0, inputArray.Length);
                cryptoStream.FlushFinalBlock();
                return memoryStream.ToArray();
            }
        }

        public static string ToBase85(this byte[] inputArray) => ASCII85.Encode(inputArray);

        public static string ToBase64(this byte[] inputArray) => Convert.ToBase64String(inputArray);

        public static string ToASCIISHA1Hash(this byte[] inputArray)
        {
            return BitConverter.ToString(SHA1.Create().ComputeHash(inputArray)).Replace("-", string.Empty);
        }
    }
}
