using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSprite : IBTNeedUpdate
{
    public GameObject SpriteGameObject;

    private string _btConfigFileName = string.Empty;
    private BTConcrete _bt;
    public BTBase BTBase
    {
        get { return _bt; }
    }

    public void Init(Vector3 position)
    {
        _bt = new BTConcrete(_btConfigFileName);
        _bt.SetOwner(this);

        SpriteGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        SpriteGameObject.transform.localScale = Vector3.one;
        SpriteGameObject.transform.position = position;
    }

    public bool CanRunningBT()
    {
        return true;
    }

}
