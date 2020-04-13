using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class HumanController : MonoBehaviour
{
    private Human _human;
    // 厨房
    private Transform kitchen;
    // 餐桌
    private Transform diningTable;
    // TV
    private Transform TV;

    BehaviorData behaviorData = new BehaviorData();

    // Start is called before the first frame update
    void Start()
    {
        //Singleton<ConfigLoad>.Instance.Load(null);

        StartCoroutine(behaviorData.LoadConfig());

        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(2);

        GameObject target = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        target.name = "Human";
        target.transform.position = Vector3.zero;

        kitchen = Create("厨房", new Vector3(0, 0, 0), new Vector3(90, 0, 0));
        diningTable = Create("餐桌", new Vector3(5, 0, 0), new Vector3(90, 0, 0));
        TV = Create("电视", new Vector3(0, 0, 5), new Vector3(90, 0, 0));

        _human = new Human(target.transform, kitchen.gameObject, diningTable.gameObject, TV.gameObject);
        BehaviorTreeData behaviorTreeData = behaviorData.GetBehaviorInfo("Human");
        _human.SetData(behaviorTreeData);
    }

    // Update is called once per frame
    void Update()
    {
        if (null != Human)
        {
            Human.Update();
        }
    }

    public Human Human
    {
        get { return _human; }
    }

    private Transform Create(string name, Vector3 pos, Vector3 rot)
    {
        GameObject go = new GameObject(name);
        go.transform.position = pos;
        go.transform.rotation = Quaternion.Euler(rot);
        go.transform.localScale = Vector3.one * 0.1f;

        TextMesh textMesh = go.AddComponent<TextMesh>();
        if (null != textMesh)
        {
            textMesh.text = name;
            textMesh.fontSize = 100;
        }

        return go.transform;
    }

}
