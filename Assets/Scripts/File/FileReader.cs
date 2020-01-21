using System;
using UnityEngine;
/// <summary>
/// 这是一个读文件的静态工具类
/// 进行文件读操作，直接与文件接触
/// </summary>
public static class FileReader
{
    /// <summary>
    /// 这就是个空函数，供测试用的
    /// </summary>
    public static void Read() { }


    /// <summary>
    /// 根据路径读取
    /// </summary>
    /// <param name="path">指定路径</param>
    /// <param name="contents">读取到的内容</param>
    public static void Read(string path, out string contents)
    {
        contents = default;
        FileBase.PathAnalyse(path, out var dp);
        if (FileBase.PathCheck(dp, FileCheckMode.None))
            contents = ReadStart(path);

    }


    /// <summary>
    /// 执行读文件
    /// </summary>
    /// <param name="path">路径</param>
    /// <returns></returns>
    private static string ReadStart(string path)
    {
        try
        {
            // 开始读文件
            using (System.IO.StreamReader file = new System.IO.StreamReader(path))
            {
                var contents = file.ReadToEnd();
                file.Dispose();
                return contents;
            }
            //var reader = new StreamReader(_path);
            //contents = reader.ReadToEnd();
            //reader.Dispose();
        }
        catch (Exception e)
        {
            Debug.Log(string.Format("数据读出失败，异常信息抛出{0}：", e));
        }

        return default;
    }
}

