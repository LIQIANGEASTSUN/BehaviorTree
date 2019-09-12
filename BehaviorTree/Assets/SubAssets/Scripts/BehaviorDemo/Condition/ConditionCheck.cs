using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class ConditionCheck : IConditionCheck
{
    // 保存当前世界状态中所有参数动态变化的值
    private Dictionary<string, BehaviorParameter> _parameterDic = new Dictionary<string, BehaviorParameter>();
    
    // 缓存当前行为树使用到的所有参数类型
    private Dictionary<string, BehaviorParameter> _allParameterDic = new Dictionary<string, BehaviorParameter>();

    public ConditionCheck()
    {

    }

    public void SetParameter(string parameterName, object value)
    {
        BehaviorParameter parameter = null;
        if (!EnableAdd(parameterName, ref parameter)) // 当前行为树不需要的参数值就不保存了
        {
            return;
        }

        if (parameter.parameterType == (int)BehaviorParameterType.Float)
        {
            if (value.GetType() != typeof(float)
                && value.GetType() != typeof(int))
            {
                Debug.LogError("parameter type invalid:" + parameterName);
                return;
            }

            parameter.floatValue = (float)value;
        }

        if (parameter.parameterType == (int)BehaviorParameterType.Int)
        {
            if (value.GetType() != typeof(int)
                && value.GetType() != typeof(float))
            {
                Debug.LogError("parameter type invalid:" + parameterName);
                return;
            }

            parameter.intValue = (int)value;
        }

        if (parameter.parameterType == (int)BehaviorParameterType.Bool)
        {
            if (value.GetType() != typeof(bool))
            {
                Debug.LogError("parameter type invalid:" + parameterName);
                return;
            }

            parameter.boolValue = (bool)value;
        }

        Debug.LogError("SetParameter:" + parameterName + "    " + value);
        _parameterDic[parameterName] = parameter;
    }

    private bool EnableAdd(string parameterName, ref BehaviorParameter parameter)
    {
        BehaviorParameter cacheParameter = null;
        if (!_allParameterDic.TryGetValue(parameterName, out cacheParameter)) // 当前行为树不需要的参数值就不保存了
        {
            return false;
        }

        parameter = cacheParameter.Clone();
        return true;
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

            Debug.LogError(parameter.parameterName + "     " + (BehaviorParameterType)(parameter.parameterType));
            _allParameterDic[parameter.parameterName] = parameter;
        }
    }

    public bool CompareParameter(BehaviorParameter parameter)
    {
        BehaviorParameter cacheParameter = null;
        if (!_parameterDic.TryGetValue(parameter.parameterName, out cacheParameter))
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
            bool value = CompareParameter(parameter);
            if (!value)
            {
                result = value;
                break;
            }
        }

        return result;
    }

}
