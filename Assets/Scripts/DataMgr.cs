using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataMgr : BaseManager<DataMgr>
{
    public Dictionary<string, string> Res_Dictionary = new Dictionary<string, string>();
    public Dictionary<string, Variants> CHS_Subtile_Dictionary = new Dictionary<string, Variants>();
    public Dictionary<string, Variants> ENG_Subtile_Dictionary = new Dictionary<string, Variants>();

    /// <summary>
    /// 数据初始化，把Json文件中的数据加入到此管理器中
    /// </summary>
    public void Init()
    {
        //加载Resources文件夹下的json文件，获取它的内容
        string info1 = ResMgr.GetInstance().Load<TextAsset>("Json/voiceovermap").text;
        string info2 = ResMgr.GetInstance().Load<TextAsset>("Json/voiceovermap_1").text;
        string info3 = ResMgr.GetInstance().Load<TextAsset>("Json/voiceovermap_helmet").text;
        string info4 = ResMgr.GetInstance().Load<TextAsset>("Json/voiceovermap_holocall").text;
        string info5 = ResMgr.GetInstance().Load<TextAsset>("Json/voiceovermap_rewinded").text;

        //根据json文件的内容，解析成对应的数据结构，并存储起来
        AudioID_Dic_List tempList1 = JsonUtility.FromJson<AudioID_Dic_List>(info1);
        AudioID_Dic_List tempList2 = JsonUtility.FromJson<AudioID_Dic_List>(info2);
        AudioID_Dic_List tempList3 = JsonUtility.FromJson<AudioID_Dic_List>(info3);
        AudioID_Dic_List tempList4 = JsonUtility.FromJson<AudioID_Dic_List>(info4);
        AudioID_Dic_List tempList5 = JsonUtility.FromJson<AudioID_Dic_List>(info5);

        AddListToDic(tempList1);
        AddListToDic(tempList2);
        AddListToDic(tempList3);
        AddListToDic(tempList4);
        AddListToDic(tempList5);

        TextAsset[] chs_txtAssets = Resources.LoadAll<TextAsset>("CHS");
        for (int i = 0; i < chs_txtAssets.Length; i++)
        {
            string info = chs_txtAssets[i].text;

            if (!info.Contains("entries"))
                continue;
    
            SubtileID_List list = JsonUtility.FromJson<SubtileID_List>(info);
            AddListToDic(list, CHS_Subtile_Dictionary);
        }

        TextAsset[] eng_txtAssets = Resources.LoadAll<TextAsset>("ENG");
        for (int i = 0; i < eng_txtAssets.Length; i++)
        {
            string info = eng_txtAssets[i].text;
            if (!info.Contains("entries"))
                continue;
            SubtileID_List list = JsonUtility.FromJson<SubtileID_List>(info);
            AddListToDic(list, ENG_Subtile_Dictionary);
        }
    }

    public void Test()
    {

    }

    private void AddListToDic(AudioID_Dic_List list)
    {
        for (int i = 0; i < list.entries.Count; i++)
        {
            if (!Res_Dictionary.ContainsKey(list.entries[i].femaleResPath))
            {
                Res_Dictionary.Add(list.entries[i].femaleResPath, list.entries[i].stringId);
            }

            if (!Res_Dictionary.ContainsKey(list.entries[i].maleResPath))
            {
                Res_Dictionary.Add(list.entries[i].maleResPath, list.entries[i].stringId);
            }
        }
    }

    private void AddListToDic(SubtileID_List list, Dictionary<string, Variants> dictionary)
    {
        for (int j = 0; j < list.entries.Count; j++)
        {
            if(list.entries[j].stringId != null && !dictionary.ContainsKey(list.entries[j].stringId))
            {
                Variants variants = new Variants();
                variants.female = list.entries[j].femaleVariant;
                variants.male = list.entries[j].maleVariant;

                dictionary.Add(list.entries[j].stringId, variants);
            }
        }
    }
}

//语音文件结构，ID，男女V路径
[System.Serializable]
public class AuidoID_Dic
{
    public string stringId;
    public string femaleResPath;
    public string maleResPath;
}

public class AudioID_Dic_List
{
    public List<AuidoID_Dic> entries;
}

//字幕文件结构，ID，字幕内容
[System.Serializable]
public class SubtileID
{
    public string stringId;
    public string femaleVariant;
    public string maleVariant;
}

public class SubtileID_List
{
    public List<SubtileID> entries;
}

public class Variants
{
    public string female;
    public string male;
}


