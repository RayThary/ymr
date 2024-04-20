using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBullet : Bullet
{

    private void Reflex()
    {
        Vector3 dir = transform.forward;
        Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, float.MaxValue);
        Vector2 vector = Vector2.Reflect(new Vector2(dir.x, dir.z), new Vector2(hit.normal.x, hit.normal.z));
        transform.eulerAngles = new Vector3(0, AngleCalculator.DirAngle(vector), 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        Unit unit = other.GetComponent<Unit>();
        if(unit != null)
        {
            if (straight != null)
            {
                StopCoroutine(straight);
            }

            Judgment(other);

            PoolingManager.Instance.RemovePoolingObject(gameObject);
        }
        else if (1 << other.gameObject.layer == LayerMask.GetMask("Wall"))
        {
            Reflex();
        }
    }
}
