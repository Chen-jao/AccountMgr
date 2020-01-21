
using System;
using UnityEngine;

public static class Exten_Object
{
    /// <summary>
    /// 判断对象实例是否存在
    /// </summary>
    /// <param name="o">实例</param>
    /// <returns></returns>
    public static bool IsExist<T>(this T t)
    {
        return t != default;
    }
    /// <summary>
    /// 判断对象实例是否存在
    /// </summary>
    /// <param name="o">实例</param>
    /// <returns></returns>
    public static bool IsEmpty<T>(this T t)
    {
        return t == default;
    }

    /// <summary>
    /// 泛型实例转object 装箱
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <param name="o"></param>
    public static void CastTo<T>(this T t, out object o)
    {
        o = default;
        try
        {
            o = t as object;
        }
        catch (Exception e)
        {
            Log(e.ToString());
        }
    }
    /// <summary>
    /// object转泛型实例 拆箱
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="o"></param>
    /// <param name="result"></param>
    public static void CastTo<T>(this object o, out T result)
    {
        result = default;
        try
        {
            var type = typeof(T);
            if (type.IsExist())
                if (type.Equals(typeof(Enum)))
                    result = (T)Enum.Parse(type, o.ToString());
                else if (type.Equals(typeof(Guid)))
                    result = (T)(object)Guid.Parse(o.ToString());
                else
                    result = (T)Convert.ChangeType(o, type);
        }
        catch (Exception e)
        {
            Log(e.ToString());
        }
    }

    /// <summary>
    /// 返回实例的类型名字
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    public static string Name<T>(this T t)
    {
        return typeof(T).Name;
    }

    private static void Log(string msg)
    {
        Debug.Log(string.Format("cast defeat, the rewason is : {0}", msg));
    }
}