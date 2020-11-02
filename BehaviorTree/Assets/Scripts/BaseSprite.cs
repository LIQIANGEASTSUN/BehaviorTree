using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseSprite : IBTNeedUpdate
{
    private int _spriteId;
    private BTConcrete _bt;
    public GameObject SpriteGameObject;

    public float _energy;
    public float _energyFull = 100;
    public Npc _enemyNpc = null;

    private string _btConfigFileName = "Player";

    public BTBase BTBase
    {
        get { return _bt; }
    }

    public void Init(Vector3 position)
    {
        SpriteGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        SpriteGameObject.name = "BaseSprite";
        SpriteGameObject.transform.localScale = Vector3.one;
        SpriteGameObject.transform.position = position;

        _bt = new BTConcrete(_btConfigFileName);
        _bt.SetOwner(this);

        _bt.UpdateParameter(BTConstant.IsSurvial, true);

        BTBase.UpdateParameter(BTConstant.Energy, Energy());
    }

    public void Update()
    {

    }

    public int SpriteID
    {
        get { return _spriteId; }
    }

    public bool CanRunningBT()
    {
        return true;
    }

    #region Energy
    public float Energy()
    {
        return _energy;
    }

    public bool ReplenishEnergy(float vlaue)
    {
        _energy += vlaue;
        _energy = Mathf.Min(_energy, _energyFull);
        return _energy >= _energyFull;
    }

    public void SubEnergy(float value)
    {
        _energy -= value;
        BTBase.UpdateParameter(BTConstant.Energy, Energy());
    }

    #endregion

    #region Enemy
    public Npc Enemy
    {
        get { return _enemyNpc; }
        set { _enemyNpc = value; }
    }

    private IMove _moveFollowEnemy;
    private IMove MoveEnemy()
    {
        if (null == _moveFollowEnemy)
        {
            _moveFollowEnemy = new SpriteFollowEnemy(this);
        }
        return _moveFollowEnemy;
    }

    #endregion

    public Vector3 Position
    {
        get
        {
            return SpriteGameObject.transform.position;
        }
        set
        {
            SpriteGameObject.transform.position = value;
        }
    }

    public IMove GetIMove(TargetTypeEnum targetType)
    {
        IMove iMove = null;
        if (targetType == TargetTypeEnum.ENEMY)
        {
            iMove = MoveEnemy();
        }
        else if (targetType == TargetTypeEnum.ENERY_SUPPLY)
        {
            iMove = EnergyStation.GetInstance();
        }
        else if (targetType == TargetTypeEnum.PATROL)
        {
        }

        return iMove;
    }

}


public class SpriteFollowEnemy : IMove
{
    private BaseSprite _sprite;
    public SpriteFollowEnemy(BaseSprite sprite)
    {
        _sprite = sprite;
    }

    public void Move(ref float speed, ref Vector3 position, ref float distance)
    {
        speed = 3;
        position = Vector3.zero;
        if (null != _sprite.Enemy)
        {
            position = _sprite.Enemy.Position();
        }
        distance = 2f;
    }
}