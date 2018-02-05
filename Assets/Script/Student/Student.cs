using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class Student : MonoBehaviour {

    private NodeRoot rootNode = null; // 根节点

    private float energy = 0;      // 能量值
    private float minEnergy = 50;  // 能量下限：当能量 <= 能量下限      感到饥饿
    private float maxEnergy = 100; // 能量上限：当能量 >= 能量上线时    表示吃饱了

    private float food = 0;        // 食物量：
    private float minFood = 0;     // 食物下限：当食物量 <= 食物下限时  没有食物了
    private float maxFood = 100;   // 食物上限：当食物量 >= 食物上限时  表示饭做好了

    // Use this for initialization
    void Start()
    {
        Init();
    }

    public int index = 0;
    private float time;

    public static float intervalTime = 0.3f;
    // Update is called once per frame
    void Update()
    {
        ChangeEnergy(-0.8f); // 每帧执行消耗能量

        if (rootNode != null)
        {
            rootNode.Execute();  // 每帧调用行为树根节点，行为树入口
        }
    }

    /// <summary>
    /// 初始化添加行为树节点
    /// </summary>
    private void Init()
    {
        NodeAsset nodeAsset = Resources.Load<NodeAsset>("NodeAsset/Demo");
        if (nodeAsset == null)
        {
            Debug.LogError("NodeAsset is null");
            return;
        }

        NodeAssetToNode nodeAssetToNode = new NodeAssetToNode();
        rootNode = nodeAssetToNode.GetNode(nodeAsset);
    }

    #region 饿了吃饭
    public bool IsHungry()
    {
        return energy <= minEnergy;
    }

    public void AddEnergy(float value)
    {
        ChangeEnergy(value);
        Debug.LogError("Eat");
    }

    private void ChangeEnergy(float value)
    {
        energy += value;
    }

    public bool IsFull()
    {
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
