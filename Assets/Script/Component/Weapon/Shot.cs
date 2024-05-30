using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : AttackType
{
    readonly Transform _muzzle;
    readonly float _angle;

    public Shot(NewLauncher newLauncher, Player player, Transform muzzle, PoolingManager.ePoolingObject ePoolingObject, float angle, float rate, float timer) : base(newLauncher, player, rate, timer)
    {
        _player = player;
        _muzzle = muzzle;
        _poolingObject = ePoolingObject;
        _angle = angle;
    }

    public override bool Fire()
    {
        ComponentBullet bullet = CreateBullet();
        bullet.transform.position = _muzzle.position;
        bullet.transform.eulerAngles = _muzzle.eulerAngles + new Vector3(0, _angle, 0);

        _player.componentController.CallAddComponent(bullet);

        bullet.Fire(_player, _speed, _timer);
        SoundManager.instance.SFXCreate(SoundManager.Clips.Arrow, 1, 0.2f);
        return true;
    }

    public override void LeftUp() { }

    private ComponentBullet CreateBullet()
    {
        return PoolingManager.Instance.CreateObject(_poolingObject, GameManager.instance.GetPlayerBulletParent).GetComponent<ComponentBullet>();
    }
}
