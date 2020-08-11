using System;
using System.Collections.Generic;
using BehaviorTree;

public abstract class BTBase : IAIPerformer
{
    protected BehaviorTreeEntity _btEntity = null;

    protected void SetData(BehaviorTreeData data, LoadConfigInfoEvent loadEvent)
    {
        _btEntity = new BehaviorTreeEntity(data, loadEvent);
    }

    public void UpdateParameter(string name, bool para)
    {
        _btEntity.ConditionCheck.SetParameter(name, para);
    }

    public void UpdateParameter(string name, int para)
    {
        _btEntity.ConditionCheck.SetParameter(name, para);
    }

    public void UpdateParameter(string name, float para)
    {
        _btEntity.ConditionCheck.SetParameter(name, para);
    }

    public void UpdateParameter(string name, string para)
    {
        _btEntity.ConditionCheck.SetParameter(name, para);
    }

    public bool GetParameterValue(string parameterName, ref int value)
    {
        return _btEntity.ConditionCheck.GetParameterValue(parameterName, ref value);
    }

    public bool GetParameterValue(string parameterName, ref float value)
    {
        return _btEntity.ConditionCheck.GetParameterValue(parameterName, ref value);
    }

    public bool GetParameterValue(string parameterName, ref bool value)
    {
        return _btEntity.ConditionCheck.GetParameterValue(parameterName, ref value);
    }

    public bool GetParameterValue(string parameterName, ref string value)
    {
        return _btEntity.ConditionCheck.GetParameterValue(parameterName, ref value);
    }

    public virtual void Update()
    {
        if (_btEntity != null)
        {
            _btEntity.Execute();
        }
    }
}
