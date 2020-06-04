using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{

    public interface IConditionCheck
    {
        void InitParmeter();

        void AddParameter(List<BehaviorParameter> parameterList);

        void SetParameter(string parameterName, bool boolValue);

        void SetParameter(string parameterName, float floatValue);

        void SetParameter(string parameterName, int intValue);

        void SetParameter(BehaviorParameter parameter);

        bool Condition(BehaviorParameter parameter);

        bool Condition(ConditionParameter conditionParameter);

        List<BehaviorParameter> GetAllParameter();
    }

}

