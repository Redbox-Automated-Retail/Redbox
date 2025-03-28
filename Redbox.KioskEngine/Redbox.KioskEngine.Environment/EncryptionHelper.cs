using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Redbox.KioskEngine.Environment
{
  public static class EncryptionHelper
  {
    private static readonly byte[] m_keyValue = new Guid("{02CAD7C4-E0F5-4b1b-BD04-808B840A61D2}").ToByteArray();
    private static readonly byte[] m_initialVector = new Guid("{AD186FCD-2ACD-41a8-958D-6CA7DAEB1DE5}").ToByteArray();

    public static string Encrypt(string data)
    {
      if (data == null)
        return (string) null;
      byte[] bytes = Encoding.Default.GetBytes(data);
      using (TripleDESCryptoServiceProvider cryptoServiceProvider = new TripleDESCryptoServiceProvider())
      {
        using (ICryptoTransform encryptor = cryptoServiceProvider.CreateEncryptor(EncryptionHelper.m_keyValue, EncryptionHelper.m_initialVector))
        {
          using (MemoryStream memoryStream = new MemoryStream())
          {
            CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();
            return Convert.ToBase64String(memoryStream.ToArray());
          }
        }
      }
    }
  }
}
