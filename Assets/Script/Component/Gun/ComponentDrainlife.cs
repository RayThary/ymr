using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentDrainlife : IComponentObject
{
    private ComponentObject _bullet;
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
        if (other.GetComponent<Unit>() != null)
        {
            _bullet.Player.STAT.RecoveryHP(_drain, null);
            Debug.Log("component로 인한 회복");
        }
    }
}
