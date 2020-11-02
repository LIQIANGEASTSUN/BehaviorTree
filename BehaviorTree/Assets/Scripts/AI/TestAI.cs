using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAI : MonoBehaviour
{
    private List<BaseSprite> _spriteList = new List<BaseSprite>();
    private SpriteBTUpdateManager _spriteBTUpdateManager;

    // Start is called before the first frame update
    void Start()
    {
        _spriteBTUpdateManager = new SpriteBTUpdateManager();

        ConfigLoad.loadEndCallBack = ConfigLoadEnd;

        CheckNpc();
    }

    private void ConfigLoadEnd()
    {
        BaseSprite sprite = new BaseSprite();
        sprite.Init(Vector3.zero);
        _spriteList.Add(sprite);

        for (int i = 0; i < _spriteList.Count; ++i)
        {
            _spriteBTUpdateManager.AddSprite(sprite);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _spriteList.Count; ++i)
        {
            _spriteList[i].Update();
        }

        _spriteBTUpdateManager.Update();

        BulletManager.GetInstance().Update();
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 50), "CreateNpc"))
        {
            CheckNpc();
        }
    }

    private void CheckNpc()
    {
        GameObject npcGo = GameObject.Find("Npc");
        if (npcGo)
        {
            return;
        }

        GameObject npc = Resources.Load<GameObject>("Npc");
        npcGo = GameObject.Instantiate<GameObject>(npc);
        npcGo.name = "Npc";

        Vector3 randomPos = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
        npcGo.transform.position = new Vector3(-10, 0, 2) + randomPos;
    }

}
