using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class ConditionCheck : IConditionCheck
{
    //存储所有能用到的参数，数据来源于配置，在Init时候存到_environmentParameterDic中去，如果除了初始化时没别的地方用到，可以省略
    private List<BehaviorParameter> _parameterList = new List<BehaviorParameter>();
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

    public void SetParameter(string parameterName, string stringValue)
    {
        BehaviorParameter parameter = null;
        if (!_environmentParameterDic.TryGetValue(parameterName, out parameter)) // 当前行为树不需要的参数值就不保存了
        {
            return;
        }

        if (parameter.parameterType == (int)BehaviorParameterType.Int)
        {
            parameter.stringValue = stringValue;
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
            //ProDebug.Logger.LogError("parameter type invalid:" + parameter.parameterName);
            return;
        }

        environmentParameter.CloneFrom(parameter);
        _environmentParameterDic[parameter.parameterName] = environmentParameter;
    }

    private BehaviorParameter GetParameter(string parameterName)
    {
        BehaviorParameter environmentParameter = null;
        if (_environmentParameterDic.TryGetValue(parameterName, out environmentParameter)) // 当前行为树不需要的参数值就不保存了
        {
            return environmentParameter;
        }
        return null;
    }

    public bool GetParameterValue(string parameterName, ref int value)
    {
        BehaviorParameter environmentParameter = GetParameter(parameterName);
        if (null == parameterName) // 当前行为树不需要的参数值就不保存了
        {
            return false;
        }

        value = environmentParameter.intValue;
        return true;
    }

    public bool GetParameterValue(string parameterName, ref float value)
    {
        BehaviorParameter environmentParameter = GetParameter(parameterName);
        if (null == parameterName) // 当前行为树不需要的参数值就不保存了
        {
            return false;
        }

        value = environmentParameter.floatValue;
        return true;
    }

    public bool GetParameterValue(string parameterName, ref bool value)
    {
        BehaviorParameter environmentParameter = GetParameter(parameterName);
        if (null == parameterName) // 当前行为树不需要的参数值就不保存了
        {
            return false;
        }

        value = environmentParameter.boolValue;
        return true;
    }

    public bool GetParameterValue(string parameterName, ref string value)
    {
        BehaviorParameter environmentParameter = GetParameter(parameterName);
        if (null == parameterName) // 当前行为树不需要的参数值就不保存了
        {
            return false;
        }

        value = environmentParameter.stringValue;
        return true;
    }

    //将配置好的Parameter存到环境dic中
    public void InitParmeter()
    {
        for (int i = 0; i < _parameterList.Count; ++i)
        {
            BehaviorParameter parameter = _parameterList[i];

            BehaviorParameter cacheParaemter = null;
            if (!_environmentParameterDic.TryGetValue(parameter.parameterName, out cacheParaemter))
            {
                cacheParaemter = parameter.Clone();
                _environmentParameterDic[parameter.parameterName] = cacheParaemter;
            }

            cacheParaemter.intValue = parameter.intValue;
            cacheParaemter.floatValue = parameter.floatValue;
            cacheParaemter.boolValue = parameter.boolValue;
            cacheParaemter.stringValue = parameter.stringValue;
        }
    }

    public void AddParameter(List<BehaviorParameter> parameterList)
    {
        _parameterList.AddRange(parameterList);
        InitParmeter();
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
            //ProDebug.Logger.LogError("parameter Type not Equal:" + environmentParameter.parameterName + "    " + environmentParameter.parameterType + "    " + parameter.parameterType);
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

    public bool Condition(ConditionParameter conditionParameter)
    {
        bool result = true;
        for (int i = 0; i < conditionParameter.groupList.Count; ++i)
        {
            ConditionGroupParameter groupParameter = conditionParameter.groupList[i];
            result = true;

            for (int j = 0; j < groupParameter.parameterList.Count; ++j)
            {
                BehaviorParameter parameter = groupParameter.parameterList[j];
                bool value = Condition(parameter);
                if (!value)
                {
                    result = false;
                    break;
                }
            }

            if (result)
            {
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
