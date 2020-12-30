using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;

namespace BehaviorTree
{
    public class BehaviorReadWrite
    {
        #region BehaviorTreeData
        public bool WriteJson(BehaviorTreeData data, string filePath)
        {
            string content = LitJson.JsonMapper.ToJson(data);
            bool value = FileReadWrite.Write(filePath, content);

            if (value)
            {
                Debug.Log("Write Sucess:" + filePath);
            }
            else
            {
                Debug.LogError("Write Fail:" + filePath);
            }

            return value;
        }

        public BehaviorTreeData ReadJson(string filePath)
        {
            Debug.Log("Read:" + filePath);
            BehaviorTreeData behaviorData = new BehaviorTreeData();

            string content = FileReadWrite.Read(filePath);
            if (string.IsNullOrEmpty(content))
            {
                return behaviorData;
            }

            JsonData jsonData = JsonMapper.ToObject(content);

            behaviorData.fileName = jsonData["fileName"].ToString();

            behaviorData.rootNodeId = int.Parse(jsonData["rootNodeId"].ToString());

            JsonData nodeList = jsonData["nodeList"];
            behaviorData.nodeList = GetNodeList(nodeList);

            JsonData parameterList = jsonData["parameterList"];
            behaviorData.parameterList = GetParameterList(parameterList);

            behaviorData.descript = jsonData["descript"].ToString();

            return behaviorData;
        }

        private List<NodeValue> GetNodeList(JsonData data)
        {
            List<NodeValue> nodeList = new List<NodeValue>();

            foreach (JsonData item in data)
            {
                NodeValue nodeValue = new NodeValue();
                nodeValue.identificationName = item["identificationName"].ToString();
                nodeValue.id = int.Parse(item["id"].ToString());
                nodeValue.isRootNode = bool.Parse(item["isRootNode"].ToString());
                nodeValue.NodeType = int.Parse(item["NodeType"].ToString());
                nodeValue.parentNodeID = int.Parse(item["parentNodeID"].ToString());
                nodeValue.priority = int.Parse(item["priority"].ToString());

                JsonData childNodeList = item["childNodeList"];
                nodeValue.childNodeList = GetChildIdList(childNodeList);

                nodeValue.repeatTimes = int.Parse(item["repeatTimes"].ToString());
                nodeValue.nodeName = item["nodeName"].ToString();
                nodeValue.descript = item["descript"].ToString();
                nodeValue.function = item["function"].ToString();

                JsonData conditionGroupList = item["conditionGroupList"];
                nodeValue.conditionGroupList = GetConditionGroupList(conditionGroupList);

                JsonData ifJudgeDataList = item["ifJudgeDataList"];
                nodeValue.ifJudgeDataList = GetIfJudgeDataList(ifJudgeDataList);

                JsonData parameterList = item["parameterList"];
                nodeValue.parameterList = GetParameterList(parameterList);

                JsonData position = item["position"];
                nodeValue.position = GetPosition(position);

                nodeValue.parentSubTreeNodeId = int.Parse(item["parentSubTreeNodeId"].ToString());
                nodeValue.subTreeEntry = bool.Parse(item["subTreeEntry"].ToString());
                nodeValue.subTreeType = int.Parse(item["subTreeType"].ToString());
                nodeValue.subTreeConfig = item["subTreeConfig"].ToString();
                nodeValue.subTreeValue = long.Parse(item["subTreeValue"].ToString());

                nodeList.Add(nodeValue);
            }

            return nodeList;
        }

        private List<int> GetChildIdList(JsonData jsonData)
        {
            List<int> childIdList = new List<int>();
            for (int i = 0; i < jsonData.Count; ++i)
            {
                int value = int.Parse(jsonData[i].ToString());
                childIdList.Add(value);
            }

            return childIdList;
        }

        private List<ConditionGroup> GetConditionGroupList(JsonData jsonData)
        {
            List<ConditionGroup> conditionGroupList = new List<ConditionGroup>();
            foreach (JsonData item in jsonData)
            {
                ConditionGroup conditionGroup = new ConditionGroup();
                conditionGroup.index = int.Parse(item["index"].ToString()); 
                JsonData croupData = item["parameterList"];
                for (int i = 0; i < croupData.Count; ++i)
                {
                    string parameterName = croupData[i].ToString();
                    conditionGroup.parameterList.Add(parameterName);
                }

                conditionGroupList.Add(conditionGroup);
            }

            return conditionGroupList;
        }

        private List<IfJudgeData> GetIfJudgeDataList (JsonData jsonData)
        {
            List<IfJudgeData> ifJudgeDataList = new List<IfJudgeData>();
            foreach(JsonData item in jsonData)
            {
                IfJudgeData judgeData = new IfJudgeData();
                judgeData.nodeId = int.Parse(item["nodeId"].ToString());
                judgeData.ifJudegType = int.Parse(item["ifJudegType"].ToString());
                judgeData.ifResult = int.Parse(item["ifResult"].ToString());

                ifJudgeDataList.Add(judgeData);
            }
            return ifJudgeDataList;
        }


        private RectT GetPosition(JsonData data)
        {
            float x = int.Parse(data["x"].ToString());
            float y = int.Parse(data["y"].ToString());
            float width = int.Parse(data["width"].ToString());
            float height = int.Parse(data["height"].ToString());

            RectT position = new RectT(x, y, width, height);
            return position;
        }

        private List<BehaviorParameter> GetParameterList(JsonData data)
        {
            List<BehaviorParameter> dataList = new List<BehaviorParameter>();
            foreach (JsonData item in data)
            {
                BehaviorParameter parameter = new BehaviorParameter();
                parameter.parameterType = int.Parse(item["parameterType"].ToString());
                parameter.parameterName = item["parameterName"].ToString();
                if (((IDictionary)item).Contains("CNName"))
                {
                    parameter.CNName = item["CNName"].ToString();
                }

                parameter.intValue = int.Parse(item["intValue"].ToString());
                if (((IDictionary)item).Contains("longValue"))
                {
                    parameter.longValue = long.Parse(item["longValue"].ToString());
                }
                parameter.floatValue = float.Parse(item["floatValue"].ToString());
                parameter.boolValue = bool.Parse(item["boolValue"].ToString());
                parameter.stringValue = item["stringValue"].ToString();
                parameter.compare = int.Parse(item["compare"].ToString());

                dataList.Add(parameter);
            }

            return dataList;
        }
        #endregion
    }

}
