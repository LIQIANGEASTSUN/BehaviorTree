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
            GlobalParameter globalParameter = _runtimeParameterModel.GlobalParameter;
            _runtimeParameterView.Draw(globalParameter);
        }
    }

    public class BehaviorRuntimeParameterModel
    {
        public BehaviorRuntimeParameterModel()
        {
        }

        public GlobalParameter GlobalParameter
        {
            get
            {
                return BehaviorManager.Instance.GlobalParameter;
            }
        }
    }

    public class BehaviorRuntimeParameterView
    {
        private Vector2 scrollPos = Vector2.zero;
        public void Draw(GlobalParameter globalParameter)
        {
            EditorGUILayout.LabelField("运行时变量");

            EditorGUILayout.BeginVertical("box", GUILayout.ExpandWidth(true));
            {
                EditorGUILayout.LabelField("条件参数");
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandHeight(true));
                {
                    GUI.backgroundColor = new Color(0.85f, 0.85f, 0.85f, 1f);
                    for (int i = 0; i < globalParameter.parameterList.Count; ++i)
                    {
                        BehaviorParameter behaviorParameter = globalParameter.parameterList[i];

                        Action DelCallBack = () =>
                        {
                            if (null != BehaviorManager.behaviorNodeParameter)
                            {
                                BehaviorManager.globalParameterChange(behaviorParameter, false);
                            }
                        };

                        EditorGUILayout.BeginVertical("box");
                        {
                            behaviorParameter = DrawParameter.Draw(behaviorParameter, DrawParameterType.RUNTIME_PARAMETER, DelCallBack);
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





