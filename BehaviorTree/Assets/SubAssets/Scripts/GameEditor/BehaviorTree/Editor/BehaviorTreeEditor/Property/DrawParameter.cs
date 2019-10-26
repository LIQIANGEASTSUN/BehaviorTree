using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

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

            {
                string[] parameterNameArr = EnumNames.GetEnumNames<BehaviorParameterType>();
                int index = EnumNames.GetEnumIndex<BehaviorParameterType>((BehaviorParameterType)(behaviorParameter.parameterType));
                BehaviorParameterType behaviorParameterType = EnumNames.GetEnum<BehaviorParameterType>(index);

                bool enableChangeType = (drawParameterType == DrawParameterType.BEHAVIOR_PARAMETER_ADD);
                GUI.enabled = enableChangeType;
                {
                    index = EditorGUILayout.Popup(index, parameterNameArr);
                    behaviorParameter.parameterType = (int)EnumNames.GetEnum<BehaviorParameterType>(index);
                    GUILayout.Space(5);
                }
                GUI.enabled = true;
            }

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
                        parameterArr[i] = p.parameterName;
                        if (behaviorParameter.parameterName.CompareTo(p.parameterName) == 0)
                        {
                            index = i;
                        }
                    }
                    
                    int result = EditorGUILayout.Popup(index, parameterArr, GUILayout.ExpandWidth(true));
                    if (result != index)
                    {
                        behaviorParameter.parameterName = parameterArr[result];
                    }
                }
                else if (drawParameterType == DrawParameterType.BEHAVIOR_PARAMETER
                    || (drawParameterType == DrawParameterType.BEHAVIOR_PARAMETER_ADD)
                    || drawParameterType == DrawParameterType.RUNTIME_PARAMETER)
                {
                    GUI.enabled = (drawParameterType == DrawParameterType.BEHAVIOR_PARAMETER_ADD);
                    behaviorParameter.parameterName = EditorGUILayout.TextField(behaviorParameter.parameterName);
                    GUI.enabled = true;
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
                {
                    compare = EditorGUILayout.Popup(compare, compareArr, GUILayout.Width(65));
                    behaviorParameter.compare = (int)(compareEnumArr[compare]);
                }
                GUI.enabled = true;
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            {
                GUI.enabled = true;// (drawParameterType != DrawParameterType.BEHAVIOR_PARAMETER);
                {
                    if (behaviorParameter.parameterType == (int)BehaviorParameterType.Int)
                    {
                        behaviorParameter.intValue = EditorGUILayout.IntField("IntValue", behaviorParameter.intValue);
                    }

                    if (behaviorParameter.parameterType == (int)BehaviorParameterType.Float)
                    {
                        behaviorParameter.floatValue = EditorGUILayout.FloatField("FloatValue", behaviorParameter.floatValue);
                    }

                    if (behaviorParameter.parameterType == (int)BehaviorParameterType.Bool)
                    {
                        behaviorParameter.boolValue = EditorGUILayout.Toggle("BoolValue", behaviorParameter.boolValue);
                    }
                }
                GUI.enabled = true;

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

            return behaviorParameter;
        }

    }
}


