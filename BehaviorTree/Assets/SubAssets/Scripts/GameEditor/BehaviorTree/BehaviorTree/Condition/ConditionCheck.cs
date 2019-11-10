using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class ConditionCheck : IConditionCheck
{
    // 缓存当前行为树使用到的所有参数类型,保存当前世界状态中所有参数动态变化的值
    private Dictionary<string, BehaviorParameter> _environmentParameterDic = new Dictionary<string, BehaviorParameter>();

    public ConditionCheck()
    {

    }

    public void SetParameter(string parameterName, bool boolValue)
    {
        BehaviorParameter parameter = null;
        if (!_environmentParameterDic.TryGetValue(parameterName, out parameter)) // 当前行为树不需要的参数值就不保存了
        {
            return;
        }

        if (parameter.parameterType == (int)BehaviorParameterType.Bool)
        {
            parameter.boolValue = boolValue;
            _environmentParameterDic[parameterName] = parameter;
        }
    }

    public void SetParameter(string parameterName, float floatValue)
    {
        BehaviorParameter parameter = null;
        if (!_environmentParameterDic.TryGetValue(parameterName, out parameter)) // 当前行为树不需要的参数值就不保存了
        {
            return;
        }

        if (parameter.parameterType == (int)BehaviorParameterType.Float)
        {
            parameter.floatValue = floatValue;
            _environmentParameterDic[parameterName] = parameter;
        }
    }

    public void SetParameter(string parameterName, int intValue)
    {
        BehaviorParameter parameter = null;
        if (!_environmentParameterDic.TryGetValue(parameterName, out parameter)) // 当前行为树不需要的参数值就不保存了
        {
            return;
        }

        if (parameter.parameterType == (int)BehaviorParameterType.Int)
        {
            parameter.intValue = intValue;
            _environmentParameterDic[parameterName] = parameter;
        }
    }

    public void SetParameter(BehaviorParameter parameter)
    {
        BehaviorParameter environmentParameter = null;
        if (!_environmentParameterDic.TryGetValue(parameter.parameterName, out environmentParameter)) // 当前行为树不需要的参数值就不保存了
        {
            return;
        }

        if (parameter.parameterType != environmentParameter.parameterType)
        {
            Debug.LogError("parameter type invalid:" + parameter.parameterName);
            return;
        }

        environmentParameter.CloneFrom(parameter);
        _environmentParameterDic[parameter.parameterName] = environmentParameter;
    }

    public void AddParameter(List<BehaviorParameter> parameterList)
    {
        for (int i = 0; i < parameterList.Count; ++i)
        {
            BehaviorParameter parameter = parameterList[i];
            if (_environmentParameterDic.ContainsKey(parameter.parameterName))
            {
                continue;
            }

            _environmentParameterDic[parameter.parameterName] = parameter.Clone();
            _environmentParameterDic[parameter.parameterName].intValue = parameter.intValue;
            _environmentParameterDic[parameter.parameterName].floatValue = parameter.floatValue;
            _environmentParameterDic[parameter.parameterName].boolValue = parameter.boolValue;
        }
    }

    public bool CompareParameter(BehaviorParameter parameter)
    {
        BehaviorParameter environmentParameter = null;
        if (!_environmentParameterDic.TryGetValue(parameter.parameterName, out environmentParameter))
        {
            return false;
        }

        if (environmentParameter.parameterType != parameter.parameterType)
        {
            Debug.LogError("parameter Type not Equal:" + environmentParameter.parameterName + "    " + environmentParameter.parameterType + "    " + parameter.parameterType);
            return false;
        }

        BehaviorCompare behaviorCompare = environmentParameter.Compare(parameter);
        int value = (parameter.compare) & (int)behaviorCompare;
        return value > 0;
    }

    public bool Condition(BehaviorParameter parameter)
    {
        return CompareParameter(parameter);
    }

    public bool Condition(List<BehaviorParameter> parameterList)
    {
        bool result = true;
        for (int i = 0; i < parameterList.Count; ++i)
        {
            BehaviorParameter parameter = parameterList[i];
            bool value = Condition(parameter);
            if (!value)
            {
                result = value;
                break;
            }
        }

        return result;
    }

    public List<BehaviorParameter> GetAllParameter()
    {
        List<BehaviorParameter> parameterList = new List<BehaviorParameter>();
        foreach(var kv in _environmentParameterDic)
        {
            parameterList.Add(kv.Value);
        }

        return parameterList;
    }

}
