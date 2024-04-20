using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButterflyPatten : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1;
    //터지는시간
    [SerializeField] private float bombTime = 5;
    private float timer = 0;
    [SerializeField]private bool bombCheck = false;

    private float bombtimer = 0;

    private Transform parentTrs;
    private DangerZone dangerzone;
    private Transform target;
    private SpriteRenderer spr;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            bombCheck = true;
            timer = 0;
            StartCoroutine(crateButterfly());
            dangerzone.enabled = false;
            spr.enabled = false;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            bombCheck = true;
            timer = 0;
            StartCoroutine(crateButterfly());
            dangerzone.enabled = false;
            spr.enabled = false;
        }
    }

    void Start()
    {
        parentTrs = transform.parent.GetComponent<Transform>();
        target = GameManager.instance.GetPlayerTransform;
        spr = GetComponent<SpriteRenderer>();
        dangerzone = GetComponent<DangerZone>();
        dangerzone.enabled = false;
    }

    void Update()
    {
        butterflyMove();
        butterflyBomb();
    }

    private void butterflyMove()
    {
        if (bombCheck == false)
        {
            timer += Time.deltaTime;
            //이동
            Vector3 moveTarget = target.position - transform.position;
            parentTrs.position += moveTarget.normalized * Time.deltaTime * moveSpeed;

            #region
            // 각도
            //Vector3 targetPos = target.position;
            //Vector3 fromPos = transform.position;
            //targetPos.y = 0f;
            //fromPos.y = 0f;
            //Vector3 targetVec = fromPos - targetPos;


            //float fixedAngle = (fromPos - targetPos).x < 0f == false ? -90f : 90;
            //float targetAgleY = Quaternion.FromToRotation(targetVec, Vector3.up).eulerAngles.y + fixedAngle;

            //parentTrs.rotation = Quaternion.Euler(new Vector3(0, targetAgleY, 0));
            #endregion

            parentTrs.LookAt(target);
            if (timer >= bombTime)
            {
                dangerzone.enabled = true;
            }
            
        }

    }


    private void butterflyBomb()
    {
        if (dangerzone.enabled == true)
        {
            bombtimer += Time.deltaTime;
            if (bombtimer >= 1f)
            {
                bombCheck = true;
                timer = 0;
                StartCoroutine(crateButterfly());
                spr.enabled = false;
                dangerzone.enabled = false;
            }
        }
    }

    IEnumerator crateButterfly()
    {
        
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.1f);
            Vector3 nowPos = transform.position;

            GameObject butterfly;
            float rangeX = Random.Range(-0.5f, 0.5f);
            float rangeZ = Random.Range(-0.5f, 0.5f);
            Vector3 randomPos = new Vector3(rangeX, 0, rangeZ);
            
            nowPos += randomPos;
            butterfly = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.RedButterflyBomb, GameManager.instance.GetenemyObjectBox);
            butterfly.transform.position = nowPos;

        }
        bombCheck = false;
        PoolingManager.Instance.RemovePoolingObject(transform.parent.gameObject);
        spr.enabled = true;
        
    }

}
