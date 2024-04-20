using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBulletPatten : MonoBehaviour
{
    /// <summary> 큰총알을발사하는패턴 돌면서 4방향으로 총알을발사하는패턴
    ///
    /// </summary>


    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;

    private int bulletCount = 0;
    private Launcher[] launcher;
    private Coroutine operationCoroutine = null;

    private bool spawnCheck = false;


    [SerializeField] private Transform[] muzzles;
    private Transform sprTrs;
    [SerializeField] private Unit boss;
    public Unit Boss { set => boss = value; }

    public float rate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            PoolingManager.Instance.RemovePoolingObject(gameObject);
        }
    }

    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        bigBulletMove();
    }
    private void bigBulletMove()
    {
        transform.position += transform.forward * Time.deltaTime * moveSpeed;
        sprTrs.transform.Rotate(new Vector3(0, 0, -1) * rotateSpeed * Time.deltaTime);
    }

    public void Operation()
    {
        SpriteRenderer spr = GetComponentInChildren<SpriteRenderer>();
        sprTrs = spr.GetComponent<Transform>();

        launcher = new Launcher[muzzles.Length];
        for (int i = 0; i < muzzles.Length; i++)
        {
            launcher[i] = new Launcher(boss, sprTrs, muzzles[i], 0, rate, GameManager.instance.GetEnemyAttackObjectPatten);
            launcher[i].BulletPool = PoolingManager.ePoolingObject.Type2RedBullet;

        }
        operationCoroutine = StartCoroutine(OperationCoroutine());
    }

    //작동 중지
    public void OperationStop()
    {
        if (operationCoroutine != null)
            StopCoroutine(operationCoroutine);
    }

    //자동 발사
    private IEnumerator OperationCoroutine()
    {
        while (true)
        {
            for (int i = 0; i < launcher.Length; i++)
            {
                launcher[i].angle = muzzles[i].eulerAngles.y;
                launcher[i].Fire();
                
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
