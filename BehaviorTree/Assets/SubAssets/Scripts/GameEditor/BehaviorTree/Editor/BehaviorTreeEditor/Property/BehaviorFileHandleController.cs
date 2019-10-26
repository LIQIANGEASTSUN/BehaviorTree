using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System;

namespace BehaviorTree
{

    public class BehaviorFileHandleController
    {
        private BehaviorFileHandleModel _fileHandleModel;
        private BehaviorFileHandleView _fileHandleView;

        public BehaviorFileHandleController()
        {
            Init();
        }

        public void Init()
        {
            _fileHandleModel = new BehaviorFileHandleModel();
            _fileHandleView = new BehaviorFileHandleView();
        }

        public void OnDestroy()
        {

        }

        public void OnGUI()
        {
            _fileHandleView.Draw();
        }

    }

    public class BehaviorFileHandleModel
    {


    }

    public class BehaviorFileHandleView
    {

        public void Draw()
        {
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("选择文件"))
                    {
                        SelectFile(BehaviorManager.Instance.FilePath);
                    }

                    if (GUILayout.Button("保存"))
                    {
                        CreateSaveFile(BehaviorManager.Instance.FileName);
                        AssetDatabase.Refresh();
                    }

                    if (GUILayout.Button("删除"))
                    {
                        DeleteFile(BehaviorManager.Instance.FileName);
                        AssetDatabase.Refresh();
                    }
                    if (GUILayout.Button("批量更新"))
                    {
                        UpdateAllFile(BehaviorManager.Instance.FilePath);
                        AssetDatabase.Refresh();
                    }
                }
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(5);

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("文件名", GUILayout.Width(80));
                    BehaviorManager.Instance.FileName = EditorGUILayout.TextField(BehaviorManager.Instance.FileName);
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }

        private static void SelectFile(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            GUILayout.Space(8);

            string filePath = EditorUtility.OpenFilePanel("选择技能ID文件", path, "bytes");
            if (!string.IsNullOrEmpty(filePath))
            {
                string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
                if (!string.IsNullOrEmpty(fileName))
                {
                    if (null != BehaviorManager.behaviorLoadFile)
                    {
                        BehaviorManager.behaviorLoadFile(fileName);
                    }
                }
            }
        }

        private static void CreateSaveFile(string fileName)
        {
            if (null != BehaviorManager.behaviorSaveFile)
            {
                BehaviorManager.behaviorSaveFile(fileName);
            }
        }

        private static void DeleteFile(string fileName)
        {
            if (null != BehaviorManager.behaviorDeleteFile)
            {
                BehaviorManager.behaviorDeleteFile(fileName);
            }
        }

        private static void UpdateAllFile(string filePath)
        {
            DirectoryInfo dInfo = new DirectoryInfo(filePath);
            FileInfo[] fileInfoArr = dInfo.GetFiles("*.bytes", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < fileInfoArr.Length; ++i)
            {
                string fullName = fileInfoArr[i].FullName;
                BehaviorReadWrite readWrite = new BehaviorReadWrite();
                BehaviorTreeData treeData = readWrite.ReadJson(fullName);

                treeData = UpdateData(treeData);

                string jsonFilePath = System.IO.Path.GetDirectoryName(filePath) + "/Json/" + System.IO.Path.GetFileName(fullName);
                bool value = readWrite.WriteJson(treeData, jsonFilePath);
                if (!value)
                {
                    Debug.LogError("WriteError:" + jsonFilePath);
                }
            }
        }

        private static BehaviorTreeData UpdateData(BehaviorTreeData treeData)
        {
            return treeData;
        }

    }
}
