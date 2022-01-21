using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

enum Language
{
    Chinese = 0,
    English
}

public class MainPanel : MonoBehaviour
{
    public Button btnRename;
    public InputField inputField;
    public Toggle toggleHeader;
    public Dropdown dropdownLanguage;

    private Language currentLanguage = Language.Chinese;

    void Start()
    {
        btnRename.onClick.AddListener(BtnOnClick);
        dropdownLanguage.onValueChanged.AddListener(DropdownOnChanged);
    }

    void BtnOnClick()
    {
        string input = @inputField.text;
        if (!Directory.Exists(input))
            return;

        TriggerIntereact(false);

        DirectoryInfo fileDirectory = new DirectoryInfo(input);
        FileInfo[] fileInfos = fileDirectory.GetFiles("*", SearchOption.AllDirectories);
        Dictionary<string, Variants> langDic;
        switch (currentLanguage)
        {
            case Language.Chinese:
                langDic = DataMgr.GetInstance().CHS_Subtile_Dictionary;
                break;
            case Language.English:
                langDic = DataMgr.GetInstance().ENG_Subtile_Dictionary;
                break;
            default:
                langDic = DataMgr.GetInstance().CHS_Subtile_Dictionary;
                break;
        }
        for (int i = 0; i < fileInfos.Length; i++)
        {
            Rename(fileInfos[i], i, langDic);
        }

        TriggerIntereact(true);
    }

    void Rename(FileInfo fileInfo, int i, Dictionary<string, Variants> dictionary)
    {
        string fileName = fileInfo.Name.Replace(fileInfo.Extension, "");
        if (DataMgr.GetInstance().Res_Dictionary.ContainsKey(fileName))
        {
            string ID = DataMgr.GetInstance().Res_Dictionary[fileName];
            if (ID == null || !dictionary.ContainsKey(ID))
                return;
            
            Variants variants = dictionary[ID];
            string source = @fileInfo.FullName;
            string desire;
            string name;
            if (fileName.Contains("_m_") && variants.male != null)
            {
                if(variants.male != null)
                {
                    HandleMale(variants, out name);
                }
                else
                {
                    HandleFemale(variants, out name);
                }
            }
            else
            {
                if (variants.female != null)
                {
                    HandleFemale(variants, out name);
                }
                else
                {
                    HandleMale(variants, out name);
                }
            }

            //首标
            if (toggleHeader.isOn)
            {
                string[] sArray = fileName.Split('_');
                desire = @fileInfo.DirectoryName + "\\" + sArray[0] + "_" + name;
            }
            else
            {
                desire = @fileInfo.DirectoryName + "\\" + name;
            }

            //重命名           
            if (File.Exists(desire + fileInfo.Extension))
                File.Move(source, desire + i.ToString() + fileInfo.Extension);
            else
            {
                File.Move(source, desire + fileInfo.Extension);
            }                
        }
    }

    string HandleKiroshi(Variants variants, bool male)
    {
        string str = "";
        bool trigger = false;
        string genderStr;

        if (male)
            genderStr = variants.male;
        else
            genderStr = variants.female;

        for (int i = 0; i < genderStr.Length; i++)
        {
            if (genderStr[i] == '\"')
            {
                trigger = !trigger;
                continue;
            }

            if (trigger)
            {
                str += genderStr[i];
            }
        }
        str = RemoveInvalidChar(str);
        return str;
    }

    string RemoveInvalidChar(string str)
    {
        //Regex -- find invalid chars
        string pattern = " *[\\:~#%&*{}/<>?|\"\n-]+ *";
        string replacement = " ";
        Regex regEx = new Regex(pattern);
        string sanitized = regEx.Replace(str, replacement);
        sanitized = sanitized.Replace("\\", "");
        return sanitized;
    }

    void HandleMale(Variants variants, out string name)
    {
        if(variants.male.Contains("<"))
        {
            name = HandleKiroshi(variants, true);
        }
        else
        {
            name = RemoveInvalidChar(variants.male);
        }
    }

    void HandleFemale(Variants variants, out string name)
    {
        if (variants.female.Contains("<"))
        {
            name = HandleKiroshi(variants, false);
        }
        else
        {
            name = RemoveInvalidChar(variants.female);
        }
    }

    void TriggerIntereact(bool on)
    {
        inputField.interactable = on;
        btnRename.interactable = on;
        dropdownLanguage.interactable = on;
        toggleHeader.interactable = on;
    }

    void DropdownOnChanged(int value)
    {
        currentLanguage = (Language)value;
    }
}
