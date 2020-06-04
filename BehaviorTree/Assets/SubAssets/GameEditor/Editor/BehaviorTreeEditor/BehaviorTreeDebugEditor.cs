using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
        _treeDebug.OnSelect(false);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }


}
