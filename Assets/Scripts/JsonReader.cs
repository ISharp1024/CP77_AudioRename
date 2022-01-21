using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine.UI;

public class JsonReader : MonoBehaviour
{
    public Button assembleBtn;
    public InputField jsonField;

    void Start()
    {
        assembleBtn.onClick.AddListener(Func_Btn);
    }

    public void CreateTXT()
    {
        string JSON_Path = "Assets/JSON/";
        string TXT_Path = "Assets/TXT/";

        //获取指定路径下面的所有资源文件  
        if (Directory.Exists(JSON_Path))
        {
            DirectoryInfo json_direction = new DirectoryInfo(JSON_Path);

            FileInfo[] json_files = json_direction.GetFiles("*", SearchOption.AllDirectories);

            
            for (int i = 0; i < json_files.Length; i++)
            {
                if (json_files[i].Name.EndsWith(".meta"))
                {
                    continue;
                }

                int rm_index = json_files[i].Name.Length -5;
                int rm_length = ".json".Length;

                File.CreateText(TXT_Path + json_files[i].Name.Remove(rm_index, rm_length) + ".txt");
            }
            //AssetDatabase.Refresh();
        }
    }

    public void ClearTXTFolder()
    {
        string TXT_Path = "Assets/TXT/";
        if (Directory.Exists(TXT_Path))
        {
            DirectoryInfo txt_direction = new DirectoryInfo(TXT_Path);

            FileInfo[] txt_files = txt_direction.GetFiles("*", SearchOption.AllDirectories);

            for (int i = 0; i < txt_files.Length; i++)
            {
                if (txt_files[i].Name.EndsWith(".meta"))
                {
                    continue;
                }

                //AssetDatabase.DeleteAsset(TXT_Path + txt_files[i].Name);               
            }
            //AssetDatabase.Refresh();
        }
        
    }

    public void ConvertJson()
    {
        string JSON_Path = "Assets/JSON/";
        string TXT_Path = "Assets/TXT/";

        //获取指定路径下面的所有资源文件  
        if (Directory.Exists(JSON_Path))
        {
            DirectoryInfo json_direction = new DirectoryInfo(JSON_Path);
            DirectoryInfo txt_direction = new DirectoryInfo(TXT_Path);

            FileInfo[] json_files = json_direction.GetFiles("*", SearchOption.AllDirectories);
            FileInfo[] txt_files = txt_direction.GetFiles("*", SearchOption.AllDirectories);

            for (int i = 0; i < json_files.Length; i++)
            {
                if (json_files[i].Name.EndsWith(".meta"))
                {
                    continue;
                }

                string first_clean_str = "";
                string str = File.ReadAllText(JSON_Path + json_files[i].Name, Encoding.UTF8);

                Regex regex1 = new Regex(@"[\u4e00-\u9fa5_a-zA-Z0-9\s·]");
                Regex regex2 = new Regex(@"[^\x00-\xff]");

                for (int j = 0; j < str.Length; j++)
                {
                    if ((regex1.IsMatch(str[j].ToString()) || regex2.IsMatch(str[j].ToString()))
                        && str[j] != 65533 && str[j] != 11 && str[j] != 12 || str[j] == 37)
                    {
                        first_clean_str += str[j];
                    }
                }

                File.WriteAllText(TXT_Path + txt_files[i].Name, first_clean_str);                
            }

            //进一步删除乱码
            for (int n = 0; n < txt_files.Length; n++)
            {
                if (txt_files[n].Name.EndsWith(".meta"))
                {
                    continue;
                }

                List<int> indexs = new List<int>(); 
                string[] txt_Lines = File.ReadAllLines(TXT_Path + txt_files[n].Name);
                for (int i = 0; i < txt_Lines.Length; i++)
                {
                    Regex regex = new Regex(@"[\u4e00-\u9fa5]");
                    if(txt_Lines[i].Length > 5 && regex.IsMatch(txt_Lines[i]))
                    {
                        indexs.Add(i);
                    }
                }
                string[] new_Lines = new string[indexs.Count];
                for (int i = 0; i < new_Lines.Length; i++)
                {
                    new_Lines[i] = txt_Lines[indexs[i]];
                }
                File.WriteAllLines(TXT_Path + txt_files[n].Name, new_Lines);
            }

            //加入换行
            for (int i = 0; i < txt_files.Length; i++)
            {
                if (txt_files[i].Name.EndsWith(".meta"))
                {
                    continue;
                }
                List<string> line_list = new List<string>();
                string[] txt_Lines = File.ReadAllLines(TXT_Path + txt_files[i].Name);
                for (int j = 0; j < txt_Lines.Length; j++)
                {
                    line_list.Add(txt_Lines[j]);
                    line_list.Add("");
                }

                string[] new_Lines = new string[line_list.Count];
                for (int l = 0; l < new_Lines.Length; l++)
                {
                    new_Lines[l] = line_list[l];
                }
                File.WriteAllLines(TXT_Path + txt_files[i].Name, new_Lines);
            }
        }

        //AssetDatabase.Refresh();
    }

    public void AssembleJson()
    {
        string TXT_Path = "Assets/TXT/Assemble.txt";
        string JSON_Path = "Assets/JSON/";

        File.CreateText(TXT_Path).Dispose();

        DirectoryInfo json_direction = new DirectoryInfo(JSON_Path);
        FileInfo[] json_files = json_direction.GetFiles("*", SearchOption.AllDirectories);

        List<string> str = new List<string>();
        for (int i = 0; i < json_files.Length; i++)
        {
            if (json_files[i].Name.EndsWith(".meta"))
            {
                continue;
            }

            str.Add(json_files[i].Name);
            string[] strArray = File.ReadAllLines(JSON_Path + json_files[i].Name, Encoding.UTF8);
            for (int j = 0; j < strArray.Length; j++)
            {
                str.Add(strArray[j]);
            }
            str.Add(" ");
        }
        File.WriteAllLines(TXT_Path, str);
        //AssetDatabase.Refresh();
    }

    public void Func_Btn()
    {
        string JSON_Path = @jsonField.text + "\\";
        string TXT_Path = @jsonField.text + "\\" + "Assemble.txt";

        if(Directory.Exists(JSON_Path))
        {
            File.CreateText(TXT_Path).Dispose();

            DirectoryInfo json_direction = new DirectoryInfo(JSON_Path);
            FileInfo[] json_files = json_direction.GetFiles("*", SearchOption.AllDirectories);

            List<string> str = new List<string>();
            for (int i = 0; i < json_files.Length; i++)
            {
                if (json_files[i].Name.EndsWith(".txt"))
                {
                    continue;
                }

                str.Add(json_files[i].Name);
                string[] strArray = File.ReadAllLines(JSON_Path + json_files[i].Name, Encoding.UTF8);
                for (int j = 0; j < strArray.Length; j++)
                {
                    str.Add(strArray[j]);
                }
                str.Add(" ");
            }
            File.WriteAllLines(TXT_Path, str);
        }        
    }
}
