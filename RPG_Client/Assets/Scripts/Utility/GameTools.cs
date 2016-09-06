using UnityEngine;
using System.Collections;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System;

public static class GameTools {

    public static FuncObj Log
    {
        get
        {
            return UITools.log;
        }
    }

    public static FuncObj LogError
    {
        get
        {
            return UITools.logError;
        }
    }
    public static FuncObj LogWarring
    {
        get
        {
            return UITools.logWarning;
        }
    }

    public static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x12, 0x34, 0x56, 0x78, 0x12, 0x34, 0x78 };
    public static string EncryptString(string sInputString)
    {
        string key = System.Text.Encoding.Default.GetString(Keys);
        return EncryptString(sInputString, key);
    }

    public static string DecryptString(string sInputString)
    {
        string key = System.Text.Encoding.Default.GetString(Keys);
        return DecryptString(sInputString, key);
    }
    // 加密字符串   
    public static string EncryptString(string sInputString, string sKey)
    {
        byte[] data = Encoding.UTF8.GetBytes(sInputString);
        DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
        DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
        DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
        ICryptoTransform desencrypt = DES.CreateEncryptor();
        byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
        return BitConverter.ToString(result);
    }
    // 解密字符串   
    public static string DecryptString(string sInputString, string sKey)
    {
        string[] sInput = sInputString.Split("-".ToCharArray());
        byte[] data = new byte[sInput.Length];
        for (int i = 0; i < sInput.Length; i++)
        {
            data[i] = byte.Parse(sInput[i], NumberStyles.HexNumber);
        }
        DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
        DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
        DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
        ICryptoTransform desencrypt = DES.CreateDecryptor();
        byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
        return Encoding.UTF8.GetString(result);
    }
}
