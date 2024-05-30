using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineMachine : Pet
{
    void Start()
    {
        _find = false;
    }

    public override void Action()
    {
        if (_timer >= _cycle)
        {
            CreateMine(out Mine mine);
            mine.transform.position = transform.position;
            mine.Spawn(_master, _value, 1.25f);
            _timer = 0;
        }
    }

    public void CreateMine(out Mine mine)
    {
        mine = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.Mine, GameManager.instance.GetPlayerBulletParent).GetComponent<Mine>();
    }
}
