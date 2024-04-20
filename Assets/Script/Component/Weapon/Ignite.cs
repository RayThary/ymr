using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ignite : AttackType
{
    public Ignite(NewLauncher newLauncher, Player player,PoolingManager.ePoolingObject ePoolingObject, float rate, float timer) : base (newLauncher, player, rate, timer)
    {
        _poolingObject = ePoolingObject;
    }

    public override bool Fire()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            ComponentFlame flame = CreateBullet();
            Vector3 hitPos = hit.point;
            hitPos.y = 0;
            flame.transform.position = hitPos;

            _player.componentController.CallAddComponent(flame);

            flame.Fire(_player, _timer);
            return true;
        }
        return false;
    }

    public override void LeftUp() { }

    private ComponentFlame CreateBullet()
    {
        return PoolingManager.Instance.CreateObject(_poolingObject, null).GetComponent<ComponentFlame>();
    }
}
