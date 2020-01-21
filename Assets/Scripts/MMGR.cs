using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 框架的基类节点，允许核心Mgr的注册/卸载/管理/与外部访问
/// 允许框架扩展，不允许非框架类实例继承
/// </summary>
public abstract class MMGR : MonoBehaviour
{
    static MMGR()
    {
        _mgrDict = new Dictionary<string, MMGR>();
    }
    /// <summary>
    /// 框架Mgr实例字典
    /// </summary>
    private static Dictionary<string, MMGR> _mgrDict;
    /// <summary>
    /// 框架Mgr实例注册
    /// </summary>
    /// <typeparam name="T">注册实例类型</typeparam>
    /// <param name="t">注册实例</param>
    protected static void RegisterMgr<T>(T t) where T : MMGR
    {
        if (_mgrDict == null) _mgrDict = new Dictionary<string, MMGR>();
        if (_mgrDict.ContainsKey(typeof(T).Name)) return;
        if (t)
            _mgrDict.Add(typeof(T).Name, t);
    }
    /// <summary>
    /// 框架Mgr实例注册,版本日期20.1.16，备注：名称修改
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    protected static void Register<T>(T t) where T : MMGR
    {
        if (_mgrDict == null) _mgrDict = new Dictionary<string, MMGR>();
        if (_mgrDict.ContainsKey(typeof(T).Name)) return;
        if (t)
            _mgrDict.Add(typeof(T).Name, t);
    }

    /// <summary>
    /// 框架Mgr实例卸载
    /// </summary>
    /// <typeparam name="T">注册实例类型</typeparam>
    protected static void UnRegisterMgr<T>()
    {
        if (_mgrDict != null && _mgrDict.Count > 0 && _mgrDict.ContainsKey(typeof(T).Name))
            _mgrDict.Remove(typeof(T).Name);
    }
    /// <summary>
    /// 框架Mgr实例卸载,版本日期20.1.16，备注：简化调用方式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    protected static void UnRegister<T>(T t)
    {
        if (_mgrDict.IsExist() && _mgrDict.Count > 0 && _mgrDict.ContainsKey(typeof(T).Name))
            _mgrDict.Remove(typeof(T).Name);
    }

    /// <summary>
    /// 框架Mgr实例获取
    /// </summary>
    /// <typeparam name="T">实例类型</typeparam>
    /// <returns>实例</returns>
    public static T Mgr<T>() where T : MMGR
    {
        try
        {
            _mgrDict.TryGetValue(typeof(T).Name, out var mgr);
            return mgr as T;
        }
        catch
        {
            Debug.Log(string.Format("加载失败，{0}未注册", typeof(T).Name));
            return null;
        }
    }
    /// <summary>
    /// 框架Mgr实例是否存在
    /// </summary>
    /// <typeparam name="T">实例类型</typeparam>
    /// <returns>布尔值存在</returns>
    protected static bool ContainsMgr<T>()
    {
        _mgrDict.TryGetValue(typeof(T).Name, out var mgr);
        return mgr != null ? true : false;
    }
}
