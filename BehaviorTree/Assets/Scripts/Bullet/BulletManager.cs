using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager
{
    public static BulletManager Instance;

    private List<Bullet> _bulletList = new List<Bullet>();

    private static object obj = new object();
    public static BulletManager GetInstance()
    {
        lock(obj)
        {
            if (null == Instance)
            {
                Instance = new BulletManager();
            }
        }

        return Instance;
    }

    private BulletManager()
    {

    }

    public void AddBullet(BulletData data)
    {
        Bullet bullet = new Bullet(data);
        _bulletList.Add(bullet);
    }

    public void Update()
    {
        for (int i = _bulletList.Count - 1; i >= 0; --i)
        {
            Bullet bullet = _bulletList[i];
            bullet.Update();
            if (!bullet.IsValid())
            {
                _bulletList.RemoveAt(i);
            }
        }

    }


}
