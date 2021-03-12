using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class BTConcrete : BTBase, IBTActionOwner
{
    private BaseSprite _owner;
    private BehaviorTreeData _data;
    private BehaviorTreeDebug _behaviorTreeDebug;
    
    public BTConcrete(BaseSprite owner, long aiFunction, string aiConfig)
    {
        _owner = owner;
        _data = DataCenter.behaviorData.GetBehaviorInfo(aiConfig);
        if (null == _data)
        {
            Debug.LogError("AIConfig not find:" + aiConfig);
        }
        SetData(aiFunction, _data);
        Init(_owner);
    }

    public void SetOwner(BaseSprite owner)
    {
        _owner = owner;
    }

    public virtual BaseSprite GetOwner()
    {
        return _owner;
    }

    protected override void Init(BaseSprite owner)
    {
        base.Init(owner);
        if (null == _behaviorTreeDebug && _owner.SpriteGameObject)
        {
            _behaviorTreeDebug = _owner.SpriteGameObject.AddComponent<BehaviorTreeDebug>();
            _behaviorTreeDebug.SetEntity(_data, _btEntity);
        }
    }

    public override void Update()
    {
        base.Update();
    }

}
