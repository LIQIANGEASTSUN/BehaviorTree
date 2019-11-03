using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{

    public interface IConditionCheck
    {
        void AddParameter(List<BehaviorParameter> parameterList);

        void SetParameter(string parameterName, object value);

        void SetParameter(BehaviorParameter parameter);

        bool Condition(BehaviorParameter parameter);

        bool Condition(List<BehaviorParameter> parameterList);

        List<BehaviorParameter> GetAllParameter();
    }

}

