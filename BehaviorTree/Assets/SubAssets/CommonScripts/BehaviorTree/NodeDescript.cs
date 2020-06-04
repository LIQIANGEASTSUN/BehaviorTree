using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{

    public class NodeDescript
    {

        public static string GetFunction(NODE_TYPE nodeType)
        {
            if (nodeType == NODE_TYPE.SELECT)
            {
                return GetSelect();
            }

            if (nodeType == NODE_TYPE.SEQUENCE)
            {
                return GetSequence();
            }

            if (nodeType == NODE_TYPE.RANDOM)
            {
                return GetRandom();
            }
            
            if (nodeType == NODE_TYPE.RANDOM_SEQUEUECE)
            {
                return GetRandomSequeuece();
            }

            if (nodeType == NODE_TYPE.RANDOM_PRIORITY)
            {
                return GetRandomPriority();
            }

            if (nodeType == NODE_TYPE.PARALLEL)
            {
                return GetParallel();
            }

            if (nodeType == NODE_TYPE.PARALLEL_SELECT)
            {
                return GetParallelSelect();
            }

            if (nodeType == NODE_TYPE.PARALLEL_ALL)
            {
                return GetParallelAll();
            }

            if (nodeType == NODE_TYPE.DECORATOR_INVERTER)
            {
                return GetDecoratorInverter();
            }

            if (nodeType == NODE_TYPE.DECORATOR_REPEAT)
            {
                return GetDecoratorRepeat();
            }

            if (nodeType == NODE_TYPE.DECORATOR_RETURN_FAIL)
            {
                return GetDecoratorReturnFail();
            }

            if (nodeType == NODE_TYPE.DECORATOR_RETURN_SUCCESS)
            {
                return GetDecoratorReturnSuccess();
            }

            if (nodeType == NODE_TYPE.DECORATOR_UNTIL_FAIL)
            {
                return GetDecoratorUntilFail();
            }

            if (nodeType == NODE_TYPE.DECORATOR_UNTIL_SUCCESS)
            {
                return GetDecoratorUntilSuccess();
            }

            return string.Empty;
        }

        private static string GetSelect()
        {
            return NodeSelect.descript;
        }

        private static string GetSequence()
        {
            return NodeSequence.descript;
        }

        private static string GetRandom()
        {
            return NodeRandom.descript;
        }

        private static string GetRandomSequeuece()
        {
            return NodeRandomSequence.descript;
        }

        private static string GetRandomPriority()
        {
            return NodeRandomPriority.descript;
        }

        private static string GetParallel()
        {
            return NodeParallel.descript;
        }

        private static string GetParallelSelect()
        {
            return NodeParallelSelect.descript;
        }

        private static string GetParallelAll()
        {
            return NodeParallelAll.descript;
        }

        private static string GetDecoratorInverter()
        {
            return NodeDecoratorInverter.descript;
        }

        private static string GetDecoratorRepeat()
        {
            return NodeDecoratorRepeat.descript;
        }

        private static string GetDecoratorReturnFail()
        {
            return NodeDecoratorReturnFail.descript;
        }

        private static string GetDecoratorReturnSuccess()
        {
            return NodeDecoratorReturnSuccess.descript;
        }

        private static string GetDecoratorUntilFail()
        {
            return NodeDecoratorUntilFail.descript;
        }

        private static string GetDecoratorUntilSuccess()
        {
            return NodeDecoratorUntilSuccess.descript;
        }

    }
}

