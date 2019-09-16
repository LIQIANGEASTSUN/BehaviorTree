using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class Human
{
    private Transform _target;

    private NodeBase _rootNode = null;
    private IConditionCheck _iconditionCheck = null;

    private float _energy = 100;            // 能量
    private float _energyConsumption = 10f;// 能耗
    private float _energyMax = 100;
    private float _energyHungry = 30;
    private float _energyMin = 10;

    private float _food = 0;
    private float _foodMax = 100;
    private float _eatSpeed = 20;

    public Human(Transform target)
    {
        _target = target;
    }

    public void Init()
    {
    }

    public void OnDestroy()
    {
    }

    public void SetData(BehaviorTreeData behaviorTreeData)
    {
        BehaviorAnalysis analysis = new BehaviorAnalysis();
        _iconditionCheck = new ConditionCheck();
        _rootNode = analysis.Analysis(behaviorTreeData, ref _iconditionCheck);
    }

    public void SetData(string content)
    {
        BehaviorAnalysis analysis = new BehaviorAnalysis();
        _iconditionCheck = new ConditionCheck();
        _rootNode = analysis.Analysis(content, ref _iconditionCheck);
    }

    public ConditionCheck ConditionCheck
    {
        get { return (ConditionCheck)_iconditionCheck; }
    }

    public void Update()
    {
        _energy -= _energyConsumption * Time.deltaTime;
        if (_energy <= _energyMin)
        {
            _energy = _energyMin;
        }

        HungryController();
        FoodController();

        Execute();
    }

    private void Execute()
    {
        if (null != _rootNode)
        {
            _rootNode.Execute();
        }
    }

    private void HungryController()
    {
        bool hungry = (_energy <= _energyHungry);
        ConditionCheck.SetParameter("IsHungry", hungry);
    }

    public void AddFood(float value)
    {
        _food += value;
        _food = Mathf.Clamp(_food, 0, _foodMax);
    }

    public bool FoodEnougth()
    {
        return _food >= _foodMax;
    }

    public void Eat()
    {
        _food -= _eatSpeed;
        _energy += _eatSpeed * 2f;
        _energy = Mathf.Clamp(_energy, 0, _energyMax);
    }

    private void FoodController()
    {
        bool hasFood = (_food >= _foodMax);
        ConditionCheck.SetParameter("HasFood", hasFood);
    }

    // 厨房
    private GameObject kitchen;
    public Vector3 KitchenPos()
    {
        if (null == kitchen)
        {
            kitchen = GameObject.CreatePrimitive(PrimitiveType.Cube);
            kitchen.name = "Kitchen";
            kitchen.transform.position = new Vector3(0, 0, 5);
        }
        return kitchen.transform.position;
    }

    // 餐桌
    private GameObject diningTable;
    public Vector3 DiningTablePos()
    {
        if (null == diningTable)
        {
            diningTable = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            diningTable.name = "DiningTable";
            diningTable.transform.position = new Vector3(5, 0, 0);
        }

        return diningTable.transform.position;
    }

    public void Translate(Vector3 targetPos)
    {
        Vector3 dir = (targetPos - Position()).normalized;
        _target.Translate(dir * Time.deltaTime, Space.World);
    }

    public Vector3 Position()
    {
        return _target.position;
    }
}
