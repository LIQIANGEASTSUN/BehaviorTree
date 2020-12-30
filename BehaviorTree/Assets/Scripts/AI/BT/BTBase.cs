using System;
using System.Collections.Generic;
using BehaviorTree;

public abstract class BTBase : IAIPerformer
{
    protected BehaviorTreeEntity _btEntity = null;

    protected void SetData(long aiFunction, BehaviorTreeData data, LoadConfigInfoEvent loadEvent)
    {
        _btEntity = new BehaviorTreeEntity(aiFunction, data, loadEvent);
    }

    protected virtual void Init(BaseSprite owner)
    {
        InitNode(owner, _btEntity.RootNode);
    }

    private void InitNode(BaseSprite owner, NodeBase nodeBase)
    {
        if (nodeBase == null)
        {
            int a = 0;
        }
        if (nodeBase.NodeType == NODE_TYPE.ACTION)
        {
            ActionBase actionBase = nodeBase as ActionBase;
            if (null != actionBase)
            {
                actionBase.SetOwner(owner);
            }
        }
        else if (nodeBase.NodeType == NODE_TYPE.CONDITION)
        {
            ConditionBase conditionBase = nodeBase as ConditionBase;
            if (null != conditionBase)
            {
                conditionBase.SetOwner(owner);
            }
        }
        else
        {
            NodeComposite nodeComposite = nodeBase as NodeComposite;
            if (null == nodeComposite)
            {
                return;
            }
            foreach (var childNode in nodeComposite.GetChilds())
            {
                InitNode(owner, childNode);
            }
        }
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
