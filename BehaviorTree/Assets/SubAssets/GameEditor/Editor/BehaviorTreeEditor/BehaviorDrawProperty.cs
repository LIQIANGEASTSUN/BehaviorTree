using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BehaviorTree
{
    public class BehaviorDrawPropertyController
    {
        private BehaviorFileHandleController _fileHandleController;
        private BehaviorPlayController _playController;
        private BehaviorPropertyOption _propertyOption;
        private BehaviorDescriptController _descriptController;
        private BehaviorNodeInspector _nodeInspector;
        private BehaviorParameterController _parameterController;
        private BehaviorRuntimeParameter _runtimeParameter;

        public void Init()
        {
            _fileHandleController = new BehaviorFileHandleController();
            _playController = new BehaviorPlayController();
            _propertyOption = new BehaviorPropertyOption();
            _descriptController = new BehaviorDescriptController();
            _nodeInspector = new BehaviorNodeInspector();
            _parameterController = new BehaviorParameterController();
            _runtimeParameter = new BehaviorRuntimeParameter();
        }

        public void OnDestroy()
        {
            _fileHandleController.OnDestroy();
            _playController.OnDestroy();
            _nodeInspector.OnDestroy();
            _parameterController.OnDestroy();
            _runtimeParameter.OnDestroy();
        }

        public void OnGUI()
        {
            _fileHandleController.OnGUI();
            GUILayout.Space(8);

            _playController.OnGUI();
            GUILayout.Space(8);

            int option = _propertyOption.OnGUI();
            if (option == 0)
            {
                _descriptController.OnGUI();
            }
            else if (option == 1)
            {
                _nodeInspector.OnGUI();
            }
            else if (option == 2)
            {
                if (BehaviorManager.Instance.PlayType == BehaviorPlayType.PLAY
                    || BehaviorManager.Instance.PlayType == BehaviorPlayType.PAUSE)
                {
                    _runtimeParameter.OnGUI();
                }
                else
                {
                    _parameterController.OnGUI();
                }
            }
        }
    }

    public class BehaviorPropertyOption
    {
        private int option = 1;
        private readonly string[] optionArr = new string[] { "Descript", "Inspect", "Parameter" };

        public int OnGUI()
        {
            int index = option;
            option = GUILayout.Toolbar(option, optionArr, EditorStyles.toolbarButton);
            return option;
        }
    }
}


