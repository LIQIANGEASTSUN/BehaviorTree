using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using BehaviorTree;

public class TreeNodeWindow : EditorWindow {
    private BehaviorDrawPropertyController _behaviorDrawPropertyController;
    private BehaviorDrawController _behaviorDrawController;

    public static TreeNodeWindow window;
    private static Rect windowsPosition = new Rect(10, 30, 1236, 864);
    
    [MenuItem("Window/CreateTree")]
    public static void ShowWindow()
    {
        window = EditorWindow.GetWindow<TreeNodeWindow>();
        window.position = windowsPosition;
        window.autoRepaintOnSceneChange = true;
        window.Show();
    }

    private void OnEnable()
    {
        BehaviorManager.Instance.Init();

        _behaviorDrawPropertyController = new BehaviorDrawPropertyController();
        _behaviorDrawPropertyController.Init();

        _behaviorDrawController = new BehaviorDrawController();
        _behaviorDrawController.Init();

        BehaviorRunTime.Instance.Init();

        EditorApplication.update += OnFrame;
    }

    private void OnDisable()
    {
        BehaviorManager.Instance.OnDestroy();
        EditorApplication.update -= OnFrame;

        _behaviorDrawController.OnDestroy();
        _behaviorDrawPropertyController.OnDestroy();

        BehaviorRunTime.Instance.OnDestroy();
    }

    private void OnFrame()
    {
        BehaviorManager.Instance.Update();
        BehaviorRunTime.Instance.Update();
    }

    private void OnGUI()
    {
        if (null == window)
        {
            return;
        }

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.BeginVertical("box", GUILayout.Width(300), GUILayout.ExpandHeight(true));
            {
                _behaviorDrawPropertyController.OnGUI();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            {
                _behaviorDrawController.OnGUI(window);
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();


        Repaint();
    }

    public void DrawWindow(Action callBack)
    {
        // 开始绘制节点 
        // 注意：必须在  BeginWindows(); 和 EndWindows(); 之间 调用 GUI.Window 才能显示
        BeginWindows();
        {
            if (null != callBack)
            {
                callBack();
            }
        }
        EndWindows();
    }

    public void ShowNotification(string meg)
    {
        ShowNotification(new GUIContent(meg));
        //RemoveNotification();
    }
}