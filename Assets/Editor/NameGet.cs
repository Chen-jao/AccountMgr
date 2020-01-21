using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

public class NameGet : Editor
{
    [MenuItem("Mytools/Print")]
    public static void PrintName()
    {
        var list = Selection.gameObjects;

        StringBuilder sb = new StringBuilder();

        foreach(var ch in list)
        {
            sb.Append(string.Format("\"{0}\",", ch.name));
        }

        Debug.Log(sb);
    }
}
