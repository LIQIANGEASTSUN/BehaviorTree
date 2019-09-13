using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{

    public class BehaviorRunTime
    {
        private NodeBase _rootNode = null;
        private IConditionCheck _iconditionCheck = null;

        private BehaviorPlayType _playState = BehaviorPlayType.STOP;
        private RunTimeRotateGo _runtimeRotateGo;
        public BehaviorRunTime()
        {
            Init();
        }

        private void Init()
        {
            _runtimeRotateGo = new RunTimeRotateGo();
            BehaviorManager.behaviorRuntimePlay += RuntimePlay;
        }

        public void OnDestroy()
        {
            BehaviorManager.behaviorRuntimePlay -= RuntimePlay;
            _runtimeRotateGo.OnDestroy();
        }

        public void Reset(BehaviorTreeData behaviorTreeData)
        {
            BehaviorAnalysis analysis = new BehaviorAnalysis();
            _iconditionCheck = new ConditionCheck();
            _rootNode = analysis.Analysis(behaviorTreeData, ref _iconditionCheck);
        }

        private void RuntimePlay(BehaviorPlayType state, BehaviorPlayType step)
        {
            if (_playState == BehaviorPlayType.STOP && state != BehaviorPlayType.STOP)
            {
                Reset(BehaviorManager.Instance.BehaviorTreeData);
            }
            _playState = state;

            if (_playState != BehaviorPlayType.PLAY && step == BehaviorPlayType.STEP)
            {
                Execute(true);
            }
        }

        public void Update()
        {
            _runtimeRotateGo.Update();

            Execute(false);
        }

        public void Execute(bool step)
        {
            if (_playState == BehaviorPlayType.STOP)
            {
                return;
            }

            if (_playState == BehaviorPlayType.PAUSE && !step)
            {
                return;
            }

            if (null != _rootNode)
            {
                _rootNode.Execute();
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

