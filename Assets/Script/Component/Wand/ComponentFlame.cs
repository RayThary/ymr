using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentFlame : ComponentObject
{
    [SerializeField]
    private Sprite[] ani;
    [SerializeField]
    private float aniTimer = 1;
    private SpriteRenderer spriteRenderer;

    private Unit _enemy;
    private float _radius = 2f;
    private float _speed = 2;

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
            PoolingManager.Instance.RemovePoolingObject(gameObject);
        }

        //적을 따라가기
        if (_enemy != null)
        {
            transform.LookAt(_enemy.transform);
            Vector3 dir = (_enemy.transform.position - transform.position).normalized;
            transform.position += new Vector3(dir.x, 0, dir.z) * Time.deltaTime * _speed;
        }
    }

    public void Find()
    {
        //주위에서 가장 가까운 적 발견
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius, LayerMask.GetMask("Enemy"));
        if (colliders.Length == 0)
            return;

        float MinDistance = Vector3.Distance(transform.position, colliders[0].transform.position);
        int index = 0;
        for (int i = 1; i < colliders.Length; i++)
        {
            float Distance = Vector3.Distance(transform.position, colliders[i].transform.position);
            if (Distance < MinDistance)
            {
                index = i;
                MinDistance = Distance;
            }
        }
        _enemy = colliders[index].GetComponent<Unit>();
    }

    public void Fire(Player player, float timer)
    {
        if(spriteRenderer == null)
            spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _player = player;
        _time = timer;
        Find();
        for (int i = 0; i < components.Count; i++)
        {
            components[i].Fire(this);
        }
        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        int index = 0;
        while(gameObject.activeSelf)
        {
            spriteRenderer.sprite = ani[index];
            yield return new WaitForSeconds(aniTimer);

            index++;
            if (index >= ani.Length)
                index = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < components.Count; i++)
        {
            components[i].Enter(other);
        }
        Unit unit = other.GetComponent<Unit>();
        if (unit != null && unit != _player)
        {
            unit.Hit(_player, _player.STAT.AD);
            _player.componentController.CallAttack(unit); 
            PoolingManager.Instance.RemovePoolingObject(gameObject);
        }
        
    }
}

/*
 private ComponentObject _componentFlame;
    private Unit _enemy;
    private float _radius = 1.5f;
    private float _speed = 2;


    public void Enter(Collider other)
    {

    }

    public void Find(ComponentObject flame)
    {
        //주위에서 가장 가까운 적 발견
        Collider[] colliders = Physics.OverlapSphere(flame.transform.position, _radius, LayerMask.GetMask("Enemy"));
        if(colliders.Length == 0 )
            return;

        float MinDistance = Vector3.Distance(flame.transform.position, colliders[0].transform.position);
        int index = 0;
        for(int i = 1; i < colliders.Length; i++)
        {
            float Distance = Vector3.Distance(flame.transform.position, colliders[i].transform.position);
            if (Distance < MinDistance)
            {
                index = i;
                MinDistance = Distance;
            }
        }
        _enemy = colliders[index].GetComponent<Unit>();
        _componentFlame = flame;
    }

    public void Update()
    {
        //적을 따라가기
        if(_enemy != null )
        {
            Vector3 dir = (_enemy.transform.position - _componentFlame.transform.position).normalized;
            _componentFlame.transform.position += new Vector3(dir.x, 0, dir.z) * Time.deltaTime * _speed;
        }
    }

    public void Destroy()
    {

    }
 */