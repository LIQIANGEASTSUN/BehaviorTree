using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataCenter 
{
    public static BehaviorData behaviorData;

    public static void Init()
    {
        behaviorData = new BehaviorData();
    }
}
