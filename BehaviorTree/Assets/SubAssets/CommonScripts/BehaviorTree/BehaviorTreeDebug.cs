using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class BehaviorTreeDebug : MonoBehaviour
{
    private BehaviorTreeData _behaviorTreeData;
    private BehaviorTreeEntity _entity;

    public delegate void BehaviorTreeDebugEvent(BehaviorTreeData behaviorTreeData, BehaviorTreeEntity entity);
    private static BehaviorTreeDebugEvent _treeDebugEvent;

    private void Awake()
    {
    }

    public void OnSelect(bool value)
    {
        if (value)
        {
            _treeDebugEvent?.Invoke(_behaviorTreeData, _entity);
        }
    }

    public void SetEntity(BehaviorTreeData behaviorTreeData, BehaviorTreeEntity entity)
    {
        _behaviorTreeData = behaviorTreeData;
        _entity = entity;
    }

    public static void AddTreeDebugEvent(BehaviorTreeDebugEvent treeDebugEvent)
    {
        _treeDebugEvent += treeDebugEvent;
    }

    public static void RemoveTreeDebugEvent(BehaviorTreeDebugEvent treeDebugEvent)
    {
        _treeDebugEvent -= treeDebugEvent;
    }
}
