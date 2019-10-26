using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{

    public interface IConditionCheck
    {
        void AddParameter(List<BehaviorParameter> parameterList);

        bool Condition(BehaviorParameter parameter);

        bool Condition(List<BehaviorParameter> parameterList);
    }

}

