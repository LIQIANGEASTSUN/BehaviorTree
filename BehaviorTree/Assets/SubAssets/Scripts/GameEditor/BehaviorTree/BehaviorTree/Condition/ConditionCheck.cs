using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class ConditionCheck : IConditionCheck
{
    // 缓存当前行为树使用到的所有参数类型,保存当前世界状态中所有参数动态变化的值
    private Dictionary<string, BehaviorParameter> _allParameterDic = new Dictionary<string, BehaviorParameter>();

    public ConditionCheck()
    {

    }

    public void SetParameter(string parameterName, bool boolValue)
    {
        BehaviorParameter parameter = null;
        if (!_allParameterDic.TryGetValue(parameterName, out parameter)) // 当前行为树不需要的参数值就不保存了
        {
            return;
        }

        if (parameter.parameterType == (int)BehaviorParameterType.Bool)
        {
            parameter.boolValue = boolValue;
            _allParameterDic[parameterName] = parameter;
        }
    }

    public void SetParameter(string parameterName, float floatValue)
    {
        BehaviorParameter parameter = null;
        if (!_allParameterDic.TryGetValue(parameterName, out parameter)) // 当前行为树不需要的参数值就不保存了
        {
            return;
        }

        if (parameter.parameterType == (int)BehaviorParameterType.Float)
        {
            parameter.floatValue = floatValue;
            _allParameterDic[parameterName] = parameter;
        }
    }

    public void SetParameter(string parameterName, int intValue)
    {
        BehaviorParameter parameter = null;
        if (!_allParameterDic.TryGetValue(parameterName, out parameter)) // 当前行为树不需要的参数值就不保存了
        {
            return;
        }

        if (parameter.parameterType == (int)BehaviorParameterType.Int)
        {
            parameter.intValue = intValue;
            _allParameterDic[parameterName] = parameter;
        }
    }

    public void SetParameter(BehaviorParameter parameter)
    {
        BehaviorParameter cacheParameter = null;
        if (!_allParameterDic.TryGetValue(parameter.parameterName, out cacheParameter)) // 当前行为树不需要的参数值就不保存了
        {
            return;
        }

        if (parameter.parameterType != cacheParameter.parameterType)
        {
            Debug.LogError("parameter type invalid:" + parameter.parameterName);
            return;
        }

        cacheParameter.CloneFrom(parameter);
        _allParameterDic[parameter.parameterName] = cacheParameter;
    }

    public void AddParameter(List<BehaviorParameter> parameterList)
    {
        for (int i = 0; i < parameterList.Count; ++i)
        {
            BehaviorParameter parameter = parameterList[i];
            if (_allParameterDic.ContainsKey(parameter.parameterName))
            {
                continue;
            }

            _allParameterDic[parameter.parameterName] = parameter.Clone();
            _allParameterDic[parameter.parameterName].intValue = parameter.intValue;
            _allParameterDic[parameter.parameterName].floatValue = parameter.floatValue;
            _allParameterDic[parameter.parameterName].boolValue = parameter.boolValue;
        }
    }

    public bool CompareParameter(BehaviorParameter parameter)
    {
        BehaviorParameter cacheParameter = null;
        if (!_allParameterDic.TryGetValue(parameter.parameterName, out cacheParameter))
        {
            return false;
        }

        if (cacheParameter.parameterType != parameter.parameterType)
        {
            Debug.LogError("parameter Type not Equal:" + cacheParameter.parameterName + "    " + cacheParameter.parameterType + "    " + parameter.parameterType);
            return false;
        }

        return parameter.Compare(cacheParameter);
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
        foreach(var kv in _allParameterDic)
        {
            parameterList.Add(kv.Value);
        }

        return parameterList;
    }

}
