using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace BehaviorTree
{

    public class BehaviorGlobalParameter
    {
        private BehaviorGlobalParameterModel _globalParameterModel;
        private BehaviorGlobalParameterView _globalParameterView;

        public BehaviorGlobalParameter()
        {
            Init();
        }

        public void Init()
        {
            _globalParameterModel = new BehaviorGlobalParameterModel();
            _globalParameterView = new BehaviorGlobalParameterView();
        }

        public void OnDestroy()
        {

        }

        public void OnGUI()
        {
            List<BehaviorParameter> parameterList = _globalParameterModel.ParameterList;
            _globalParameterView.Draw(parameterList);
        }

    }

    public class BehaviorGlobalParameterModel
    {
        public BehaviorGlobalParameterModel()
        {
        }

        public List<BehaviorParameter> ParameterList
        {
            get
            {
                return BehaviorManager.Instance.BehaviorTreeData.parameterList;
            }
        }
    }

    public class BehaviorGlobalParameterView
    {

        private Vector2 scrollPos = Vector2.zero;
        public void Draw(List<BehaviorParameter> parameterList)
        {
            EditorGUILayout.LabelField("全部变量");

            EditorGUILayout.BeginVertical("box", GUILayout.ExpandWidth(true));
            {
                EditorGUILayout.LabelField("条件参数");
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandHeight(true));
                {
                    GUI.backgroundColor = new Color(0.85f, 0.85f, 0.85f, 1f);
                    for (int i = 0; i < parameterList.Count; ++i)
                    {
                        BehaviorParameter behaviorParameter = parameterList[i];

                        Action DelCallBack = () =>
                        {
                            if (null != BehaviorManager.behaviorNodeParameter)
                            {
                                BehaviorManager.globalParameterChange(behaviorParameter, false);
                            }
                        };

                        EditorGUILayout.BeginVertical("box");
                        {
                            behaviorParameter = DrawParameter.Draw(behaviorParameter, DrawParameterType.GLOBAL_PARAMETER, DelCallBack);
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
                DrawAddParameter();
            }
            EditorGUILayout.EndVertical();
        }

        private BehaviorParameter newAddParameter = new BehaviorParameter();
        private void DrawAddParameter()
        {
            if (null == newAddParameter)
            {
                newAddParameter = new BehaviorParameter();
            }

            EditorGUILayout.BeginVertical("box");
            {
                newAddParameter = DrawParameter.Draw(newAddParameter, DrawParameterType.GLOBAL_PARAMETER_ADD, null);
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(5);

            if (GUILayout.Button("添加条件"))
            {
                if (null != BehaviorManager.behaviorNodeParameter)
                {
                    BehaviorManager.globalParameterChange(newAddParameter, true);
                }
            }
        }

    }

}

