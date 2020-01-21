using System.Collections.Generic;
/// <summary>
/// 信息节点实例
/// </summary>
[System.Serializable]
public class DataAccountPassword
{
    public DataAccountPassword() { }

    public DataAccountPassword(string account, string pwd, List<string> mailList)
    {
        this.Account = account;
        this.Pwd = pwd;
        this.MailList = mailList;
    }

    private string _account;
    private string _pwd;
    private List<string> _mailList;

    public string Account { get => _account; set => _account = value; }
    public string Pwd { get => _pwd; set => _pwd = value; }
    public List<string> MailList { get => _mailList; set => _mailList = value; }
}