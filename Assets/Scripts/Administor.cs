using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Administor
{
    private EncryptMode _encryptMode = EncryptMode.RSA;
    public EncryptMode EncryptMode { get => _encryptMode; set => _encryptMode = value; }
    private Dictionary<EncryptMode, BaseEnDeCryptMgr> _encryptMgrDict;
    private string _bufferMsgEncrypt = "";
    private string _bufferMsgDecrypt = "";

    public Administor()
    {
        _encryptMgrDict = new Dictionary<EncryptMode, BaseEnDeCryptMgr>();
        _encryptMgrDict.Add(EncryptMode.RSA, new MgrRSA(Application.dataPath + "/Keys"));
    }

    /// <summary>
    /// 加密，并返回密文
    /// </summary>
    /// <param name="msg">需要加密的文本</param>
    /// <param name="eMsg">密文</param>
    public void Encrypt(string msg, out string eMsg)
    {
        MgrGet(out var mgr);
        _bufferMsgEncrypt = mgr.Encrypt(msg);
        eMsg = _bufferMsgEncrypt;
        //Debug.Log(string.Format("需要加密的文本：{0}", msg));
        //Debug.Log(string.Format("加密后的文本：{0}", _bufferMsgEncrypt));
    }

    /// <summary>
    /// 解密并返回原文，
    /// </summary>
    /// <param name="dMsg">原文</param>
    public void Decrypt(out string dMsg)
    {
        MgrGet(out var mgr);
        _bufferMsgDecrypt = mgr.Decrypt(_bufferMsgEncrypt);
        dMsg = _bufferMsgDecrypt;
        //Debug.Log(string.Format("解密后的文本：{0}", _bufferMsgDecrypt));
    }
    /// <summary>
    /// 解密并返回原文
    /// </summary>
    /// <param name="msg">密文</param>
    /// <param name="dMsg">原文</param>
    public void Decrypt(string msg, out string dMsg)
    {
        MgrGet(out var mgr);
        _bufferMsgDecrypt = mgr.Decrypt(msg);
        dMsg = _bufferMsgDecrypt;
        //Debug.Log(string.Format("解密后的文本：{0}", _bufferMsgDecrypt));
    }

    /// <summary>
    /// 缓存数据清理
    /// </summary>
    public void BufferClear()
    {
        _bufferMsgDecrypt = "";
        _bufferMsgEncrypt = "";
    }

    /// <summary>
    /// 返回加密管理器
    /// </summary>
    /// <param name="mgr">管理器</param>
    private void MgrGet(out BaseEnDeCryptMgr mgr)
    {
        mgr = null;
        if (MgrExist()) mgr = _encryptMgrDict[_encryptMode];
    }
    
    // mgr存在判断
    private bool MgrExist()
    {
        return _encryptMgrDict.ContainsKey(_encryptMode);
    }

}




