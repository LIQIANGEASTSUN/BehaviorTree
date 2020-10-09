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
            if (null != BehaviorManager.behaviorSelectFile)
            {
                string fileName = BehaviorManager.behaviorSelectFile();
                if (null != BehaviorManager.behaviorLoadFile)
                {
                    BehaviorManager.behaviorLoadFile(fileName);
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
            TableRead.Instance.Init();
            string csvPath = string.Format("{0}/StreamingAssets/CSV/", Application.dataPath); // Extend.GameUtils.CombinePath(Application.dataPath, "StreamingAssets", "CSV"); //string.Format("{0}/StreamingAssets/CSV/", Application.dataPath);
            TableRead.Instance.ReadCustomPath(csvPath);


            DirectoryInfo dInfo = new DirectoryInfo(filePath);
            FileInfo[] fileInfoArr = dInfo.GetFiles("*.bytes", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < fileInfoArr.Length; ++i)
            {
                string fullName = fileInfoArr[i].FullName;
                BehaviorReadWrite readWrite = new BehaviorReadWrite();
                BehaviorTreeData treeData = readWrite.ReadJson(fullName);

                if (null != BehaviorManager.behaviorStandardID)
                {
                    treeData = BehaviorManager.behaviorStandardID(treeData);
                }

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
            for (int i = 0; i < treeData.parameterList.Count; ++i)
            {
                BehaviorParameter parameter = treeData.parameterList[i];
                parameter = UpdateParameter(parameter);
            }

            for (int i = 0; i < treeData.nodeList.Count; ++i)
            {
                NodeValue nodeValue = treeData.nodeList[i];
                
                for (int j = 0; j < nodeValue.parameterList.Count; ++j)
                {
                    BehaviorParameter parameter = nodeValue.parameterList[j];
                    parameter = UpdateParameter(parameter);
                }
            }

            return treeData;
        }

        private static BehaviorParameter UpdateParameter(BehaviorParameter parameter)
        {
            return parameter;
        }

        private static BehaviorParameter GetTableParameter(string parameterName)
        {
            string tableName = "BehaviorTree";
            List<int> keyList = TableRead.Instance.GetKeyList(tableName);

            for (int i = 0; i < keyList.Count; ++i)
            {
                int key = keyList[i];

                string enName = TableRead.Instance.GetData(tableName, key, "EnName");
                if (enName.CompareTo(parameterName) != 0)
                {
                    continue;
                }

                string cnName = TableRead.Instance.GetData(tableName, key, "CnName");
                int type = int.Parse(TableRead.Instance.GetData(tableName, key, "Type"));

                BehaviorParameter parameter = new BehaviorParameter();
                parameter.parameterName = enName;
                parameter.CNName = cnName;
                parameter.parameterType = type;

                return parameter;
            }

            return null;
        }

        private static BehaviorTreeData ChangeSubTree(BehaviorTreeData treeData)
        {
            //int a = 0;
            //if (a == 0)
            //{
            //    return treeData;
            //}

            //if (treeData.fileName.CompareTo("Monster") != 0)// 
            //{
            //    return treeData;
            //}

            //int subTreeId = 95;
            //int nodeId = 39;
            //List<NodeValue> nodeList = new List<NodeValue>();
            //NodeValue node = GetNode(treeData, nodeId);
            //if (null == node)
            //{
            //    return treeData;
            //}

            //{
            //    NodeValue parentNode = GetNode(treeData, node.parentNodeID);
            //    if (null != parentNode)
            //    {
            //        for (int i = 0; i < parentNode.childNodeList.Count; ++i)
            //        {
            //            if (parentNode.childNodeList[i] == nodeId)
            //            {
            //                parentNode.childNodeList.RemoveAt(i);
            //            }
            //        }
            //    }
            //}

            //NodeValue subTreeNode = GetNode(treeData, subTreeId);
            //if (null == subTreeNode)
            //{
            //    return treeData;
            //}

            //{
            //    subTreeNode.childNodeList.Clear();
            //    subTreeNode.childNodeList.Add(nodeId);
            //}

            //{
            //    node.parentSubTreeNodeId = subTreeId;
            //    node.subTreeEntry = true;
            //}

            //nodeList.Add(node);

            //FindChild(treeData, nodeId, ref nodeList);
            //for (int i = 0; i < nodeList.Count; ++i)
            //{
            //    //ProDebug.Logger.LogError(nodeList[i].id);

            //    nodeList[i].parentSubTreeNodeId = subTreeId;
            //}

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

            AssetDatabase.Refresh();
        }

        public static void ImportParameter()
        {
            BehaviorTreeData behaviorData = BehaviorManager.Instance.BehaviorTreeData;
            behaviorData = ImportParameter(behaviorData);
        }

        private static BehaviorTreeData ImportParameter(BehaviorTreeData behaviorData)
        {
            string fileName = "BehaviorTree";
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

                string longContent = TableRead.Instance.GetData(fileName, key, "LongValue");
                long longValue = long.Parse(intContent);

                string boolContent = TableRead.Instance.GetData(fileName, key, "BoolValue");
                bool boolValue = (int.Parse(boolContent) == 1);

                string stringValue = TableRead.Instance.GetData(fileName, key, "StringValue");

                BehaviorParameter parameter = new BehaviorParameter();
                parameter.parameterName = EnName;
                parameter.CNName = cnName;
                parameter.compare = (int)BehaviorCompare.EQUALS;
                parameter.parameterType = type;

                parameter.floatValue = floatValue;
                parameter.intValue = intValue;
                parameter.longValue = longValue;
                parameter.boolValue = boolValue;
                parameter.stringValue = stringValue;

                if (parameterDic.ContainsKey(EnName))
                {
                    if (parameterDic[EnName].parameterType != type)
                    {
                        Debug.LogError("已经存在参数:" + EnName + "   type:" + (BehaviorParameterType)parameterDic[EnName].parameterType + "   newType:" + (BehaviorParameterType)type);
                    }

                    parameterDic[EnName].CloneFrom(parameter);
                }
                else
                {
                    behaviorData.parameterList.Add(parameter);
                }
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