using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBulletPatten : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;

    private SpriteRenderer spr;
    private Transform sprTrs;

    [SerializeField] private float launchDelay = 1;
    private float launchtimer = 0.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            PoolingManager.Instance.RemovePoolingObject(gameObject);
        }
    }

    void Start()
    {
        spr = GetComponentInChildren<SpriteRenderer>();
        sprTrs = spr.GetComponent<Transform>();


    }

    // Update is called once per frame
    void Update()
    {
        bigBulletMove();
        bigBulletlaunch();
    }
    private void bigBulletMove()
    {
        transform.position += transform.forward * Time.deltaTime * moveSpeed;
        sprTrs.transform.Rotate(new Vector3(0, 0, -1) * rotateSpeed * Time.deltaTime);
    }

    private void bigBulletlaunch()
    {
        launchtimer += Time.deltaTime;
        if(launchtimer > launchDelay)
        {
            float z = sprTrs.eulerAngles.y;
            
            for (int i = 0; i < 4; i++)
            {
                GameObject obj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.Type2RedBullet, GameManager.instance.GetEnemyAttackObjectPatten);
                obj.transform.rotation = Quaternion.Euler(0, z, 0);
                obj.transform.position = transform.position;
                z += 90;
            }
            launchtimer = 0;
        } 
    }

    #region
    //public void Operation()
    //{
    //    SpriteRenderer spr = GetComponentInChildren<SpriteRenderer>();
    //    sprTrs = spr.GetComponent<Transform>();

    //    launcher = new Launcher[muzzles.Length];
    //    for (int i = 0; i < muzzles.Length; i++)
    //    {
    //        launcher[i] = new Launcher(boss, sprTrs, muzzles[i], 0, rate, GameManager.instance.GetEnemyAttackObjectPatten);
    //        launcher[i].BulletPool = PoolingManager.ePoolingObject.Type2RedBullet;

    //    }
    //    operationCoroutine = StartCoroutine(OperationCoroutine());
    //}

    ////작동 중지
    //public void OperationStop()
    //{
    //    if (operationCoroutine != null)
    //        StopCoroutine(operationCoroutine);
    //}

    ////자동 발사
    //private IEnumerator OperationCoroutine()
    //{
    //    while (true)
    //    {
    //        for (int i = 0; i < launcher.Length; i++)
    //        {
    //            launcher[i].angle = muzzles[i].eulerAngles.y;
    //            launcher[i].Fire();

    //        }
    //        yield return new WaitForSeconds(0.5f);
    //    }
    //}
    #endregion
}
