using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BehaviorTree
{

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
        private int option = 1;
        private readonly string[] optionArr = new string[] { "Play", "Stop", "Step" };

        public BehaviorPlayView()
        {

        }

        public void Draw()
        {
            EditorGUILayout.BeginVertical("box");
            {
                int index = option;
                option = GUILayout.Toolbar(option, optionArr, EditorStyles.toolbarButton);
                if (index != option)
                {
                    if (null != BehaviorManager.behaviorRuntimePlay)
                    {
                        BehaviorManager.behaviorRuntimePlay(option);
                    }
                }
            }
            EditorGUILayout.EndVertical();
        }
    }

}


