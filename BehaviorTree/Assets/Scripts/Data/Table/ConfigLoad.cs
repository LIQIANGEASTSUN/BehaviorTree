using CommonUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigLoad : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(LoadConfig());
    }

    private byte[] byteData = new byte[] { };
    private string textContent = string.Empty;
    private IEnumerator LoadConfig()
    {
        yield return StartCoroutine(LoadData("Bina", "BehaviorTreeConfig.bytes", 1));

        DataCenter.behaviorData.LoadData(byteData);
    }

    IEnumerator LoadData(string directory, string name, int type)
    {
        string path = FileUtils.GetStreamingAssetsFilePath(name, directory);

        WWW www = new WWW(path);
        yield return www;
        string title = name.Substring(0, name.LastIndexOf("."));

        if (type == 0)
        {
            textContent = www.text;
        }
        else
        {
            byteData = www.bytes;
        }
        yield return true;
    }
}
