public class BaseEnDeCryptMgr
{
    /// <summary>
    /// 加密，返回
    /// </summary>
    /// <param name="msg">需要加密的文本</param>
    /// <returns>加密后的文本</returns>
    public virtual string Encrypt(string msg) { return default; }
    /// <summary>
    /// 解密，返回
    /// </summary>
    /// <param name="msg">需要解密的文本</param>
    /// <returns>解密后的文本</returns>
    public virtual string Decrypt(string msg) { return default; }
}
