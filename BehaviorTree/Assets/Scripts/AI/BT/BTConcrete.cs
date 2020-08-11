using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public enum DecisionReason
{
    BySelfWhenIdle = 1, //IDLE下的自身决策
    Opreation = 2, //玩家操作 点击，拖拽
    MissionAlloc = 3,  //任务决策，双击某物品产生的任务
    TargetReason = 4, //目标丢失 或者 目标被打断
    PreTaskDone = 5, //之前的任务正常完成
}

public class BTConcrete : BTBase, IBTActionOwner
{
    private BaseSprite _owner;
    private BehaviorTreeData _data;
    private BehaviorTreeDebug _behaviorTreeDebug;
    
    public BTConcrete(string aiConfig)
    {
        _data = DataCenter.behaviorData.GetBehaviorInfo(aiConfig);
        SetData(_data, DataCenter.behaviorData.GetBehaviorInfo);
    }

    public void SetOwner(BaseSprite owner)
    {
        _owner = owner;
        Init();
    }

    public virtual BaseSprite GetOwner()
    {
        return _owner;
    }

    private void Init()
    {
        for (int i = 0; i < _btEntity.ActionNodeList.Count; ++i)
        {
            NodeAction action = _btEntity.ActionNodeList[i];
            ActionBase actionBase = action as ActionBase;
            if (null != actionBase)
            {
                actionBase.SetOwner(_owner);
            }
        }

        for (int i = 0; i < _btEntity.ConditionNodeList.Count; ++i)
        {
            NodeCondition condition = _btEntity.ConditionNodeList[i];
            ConditionBase conditionBase = condition as ConditionBase;
            if (null != conditionBase)
            {
                conditionBase.SetOwner(_owner);
            }
        }

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
