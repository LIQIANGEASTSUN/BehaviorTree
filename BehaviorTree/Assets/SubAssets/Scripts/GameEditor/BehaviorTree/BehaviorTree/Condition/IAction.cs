using System.Collections;
using System.Collections.Generic;

namespace BehaviorTree
{
    public interface IAction
    {
        bool DoAction(int nodeId, List<BehaviorParameter> parameterList);

        bool DoAction(int nodeId, BehaviorParameter parameter);
    }
}


