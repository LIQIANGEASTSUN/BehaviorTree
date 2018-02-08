using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class Student : MonoBehaviour
{

    private NodeSelect rootNode = new NodeSelect(); // 根节点

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

        {
            time += Time.deltaTime;
            if (time > intervalTime)
            {
                time = 0;
                index++;
                string path = string.Format("D:\\Gif\\Screenshot{0}.png", index);
                Application.CaptureScreenshot(path, 0);
            }
        }
    }

    /// <summary>
    /// 初始化添加行为树节点
    /// </summary>
    private void Init()
    {
        // 顺序节点 1
        NodeSequence nodeSequence_1 = new NodeSequence();
        // 顺序节点 1 添加到根节点
        rootNode.AddNode(nodeSequence_1);

        #region 是否饿了
        // 条件节点 1.1
        NodeConditionHungry nodeConditionHungry_1_1 = new NodeConditionHungry();
        nodeConditionHungry_1_1.SetStudent(this);
        // 条件节点 1.1 添加到 顺序节点1
        nodeSequence_1.AddNode(nodeConditionHungry_1_1);
        #endregion

        #region 选择节点 1.2
        {
            // 选择节点1.2
            NodeSelect nodeSelect_1_2 = new NodeSelect();
            nodeSequence_1.AddNode(nodeSelect_1_2);

            // 节点 1.2.1 是否有饭
            NodeConditionHasFood nodeConditionHasFood_1_2_1 = new NodeConditionHasFood();
            nodeConditionHasFood_1_2_1.SetStudent(this);
            nodeSelect_1_2.AddNode(nodeConditionHasFood_1_2_1);

            // 顺序节点 1.2.2
            NodeSequence nodeSequence_1_2_2 = new NodeSequence();
            nodeSelect_1_2.AddNode(nodeSequence_1_2_2);

            // 行为节点 1.2.2.1 走到厨房
            NodeActionMove nodeActionMove_1_2_2_1 = new NodeActionMove();
            nodeActionMove_1_2_2_1.SetStudent(this);
            nodeActionMove_1_2_2_1.SetTarget("Cooking");
            nodeActionMove_1_2_2_1.SetTr(transform);
            nodeSequence_1_2_2.AddNode(nodeActionMove_1_2_2_1);

            // 行为节点 1.2.2.2 做饭
            NodeActionCooking nodeActionCooking_1_2_2_2 = new NodeActionCooking();
            nodeActionCooking_1_2_2_2.SetStudent(this);
            nodeSequence_1_2_2.AddNode(nodeActionCooking_1_2_2_2);
        }
        #endregion

        #region 走到餐桌
        {
            // 行为节点 1.3 走到餐桌
            NodeActionMove nodeActionMove_1_3 = new NodeActionMove();
            nodeActionMove_1_3.SetStudent(this);
            nodeActionMove_1_3.SetTarget("Food");
            nodeActionMove_1_3.SetTr(transform);
            nodeSequence_1.AddNode(nodeActionMove_1_3);
        }
        #endregion

        #region 吃饭
        {
            // 行为节点 1.4 吃饭
            NodeActionEat nodeActionEat_1_4 = new NodeActionEat();
            nodeActionEat_1_4.SetStudent(this);
            nodeSequence_1.AddNode(nodeActionEat_1_4);
        }
        #endregion
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

//public class Student : MonoBehaviour {

//    private NodeRoot rootNode = null; // 根节点

//    private float energy = 0;      // 能量值
//    private float minEnergy = 50;  // 能量下限：当能量 <= 能量下限      感到饥饿
//    private float maxEnergy = 100; // 能量上限：当能量 >= 能量上线时    表示吃饱了

//    private float food = 0;        // 食物量：
//    private float minFood = 0;     // 食物下限：当食物量 <= 食物下限时  没有食物了
//    private float maxFood = 100;   // 食物上限：当食物量 >= 食物上限时  表示饭做好了

//    // Use this for initialization
//    void Start()
//    {
//        Init();
//    }

//    public int index = 0;
//    private float time;

//    public static float intervalTime = 0.3f;
//    // Update is called once per frame
//    void Update()
//    {
//        ChangeEnergy(-0.8f); // 每帧执行消耗能量

//        if (rootNode != null)
//        {
//            rootNode.Execute();  // 每帧调用行为树根节点，行为树入口
//        }
//    }

//    /// <summary>
//    /// 初始化添加行为树节点
//    /// </summary>
//    private void Init()
//    {
//        NodeAsset nodeAsset = Resources.Load<NodeAsset>("NodeAsset/Demo");
//        if (nodeAsset == null)
//        {
//            Debug.LogError("NodeAsset is null");
//            return;
//        }

//        NodeAssetToNode nodeAssetToNode = new NodeAssetToNode();
//        rootNode = nodeAssetToNode.GetNode(nodeAsset);
//    }

//    #region 饿了吃饭
//    public bool IsHungry()
//    {
//        return energy <= minEnergy;
//    }

//    public void AddEnergy(float value)
//    {
//        ChangeEnergy(value);
//        Debug.LogError("Eat");
//    }

//    private void ChangeEnergy(float value)
//    {
//        energy += value;
//    }

//    public bool IsFull()
//    {
//        return energy >= maxEnergy;
//    }
//    #endregion

//    #region 食物
//    public bool HasFood()
//    {
//        return food > minFood;
//    }

//    public bool FoodEnough()
//    {
//        return food >= maxFood;
//    }

//    public void Cooking(float value)
//    {
//        ChangeFood(value);
//        Debug.LogError("Cooking");
//    }

//    public void ChangeFood(float value)
//    {
//        food += value;
//    }
//    #endregion
//}
