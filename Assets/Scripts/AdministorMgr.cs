using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdministorMgr : MMGR
{
    private Administor _administor;
    #region 全局变量区域

    

    #endregion


    private void Awake()
    {
        Register(this);
    }

    private void Start()
    {
        _administor = new Administor();

        var mgr = new TreeNodeDataContainerMgr();
        DirectorySearcher.Serach(string.Format("{0}/Contents", Application.dataPath), ref mgr);
        Mgr<TreeViewMgr>().TreeView.BuildTree(mgr.RootContainer);
        Mgr<TreeViewMgr>().TreeView.ReFreshTree();
    }


    private void OnDestroy()
    {
        UnRegister(this);
    }

    #region 数据读取处理


    #endregion

    #region 数据密文处理
    /// <summary>
    /// 数据加密
    /// </summary>
    /// <param name="msg">要加密的数据</param>
    public void Encrypt(string msg)
    {
        //Debug.Log(string.Format("需要加密的文本{0}", msg));
        if (_administor.IsExist())
        {
            _administor.Encrypt(msg, out var eMsg);
        }
    }

    /// <summary>
    /// 数据加密并抛出，一般用来做测试
    /// </summary>
    /// <param name="msg">要加密的数据</param>
    /// <param name="eMsg">抛出数据</param>
    public void Encrypt(string msg, out string eMsg)
    {
        //Debug.Log(string.Format("需要加密的文本{0}", msg));
        eMsg = default;
        if(_administor.IsExist())
        {
            _administor.Encrypt(msg, out eMsg);
        }
    }

    /// <summary>
    /// 数据解密并抛出，一般用来测试
    /// </summary>
    /// <param name="msg">要解密的数据</param>
    /// <param name="dMsg">解密后的数据</param>
    public void Decrypt(string msg, out string dMsg)
    {
        //Debug.Log(string.Format("需要解密的文本{0}", msg));
        dMsg = default;
        if (_administor != null)
        {
            _administor.Decrypt(msg, out dMsg);
        }
    }

    /// <summary>
    /// 数据解密并抛出，针对有缓存存在？？？？？？？？抱歉我忘了这个是干啥的了
    /// </summary>
    /// <param name="dMsg">解密后的数据</param>
    public void Decrypt(out string dMsg)
    {
        //Debug.Log(string.Format("需要解密的文本{0}", msg));
        dMsg = default;
        if (_administor != null)
        {
            _administor.Decrypt(out dMsg);
        }
    }
    #endregion
}
