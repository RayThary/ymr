using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentDrainlife : IComponentObject
{
    private ComponentObject _bullet;
    private int count = 0;
    private float _drain = 1;

    public void Fire(ComponentObject bullet)
    {
        _bullet = bullet;
    }

    public void Update()
    {

    }

    public void Enter(Collider other)
    {
        Unit unit = other.GetComponent<Unit>();
        if (unit != null && 1 << other.gameObject.layer == LayerMask.GetMask("Enemy"))
        {
            count++;
            if(count >= 3)
            {
                _bullet.Player.STAT.RecoveryHP(_drain, null);
                count = 0;
            }
        }
    }
}
