using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public static class NodeNotify
    {
        private static Dictionary<int, float> _nodeRunTimeDic = new Dictionary<int, float>();
        private static Dictionary<int, int> _nodeDrawDic = new Dictionary<int, int>();

        private static int _playState = -1;
        public static void NotifyExecute(int nodeId, float time)
        {
            _nodeRunTimeDic[nodeId] = time;
        }

        public static void SetPlayState(int state)
        {
            _playState = state;
        }

        public static float NodeDraw(int nodeId)
        {
            float time = 0;
            if (!_nodeRunTimeDic.TryGetValue(nodeId, out time))
            {
                return 0;
            }

            if (_playState == 1)
            {
                if (!_nodeDrawDic.ContainsKey(nodeId))
                {
                    _nodeDrawDic[nodeId] = 0;
                }
            }
            else
            {
                float offset = Time.realtimeSinceStartup - time;
                if (offset > (0.5f / Time.timeScale))
                {
                    _nodeDrawDic[nodeId] = 0;
                    return 0;
                }

                if (!_nodeDrawDic.ContainsKey(nodeId))
                {
                    _nodeDrawDic[nodeId] = 0;
                }

                _nodeDrawDic[nodeId] += 1;
                _nodeDrawDic[nodeId] %= 100;
            }

            return _nodeDrawDic[nodeId] * 0.01f;
        }

        //public static void NotifyExecute(int nodeId, ResultType resultType, float time)
        //{
        //    Debug.LogError("NotifyExecute:" + nodeId + "    " + time);
        //}

    }
}

