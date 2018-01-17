using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class Student : MonoBehaviour {

    private NodeSelect nodeSelect = null;

    private float energy = 0;
    private float minEnergy = 50;
    private float maxEnergy = 100;

    private float food = 0;
    private float minFood = 0;
    private float maxFood = 100;

	// Use this for initialization
	void Start () {
        Init();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.A))
        {
            energy = 0;
        }

        if (nodeSelect != null)
        {
            nodeSelect.Execute();
        }
	}

    private void Init()
    {
        nodeSelect = new NodeSelect();

        NodeSequence nodeSequence = new NodeSequence();
        nodeSelect.AddNode(nodeSequence);

        #region 是否饿了
        NodeConditionHungry nodeConditionHungry = new NodeConditionHungry();
        nodeConditionHungry.SetStudent(this);
        nodeSequence.AddNode(nodeConditionHungry);
        #endregion

        NodeSelect nodeSelect2 = GetFood();
        nodeSequence.AddNode(nodeSelect2);

        #region 吃饭
        NodeActionEat nodeActionEat = new NodeActionEat();
        nodeActionEat.SetStudent(this);
        nodeSequence.AddNode(nodeActionEat);
        #endregion
    }

    // 搞定饭
    private NodeSelect GetFood()
    {
        NodeSelect nodeSelect2 = new NodeSelect();

        NodeConditionHasFood nodeConditionHasFood = new NodeConditionHasFood();
        nodeConditionHasFood.SetStudent(this);
        nodeSelect2.AddNode(nodeConditionHasFood);

        NodeActionCooking nodeActionCooking = new NodeActionCooking();
        nodeActionCooking.SetStudent(this);
        nodeSelect2.AddNode(nodeActionCooking);

        return nodeSelect2;
    }

    #region 饿了吃饭
    public bool IsHungry()
    {
        return energy <= minEnergy;
    }

    public void AddEnergy(float value)
    {
        energy += value;
        Debug.LogError("Eat");
    }

    public bool IsFull()
    {
        Debug.LogError("IsFull ：" + (energy >= maxEnergy));
        return energy >= maxEnergy;
    }
    #endregion

    #region 食物
    public bool HasFood()
    {
        return food > minFood;
    }

    public bool FoodEnough()
    {
        return food >= maxFood;
    }

    public void Cooking(float value)
    {
        ChangeFood(value);
        Debug.LogError("Cooking");
    }

    public void ChangeFood(float value)
    {
        food += value;
    }
    #endregion
}
