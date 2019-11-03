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

        public void Draw(NodeValue nodeValue)
        {
            if (null == nodeValue)
            {
                EditorGUILayout.LabelField("未选择节点");
                return;
            }

            EditorGUILayout.BeginVertical("box");
            {
                if (nodeValue.NodeType == (int)NODE_TYPE.CONDITION
                    || nodeValue.NodeType == (int)NODE_TYPE.ACTION)
                {
                    int index = EnumNames.GetEnumIndex<NODE_TYPE>((NODE_TYPE)nodeValue.NodeType);
                    string name = EnumNames.GetEnumName<NODE_TYPE>(index);
                    EditorGUILayout.LabelField(name);
                    GUILayout.Space(5);
                }

                string nodeName = nodeValue.nodeName;
                EditorGUILayout.LabelField(nodeName);

                string nodeId = string.Format("节点_{0}", nodeValue.id);
                EditorGUILayout.LabelField(nodeId);

                GUI.enabled = false;
                nodeValue.isRootNode = EditorGUILayout.Toggle(new GUIContent("根节点"), nodeValue.isRootNode, GUILayout.Width(50));
                GUI.enabled = true;

                if (nodeValue.parentNodeID >= 0)
                {
                    string parentName = string.Format("父节点_{0}", nodeValue.parentNodeID);
                    EditorGUILayout.LabelField(parentName);
                }

                if (nodeValue.identification > 0)
                {
                    string identificationName = string.Format("类标识_{0}", nodeValue.identification);
                    EditorGUILayout.LabelField(identificationName);

                    CustomIdentification customIdentification = CustomNode.Instance.GetIdentification((IDENTIFICATION)nodeValue.identification);
                    string className = customIdentification.Type.Name;
                    EditorGUILayout.LabelField(className);
                }

                nodeValue.descript = EditorGUILayout.TextArea(nodeValue.descript, GUILayout.Height(50));
            }
            EditorGUILayout.EndVertical();

            DrawNode(nodeValue, "参数");
        }

        private Vector2 scrollPos = Vector2.zero;
        private void DrawNode(NodeValue nodeValue, string title)
        {
            if (nodeValue.NodeType != (int)NODE_TYPE.CONDITION 
                && nodeValue.NodeType != (int)NODE_TYPE.ACTION)
            {
                return;
            }

            EditorGUILayout.BeginVertical("box", GUILayout.ExpandWidth(true));
            {
                EditorGUILayout.LabelField(title);

                int height = (nodeValue.parameterList.Count * 70) + 20;
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

            GUILayout.Space(10);
            EditorGUILayout.BeginVertical("box");
            {
                DrawAddParameter(nodeValue);
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawAddParameter(NodeValue nodeValue)
        {
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
        }

    }


}