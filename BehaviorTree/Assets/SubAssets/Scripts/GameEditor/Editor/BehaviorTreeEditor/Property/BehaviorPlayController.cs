using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BehaviorTree
{
    public enum BehaviorPlayType
    {
        INVALID = -1,
        PLAY    = 0,
        PAUSE   = 1,
        STOP    = 2,
    }

    public class BehaviorPlayController
    {
        private BehaviorPlayModel _playModel;
        private BehaviorPlayView _playView;

        public BehaviorPlayController()
        {
            Init();
        }

        private void Init()
        {
            _playModel = new BehaviorPlayModel();
            _playView = new BehaviorPlayView();
        }

        public void OnDestroy()
        {

        }

        public void OnGUI()
        {
            _playView.Draw();
        }
        
    }

    public class BehaviorPlayModel
    {

        public BehaviorPlayModel()
        {

        }

    }

    public class BehaviorPlayView
    {
        private int option = 2;
        private readonly string[] optionArr = new string[] { "Play", "Pause", "Stop"};
        public BehaviorPlayView()
        {

        }

        public void Draw()
        {
            EditorGUILayout.BeginHorizontal("box");
            {
                int index = option;
                option = GUILayout.Toolbar(option, optionArr, EditorStyles.toolbarButton);
                if (index != option)
                {
                    if (null != BehaviorManager.behaviorRuntimePlay)
                    {
                        BehaviorManager.behaviorRuntimePlay((BehaviorPlayType)option);
                    }
                }

            }
            EditorGUILayout.EndHorizontal();
        }
    }

}


