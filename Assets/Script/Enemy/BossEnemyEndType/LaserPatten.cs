using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class LaserPatten : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 1;
    private LineRenderer lineRen;
    private Transform parentTrs;

    private BoxCollider box;

    private Transform playerTrs;
    private Vector3 targetVec;

    private Player player;
    private bool firstCheck = false;

    public bool test = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall")) 
        {
            firstCheck = false;
            transform.position = parentTrs.position;
            PoolingManager.Instance.RemovePoolingObject(parentTrs.gameObject);
        }
    }
    void Start()
    {
        lineRen = GetComponentInParent<LineRenderer>();
        parentTrs = lineRen.GetComponent<Transform>();

        box = GetComponent<BoxCollider>();

        playerTrs = GameManager.instance.GetPlayerTransform;
        player = GameManager.instance.GetPlayer;

        Vector3 spawnPos = parentTrs.position;
        spawnPos.y = 0f;
        lineRen.SetPosition(0, spawnPos);
    }


    // Update is called once per frame
    void Update()
    {
        tartgetMove();
        laserMove();
        hitCheck();
        
    }

    private void tartgetMove()
    {
        if (firstCheck == false)
        {
            parentTrs.position = transform.position;
            lineRen.SetPosition(0, parentTrs.position);
            targetVec = playerTrs.transform.position - transform.position;
            targetVec.y = 0f;
            firstCheck = true;
        }
        transform.position += targetVec.normalized * Time.deltaTime * moveSpeed;
        
    }
    private void laserMove()
    {
        lineRen.SetPosition(1, transform.position);
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(lineRen.GetPosition(0), lineRen.GetPosition(1), Color.red);
    }

    private void hitCheck()
    {
        Vector3 startVec = lineRen.GetPosition(0);
        Vector3 endVec = lineRen.GetPosition(1);
        startVec.y = 0;
        endVec.y = 0;

        if (Physics.Linecast(startVec, endVec, LayerMask.GetMask("Player")))
        {
            player.Hit(null, 1);
        }
    }
}
