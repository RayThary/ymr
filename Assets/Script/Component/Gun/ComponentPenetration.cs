using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentPenetration : IComponentObject
{
    public void Update()
    {

    }

    public void Enter(Collider other)
    {

    }

    public bool Destroy(Collider other)
    {
        if (1 << other.gameObject.layer == LayerMask.GetMask("Wall"))
        {
            return true;
        }
        return false;
    }

    public void Fire(ComponentObject bullet)
    {

    }
}
