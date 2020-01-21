using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;

public static class JsonParser
{
    public static void ToJson<T>(T df, out string msgs)
    {
        msgs = null;
        try
        {
            msgs = JsonMapper.ToJson(df);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public static void ToObject<T>(string msg, out T df)
    {
        df = default;
        try
        {
            df = JsonMapper.ToObject<T>(msg);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

}
