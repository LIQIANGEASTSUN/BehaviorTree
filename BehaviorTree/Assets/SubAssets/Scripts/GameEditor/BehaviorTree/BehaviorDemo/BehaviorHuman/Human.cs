using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public interface IHuman
{
    void SetHuman(Human human);
}

public class Human
{
    private Transform _target;
    // 厨房
    private GameObject kitchen;
    // 餐桌
    private GameObject diningTable;
    // TV
    private GameObject TV;

    private BehaviorTreeEntity _behaviorTreeEntity = null;

    private float _energy = 100;            // 能量
    private float _energyConsumption = 10f;// 能耗
    private float _energyMax = 100;
    private float _energyHungry = 30;
    private float _energyMin = 10;

    private float _food = 0;
    private float _foodMax = 100;
    private float _eatSpeed = 0.8f;

    public Human(Transform target, GameObject kitchen, GameObject diningTable, GameObject TV)
    {
        _target = target;
        this.kitchen = kitchen;
        this.diningTable = diningTable;
        this.TV = TV;
    }

    public void SetData(string content)
    {
        _behaviorTreeEntity = new BehaviorTreeEntity(content);
        SetIHuman();
    }

    public void SetData(BehaviorTreeData treeData)
    {
        _behaviorTreeEntity = new BehaviorTreeEntity(treeData);
        SetIHuman();
    }

    private void SetIHuman()
    {
        for (int i = 0; i < _behaviorTreeEntity.ActionNodeList.Count; ++i)
        {
            NodeAction nodeAction = _behaviorTreeEntity.ActionNodeList[i];
            if (typeof(IHuman).IsAssignableFrom(nodeAction.GetType()))
            {
                IHuman iHuman = nodeAction as IHuman;
                iHuman.SetHuman(this);
            }
        }
    }

    public void Update()
    {
        _energy -= _energyConsumption * Time.deltaTime;
        if (_energy <= _energyMin)
        {
            _energy = _energyMin;
        }

        UpdateEnvironment();

        Execute();
    }

    private void Execute()
    {
        if (null != _behaviorTreeEntity)
        {
            _behaviorTreeEntity.Execute();
        }
    }

    public void UpdateEnvironment()
    {
        bool hungry = IsHungry();
        _behaviorTreeEntity.ConditionCheck.SetParameter("IsHungry", hungry);

        bool hasFood = (_food >= _foodMax);
        _behaviorTreeEntity.ConditionCheck.SetParameter("HasFood", hasFood);
    }

    public bool IsHungry()
    {
        return (_energy <= _energyHungry);
    }

    public bool Cooking(float value)
    {
        _food += value;
        _food = Mathf.Clamp(_food, 0, _foodMax);

        return _food < _foodMax;
    }

    public bool Eat()
    {
        _food -= _eatSpeed;
        _energy += _eatSpeed * 2f;
        _energy = Mathf.Clamp(_energy, 0, _energyMax);

        return _food > 0;
    }

    public Vector3 KitchenPos()
    {
        return kitchen.transform.position;
    }

    public Vector3 DiningTablePos()
    {
        return diningTable.transform.position;
    }

    public Vector3 TVPos()
    {
        return TV.transform.position;
    }

    public void Translate(Vector3 targetPos)
    {
        Vector3 dir = (targetPos - Position()).normalized;
        _target.Translate(dir * 3 * Time.deltaTime, Space.World);
    }

    public Vector3 Position()
    {
        return _target.position;
    }

}
