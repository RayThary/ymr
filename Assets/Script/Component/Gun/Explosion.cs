using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public void Operation(float radius, float damage, Unit unit)
    {
        transform.localScale = new Vector3(radius, 0, radius);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, LayerMask.GetMask("Enemy"));
        Unit enemy;
        for(int i = 0; i < colliders.Length; i++)
        {
            enemy = colliders[i].GetComponent<Unit>();
            if (enemy != null && enemy != GameManager.instance.GetPlayer)
            {
                enemy.Hit(unit, damage);
            }
        }
        Destroy(gameObject, 1.5f);
    }
}
