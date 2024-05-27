using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    private Unit _unit;
    private float _damage;
    private float _radius;
    [SerializeField]
    private Explosion _explosion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Spawn(Unit unit, float damage, float radius)
    {
        _unit = unit;
        _damage = damage;
        _radius = radius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (1 << other.gameObject.layer == LayerMask.GetMask("Enemy"))
        {
            if (other.GetComponent<Unit>() != null)
            {
                Explosion explosion = Instantiate(_explosion, transform.position, Quaternion.identity);
                explosion.Operation(_radius, _damage, _unit);
                gameObject.SetActive(false);
            }
        }
    }
}
