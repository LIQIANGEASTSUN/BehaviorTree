using System.Collections;
using System.Collections.Generic;

namespace BehaviorTree
{
    public interface IAction
    {
        bool DoAction(List<BehaviorParameter> parameterList);

        bool DoAction(BehaviorParameter parameter);
    }
}


