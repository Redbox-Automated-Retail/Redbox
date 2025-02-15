﻿using System;
using System.IO;
using System.Security.Cryptography;

namespace Redbox.HAL.Core
{
    public static class ByteHelper
    {
        private static readonly byte[] m_keyValue = new Guid("{776DA6AF-3033-43ee-B379-2D4F28B5F1FC}").ToByteArray();

        private static readonly byte[] m_initialVector =
            new Guid("{F375D7E0-4572-4518-9C2F-E8F022F42AA7}").ToByteArray();

        public static string ToBase64(byte[] inputArray)
        {
            return Convert.ToBase64String(inputArray);
        }

        internal static byte[] Encrypt(byte[] inputArray)
        {
            var encryptor = new TripleDESCryptoServiceProvider().CreateEncryptor(m_keyValue, m_initialVector);
            using (var memoryStream = new MemoryStream())
            {
                var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                cryptoStream.Write(inputArray, 0, inputArray.Length);
                cryptoStream.FlushFinalBlock();
                return memoryStream.ToArray();
            }
        }

        internal static byte[] Decrypt(byte[] inputArray)
        {
            var decryptor = new TripleDESCryptoServiceProvider().CreateDecryptor(m_keyValue, m_initialVector);
            using (var memoryStream = new MemoryStream())
            {
                var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write);
                cryptoStream.Write(inputArray, 0, inputArray.Length);
                cryptoStream.FlushFinalBlock();
                return memoryStream.ToArray();
            }
        }
    }
}