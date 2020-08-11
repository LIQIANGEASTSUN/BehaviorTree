using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{

    public class BehaviorRunTime
    {
        public static readonly BehaviorRunTime Instance = new BehaviorRunTime();

        private BehaviorTreeEntity _behaviorTreeEntity = null;

        private RunTimeRotateGo _runtimeRotateGo;
        private BehaviorRunTime()
        {
        }

        public void Init()
        {
            _runtimeRotateGo = new RunTimeRotateGo();

            BehaviorTreeDebug.AddTreeDebugEvent(Reset);
        }

        public void OnDestroy()
        {
            _runtimeRotateGo.OnDestroy();

            BehaviorTreeDebug.RemoveTreeDebugEvent(Reset);
        }

        public void Reset(BehaviorTreeData behaviorTreeData)
        {
            _behaviorTreeEntity = new BehaviorTreeEntity(behaviorTreeData, LoadConfig);
            BehaviorTreeEntity.CurrentDebugEntityId = _behaviorTreeEntity.EntityId;
            NodeNotify.Clear();
        }

        public void Reset(BehaviorTreeData behaviorTreeData, BehaviorTreeEntity behaviorTreeEntity)
        {
            BehaviorManager.Instance.BehaviorTreeData = behaviorTreeData;
            _behaviorTreeEntity = behaviorTreeEntity;
            BehaviorTreeEntity.CurrentDebugEntityId = _behaviorTreeEntity.EntityId;

            BehaviorManager.behaviorRuntimePlay(BehaviorPlayType.PLAY);
            NodeNotify.Clear();
        }

        private BehaviorTreeData LoadConfig(string fileName)
        {
            if (null != BehaviorManager.behaviorReadFile)
            {
                return BehaviorManager.behaviorReadFile(fileName, false);
            }
            return null;
        }

        public void Update()
        {
            _runtimeRotateGo.Update();

            Execute();
        }

        public BehaviorTreeEntity BehaviorTreeEntity
        {
            get
            {
                return _behaviorTreeEntity;
            }
        }

        public void Execute()
        {
            if (BehaviorManager.Instance.PlayType == BehaviorPlayType.STOP
                || (BehaviorManager.Instance.PlayType == BehaviorPlayType.PAUSE))
            {
                return;
            }

            if (Application.isPlaying)
            {
                return;
            }

            if (null != _behaviorTreeEntity)
            {
                _behaviorTreeEntity.Execute();
            }
        }
    }

}


// 编辑器模式下如果所有物体都静止  Time.realtimeSinceStartup 有时候会出bug，数值一直不变
public class RunTimeRotateGo
{
    private GameObject go;

    public RunTimeRotateGo()
    {

    }

    public void OnDestroy()
    {
        GameObject.DestroyImmediate(go);
    }

    public void Update()
    {
        if (!go)
        {
            go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.transform.position = Vector3.zero;
            go.transform.localScale = Vector3.one * 0.001f;
        }
        go.transform.Rotate(0, 5, 0);
    }

}

