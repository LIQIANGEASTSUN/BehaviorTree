using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class Human : IAction
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
    private float _eatSpeed = 0.3f;

    public Human(Transform target)
    {
        _target = target;
        Init();
    }

    public void Init()
    {
        Create();
    }

    public void OnDestroy()
    {
    }

    public void SetData(BehaviorTreeData behaviorTreeData)
    {
        //BehaviorAnalysis analysis = new BehaviorAnalysis();
        //_iconditionCheck = new ConditionCheck();
        //_rootNode = analysis.Analysis(behaviorTreeData, this, ref _iconditionCheck);
    }

    public void SetData(string content)
    {
        BehaviorAnalysis analysis = new BehaviorAnalysis();
        _iconditionCheck = new ConditionCheck();
        _rootNode = analysis.Analysis(content, this, _iconditionCheck);
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
        bool hungry = IsHungry();
        ConditionCheck.SetParameter("IsHungry", hungry);
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

    private void FoodController()
    {
        bool hasFood = (_food >= _foodMax);
        ConditionCheck.SetParameter("HasFood", hasFood);
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


    // 厨房
    private GameObject kitchen;
    // 餐桌
    private GameObject diningTable;
    // TV
    private GameObject TV;
    private void Create()
    {
        {
            kitchen = new GameObject("Kitchen"); // 
            kitchen.transform.rotation = Quaternion.Euler(90, 0, 0);
            kitchen.transform.localScale = Vector3.one * 0.1f;
            kitchen.transform.position = new Vector3(0, 0, 5);
            TextMesh textMesh = kitchen.AddComponent<TextMesh>();
            textMesh.fontSize = 100;
            textMesh.text = "厨房";
        }

        {
            diningTable = new GameObject("DiningTable");
            diningTable.transform.rotation = Quaternion.Euler(90, 0, 0);
            diningTable.transform.localScale = Vector3.one * 0.1f;
            diningTable.transform.position = new Vector3(5, 0, 0);
            TextMesh textMesh = diningTable.AddComponent<TextMesh>();
            textMesh.fontSize = 100;
            textMesh.text = "餐桌";
        }

        {
            TV = new GameObject("TV");
            TV.transform.rotation = Quaternion.Euler(90, 0, 0);
            TV.transform.localScale = Vector3.one * 0.1f;
            TV.transform.position = new Vector3(-5, 0, 0);
            TextMesh textMesh = TV.AddComponent<TextMesh>();
            textMesh.fontSize = 100;
            textMesh.text = "TV";
        }
       
    }

    public bool DoAction(int nodeId, List<BehaviorParameter> parameterList)
    {
        return true;
    }

    public bool DoAction(int nodeId, BehaviorParameter parameter)
    {
        return true;
    }
}
