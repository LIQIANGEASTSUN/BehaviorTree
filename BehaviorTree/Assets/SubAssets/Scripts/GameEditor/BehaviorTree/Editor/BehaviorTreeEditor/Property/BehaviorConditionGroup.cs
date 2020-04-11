using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BehaviorTree
{
    public static class BehaviorConditionGroup
    {
        private static int nodeId = -1;
        private static int selectIndex = -1;
        public static ConditionGroup DrawTransitionGroup(NodeValue nodeValue)
        {
            if (null == nodeValue)
            {
                return null;
            }

            ConditionGroup group = null;
            for (int i = 0; i < nodeValue.conditionGroupList.Count; ++i)
            {
                ConditionGroup tempGroup = nodeValue.conditionGroupList[i];
                ConditionGroup temp = DrawGroup(nodeValue, tempGroup);
                if (null != temp)
                {
                    group = temp;
                }
            }

            return group;
        }

        private static ConditionGroup DrawGroup(NodeValue nodeValue, ConditionGroup group)
        {
            Rect area = GUILayoutUtility.GetRect(0f, 1, GUILayout.ExpandWidth(true));
            bool select = (selectIndex == group.index);

            EditorGUILayout.BeginHorizontal("box", GUILayout.ExpandWidth(true));
            {
                if (selectIndex < 0 || nodeId < 0 || nodeId != nodeValue.id)
                {
                    nodeId = nodeValue.id;
                    selectIndex = group.index;
                }

                Rect rect = new Rect(area.x, area.y, area.width, 30);
                GUI.backgroundColor = select ? new Color(0, 1, 0, 1) : Color.white;// 
                GUI.Box(rect, string.Empty);
                GUI.backgroundColor = Color.white;

                if (GUILayout.Button("选择", GUILayout.Width(50)))
                {
                    selectIndex = group.index;
                }

                for (int i = group.parameterList.Count - 1; i >= 0; --i)
                {
                    string parameter = group.parameterList[i];
                    BehaviorParameter behaviorParameter = nodeValue.parameterList.Find(a => (a.parameterName.CompareTo(parameter) == 0));
                    if (null == behaviorParameter)
                    {
                        group.parameterList.RemoveAt(i);
                    }
                }

                GUI.enabled = select;
                for (int i = 0; i < nodeValue.parameterList.Count; ++i)
                {
                    BehaviorParameter parameter = nodeValue.parameterList[i];
                    string name = group.parameterList.Find(a => (a.CompareTo(parameter.parameterName) == 0));

                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField(i.ToString(), GUILayout.Width(10));
                        bool value = !string.IsNullOrEmpty(name);
                        bool oldValue = value;
                        value = EditorGUILayout.Toggle(value, GUILayout.Width(10));
                        if (value)
                        {
                            if (!oldValue)
                            {
                                group.parameterList.Add(parameter.parameterName);
                                break;
                            }
                        }
                        else
                        {
                            if (oldValue)
                            {
                                group.parameterList.Remove(parameter.parameterName);
                            }
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(10);
                }
                GUI.enabled = true;

                if (GUILayout.Button("删除"))
                {
                    if (null != BehaviorManager.behaviorAddDelConditionGroup)
                    {
                        BehaviorManager.behaviorAddDelConditionGroup(nodeValue.id, group.index, false);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            if (select)
            {
                return group;
            }
            return null;
        }

    }



}
