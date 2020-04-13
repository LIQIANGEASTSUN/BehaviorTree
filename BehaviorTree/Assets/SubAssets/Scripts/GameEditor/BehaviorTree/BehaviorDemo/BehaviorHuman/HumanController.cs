using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour
{
    public static HumanController Instance = null;
    private Human _human;
    // 厨房
    private Transform kitchen;
    // 餐桌
    private Transform diningTable;
    // TV
    private Transform TV;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        GameObject target = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        target.name = "Human";
        target.transform.position = Vector3.zero;

        kitchen = transform.Find("Kitchen");
        diningTable = transform.Find("DiningTable");
        TV = transform.Find("TV");

        _human = new Human(target.transform, kitchen.gameObject, diningTable.gameObject, TV.gameObject);
        TextAsset textAsset = Resources.Load<TextAsset>("Data/Human");
        _human.SetData(textAsset.text);
    }

    // Update is called once per frame
    void Update()
    {
        _human.Update();
    }

    public Human Human
    {
        get { return _human; }
    }


}
