using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BehaviorTree
{
    public class BehaviorDrawPropertyController
    {
        private BehaviorFileHandleController _fileHandleController;
        private BehaviorPropertyOption _propertyOption;
        private BehaviorNodeInspector _nodeInspector;
        private BehaviorGlobalParameter _globalParameterController;

        public void Init()
        {
            _fileHandleController = new BehaviorFileHandleController();
            _propertyOption = new BehaviorPropertyOption();
            _nodeInspector = new BehaviorNodeInspector();
            _globalParameterController = new BehaviorGlobalParameter();
        }

        public void OnDestroy()
        {
            _fileHandleController.OnDestroy();
            _nodeInspector.OnDestroy();
            _globalParameterController.OnDestroy();
        }

        public void OnGUI()
        {
            _fileHandleController.OnGUI();

            int option = _propertyOption.OnGUI();
            if (option == 1)
            {
                _nodeInspector.OnGUI();
            }
            else if (option == 2)
            {
                _globalParameterController.OnGUI();
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


