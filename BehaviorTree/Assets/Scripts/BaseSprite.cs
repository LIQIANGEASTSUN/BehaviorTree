using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTConstant
{
    /// <summary>
    /// 活着
    /// </summary>
    public const string IsSurvial = "IsSurvial";

    /// <summary>
    /// 有敌人
    /// </summary>
    public const string HasEneny = "HasEneny";

    /// <summary>
    /// 能量值
    /// </summary>
    public const string Energy = "Energy";

    /// <summary>
    /// 能量最小值
    /// </summary>
    public const string EnergyMin = "EnergyMin";

    /// <summary>
    /// 目标类型：敌人、能量补给站、溜达目的自
    /// </summary>
    public const string TargetType = "TargetType";

}

public enum TargetTypeEnum
{
    /// <summary>
    /// 敌人
    /// </summary>
    ENEMY = 0,

    /// <summary>
    /// 能量补给站
    /// </summary>
    ENERY_SUPPLY = 1,

    /// <summary>
    /// 巡逻地
    /// </summary>
    PATROL = 2,
}

public class BaseSprite : IBTNeedUpdate
{
    private BTConcrete _bt;
    public GameObject SpriteGameObject;

    public float _energy;
    public const float _energyMin = 10;

    public Npc _enemyNpc = null;

    private string _btConfigFileName = string.Empty;

    public BTBase BTBase
    {
        get { return _bt; }
    }

    public void Init(Vector3 position)
    {
        _bt = new BTConcrete(_btConfigFileName);
        _bt.SetOwner(this);

        _bt.UpdateParameter(BTConstant.IsSurvial, true);

        SpriteGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        SpriteGameObject.transform.localScale = Vector3.one;
        SpriteGameObject.transform.position = position;

        _energy = 100;
    }

    public bool CanRunningBT()
    {
        return true;
    }

    public float Energy()
    {
        return _energy;
    }

    public Npc Enemy
    {
        get { return _enemyNpc; }
        set { _enemyNpc = value; }
    }

    public IMove GetIMove(TargetTypeEnum targetType)
    {
        return null;
    }

}
