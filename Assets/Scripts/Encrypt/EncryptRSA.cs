using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;


/// <summary>
/// RSA加密管理器
/// </summary>
public class MgrRSA : BaseEnDeCryptMgr
{
    private string _keyDir;
    public string KeyDir { get => _keyDir; }
    private EncryptRSA rsa;

    public MgrRSA(string dir) {
        this._keyDir = dir;
        rsa = new EncryptRSA();
        CheckKeys();
    }

    /// <summary>
    /// key生成
    /// </summary>
    private void GenerateKeys()
    {
        rsa.GenerateKeys(_keyDir);
    }

    /// <summary>
    /// 校验RSAkey是否存在
    /// </summary>
    private void CheckKeys()
    {
        // 检查秘钥路径是否存在，否则创建
        if (!Directory.Exists(_keyDir))
        {
            Directory.CreateDirectory(_keyDir);
            GenerateKeys();
        }
        else
        {
            // 检查秘钥是否存在，否则创建
            if (!File.Exists(_keyDir + "/" + rsa.PublicKeyFileName) || !File.Exists(_keyDir + "/" + rsa.PrivateKeyFileName))
            {
                GenerateKeys();
            }
        }
    }

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="msg">文本</param>
    /// <returns>返回结果</returns>
    public override string Encrypt(string msg)
    {
        return rsa.Encrypt(msg, _keyDir + "/" + rsa.PublicKeyFileName);
    }

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="msg">文本</param>
    /// <returns>返回结果</returns>
    public override string Decrypt(string msg)
    {
        return rsa.Decrypt(msg, _keyDir + "/" + rsa.PrivateKeyFileName);
    }
}


/// <summary>
/// 信息加密
/// </summary>
public class EncryptRSA
{
    private const int _RsaKeySize = 2048;
    private const string _publicKeyFileName = "RSA.Pub";
    public string PublicKeyFileName => _publicKeyFileName;
    private const string _privateKeyFileName = "RSA.Private";
    public string PrivateKeyFileName => _privateKeyFileName;

    /// <summary>        
    /// 在给定路径中生成XML格式的私钥和公钥。        
    /// </summary>        
    public void GenerateKeys(string path)
    {
        using (var rsa = new RSACryptoServiceProvider(_RsaKeySize))
        {
            try
            {
                // 获取私钥和公钥。                    
                var publicKey = rsa.ToXmlString(false);
                var privateKey = rsa.ToXmlString(true);

                // 保存到磁盘                    
                File.WriteAllText(Path.Combine(path, _publicKeyFileName), publicKey);
                File.WriteAllText(Path.Combine(path, _privateKeyFileName), privateKey);
                Debug.Log(string.Format("生成的RSA密钥的路径: {0}\\ [{1}, {2}]", path, _publicKeyFileName, _privateKeyFileName));
            }
            finally
            {
                rsa.PersistKeyInCsp = false;
            }
        }
    }

    /// <summary>        
    /// 用给定路径的RSA公钥文件加密纯文本。       
    /// </summary>        
    /// <param name="plainText">要加密的文本</param>        
    /// <param name="pathToPublicKey">用于加密的公钥路径.</param>        
    /// <returns>表示加密数据的64位编码字符串.</returns>        
    public string Encrypt(string plainText, string pathToPublicKey)
    {
        using (var rsa = new RSACryptoServiceProvider(_RsaKeySize))
        {
            try
            {
                //加载公钥                    
                var publicXmlKey = File.ReadAllText(pathToPublicKey);
                rsa.FromXmlString(publicXmlKey);
                var bytesToEncrypt = System.Text.Encoding.Unicode.GetBytes(plainText);
                var bytesEncrypted = rsa.Encrypt(bytesToEncrypt, false);
                return Convert.ToBase64String(bytesEncrypted);
            }
            finally
            {
                rsa.PersistKeyInCsp = false;
            }
        }
    }


    /// <summary>        
    /// Decrypts encrypted text given a RSA private key file path.给定路径的RSA私钥文件解密 加密文本        
    /// </summary>        
    /// <param name="encryptedText">加密的密文</param>        
    /// <param name="pathToPrivateKey">用于加密的私钥路径.</param>        
    /// <returns>未加密数据的字符串</returns>        
    public string Decrypt(string encryptedText, string pathToPrivateKey)
    {
        if(MsgEmpty(encryptedText))
        {
            Debug.Log(string.Format("空密文，无法解析！"));
            return null;
        }
        using (var rsa = new RSACryptoServiceProvider(_RsaKeySize))
        {
            try
            {
                var privateXmlKey = File.ReadAllText(pathToPrivateKey); rsa.FromXmlString(privateXmlKey);
                var bytesEncrypted = Convert.FromBase64String(encryptedText);
                var bytesPlainText = rsa.Decrypt(bytesEncrypted, false);
                return System.Text.Encoding.Unicode.GetString(bytesPlainText);
            }
            finally
            {
                rsa.PersistKeyInCsp = false;
            }
        }
    }


    private bool MsgEmpty(string msg)
    {
        return string.IsNullOrEmpty(msg);
    }
}