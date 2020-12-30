using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class ConditionCheck : IConditionCheck
{
    private Dictionary<string, int> _intEnvironmentDic = new Dictionary<string, int>();
    private Dictionary<string, long> _longEnvironmentDic = new Dictionary<string, long>();
    private Dictionary<string, float> _floatEnvironmentDic = new Dictionary<string, float>();
    private Dictionary<string, bool> _boolEnvironmentDic = new Dictionary<string, bool>();
    private Dictionary<string, string> _stringEnvironmentDic = new Dictionary<string, string>();

    public ConditionCheck() {    }

    public void SetParameter(string parameterName, bool boolValue)
    {
        _boolEnvironmentDic[parameterName] = boolValue;
    }

    public void SetParameter(string parameterName, float floatValue)
    {
        _floatEnvironmentDic[parameterName] = floatValue;
    }

    public void SetParameter(string parameterName, int intValue)
    {
        _intEnvironmentDic[parameterName] = intValue;
    }

    public void SetParameter(string parameterName, long longValue)
    {
        _longEnvironmentDic[parameterName] = longValue;
    }

    public void SetParameter(string parameterName, string stringValue)
    {
        _stringEnvironmentDic[parameterName] = stringValue;
    }

    public void SetParameter(BehaviorParameter parameter)
    {
        if (parameter.parameterType == (int)(BehaviorParameterType.Int))
        {
            SetParameter(parameter.parameterName, parameter.intValue);
        }
        else if (parameter.parameterType == (int)(BehaviorParameterType.Long))
        {
            SetParameter(parameter.parameterName, parameter.longValue);
        }
        else if (parameter.parameterType == (int)(BehaviorParameterType.Float))
        {
            SetParameter(parameter.parameterName, parameter.floatValue);
        }
        else if (parameter.parameterType == (int)(BehaviorParameterType.Bool))
        {
            SetParameter(parameter.parameterName, parameter.boolValue);
        }
        else if (parameter.parameterType == (int)(BehaviorParameterType.String))
        {
            SetParameter(parameter.parameterName, parameter.stringValue);
        }
    }

    public bool GetParameterValue(string parameterName, ref int value)
    {
        if (_intEnvironmentDic.TryGetValue(parameterName, out value))
        {
            return true;
        }
        ////ProDebug.Logger.Log(//ProDebug.Logger.StrConcat("Not Fount Parameter:", parameterName));
        return false;
    }

    public bool GetParameterValue(string parameterName, ref long value)
    {
        if (_longEnvironmentDic.TryGetValue(parameterName, out value))
        {
            return true;
        }
        //ProDebug.Logger.Log(//ProDebug.Logger.StrConcat("Not Fount Parameter:", parameterName));
        return false;
    }

    public bool GetParameterValue(string parameterName, ref float value)
    {
        if (_floatEnvironmentDic.TryGetValue(parameterName, out value))
        {
            return true;
        }
        //ProDebug.Logger.Log(//ProDebug.Logger.StrConcat("Not Fount Parameter:", parameterName));
        return false;
    }

    public bool GetParameterValue(string parameterName, ref bool value)
    {
        if (_boolEnvironmentDic.TryGetValue(parameterName, out value))
        {
            return true;
        }
        //ProDebug.Logger.Log(//ProDebug.Logger.StrConcat("Not Fount Parameter:", parameterName));
        return false;
    }

    public bool GetParameterValue(string parameterName, ref string value)
    {
        if (_stringEnvironmentDic.TryGetValue(parameterName, out value))
        {
            return true;
        }
        //ProDebug.Logger.Log(//ProDebug.Logger.StrConcat("Not Fount Parameter:", parameterName));
        return false;
    }

    //将配置好的Parameter存到环境dic中
    public void InitParmeter()
    {

    }

    public void AddParameter(List<BehaviorParameter> parameterList)
    {
        UnityEngine.Profiling.Profiler.BeginSample("AddParameter");
        for (int i = 0; i < parameterList.Count; ++i)
        {
            BehaviorParameter parameter = parameterList[i];
            SetParameter(parameter);
        }
        UnityEngine.Profiling.Profiler.EndSample();
    }

    public bool Condition(BehaviorParameter parameter)
    {
        BehaviorCompare behaviorCompare = BehaviorCompare.EQUALS;
        if (parameter.parameterType == (int)(BehaviorParameterType.Int))
        {
            int intValue = 0;
            if (!GetParameterValue(parameter.parameterName, ref intValue))
            {
                return false;
            }
            behaviorCompare = BehaviorParameter.CompareInt(intValue, parameter.intValue);
        }
        else if (parameter.parameterType == (int)(BehaviorParameterType.Long))
        {
            long longValue = 0;
            if (!GetParameterValue(parameter.parameterName, ref longValue))
            {
                return false;
            }
            behaviorCompare = BehaviorParameter.CompareLong(longValue, parameter.longValue);
        }
        else if (parameter.parameterType == (int)(BehaviorParameterType.Float))
        {
            float floatValue = 0;
            if (!GetParameterValue(parameter.parameterName, ref floatValue))
            {
                return false;
            }
            behaviorCompare = BehaviorParameter.CompareFloat(floatValue, parameter.floatValue);
        }
        else if (parameter.parameterType == (int)(BehaviorParameterType.Bool))
        {
            bool boolValue = false;
            if (!GetParameterValue(parameter.parameterName, ref boolValue))
            {
                return false;
            }
            behaviorCompare = BehaviorParameter.CompareBool(boolValue, parameter.boolValue);
        }
        else if (parameter.parameterType == (int)(BehaviorParameterType.String))
        {
            string stringValue = string.Empty;
            if (!GetParameterValue(parameter.parameterName, ref stringValue))
            {
                return false;
            }
            behaviorCompare = BehaviorParameter.CompareString(stringValue, parameter.stringValue);
        }

        int value = (parameter.compare) & (int)behaviorCompare;
        return value > 0;

        //BehaviorParameter environmentParameter = null;
        //if (!_environmentParameterDic.TryGetValue(parameter.parameterName, out environmentParameter))
        //{
        //    return false;
        //}

        //if (environmentParameter.parameterType != parameter.parameterType)
        //{
        //    //ProDebug.Logger.LogError("parameter Type not Equal:" + environmentParameter.parameterName + "    " + environmentParameter.parameterType + "    " + parameter.parameterType);
        //    return false;
        //}

        //BehaviorCompare behaviorCompare = environmentParameter.Compare(parameter);
        //int value = (parameter.compare) & (int)behaviorCompare;
        //return value > 0;
    }

    public bool Condition(ConditionParameter conditionParameter)
    {
        bool result = true;
        List<ConditionGroupParameter> groupList = conditionParameter.GetGroupList();
        for (int i = 0; i < groupList.Count; ++i)
        {
            ConditionGroupParameter groupParameter = groupList[i];
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

        foreach(var kv in _intEnvironmentDic)
        {
            BehaviorParameter parameter = new BehaviorParameter();
            parameter.parameterName = kv.Key;
            parameter.parameterType = (int)(BehaviorParameterType.Int);
            parameter.intValue = kv.Value;

            parameterList.Add(parameter);
        }


        foreach (var kv in _longEnvironmentDic)
        {
            BehaviorParameter parameter = new BehaviorParameter();
            parameter.parameterName = kv.Key;
            parameter.parameterType = (int)(BehaviorParameterType.Long);
            parameter.longValue = kv.Value;

            parameterList.Add(parameter);
        }

        foreach (var kv in _floatEnvironmentDic)
        {
            BehaviorParameter parameter = new BehaviorParameter();
            parameter.parameterName = kv.Key;
            parameter.parameterType = (int)(BehaviorParameterType.Float);
            parameter.floatValue = kv.Value;

            parameterList.Add(parameter);
        }


        foreach (var kv in _boolEnvironmentDic)
        {
            BehaviorParameter parameter = new BehaviorParameter();
            parameter.parameterName = kv.Key;
            parameter.parameterType = (int)(BehaviorParameterType.Bool);
            parameter.boolValue = kv.Value;

            parameterList.Add(parameter);
        }

        foreach (var kv in _stringEnvironmentDic)
        {
            BehaviorParameter parameter = new BehaviorParameter();
            parameter.parameterName = kv.Key;
            parameter.parameterType = (int)(BehaviorParameterType.String);
            parameter.stringValue = kv.Value;

            parameterList.Add(parameter);
        }

        return parameterList;
    }

}




//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using BehaviorTree;

//public class ConditionCheck : IConditionCheck
//{
//    //存储所有能用到的参数，数据来源于配置，在Init时候存到_environmentParameterDic中去，如果除了初始化时没别的地方用到，可以省略
//    //private List<BehaviorParameter> _parameterList = new List<BehaviorParameter>();
//    // 缓存当前行为树使用到的所有参数类型,保存当前世界状态中所有参数动态变化的值
//    private Dictionary<string, BehaviorParameter> _environmentParameterDic = new Dictionary<string, BehaviorParameter>();

//    private Dictionary<string, int> _intEnvironmentDic = new Dictionary<string, int>();
//    private Dictionary<string, long> _longEnvironmentDic = new Dictionary<string, long>();
//    private Dictionary<string, float> _floatEnvironmentDic = new Dictionary<string, float>();
//    private Dictionary<string, bool> _boolEnvironmentDic = new Dictionary<string, bool>();
//    private Dictionary<string, string> _stringEnvironmentDic = new Dictionary<string, string>();

//    public ConditionCheck()
//    {

//    }

//    private BehaviorParameter CreateParameterBool(string parameterName, bool boolValue)
//    {
//        BehaviorParameter parameter = new BehaviorParameter();
//        parameter.parameterType = (int)(BehaviorParameterType.Bool);
//        parameter.boolValue = boolValue;
//        return parameter;
//    }

//    public void SetParameter(string parameterName, bool boolValue)
//    {
//        BehaviorParameter parameter = null;
//        if (!_environmentParameterDic.TryGetValue(parameterName, out parameter)) // 当前行为树不需要的参数值就不保存了
//        {
//            parameter = CreateParameterBool(parameterName, boolValue);
//            _environmentParameterDic[parameter.parameterName] = parameter;
//        }

//        if (parameter.parameterType != (int)(BehaviorParameterType.Bool))
//        {
//            //ProDebug.Logger.LogError(//ProDebug.Logger.StrConcat("parameter not Bool Type:", parameter.parameterName));
//            return;
//        }

//        parameter.boolValue = boolValue;
//    }

//    private BehaviorParameter CreateParameterFloat(string parameterName, float floatValue)
//    {
//        BehaviorParameter parameter = new BehaviorParameter();
//        parameter.parameterType = (int)(BehaviorParameterType.Float);
//        parameter.floatValue = floatValue;
//        return parameter;
//    }

//    public void SetParameter(string parameterName, float floatValue)
//    {
//        BehaviorParameter parameter = null;
//        if (!_environmentParameterDic.TryGetValue(parameterName, out parameter)) // 当前行为树不需要的参数值就不保存了
//        {
//            parameter = CreateParameterFloat(parameterName, floatValue);
//            _environmentParameterDic[parameter.parameterName] = parameter;
//        }

//        if (parameter.parameterType != (int)(BehaviorParameterType.Float))
//        {
//            //ProDebug.Logger.LogError(//ProDebug.Logger.StrConcat("parameter not Float Type:", parameter.parameterName));
//            return;
//        }

//        parameter.floatValue = floatValue;
//    }

//    private BehaviorParameter CreateParameterInt(string parameterName, int intValue)
//    {
//        BehaviorParameter parameter = new BehaviorParameter();
//        parameter.parameterType = (int)(BehaviorParameterType.Int);
//        parameter.intValue = intValue;
//        return parameter;
//    }

//    public void SetParameter(string parameterName, int intValue)
//    {
//        BehaviorParameter parameter = null;
//        if (!_environmentParameterDic.TryGetValue(parameterName, out parameter)) // 当前行为树不需要的参数值就不保存了
//        {
//            parameter = CreateParameterInt(parameterName, intValue);
//            _environmentParameterDic[parameter.parameterName] = parameter;
//        }

//        if (parameter.parameterType != (int)(BehaviorParameterType.Int))
//        {
//            //ProDebug.Logger.LogError(//ProDebug.Logger.StrConcat("parameter not Int Type:", parameter.parameterName));
//            return;
//        }

//        parameter.intValue = intValue;
//    }

//    private BehaviorParameter CreateParameterLong(string parameterName, long longValue)
//    {
//        BehaviorParameter parameter = new BehaviorParameter();
//        parameter.parameterType = (int)(BehaviorParameterType.Long);
//        parameter.longValue = longValue;
//        return parameter;
//    }

//    public void SetParameter(string parameterName, long longValue)
//    {
//        BehaviorParameter parameter = null;
//        if (!_environmentParameterDic.TryGetValue(parameterName, out parameter)) // 当前行为树不需要的参数值就不保存了
//        {
//            parameter = CreateParameterLong(parameterName, longValue);
//            _environmentParameterDic[parameter.parameterName] = parameter;
//        }

//        if (parameter.parameterType != (int)(BehaviorParameterType.Long))
//        {
//            //ProDebug.Logger.LogError(//ProDebug.Logger.StrConcat("parameter not Long Type:", parameter.parameterName));
//            return;
//        }

//        parameter.longValue = longValue;
//    }

//    private BehaviorParameter CreateParameterString(string parameterName, string stringValue)
//    {
//        BehaviorParameter parameter = new BehaviorParameter();
//        parameter.parameterType = (int)(BehaviorParameterType.String);
//        parameter.stringValue = stringValue;
//        return parameter;
//    }

//    public void SetParameter(string parameterName, string stringValue)
//    {
//        BehaviorParameter parameter = null;
//        if (!_environmentParameterDic.TryGetValue(parameterName, out parameter)) // 当前行为树不需要的参数值就不保存了
//        {
//            parameter = CreateParameterString(parameterName, stringValue);
//            _environmentParameterDic[parameter.parameterName] = parameter;
//        }

//        if (parameter.parameterType != (int)(BehaviorParameterType.String))
//        {
//            //ProDebug.Logger.LogError(//ProDebug.Logger.StrConcat("parameter not String Type:", parameter.parameterName));
//            return;
//        }

//        parameter.stringValue = stringValue;
//    }

//    public void SetParameter(BehaviorParameter parameter)
//    {
//        BehaviorParameter environmentParameter = null;
//        if (!_environmentParameterDic.TryGetValue(parameter.parameterName, out environmentParameter)) // 当前行为树不需要的参数值就不保存了
//        {
//            return;
//        }

//        if (parameter.parameterType != environmentParameter.parameterType)
//        {
//            //ProDebug.Logger.LogError(//ProDebug.Logger.StrConcat("parameter type invalid:", parameter.parameterName));
//            return;
//        }

//        environmentParameter.CloneFrom(parameter);
//    }

//    private BehaviorParameter GetParameter(string parameterName)
//    {
//        BehaviorParameter environmentParameter = null;
//        if (_environmentParameterDic.TryGetValue(parameterName, out environmentParameter)) // 当前行为树不需要的参数值就不保存了
//        {
//            return environmentParameter;
//        }
//        return null;
//    }

//    public bool GetParameterValue(string parameterName, ref int value)
//    {
//        BehaviorParameter environmentParameter = GetParameter(parameterName);
//        if (null == environmentParameter) // 当前行为树不需要的参数值就不保存了
//        {
//            return false;
//        }

//        if (environmentParameter.parameterType != (int)(BehaviorParameterType.Int))
//        {
//            //ProDebug.Logger.LogError(//ProDebug.Logger.StrConcat("parameter not Int Type:", environmentParameter.parameterName));
//            return false;
//        }

//        value = environmentParameter.intValue;
//        return true;
//    }

//    public bool GetParameterValue(string parameterName, ref long value)
//    {
//        BehaviorParameter environmentParameter = GetParameter(parameterName);
//        if (null == environmentParameter) // 当前行为树不需要的参数值就不保存了
//        {
//            return false;
//        }

//        if (environmentParameter.parameterType != (int)(BehaviorParameterType.Long))
//        {
//            //ProDebug.Logger.LogError(//ProDebug.Logger.StrConcat("parameter not Long Type:", environmentParameter.parameterName));
//            return false;
//        }

//        value = environmentParameter.longValue;
//        return true;
//    }

//    public bool GetParameterValue(string parameterName, ref float value)
//    {
//        BehaviorParameter environmentParameter = GetParameter(parameterName);
//        if (null == environmentParameter) // 当前行为树不需要的参数值就不保存了
//        {
//            return false;
//        }

//        if (environmentParameter.parameterType != (int)(BehaviorParameterType.Float))
//        {
//            //ProDebug.Logger.LogError(//ProDebug.Logger.StrConcat("parameter not Float Type:", environmentParameter.parameterName));
//            return false;
//        }

//        value = environmentParameter.floatValue;
//        return true;
//    }

//    public bool GetParameterValue(string parameterName, ref bool value)
//    {
//        BehaviorParameter environmentParameter = GetParameter(parameterName);
//        if (null == environmentParameter) // 当前行为树不需要的参数值就不保存了
//        {
//            return false;
//        }

//        if (environmentParameter.parameterType != (int)(BehaviorParameterType.Bool))
//        {
//            //ProDebug.Logger.LogError(//ProDebug.Logger.StrConcat("parameter not Bool Type:", environmentParameter.parameterName));
//            return false;
//        }

//        value = environmentParameter.boolValue;
//        return true;
//    }

//    public bool GetParameterValue(string parameterName, ref string value)
//    {
//        BehaviorParameter environmentParameter = GetParameter(parameterName);
//        if (null == environmentParameter) // 当前行为树不需要的参数值就不保存了
//        {
//            return false;
//        }

//        if (environmentParameter.parameterType != (int)(BehaviorParameterType.String))
//        {
//            //ProDebug.Logger.LogError(//ProDebug.Logger.StrConcat("parameter not String Type:", environmentParameter.parameterName));
//            return false;
//        }

//        value = environmentParameter.stringValue;
//        return true;
//    }

//    //将配置好的Parameter存到环境dic中
//    public void InitParmeter()
//    {

//    }

//    public void AddParameter(List<BehaviorParameter> parameterList)
//    {
//        UnityEngine.Profiling.Profiler.BeginSample("AddParameter");
//        for (int i = 0; i < parameterList.Count; ++i)
//        {
//            BehaviorParameter parameter = parameterList[i];

//            BehaviorParameter cacheParaemter = null;
//            if (!_environmentParameterDic.TryGetValue(parameter.parameterName, out cacheParaemter))
//            {
//                cacheParaemter = parameter.Clone();
//                _environmentParameterDic[parameter.parameterName] = cacheParaemter;
//            }
//        }
//        UnityEngine.Profiling.Profiler.EndSample();
//    }

//    public bool Condition(BehaviorParameter parameter)
//    {
//        BehaviorParameter environmentParameter = null;
//        if (!_environmentParameterDic.TryGetValue(parameter.parameterName, out environmentParameter))
//        {
//            return false;
//        }

//        if (environmentParameter.parameterType != parameter.parameterType)
//        {
//            //ProDebug.Logger.LogError("parameter Type not Equal:" + environmentParameter.parameterName + "    " + environmentParameter.parameterType + "    " + parameter.parameterType);
//            return false;
//        }

//        BehaviorCompare behaviorCompare = environmentParameter.Compare(parameter);
//        int value = (parameter.compare) & (int)behaviorCompare;
//        return value > 0;
//    }

//    public bool Condition(ConditionParameter conditionParameter)
//    {
//        bool result = true;
//        for (int i = 0; i < conditionParameter.groupList.Count; ++i)
//        {
//            ConditionGroupParameter groupParameter = conditionParameter.groupList[i];
//            result = true;

//            for (int j = 0; j < groupParameter.parameterList.Count; ++j)
//            {
//                BehaviorParameter parameter = groupParameter.parameterList[j];
//                bool value = Condition(parameter);
//                if (!value)
//                {
//                    result = false;
//                    break;
//                }
//            }

//            if (result)
//            {
//                break;
//            }
//        }

//        return result;
//    }

//    public List<BehaviorParameter> GetAllParameter()
//    {
//        List<BehaviorParameter> parameterList = new List<BehaviorParameter>();
//        foreach(var kv in _environmentParameterDic)
//        {
//            parameterList.Add(kv.Value);
//        }

//        return parameterList;
//    }

//}
