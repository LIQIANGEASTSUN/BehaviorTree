using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{

    public class ConditionGroupParameter
    {
        public List<BehaviorParameter> parameterList = new List<BehaviorParameter>();

        public void AddParameter(BehaviorParameter parameter)
        {
            parameterList.Add(parameter);
        }

    }

    public class ConditionParameter
    {
        public List<ConditionGroupParameter> groupList = new List<ConditionGroupParameter>();

        public ConditionParameter()
        {

        }

        public void SetGroup(List<ConditionGroup> conditionGroupList, List<BehaviorParameter> parameterList)
        {
            for (int i = 0; i < conditionGroupList.Count; ++i)
            {
                ConditionGroup conditionGroup = conditionGroupList[i];
                ConditionGroupParameter group = GetParameter(conditionGroup, parameterList);
                groupList.Add(group);
            }
        }

        private ConditionGroupParameter GetParameter(ConditionGroup conditionGroup, List<BehaviorParameter> parameterList)
        {
            ConditionGroupParameter group = new ConditionGroupParameter();
            for (int i = 0; i < conditionGroup.parameterList.Count; ++i)
            {
                string parameter = conditionGroup.parameterList[i];
                for (int j = 0; j < parameterList.Count; ++j)
                {
                    if (parameter.CompareTo(parameterList[j].parameterName) == 0)
                    {
                        group.parameterList.Add(parameterList[j]);
                    }
                }
            }

            return group;
        }

    }
}
