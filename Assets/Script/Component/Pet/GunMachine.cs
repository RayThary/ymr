using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMachine : Pet
{
    private void Start()
    {

    }

    public override void Action()
    {
        if(_timer > _cycle)
        {
            if (_target == null)
                return;
            CreateBullet(out ComponentBullet componentBullet);
            if (componentBullet != null)
            {
                componentBullet.transform.position = transform.position;
                componentBullet.transform.eulerAngles = new Vector3 (0, AngleCalculator.ScreenAngleCalculate(Camera.main.WorldToScreenPoint(_target.transform.position), Camera.main.WorldToScreenPoint(transform.position)), 0);
                _master.componentController.CallAddComponent(componentBullet);
                componentBullet.Fire(_master, 3, 10);
            }

            _timer = 0;
        }
    }

    public void CreateBullet(out ComponentBullet componentBullet)
    {
        componentBullet = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.ComponuntBulle, GameManager.instance.GetPlayerBulletParent).GetComponent<ComponentBullet>();
    }
}
