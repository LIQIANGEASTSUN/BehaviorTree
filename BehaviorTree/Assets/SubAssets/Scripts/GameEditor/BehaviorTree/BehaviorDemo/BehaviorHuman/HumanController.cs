﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour
{
    public static HumanController Instance = null;
    private Human _human;
    // 厨房
    public GameObject kitchen;
    // 餐桌
    public GameObject diningTable;
    // TV
    public GameObject TV;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        GameObject target = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        target.name = "Human";
        target.transform.position = Vector3.zero;
        _human = new Human(target.transform, kitchen, diningTable, TV);
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
