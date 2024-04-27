using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentBullet : ComponentObject
{
    private float _speed;

    void Update()
    {
        for (int i = 0; i < components.Count; i++)
        {
            components[i].Update();
        }

        transform.position +=  _speed * Time.deltaTime * transform.forward;
        
        _time -= Time.deltaTime;
        if(_time < 0)
        {
            PoolingManager.Instance.RemovePoolingObject(gameObject);
        }
    }

    public void Fire(Player player, float speed, float timer)
    {
        _player = player;
        _speed = speed;
        _time = timer;
        for (int i = 0; i < components.Count; i++)
        {
            components[i].Fire(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < components.Count; i++)
        {
            components[i].Enter(other);
        }

        Unit unit = other.GetComponent<Unit>();
        if (other.GetComponent<Player>() == _player)
            return;

        bool destroy = true;

        if(unit != null)
        {
            unit.Hit(_player, _player.STAT.AD);
            _player.componentController.CallAttack(unit);
            if (unit.STAT.HP <= 0)
            {
                _player.componentController.CallKill(unit);
            }
        }
        for (int i = 0; i < components.Count; i++)
        {
            if (!components[i].Destroy(other))
            {
                destroy = false;
            }
        }

        if (destroy)
        {
            if (1 << other.gameObject.layer ==  LayerMask.GetMask("Enemy") || 1 << other.gameObject.layer == LayerMask.GetMask("Wall"))
                PoolingManager.Instance.RemovePoolingObject(gameObject);
        }
    }
}
