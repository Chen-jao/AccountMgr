using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReultPannelMgr : MMGR
{
    [SerializeField]
    private InputField _account = default;
    [SerializeField]
    private InputField _password = default;

    private void Awake()
    {
        Register(this);
        ClearData();
    }

    private void OnDestroy()
    {
        UnRegister(this);
    }

    #region 方法区域
        
    public void Show(DataAccountPassword data)
    {
        gameObject.SetActive(true);
        SetData(data);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        ClearData();
    }

    private void SetData(DataAccountPassword data)
    {
        if (_account.IsExist())
            _account.text = data.Account;
        if (_password.IsExist())
            _password.text = data.Pwd;
    }

    private void ClearData()
    {
        if (_account.IsExist()) _account.text = default;
        if (_password.IsExist()) _password.text = default;
    }


    public DataAccountPassword InfoOut() => new DataAccountPassword()
    {
        Account = _account.IsExist() ? _account.text : default,
        Pwd = _password.IsExist() ? _password.text : default,
        MailList = default
    };

    #endregion
}
