using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace BehaviorTree
{


    public class BehaviorNodeInspector
    {
        private BehaviorNodeInspectorModel _nodeInspectorModel;
        private BehaviorNodeInspectorView _nodeInspectorView;

        public BehaviorNodeInspector()
        {
            Init();
        }

        public void Init()
        {
            _nodeInspectorModel = new BehaviorNodeInspectorModel();
            _nodeInspectorView = new BehaviorNodeInspectorView();
        }

        public void OnDestroy()
        {

        }

        public void OnGUI()
        {
            NodeValue nodeValue = _nodeInspectorModel.GetCurrentSelectNode();
            _nodeInspectorView.Draw(nodeValue);
        }

    }


    public class BehaviorNodeInspectorModel
    {
        public NodeValue GetCurrentSelectNode()
        {
            return BehaviorManager.Instance.CurrentNode;
        }
    }

    public class BehaviorNodeInspectorView
    {

        private Color32[] colorArr = new Color32[] { new Color32(178, 226, 221, 255), new Color32(220, 226, 178, 255), new Color32(209, 178, 226, 255), new Color32(178, 185, 226, 255) };

        public void Draw(NodeValue nodeValue)
        {
            if (null == nodeValue)
            {
                EditorGUILayout.LabelField("未选择节点");
                return;
            }

            GUI.enabled = !BehaviorManager.Instance.CurrentOpenConfigSubTree();

            EditorGUILayout.BeginVertical("box");
            {
                if (nodeValue.NodeType == (int)NODE_TYPE.CONDITION
                    || nodeValue.NodeType == (int)NODE_TYPE.ACTION)
                {
                    int index = EnumNames.GetEnumIndex<NODE_TYPE>((NODE_TYPE)nodeValue.NodeType);
                    string name = EnumNames.GetEnumName<NODE_TYPE>(index);
                    name = string.Format("{0}_{1}", name, nodeValue.id);
                    EditorGUILayout.LabelField(name);
                    GUILayout.Space(5);
                }

                string nodeName = nodeValue.nodeName;
                string msg = string.Format("{0}_{1}", nodeName, nodeValue.id);
                EditorGUILayout.LabelField(msg);

                if (nodeValue.NodeType == (int)NODE_TYPE.SUB_TREE)
                {
                    SubTreeNode(nodeValue);
                }

                if (nodeValue.NodeType != (int)NODE_TYPE.CONDITION
                && nodeValue.NodeType != (int)NODE_TYPE.ACTION)
                {
                    CompositeNode(nodeValue);
                }

                EntryNode(nodeValue);

                if (nodeValue.NodeType == (int)NODE_TYPE.DECORATOR_REPEAT)
                {
                    nodeValue.repeatTimes = EditorGUILayout.IntField("重复执行次数", nodeValue.repeatTimes);
                }

                if (nodeValue.NodeType == (int)NODE_TYPE.IF_JUDEG)
                {
                    IfJudge(nodeValue);
                }

                if (nodeValue.childNodeList.Count > 0)
                {
                    if (nodeValue.NodeType != (int)NODE_TYPE.SUB_TREE)
                    {
                        nodeValue.moveWithChild = EditorGUILayout.Toggle(new GUIContent("同步移动子节点"), nodeValue.moveWithChild);
                    }
                    else
                    {
                        nodeValue.moveWithChild = false;
                    }
                }

                if (nodeValue.parentNodeID >= 0)
                {
                    string parentName = string.Format("父节点_{0}", nodeValue.parentNodeID);
                    EditorGUILayout.LabelField(parentName);
                }

                if (nodeValue.childNodeList.Count > 0 && nodeValue.NodeType == (int)NODE_TYPE.RANDOM_PRIORITY)
                {
                    EditorGUILayout.BeginVertical("box");
                    {
                        for (int i = 0; i < nodeValue.childNodeList.Count; ++i)
                        {
                            NodeValue childNode = BehaviorManager.Instance.GetNode(nodeValue.childNodeList[i]);
                            string nodeMsg = string.Format("子节点:{0} 权值:", childNode.id);
                            childNode.priority = EditorGUILayout.IntField(nodeMsg, childNode.priority);
                            childNode.priority = Mathf.Max(1, childNode.priority);
                        }
                    }
                    EditorGUILayout.EndVertical();
                }

                if (!string.IsNullOrEmpty(nodeValue.identificationName))
                {
                    string identificationName = string.Format("类标识_{0}", nodeValue.identificationName);
                    EditorGUILayout.LabelField(identificationName);

                    CustomIdentification customIdentification = CustomNode.Instance.GetIdentification(nodeValue.identificationName);
                    string className = customIdentification.ClassType.Name;
                    EditorGUILayout.LabelField(className);
                }

                EditorGUILayout.LabelField("备注");
                nodeValue.descript = EditorGUILayout.TextArea(nodeValue.descript, GUILayout.Width(250));

                if ((nodeValue.NodeType != (int)NODE_TYPE.CONDITION && nodeValue.NodeType != (int)NODE_TYPE.ACTION))
                {
                    GUILayout.Space(20);
                    EditorGUILayout.LabelField("组合节点功能描述");
                    nodeValue.function = EditorGUILayout.TextArea(nodeValue.function, GUILayout.Height(150), GUILayout.Width(300));
                }
            }
            EditorGUILayout.EndVertical();

            GUI.enabled = true;
            DrawNode(nodeValue, "参数");
            GUI.enabled = true;

            ParentInfo();
        }

        private void EntryNode(NodeValue nodeValue)
        {
            if (nodeValue.NodeType == (int)NODE_TYPE.CONDITION
                    || nodeValue.NodeType == (int)NODE_TYPE.ACTION
                    || nodeValue.NodeType == (int)NODE_TYPE.SUB_TREE
                    )
            {
                return;
            }

            if (nodeValue.parentSubTreeNodeId < 0)
            {
                bool oldValue = nodeValue.isRootNode;
                nodeValue.isRootNode = EditorGUILayout.Toggle(new GUIContent("根节点"), nodeValue.isRootNode/*, GUILayout.Width(50)*/);
                if (nodeValue.isRootNode && oldValue != nodeValue.isRootNode)
                {
                    if (null != BehaviorManager.behaviorChangeRootNode)
                    {
                        BehaviorManager.behaviorChangeRootNode(nodeValue.id);
                    }
                }
            }
            else
            {
                bool oldValue = nodeValue.subTreeEntry;
                nodeValue.subTreeEntry = EditorGUILayout.Toggle(new GUIContent("子树入口节点"), nodeValue.subTreeEntry/*, GUILayout.Width(50)*/);
                if (nodeValue.subTreeEntry && oldValue != nodeValue.subTreeEntry)
                {
                    if (null != BehaviorManager.behaviorChangeSubTreeEntryNode)
                    {
                        BehaviorManager.behaviorChangeSubTreeEntryNode(nodeValue.parentSubTreeNodeId, nodeValue.id);
                    }
                }
            }
        }

        private void CompositeNode(NodeValue nodeValue)
        {
            if (nodeValue.NodeType == (int)NODE_TYPE.CONDITION
                || nodeValue.NodeType == (int)NODE_TYPE.ACTION)
            {
                return;
            }

            EditorGUILayout.BeginHorizontal(/*"box"*/);
            {
                string[] nameArr = EnumNames.GetEnumNames<NODE_TYPE>();
                List<string> nameList = new List<string>(nameArr);

                NODE_TYPE[] removeTypeArr = new NODE_TYPE[2] { NODE_TYPE.ACTION, NODE_TYPE.CONDITION };
                for (int i = nameList.Count - 1; i >= 0; --i)
                {
                    for (int j = 0; j < removeTypeArr.Length; ++j)
                    {
                        NODE_TYPE type = removeTypeArr[j];
                        int value = EnumNames.GetEnumIndex<NODE_TYPE>(type);
                        string name = EnumNames.GetEnumName<NODE_TYPE>(value);
                        if (name.CompareTo(nameList[i]) == 0)
                        {
                            nameList.RemoveAt(i);
                            break;
                        }
                    }
                }
                nameArr = nameList.ToArray();
                int index = EnumNames.GetEnumIndex<NODE_TYPE>((NODE_TYPE)nodeValue.NodeType);
                if (index > nameArr.Length)
                {
                    index -= 2;//把 条件节点、行为节点，两个节点减掉
                }

                if ((NODE_TYPE)nodeValue.NodeType != NODE_TYPE.SUB_TREE)
                {
                    int result = EditorGUILayout.Popup(new GUIContent("改变节点类型"), index, nameArr);
                    if (result != index)
                    {
                        nodeValue.NodeType = (int)(EnumNames.GetEnum<NODE_TYPE>(result));
                        nodeValue.nodeName = EnumNames.GetEnumName<NODE_TYPE>(result);
                        nodeValue.function = NodeDescript.GetFunction((NODE_TYPE)nodeValue.NodeType); ;

                        Debug.LogError(nodeValue.nodeName);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void IfJudge(NodeValue nodeValue)
        {
            HashSet<int> childHash = new HashSet<int>();
            if (nodeValue.childNodeList.Count > 3)
            {
                if (null != TreeNodeWindow.window)
                {
                    string msg = string.Format("判断节点 {0} 最多只能有三个节点", nodeValue.id);
                    TreeNodeWindow.window.ShowNotification(msg);
                }

                while(nodeValue.childNodeList.Count > 3)
                {
                    nodeValue.childNodeList.RemoveAt(nodeValue.childNodeList.Count - 1);
                }

                while (nodeValue.ifJudgeDataList.Count > 3)
                {
                    nodeValue.ifJudgeDataList.RemoveAt(nodeValue.ifJudgeDataList.Count - 1);
                }
            }

            EditorGUILayout.BeginVertical("box");
            {
                for (int i = 0; i < nodeValue.childNodeList.Count; ++i)
                {
                    int childId = nodeValue.childNodeList[i];
                    childHash.Add(childId);
                    IfJudgeData judgeData = nodeValue.ifJudgeDataList.Find((a) => { return a.nodeId == childId; });
                    if (null == judgeData)
                    {
                        judgeData = new IfJudgeData();
                        judgeData.nodeId = childId;
                        nodeValue.ifJudgeDataList.Add(judgeData);
                    }
                    judgeData.ifJudegType = ((i == 0) ? (int)NodeIfJudgeEnum.IF : (int)NodeIfJudgeEnum.ACTION);

                    EditorGUILayout.BeginVertical("box");
                    {
                        GUI.enabled = false;
                        EditorGUILayout.IntField("nodeId:", judgeData.nodeId);
                        {
                            string[] nameArr = EnumNames.GetEnumNames<NodeIfJudgeEnum>();
                            int index = EnumNames.GetEnumIndex<NodeIfJudgeEnum>((NodeIfJudgeEnum)judgeData.ifJudegType);
                            int result = EditorGUILayout.Popup(new GUIContent("节点类型"), index, nameArr);
                            judgeData.ifJudegType = (int)(EnumNames.GetEnum<NodeIfJudgeEnum>(result));
                        }
                        GUI.enabled = true;

                        if (judgeData.ifJudegType == (int)NodeIfJudgeEnum.ACTION)
                        {
                            string[] nameArr = new string[] { "Fail", "Success"};
                            int oldValue = judgeData.ifResult;
                            int result = EditorGUILayout.Popup(new GUIContent("执行条件"), judgeData.ifResult, nameArr);
                            if (oldValue != result)
                            {
                                JudgeNodeChangeChildCondition(nodeValue, judgeData.nodeId, (ResultType)result);
                            }
                        }

                    }
                    EditorGUILayout.EndVertical();
                }
            }
            EditorGUILayout.EndVertical();

            for(int i = nodeValue.ifJudgeDataList.Count - 1; i >= 0; --i)
            {
                IfJudgeData data = nodeValue.ifJudgeDataList[i];
                if (!childHash.Contains(data.nodeId))
                {
                    nodeValue.ifJudgeDataList.RemoveAt(i);
                }
            }

            nodeValue.ifJudgeDataList.Sort((a, b) =>
            {
                return a.ifJudegType - b.ifJudegType;
            });
        }

        private void JudgeNodeChangeChildCondition(NodeValue nodeValue, int childId, ResultType resultType)
        {
            for (int i = 0; i < nodeValue.ifJudgeDataList.Count; ++i)
            {
                IfJudgeData judgeData = nodeValue.ifJudgeDataList[i];
                if (judgeData.ifJudegType == (int)NodeIfJudgeEnum.IF)
                {
                    continue;
                }
               
                if (judgeData.nodeId == childId)
                {
                    judgeData.ifResult = (int)resultType;
                }
                else
                {
                    int result = (int)resultType - (int)ResultType.Success;
                    judgeData.ifResult = Mathf.Abs(result);
                }
            }
        }

        private void SubTreeNode(NodeValue nodeValue)
        {
            string[] nameArr = EnumNames.GetEnumNames<SUB_TREE_TYPE>();
            int index = EnumNames.GetEnumIndex<SUB_TREE_TYPE>((SUB_TREE_TYPE)nodeValue.subTreeType);
            int result = EditorGUILayout.Popup(new GUIContent("子树类型"), index, nameArr);
            nodeValue.subTreeType = (int)(EnumNames.GetEnum<SUB_TREE_TYPE>(result));

            GUILayout.Space(8);
            nodeValue.subTreeConfig = EditorGUILayout.TextField(new GUIContent("配置文件"), nodeValue.subTreeConfig);
            GUILayout.Space(5);

            if (nodeValue.subTreeType == (int)SUB_TREE_TYPE.CONFIG)
            {
                if (null != BehaviorManager.behaviorDeleteSubTreeChild)
                {
                    BehaviorManager.behaviorDeleteSubTreeChild(nodeValue.id);
                }

                if (GUILayout.Button("选择子树配置文件"))
                {
                    if (null != BehaviorManager.behaviorSelectFile)
                    {
                        nodeValue.subTreeConfig = BehaviorManager.behaviorSelectFile();
                    }
                }
            }
            else
            {
                if (GUILayout.Button("将子树存储为配置文件"))
                {
                    if (null != BehaviorManager.behaviorSaveSubTree)
                    {
                        BehaviorManager.behaviorSaveSubTree(nodeValue.subTreeConfig, nodeValue.id);
                    }
                }
            }
        }

        private Vector2 scrollPos = Vector2.zero;
        private void DrawNode(NodeValue nodeValue, string title)
        {
            if (nodeValue.NodeType != (int)NODE_TYPE.CONDITION 
                && nodeValue.NodeType != (int)NODE_TYPE.ACTION)
            {
                return;
            }

            ConditionGroup conditionGroup = DrawConditionGroup(nodeValue);

            for (int i = 0; i < nodeValue.parameterList.Count; ++i)
            {
                nodeValue.parameterList[i].index = i;
            }

            GUI.enabled = !BehaviorManager.Instance.CurrentOpenConfigSubTree();
            EditorGUILayout.BeginVertical("box", GUILayout.ExpandWidth(true));
            {
                EditorGUILayout.LabelField(title);

                int height = (nodeValue.parameterList.Count * 58);
                height = height <= 300 ? height : 300;
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(height));
                {
                    GUI.backgroundColor = new Color(0.85f, 0.85f, 0.85f, 1f);
                    for (int i = 0; i < nodeValue.parameterList.Count; ++i)
                    {
                        BehaviorParameter behaviorParameter = nodeValue.parameterList[i];

                        Action DelCallBack = () =>
                        {
                            if (null != BehaviorManager.behaviorNodeParameter)
                            {
                                BehaviorManager.behaviorNodeParameter(nodeValue.id, behaviorParameter, false);
                            }
                        };

                        Color color = Color.white;
                        if (null != conditionGroup)
                        {
                            string name = conditionGroup.parameterList.Find(a => (a.CompareTo(behaviorParameter.parameterName) == 0));
                            if (!string.IsNullOrEmpty(name))
                            {
                                color = colorArr[0];
                            }
                        }

                        GUI.backgroundColor = color; // new Color(0.85f, 0.85f, 0.85f, 1f);

                        EditorGUILayout.BeginVertical("box");
                        {
                            string parameterName = behaviorParameter.parameterName;

                            BehaviorParameter tempParameter = behaviorParameter.Clone();
                            tempParameter = DrawParameter.Draw(behaviorParameter, DrawParameterType.NODE_PARAMETER, DelCallBack);
                            if (parameterName.CompareTo(behaviorParameter.parameterName) != 0)
                            {
                                if (null != BehaviorManager.behaviorNodeChangeParameter)
                                {
                                    BehaviorManager.behaviorNodeChangeParameter(nodeValue.id, parameterName, behaviorParameter.parameterName);
                                }
                            }
                            else
                            {
                                behaviorParameter.CloneFrom(tempParameter);
                            }
                        }
                        EditorGUILayout.EndVertical();
                    }
                    GUI.backgroundColor = Color.white;
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
            GUI.enabled = true;

            GUILayout.Space(10);
            EditorGUILayout.BeginVertical("box");
            {
                DrawAddParameter(nodeValue);
            }
            EditorGUILayout.EndVertical();
        }

        private ConditionGroup DrawConditionGroup(NodeValue nodeValue)
        {
            ConditionGroup conditionGroup = null;
            if (nodeValue.NodeType != (int)NODE_TYPE.CONDITION)
            {
                return conditionGroup;
            }

            EditorGUILayout.BeginVertical("box");
            {
                conditionGroup = BehaviorConditionGroup.DrawTransitionGroup(nodeValue);

                GUI.enabled = !BehaviorManager.Instance.CurrentOpenConfigSubTree();
                if (GUILayout.Button("添加组"))
                {
                    if (null != BehaviorManager.behaviorAddDelConditionGroup)
                    {
                        BehaviorManager.behaviorAddDelConditionGroup(nodeValue.id, -1, true);
                    }
                }

                GUI.enabled = true;
            }
            EditorGUILayout.EndVertical();

            return conditionGroup;
        }

        private void DrawAddParameter(NodeValue nodeValue)
        {
            GUI.enabled = !BehaviorManager.Instance.CurrentOpenConfigSubTree();
            if (GUILayout.Button("添加条件"))
            {
                if (BehaviorManager.Instance.BehaviorTreeData.parameterList.Count <= 0)
                {
                    string msg = "没有参数可添加，请先添加参数";

                    if (TreeNodeWindow.window != null)
                    {
                        TreeNodeWindow.window.ShowNotification(msg);
                    }
                }
                else
                {
                    if (null != BehaviorManager.behaviorNodeParameter)
                    {
                        BehaviorParameter behaviorParameter = BehaviorManager.Instance.BehaviorTreeData.parameterList[0];
                        BehaviorManager.behaviorNodeParameter(nodeValue.id, behaviorParameter, true);
                    }
                }
            }
            GUI.enabled = true;
        }

        private int _nodeID = -1;
        private void ParentInfo()
        {
            GUILayout.Space(20);
            _nodeID = EditorGUILayout.IntField("节点ID", _nodeID);
            if (GUILayout.Button("打印节点所有父节点路径"))
            {
                if (null != BehaviorManager.behaviorDebugNodeParentInfo)
                {
                    BehaviorManager.behaviorDebugNodeParentInfo(_nodeID);
                }
            }
        }

    }


}