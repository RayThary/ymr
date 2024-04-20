using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : AttackType
{
    Transform _weapon;
    float compareAngle = 30;
    float _range  = 2;
    // 1 << 8 == Unit , 1 << 9 == Bullet
    int layer = LayerMask.GetMask("Enemy");

    public Swing(NewLauncher newLauncher, Player player, Transform weapon, PoolingManager.ePoolingObject ePoolingObject, float rate, float timer) : base (newLauncher, player, rate, timer)
    {
        _weapon = weapon;
        _poolingObject = ePoolingObject;
    }

    public override bool Fire()
    {
        ComponentTrack track = CreateBullet();
        track.transform.position = _weapon.transform.position;
        track.transform.eulerAngles = _weapon.transform.eulerAngles;

        _player.componentController.CallAddComponent(track);

        track.Fire(_player, _timer, compareAngle * 2, _range);
        return true;
    }

    public override void LeftUp() { }

    private ComponentTrack CreateBullet()
    {
        return PoolingManager.Instance.CreateObject(_poolingObject, null).GetComponent<ComponentTrack>();
    }
}

public class ComponentCut : IComponentObject
{
    public void Enter(Collider other)
    {
        if(1 << other.gameObject.layer == LayerMask.GetMask("Bullet"))
        {
            PoolingManager.Instance.RemovePoolingObject(other.gameObject);
        }
    }

    public void Fire(ComponentObject componentObject)
    {

    }

    public void Update()
    {

    }
}

////왜 이건 9로 나온는거지 1 << 9 가 아니라
//if (1 << colliders[i].gameObject.layer == LayerMask.GetMask("Bullet"))
//{
//    //총알을 벤 경우
//    Debug.Log("총알을 벰");

//    //bulletCutting?.Invoke();

//    PoolingManager.Instance.RemovePoolingObject(colliders[i].gameObject);
//}