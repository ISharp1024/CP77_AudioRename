using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MoveVsAudio : MonoBehaviour
{
    public string directory;
    public string targetDirectory;

    public void DivideM_F()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(@directory);
        FileInfo[] fileInfos = directoryInfo.GetFiles("*", SearchOption.AllDirectories);

        for (int i = 0; i < fileInfos.Length; i++)
        {
            string[] strs = fileInfos[i].Name.Split('_');
            if (strs[0] == "v" && fileInfos[i].Name.Contains("_f_"))
            {
                File.Move(@fileInfos[i].FullName, @targetDirectory + "\\" + fileInfos[i].Name);
            }
        }
    }
}
