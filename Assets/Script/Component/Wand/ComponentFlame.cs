using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentFlame : ComponentObject
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < components.Count; i++)
        {
            components[i].Update();
        }
        _time -= Time.deltaTime;
        if (_time < 0)
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Destroy(null);
            }
            PoolingManager.Instance.RemovePoolingObject(gameObject);
        }
    }

    public void Fire(Player player, float timer)
    {
        _player = player;
        _time = timer;
        for (int i = 0; i < components.Count; i++)
        {
            components[i].Fire(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Unit unit = other.GetComponent<Unit>();
        if (unit != null)
        {
            unit.Hit(_player, _player.STAT.AD);
            _player.componentController.CallAttack(unit);
        }
        for (int i = 0; i < components.Count; i++)
        {
            components[i].Enter(other);
        }
    }
}
