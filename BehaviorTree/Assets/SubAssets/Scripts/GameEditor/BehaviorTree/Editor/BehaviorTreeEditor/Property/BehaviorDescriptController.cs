using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BehaviorTree
{
    public class BehaviorDescriptController
    {
        private BehaviorDescriptModel _descriptModel;
        private BehaviorDescriptView _descriptView;

        public BehaviorDescriptController()
        {
            Init();
        }

        public void Init()
        {
            _descriptModel = new BehaviorDescriptModel();
            _descriptView = new BehaviorDescriptView();
        }

        public void OnDestroy()
        {

        }

        public void OnGUI()
        {
            BehaviorTreeData data = _descriptModel.GetData();
            _descriptView.Draw(data);
        }

    }


    public class BehaviorDescriptModel
    {
        public BehaviorTreeData GetData()
        {
            return BehaviorManager.Instance.BehaviorTreeData;
        }
    }

    public class BehaviorDescriptView
    {

        public void Draw(BehaviorTreeData data)
        {
            Rect rect = GUILayoutUtility.GetRect(0f, 0, GUILayout.ExpandWidth(true));
            EditorGUILayout.BeginVertical("box");
            {
                data.descript = EditorGUILayout.TextArea(data.descript, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            }
            EditorGUILayout.EndVertical();
        }

    }


}