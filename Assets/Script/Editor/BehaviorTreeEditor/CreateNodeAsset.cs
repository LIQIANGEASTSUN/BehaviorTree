using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using BehaviorTree;

public class CreateNodeAsset {

    // 在菜单栏创建功能项
    [MenuItem("Window/CreateNodeAsset")]
    static void Create()
    {
        string assetName = (typeof(NodeAsset).ToString());

        CreateAsset(null, assetName);
    }

    public static void CreateAsset(NodeValue nodeValue, string fileName)
    {
        // 实例化类  Bullet
        ScriptableObject nodeAsset = ScriptableObject.CreateInstance<NodeAsset>();

        // 如果实例化 Bullet 类为空，返回
        if (!nodeAsset)
        {
            Debug.LogWarning("Bullet not found");
            return;
        }

        fileName = string.IsNullOrEmpty(fileName) ? (typeof(NodeAsset).ToString()) : fileName;
        // 自定义资源保存路径
        //将类名 Bullet 转换为字符串
        //拼接保存自定义资源（.asset） 路径
        string path = string.Format("Assets/NodeAsset/{0}.asset", fileName);

        if (File.Exists(path))
        {
            if (!EditorUtility.DisplayDialog("已存在文件", "是否替换新文件", "替换", "取消"))
            {
                return;
            }
        }

        // 如果项目总不包含该路径，创建一个
        string directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        NodeAsset node = (NodeAsset)nodeAsset;
        node.nodeValue = nodeValue;

        EditorUtility.SetDirty(nodeAsset);

        // 生成自定义资源到指定路径
        AssetDatabase.CreateAsset(nodeAsset, path);

        EditorUtility.SetDirty(nodeAsset);

        AssetDatabase.Refresh();
    }
}