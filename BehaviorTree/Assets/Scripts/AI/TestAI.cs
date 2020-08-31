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
    }
}
