using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class ConditionCheck
{

    /*
             public int parameterType = 0;
        public string parameterName = string.Empty;
        public int intValue = 0;
        public float floatValue = 0;
        public bool boolValue = true;
        public int compare;
    */

    private Dictionary<string, BehaviorParameter> _parameterDic = new Dictionary<string, BehaviorParameter>();

    public ConditionCheck()
    {

    }

    public void SetParameter(BehaviorParameter parameter)
    {
        _parameterDic[parameter.parameterName] = parameter;
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

        return true;
    }

}
