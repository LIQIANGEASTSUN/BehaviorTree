using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace BehaviorTree
{

    public class BehaviorRuntimeParameter
    {
        private BehaviorRuntimeParameterModel _runtimeParameterModel;
        private BehaviorRuntimeParameterView _runtimeParameterView;

        public BehaviorRuntimeParameter()
        {
            Init();
        }

        public void Init()
        {
            _runtimeParameterModel = new BehaviorRuntimeParameterModel();
            _runtimeParameterView = new BehaviorRuntimeParameterView();
        }

        public void OnDestroy()
        {

        }

        public void OnGUI()
        {
            if (_runtimeParameterModel.ParameterList.Count <= 0)
            {
                List<BehaviorParameter> parameterList = BehaviorRunTime.Instance.ConditionCheck.GetAllParameter();
                _runtimeParameterModel.AddParameter(parameterList);
            }

            _runtimeParameterView.Draw(_runtimeParameterModel.ParameterList);
        }

    }

    public class BehaviorRuntimeParameterModel
    {
        private List<BehaviorParameter> _parameterList = new List<BehaviorParameter>();

        public BehaviorRuntimeParameterModel()
        {
        }

        public void AddParameter(List<BehaviorParameter> parameterList)
        {
            _parameterList = parameterList;
        }

        public List<BehaviorParameter> ParameterList
        {
            get
            {
                return _parameterList;
            }
        }

    }

    public class BehaviorRuntimeParameterView
    {
        private Vector2 scrollPos = Vector2.zero;
        public void Draw(List<BehaviorParameter> parameterList)
        {
            EditorGUILayout.LabelField("运行时变量");

            EditorGUILayout.BeginVertical("box", GUILayout.ExpandWidth(true));
            {
                EditorGUILayout.LabelField("条件参数");
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandHeight(true));
                {
                    GUI.backgroundColor = new Color(0.85f, 0.85f, 0.85f, 1f);
                    for (int i = 0; i < parameterList.Count; ++i)
                    {
                        BehaviorParameter behaviorParameter = parameterList[i];
                        EditorGUILayout.BeginVertical("box");
                        {
                            behaviorParameter = DrawParameter.Draw(behaviorParameter, DrawParameterType.RUNTIME_PARAMETER, null);
                        }
                        EditorGUILayout.EndVertical();
                    }
                    GUI.backgroundColor = Color.white;
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }

    }

}





