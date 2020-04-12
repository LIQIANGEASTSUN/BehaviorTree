using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Text.RegularExpressions;

namespace BehaviorTree
{
    public enum DrawParameterType
    {
        NODE_PARAMETER = 0,
        BEHAVIOR_PARAMETER,
        BEHAVIOR_PARAMETER_ADD,
        RUNTIME_PARAMETER,
    }

    public class DrawParameter
    {
        public static BehaviorParameter Draw(BehaviorParameter behaviorParameter, DrawParameterType drawParameterType, Action DelCallBack)
        {
            if (null == behaviorParameter)
            {
                return behaviorParameter;
            }

            EditorGUILayout.BeginHorizontal();
            {
                string[] parameterNameArr = EnumNames.GetEnumNames<BehaviorParameterType>();
                int index = EnumNames.GetEnumIndex<BehaviorParameterType>((BehaviorParameterType)(behaviorParameter.parameterType));
                BehaviorParameterType behaviorParameterType = EnumNames.GetEnum<BehaviorParameterType>(index);

                GUI.enabled = false;
                if (drawParameterType == DrawParameterType.NODE_PARAMETER)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        behaviorParameter.index = EditorGUILayout.IntField(behaviorParameter.index, GUILayout.Width(30));
                    }
                    EditorGUILayout.EndHorizontal();
                }

                bool enableChangeType = (drawParameterType == DrawParameterType.BEHAVIOR_PARAMETER_ADD);
                GUI.enabled = enableChangeType;
                {
                    index = EditorGUILayout.Popup(index, parameterNameArr);
                    behaviorParameter.parameterType = (int)EnumNames.GetEnum<BehaviorParameterType>(index);
                    GUILayout.Space(5);
                }
                GUI.enabled = true;

                if (drawParameterType == DrawParameterType.NODE_PARAMETER)
                {
                    GUI.enabled = false;
                    EditorGUILayout.BeginHorizontal();
                    {
                        behaviorParameter.parameterName = EditorGUILayout.TextField(behaviorParameter.parameterName);
                    }
                    EditorGUILayout.EndHorizontal();
                    GUI.enabled = true;
                }

                if (drawParameterType == DrawParameterType.NODE_PARAMETER || drawParameterType == DrawParameterType.BEHAVIOR_PARAMETER)
                {
                    if (GUILayout.Button("Del"))
                    {
                        if (null != DelCallBack)
                        {
                            DelCallBack();
                        }
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                if (drawParameterType == DrawParameterType.NODE_PARAMETER)
                {
                    List<BehaviorParameter> parameterList = BehaviorManager.Instance.BehaviorTreeData.parameterList;
                    string[] parameterArr = new string[parameterList.Count];
                    int index = -1;
                    for (int i = 0; i < parameterList.Count; ++i)
                    {
                        BehaviorParameter p = parameterList[i];
                        parameterArr[i] = p.CNName;
                        if (behaviorParameter.parameterName.CompareTo(p.parameterName) == 0)
                        {
                            index = i;
                        }
                    }
                    
                    int result = EditorGUILayout.Popup(index, parameterArr, GUILayout.ExpandWidth(true));
                    if (result != index)
                    {
                        behaviorParameter.parameterName = parameterList[result].parameterName;
                    }
                }
                else if (drawParameterType == DrawParameterType.BEHAVIOR_PARAMETER
                    || drawParameterType == DrawParameterType.RUNTIME_PARAMETER)
                {
                    GUI.enabled = (drawParameterType == DrawParameterType.BEHAVIOR_PARAMETER_ADD);
                    behaviorParameter.parameterName = EditorGUILayout.TextField(behaviorParameter.parameterName);
                    behaviorParameter.CNName = EditorGUILayout.TextField(behaviorParameter.CNName);
                    GUI.enabled = true;
                }
                else if (drawParameterType == DrawParameterType.BEHAVIOR_PARAMETER_ADD)
                {
                    EditorGUILayout.BeginVertical();
                    {
                        string oldName = behaviorParameter.parameterName;
                        behaviorParameter.parameterName = EditorGUILayout.TextField("英文:", behaviorParameter.parameterName);
                        if (oldName.CompareTo(behaviorParameter.parameterName) != 0)
                        {
                            bool isNumOrAlp = IsNumOrAlp(behaviorParameter.parameterName);
                            if (!isNumOrAlp)
                            {
                                string msg = string.Format("参数名只能包含:数字、字母、下划线，且数字不能放在第一个字符位置");
                                TreeNodeWindow.window.ShowNotification(msg);
                                behaviorParameter.parameterName = oldName;
                            }
                        }

                        behaviorParameter.CNName = EditorGUILayout.TextField("中文", behaviorParameter.CNName);
                    }
                    EditorGUILayout.EndVertical();
                }

                BehaviorCompare[] compareEnumArr = new BehaviorCompare[] { };
                if (behaviorParameter.parameterType == (int)BehaviorParameterType.Int)
                {
                    compareEnumArr = new BehaviorCompare[] { BehaviorCompare.GREATER, BehaviorCompare.GREATER_EQUALS, BehaviorCompare.LESS_EQUAL, BehaviorCompare.LESS, BehaviorCompare.EQUALS, BehaviorCompare.NOT_EQUAL };
                }
                if (behaviorParameter.parameterType == (int)BehaviorParameterType.Float)
                {
                    compareEnumArr = new BehaviorCompare[] { BehaviorCompare.GREATER, BehaviorCompare.LESS };
                }
                if (behaviorParameter.parameterType == (int)BehaviorParameterType.Bool)
                {
                    compareEnumArr = new BehaviorCompare[] { BehaviorCompare.EQUALS, BehaviorCompare.NOT_EQUAL };
                }
                string[] compareArr = new string[compareEnumArr.Length];
                int compare = behaviorParameter.compare;
                bool found = false;
                for (int i = 0; i < compareEnumArr.Length; ++i)
                {
                    string name = System.Enum.GetName(typeof(BehaviorCompare), compareEnumArr[i]);
                    compareArr[i] = name;
                    if ((BehaviorCompare)behaviorParameter.compare == compareEnumArr[i])
                    {
                        compare = i;
                        found = true;
                    }
                }

                if (!found)
                {
                    compare = 0;
                }

                GUI.enabled = (drawParameterType != DrawParameterType.BEHAVIOR_PARAMETER) && (drawParameterType != DrawParameterType.RUNTIME_PARAMETER);
                bool value = (drawParameterType != DrawParameterType.BEHAVIOR_PARAMETER) && (drawParameterType != DrawParameterType.RUNTIME_PARAMETER) && (drawParameterType != DrawParameterType.BEHAVIOR_PARAMETER_ADD);
                bool boolType = (behaviorParameter.parameterType == (int)BehaviorParameterType.Bool);
                if (value && !boolType)
                {
                    compare = EditorGUILayout.Popup(compare, compareArr, GUILayout.Width(65));
                    behaviorParameter.compare = (int)(compareEnumArr[compare]);
                }
                GUI.enabled = true;

                if (boolType)
                {
                    behaviorParameter.compare = (int)BehaviorCompare.EQUALS;
                }

                GUI.enabled = true;// (drawParameterType != DrawParameterType.BEHAVIOR_PARAMETER);
                {
                    if (behaviorParameter.parameterType == (int)BehaviorParameterType.Int)
                    {
                        behaviorParameter.intValue = EditorGUILayout.IntField(behaviorParameter.intValue, GUILayout.Width(60));
                    }

                    if (behaviorParameter.parameterType == (int)BehaviorParameterType.Float)
                    {
                        behaviorParameter.floatValue = EditorGUILayout.FloatField(behaviorParameter.floatValue, GUILayout.Width(60));
                    }

                    if (behaviorParameter.parameterType == (int)BehaviorParameterType.Bool)
                    {
                        behaviorParameter.boolValue = EditorGUILayout.Toggle(behaviorParameter.boolValue, GUILayout.Width(60));
                    }
                }
                GUI.enabled = true;
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);

            return behaviorParameter;
        }

        private static bool IsNumOrAlp(string str)
        {
            string pattern = @"^[a-zA-Z_][A-Za-z0-9_]*$";
            Match match = Regex.Match(str, pattern);
            return match.Success;
        }

    }

}