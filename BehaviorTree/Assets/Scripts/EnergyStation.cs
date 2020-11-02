using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyStation : IMove
{
    private static EnergyStation Instance;
    private GameObject _EnergyStation;
    private static float _replenishSpeed = 0.1f;

    private static object obj = new object();
    public static EnergyStation GetInstance()
    {
        lock(obj)
        {
            if (null == Instance)
            {
                Instance = new EnergyStation();
            }
        }

        return Instance;
    }

    private EnergyStation()  { }

    public void Move(ref float speed, ref Vector3 position, ref float distance)
    {
        speed = 2;
        position = Vector3.zero;
        if (!_EnergyStation)
        {
            _EnergyStation = GameObject.Find("EnergyStation");
        }

        if (_EnergyStation)
        {
            position = _EnergyStation.transform.position;
        }

        distance = 1f;
    }

    public float Execute()
    {
        return _replenishSpeed;
    }


}
