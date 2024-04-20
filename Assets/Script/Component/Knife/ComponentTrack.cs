using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentTrack : ComponentObject
{
    float _compareAngle;
    float _range = 2;
    List<Unit> units = new ();
    List<GameObject> bullets = new();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    Unit unit;
    GameObject bullet;
    // Update is called once per frame
    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _range);
        for (int i = 0; i < colliders.Length; i++)
        {
            Vector3 target = (colliders[i].transform.position - transform.position).normalized;
            float angle = Mathf.Acos(Vector3.Dot(transform.forward, target)) * Mathf.Rad2Deg;
            if (angle < _compareAngle && angle > -_compareAngle)
            {
                unit = colliders[i].GetComponent<Unit>();
                if (1 << colliders[i].gameObject.layer == LayerMask.GetMask("Bullet"))
                    bullet = colliders[i].gameObject;
                else
                    bullet = null;

                if (unit != null && !units.Contains(unit))
                {
                    units.Add(unit);
                    unit.Hit(_player, _player.STAT.AD);
                    _player.componentController.CallAttack(unit);
                    for(int j = 0; j < components.Count; j++)
                    {
                        components[j].Enter(colliders[i]);
                    }
                }
                if(bullet != null && !bullets.Contains(bullet))
                {
                    bullets.Add(bullet);
                    for (int j = 0; j < components.Count; j++)
                    {
                        components[j].Enter(colliders[i]);
                    }
                }
            }
        }

        _time -= Time.deltaTime;
        if (_time < 0)
        {
            PoolingManager.Instance.RemovePoolingObject(gameObject);
        }
    }

    public void Fire(Player player, float timer, float angle, float range)
    {
        _player = player;
        _time = timer;
        _range = range;
        _compareAngle = angle;
        for (int i = 0; i < components.Count; i++)
        {
            components[i].Fire(this);
        }
    }
}
