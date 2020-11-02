using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    private float _hp = 20;
    private Vector3 _position = new Vector3(-10, 0, 2);
    private Vector3 _movePos = Vector3.zero;
    private float _speed = 1;

    private float _interval = 3;
    private float _lastTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckMovePos();

        Vector3 forward = (_movePos - transform.position).normalized;
        transform.position += forward * _speed * Time.deltaTime;
    }

    private void CheckMovePos()
    {
        if (Time.realtimeSinceStartup - _lastTime <= _interval)
        {
            return;
        }
        _lastTime = Time.realtimeSinceStartup;

        _movePos = _position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
    }

    public Vector3 Position()
    {
        return transform.position;
    }

    public void BeDamage(float damage)
    {
        _hp -= damage;
        if (_hp <= 0)
        {
            GameObject.Destroy(gameObject);
        }

        Debug.Log("BeDamage:" + damage);
    }
}
