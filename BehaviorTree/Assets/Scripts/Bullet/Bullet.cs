﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletData
{
    public Vector3 startPos;
    public Transform target;
    public float speed;
    public float damage;
}

public class Bullet
{
    private BulletData _bulletData;
    private GameObject _go;
    private bool _isValid = true;
    
    public Bullet(BulletData data)
    {
        _bulletData = data;

        _go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _go.transform.localScale = Vector3.one * 0.3f;
        _go.transform.position = _bulletData.startPos;
        _go.name = "Bullet";
    }

    public void Update()
    {
        Vector3 forward = (_bulletData.target.position - _go.transform.position).normalized;
        _go.transform.position += forward * Time.deltaTime;
        if (Vector3.Distance(_go.transform.position, _bulletData.target.position) <= 0.2f)
        {
            _isValid = false;
            Destroy();
        }
    }

    public void Destroy()
    {
        GameObject.Destroy(_go);
    }

    public bool IsValid()
    {
        return _isValid;
    }

}
