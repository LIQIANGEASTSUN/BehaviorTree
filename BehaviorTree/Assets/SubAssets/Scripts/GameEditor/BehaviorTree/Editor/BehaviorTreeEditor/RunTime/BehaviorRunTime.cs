using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{

    public class BehaviorRunTime : IAction
    {
        public static readonly BehaviorRunTime Instance = new BehaviorRunTime();

        private NodeBase _rootNode = null;
        private IConditionCheck _iconditionCheck = null;

        private RunTimeRotateGo _runtimeRotateGo;
        private BehaviorRunTime()
        {
        }

        public void Init()
        {
            _runtimeRotateGo = new RunTimeRotateGo();
        }

        public void OnDestroy()
        {
            _runtimeRotateGo.OnDestroy();
        }

        public void Reset(BehaviorTreeData behaviorTreeData)
        {
            BehaviorAnalysis analysis = new BehaviorAnalysis();
            _iconditionCheck = new ConditionCheck();
            List<NodeLeaf> nodeLeafList = new List<NodeLeaf>();
            _rootNode = analysis.Analysis(behaviorTreeData, this, ref _iconditionCheck, ref nodeLeafList);
        }

        public ConditionCheck ConditionCheck
        {
            get { return (ConditionCheck)_iconditionCheck; }
        }

        public void RuntimePlay(BehaviorPlayType oldState, BehaviorPlayType newState)
        {
            if (oldState == BehaviorPlayType.STOP && newState != BehaviorPlayType.STOP)
            {
                Reset(BehaviorManager.Instance.BehaviorTreeData);
            }

            if (newState != BehaviorPlayType.PLAY)
            {
                Execute();
            }
        }

        public void Update()
        {
            _runtimeRotateGo.Update();

            Execute();
        }

        public void Execute()
        {
            if (BehaviorManager.Instance.PlayType == BehaviorPlayType.STOP)
            {
                return;
            }

            if (BehaviorManager.Instance.PlayType == BehaviorPlayType.PAUSE)
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

