using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Convert : MonoBehaviour
{
    public string fileName;

    public void ModifyJson()
    {
        string path = "Assets/TXT/"+fileName+".json";
        //File.CreateText(path);

        if (!File.Exists(path))
            return;

        string[] strs = File.ReadAllLines(path);
        for (int i = 0; i < strs.Length; i++)
        {
            if (strs[i].Contains("stringId"))
            {
                strs[i] = strs[i].Insert(strs[i].Length - 1, "\"");
                strs[i] = strs[i].Insert(strs[i].IndexOf(":") + 2, "\"");
            }

            if (strs[i].Contains("base\\\\localization\\\\zh-cn\\\\vo\\\\"))
            {
                strs[i] = strs[i].Replace("base\\\\localization\\\\zh-cn\\\\vo\\\\", "");
            }

            if (strs[i].Contains("base\\\\localization\\\\zh-cn\\\\vo_helmet\\\\"))
            {
                strs[i] = strs[i].Replace("base\\\\localization\\\\zh-cn\\\\vo_helmet\\\\", "");
            }

            if (strs[i].Contains("base\\\\localization\\\\zh-cn\\\\vo_holocall\\\\"))
            {
                strs[i] = strs[i].Replace("base\\\\localization\\\\zh-cn\\\\vo_holocall\\\\", "");
            }

            if (strs[i].Contains("base\\\\localization\\\\zh-cn\\\\vo_rewinded\\\\"))
            {
                strs[i] = strs[i].Replace("base\\\\localization\\\\zh-cn\\\\vo_rewinded\\\\", "");
            }
        }
        File.WriteAllLines(path, strs);

        //AssetDatabase.Refresh();
    }

    public void ModifySubtileJsonBatch()
    {
        string path = "Assets/Resources/ENG/";
        DirectoryInfo json_direction = new DirectoryInfo(path);
        FileInfo[] json_files = json_direction.GetFiles("*", SearchOption.AllDirectories);

        for (int i = 0; i < json_files.Length; i++)
        {
            if (json_files[i].Name.EndsWith(".meta"))
            {
                continue;
            }

            ModifySubtileJson(path + json_files[i].Name);
        }

        //AssetDatabase.Refresh();
    }

    private void ModifySubtileJson(string path)
    {
        if (!File.Exists(path))
            return;
        string[] strs = File.ReadAllLines(path);
        for (int i = 0; i < strs.Length; i++)
        {
            if (strs[i].Contains("stringId"))
            {
                strs[i] = strs[i].Insert(strs[i].Length - 1, "\"");
                strs[i] = strs[i].Insert(strs[i].IndexOf(":") + 2, "\"");
            }

            if (i < 16 && i != 0)
            {
                strs[i] = "";
            }

            if (i > strs.Length - 4)
            {
                strs[i] = "";
            }
        }
        File.WriteAllLines(path, strs);
    }
}
