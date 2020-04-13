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
                    if (GUILayout.Button("合并"))
                    {
                        MergeFile(BehaviorManager.Instance.FilePath);
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

                treeData = UpdateData( treeData);

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

        private static void MergeFile(string filePath)
        {
            DirectoryInfo direcotryInfo = Directory.CreateDirectory(filePath);
            FileInfo[] fileInfoArr = direcotryInfo.GetFiles("*.bytes", SearchOption.TopDirectoryOnly);

            List<PBConfigWriteFile> fileList = new List<PBConfigWriteFile>();
            for (int i = 0; i < fileInfoArr.Length; ++i)
            {
                string fullName = fileInfoArr[i].FullName;

                BehaviorReadWrite readWrite = new BehaviorReadWrite();
                BehaviorTreeData behaviorTreeData = readWrite.ReadJson(fullName);

                string content = LitJson.JsonMapper.ToJson(behaviorTreeData);
                byte[] byteData = System.Text.Encoding.Default.GetBytes(content);

                string fileName = System.IO.Path.GetFileNameWithoutExtension(fullName);
                if (byteData.Length <= 0)
                {
                    Debug.LogError("无效得配置文件");
                    return;
                }

                PBConfigWriteFile skillConfigWriteFile = new PBConfigWriteFile();
                skillConfigWriteFile.filePath = filePath;
                skillConfigWriteFile.byteData = byteData;
                fileList.Add(skillConfigWriteFile);

                Debug.Log("end mergeFile:" + filePath);
            }

            ByteBufferWrite bbw = new ByteBufferWrite();
            bbw.WriteInt32(fileList.Count);

            int start = 4 + fileList.Count * (4 + 4);
            for (int i = 0; i < fileList.Count; ++i)
            {
                PBConfigWriteFile skillConfigWriteFile = fileList[i];
                bbw.WriteInt32(start);
                bbw.WriteInt32(skillConfigWriteFile.byteData.Length);
                start += skillConfigWriteFile.byteData.Length;
            }

            for (int i = 0; i < fileList.Count; ++i)
            {
                PBConfigWriteFile skillHsmWriteFile = fileList[i];
                bbw.WriteBytes(skillHsmWriteFile.byteData, skillHsmWriteFile.byteData.Length);
            }

            {
                string mergeFilePath = string.Format("{0}/StreamingAssets/Bina/BehaviorTreeConfig.bytes", Application.dataPath);

                if (System.IO.File.Exists(mergeFilePath))
                {
                    System.IO.File.Delete(mergeFilePath);
                    AssetDatabase.Refresh();
                }
                byte[] byteData = bbw.GetBytes();
                FileReadWrite.Write(mergeFilePath, byteData);
            }

        }

        public static void ImportParameter()
        {
            BehaviorTreeData behaviorData = BehaviorManager.Instance.BehaviorTreeData;
            string fileName = "BehaviorTree";

            behaviorData = ImportParameter(behaviorData, fileName);
        }

        private static BehaviorTreeData ImportParameter(BehaviorTreeData behaviorData, string fileName)
        {
            Debug.LogError(fileName);
            TableRead.Instance.Init();
            string csvPath =string.Format("{0}/StreamingAssets/CSV/", Application.dataPath); // Extend.GameUtils.CombinePath(Application.dataPath, "StreamingAssets", "CSV"); //string.Format("{0}/StreamingAssets/CSV/", Application.dataPath);
            TableRead.Instance.ReadCustomPath(csvPath);

            // Debug.LogError(filePath + "   " + fileName);
            List<int> keyList = TableRead.Instance.GetKeyList(fileName);

            Dictionary<string, BehaviorParameter> parameterDic = new Dictionary<string, BehaviorParameter>();
            for (int i = 0; i < behaviorData.parameterList.Count; ++i)
            {
                BehaviorParameter parameter = behaviorData.parameterList[i];
                parameterDic[parameter.parameterName] = parameter;
            }

            for (int i = 0; i < keyList.Count; ++i)
            {
                int key = keyList[i];
                string EnName = TableRead.Instance.GetData(fileName, key, "EnName");
                string cnName = TableRead.Instance.GetData(fileName, key, "CnName");
                string typeName = TableRead.Instance.GetData(fileName, key, "Type");
                int type = int.Parse(typeName);

                string floatContent = TableRead.Instance.GetData(fileName, key, "FloatValue");
                float floatValue = float.Parse(floatContent);

                string intContent = TableRead.Instance.GetData(fileName, key, "IntValue");
                int intValue = int.Parse(intContent);

                string boolContent = TableRead.Instance.GetData(fileName, key, "BoolValue");
                bool boolValue = (int.Parse(boolContent) == 1);

                if (parameterDic.ContainsKey(EnName))
                {
                    if (parameterDic[EnName].parameterType != type)
                    {
                        Debug.LogError("已经存在参数:" + EnName + "   type:" + (BehaviorParameterType)parameterDic[EnName].parameterType + "   newType:" + (BehaviorParameterType)type);
                    }
                    else
                    {
                        Debug.LogError("已经存在参数:" + EnName);
                    }
                    parameterDic.Remove(EnName);

                    for (int j = 0; j < behaviorData.parameterList.Count; ++j)
                    {
                        BehaviorParameter cacheParameter = behaviorData.parameterList[j];
                        if (cacheParameter.parameterName == EnName)
                        {
                            behaviorData.parameterList.RemoveAt(j);
                            break;
                        }
                    }

                    //continue;
                }

                //Debug.LogError(EnName + "    " +cnName + "    " + typeName);

                BehaviorParameter parameter = new BehaviorParameter();
                parameter.parameterName = EnName;
                parameter.CNName = cnName;
                parameter.compare = (int)BehaviorCompare.EQUALS;
                parameter.parameterType = type;
                parameter.boolValue = false;

                if (type == (int)BehaviorParameterType.Float)
                {
                    parameter.floatValue = floatValue;
                }

                if (type == (int)BehaviorParameterType.Int)
                {
                    parameter.intValue = intValue;
                }

                if (type == (int)BehaviorParameterType.Float)
                {
                    parameter.boolValue = boolValue;
                }

                behaviorData.parameterList.Add(parameter);
            }

            foreach (var kv in parameterDic)
            {
                Debug.LogError("==========缺失的参数:" + kv.Key);
            }

            return behaviorData;
        }

    }
}


public class PBConfigWriteFile
{
    public string filePath;
    public byte[] byteData;
}