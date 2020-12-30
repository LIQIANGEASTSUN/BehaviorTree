using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(BehaviorTreeDebug))]
public class BehaviorTreeDebugEditor : Editor
{
    private BehaviorTreeDebug _treeDebug;

    private void OnEnable()
    {
        _treeDebug = target as BehaviorTreeDebug;

        _treeDebug.OnSelect(true);
    }

    private void OnDisable()
    {
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }


}
