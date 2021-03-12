﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System;
using System.Reflection;

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
            string csvPath = string.Format("{0}/StreamingAssets/CSVAssets/", Application.dataPath); // Extend.GameUtils.CombinePath(Application.dataPath, "StreamingAssets", "CSV"); //string.Format("{0}/StreamingAssets/CSV/", Application.dataPath);
            TableRead.Instance.ReadCustomPath(csvPath);

            //CompareTableParameter();

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

                CheckChildID(treeData, treeData.rootNodeId);

                treeData = UpdateData(treeData);

                string fileName = System.IO.Path.GetFileNameWithoutExtension(fullName);

                string jsonFilePath = System.IO.Path.GetDirectoryName(filePath) + "/Json/" + fileName + ".bytes";
                bool value = readWrite.WriteJson(treeData, jsonFilePath);
                if (!value)
                {
                    Debug.LogError("WriteError:" + jsonFilePath);
                }
            }
        }

        private static BehaviorTreeData UpdateData(BehaviorTreeData treeData)
        {
            //treeData = CheckParameter(treeData);
            return treeData;
        }

        private static BehaviorTreeData CheckParameter(BehaviorTreeData treeData)
        {
            for (int i = treeData.parameterList.Count - 1; i >= 0; --i)
            {
                BehaviorParameter parameter = treeData.parameterList[i];
                if (parameter.parameterName.CompareTo(BTConstant.IsSurvial) == 0)
                {
                    //ProDebug.Logger.LogError(3 + "    " + treeData.fileName);
                    treeData.parameterList.RemoveAt(i);
                }
            }

            for (int i = 0; i < treeData.nodeList.Count; ++i)
            {
                NodeValue nodeValue = treeData.nodeList[i];

                for (int j = 0; j < nodeValue.parameterList.Count; ++j)
                {
                    BehaviorParameter parameter = nodeValue.parameterList[j];
                    if (parameter.parameterName.CompareTo(BTConstant.IsSurvial) == 0)
                    {
                        //ProDebug.Logger.LogError(1 + "    " + treeData.fileName + "     " + nodeValue.id);
                    }
                }

                for (int j = 0; j < nodeValue.conditionGroupList.Count; ++j)
                {
                    ConditionGroup group = nodeValue.conditionGroupList[j];
                    for (int k = 0; k < group.parameterList.Count; ++k)
                    {
                        if (group.parameterList[k].CompareTo(BTConstant.IsSurvial) == 0)
                        {
                            //ProDebug.Logger.LogError(2 + "   " + treeData.fileName + "     " + nodeValue.id);
                        }
                    }
                }
            }

            return treeData;
        }

        private static void CheckChildID(BehaviorTreeData treeData, int id)
        {
            NodeValue nodeValue = treeData.nodeList.Find((a)=> {
                return a.id == id;
            });

            if (null == nodeValue || nodeValue.childNodeList.Count <= 0)
            {
                return;
            }

            HashSet<int> hash = new HashSet<int>();
            for (int i = nodeValue.childNodeList.Count - 1; i >= 0 ; --i)
            {
                int childId = nodeValue.childNodeList[i];
                if (hash.Contains(childId))
                {
                    nodeValue.childNodeList.RemoveAt(i);
                    Debug.LogError(treeData.fileName + "   NodeValue:" + nodeValue.id + "  Has Same Child:" + childId);
                }
                else
                {
                    hash.Add(childId);
                }

                CheckChildID(treeData, childId);
            }
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
                behaviorTreeData = RemoveInvalidParameter(behaviorTreeData);

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
                string mergeFilePath = string.Format("{0}/StreamingAssets/Bina/behavior_tree_config.bytes", Application.dataPath);

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
            string fileName = "table_behaviortree";
            TableRead.Instance.Init();
            string csvPath =string.Format("{0}/StreamingAssets/CSVAssets/", Application.dataPath); // Extend.GameUtils.CombinePath(Application.dataPath, "StreamingAssets", "CSV"); //string.Format("{0}/StreamingAssets/CSV/", Application.dataPath);
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

        private static BehaviorTreeData RemoveInvalidParameter(BehaviorTreeData behaviorData)
        {
            HashSet<string> _usedParameterHash = new HashSet<string>();

            for (int i = 0; i < behaviorData.nodeList.Count; ++i)
            {
                NodeValue nodeValue = behaviorData.nodeList[i];
                
                for (int j = 0; j < nodeValue.parameterList.Count; ++j)
                {
                    BehaviorParameter parameter = nodeValue.parameterList[j];
                    if (!_usedParameterHash.Contains(parameter.parameterName))
                    {
                        _usedParameterHash.Add(parameter.parameterName);
                    }
                }

                for (int j = 0; j < nodeValue.conditionGroupList.Count; ++j)
                {
                    ConditionGroup group = nodeValue.conditionGroupList[j];
                    for (int k = 0; k < group.parameterList.Count; ++k)
                    {
                        string name = group.parameterList[k];
                        if (!_usedParameterHash.Contains(name))
                        {
                            _usedParameterHash.Add(name);
                        }
                    }
                }
            }

            for (int i = behaviorData.parameterList.Count - 1; i >= 0; --i)
            {
                BehaviorParameter parameter = behaviorData.parameterList[i];
                if (!_usedParameterHash.Contains(parameter.parameterName))
                {
                    behaviorData.parameterList.RemoveAt(i);
                }
            }

            return behaviorData;
        }

        private static void CompareTableParameter()
        {
            // Debug.LogError(filePath + "   " + fileName);
            List<int> keyList = TableRead.Instance.GetKeyList("table_behaviortree");
            HashSet<string> tableHash = new HashSet<string>();
            for (int i = 0; i < keyList.Count; ++i)
            {
                string enName = TableRead.Instance.GetData("table_behaviortree", keyList[i], "EnName");
                tableHash.Add(enName);
            }

            FieldInfo[] infoArr = typeof(BTConstant).GetFields(BindingFlags.Static | BindingFlags.Public);
            HashSet<string> btConstantHash = new HashSet<string>();
            foreach(var info in infoArr)
            {
                string name = info.Name;
                btConstantHash.Add(name);
                if (!tableHash.Contains(name))
                {
                    Debug.LogError("BTConstant has table not found:" + info.Name);
                }
            }

            foreach(var name in tableHash)
            {
                if (!btConstantHash.Contains(name))
                {
                    Debug.LogError("Table has BTConstant not found:" + name);
                }
            }
        }

    }
}


public class PBConfigWriteFile
{
    public string filePath;
    public byte[] byteData;
}