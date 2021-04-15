using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Risfond.File.Infrastructure
{
  public class DesConfig {
    public string DesIV { get; set; }
    public string DesKey { get; set; }
  }
  public class DesHelper
  {
    private byte[] rgbIV;
    private byte[] rgbKey;    
    public DesHelper(DesConfig config)
    {
      this.rgbIV = config.DesIV.Split(',').Select(val => Convert.ToByte(val)).ToArray();
      this.rgbKey = Encoding.UTF8.GetBytes(config.DesKey.Substring(0, 8));
    }
    public byte[] EnctryptToBytes(string input)
    {
        byte[] stringToBytes = Encoding.UTF8.GetBytes(input);
        return GetBytes(stringToBytes, CreateEncryptor());
    }
    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public string EnctryptToString(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;
        return Convert.ToBase64String(EnctryptToBytes(input));
    }
    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public string Decrypt(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;
        return Encoding.UTF8.GetString(GetBytes(Convert.FromBase64String(input), CreateDecryptor()));
    }


    private byte[] GetBytes(byte[] inputByteArray, ICryptoTransform cryptoTransform)
    {
        using (MemoryStream mStream = new MemoryStream())
        {
            using (CryptoStream cStream = new CryptoStream(mStream, cryptoTransform, CryptoStreamMode.Write))
            {
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return mStream.ToArray();
            }
        }
    }

    private ICryptoTransform CreateEncryptor()
    {
        return new DESCryptoServiceProvider().CreateEncryptor(rgbKey, rgbIV);
    }

    private ICryptoTransform CreateDecryptor()
    {
        return new DESCryptoServiceProvider().CreateDecryptor(rgbKey, rgbIV);
    }
  }
}
