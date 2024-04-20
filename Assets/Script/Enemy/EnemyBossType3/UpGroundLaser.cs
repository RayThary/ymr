using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpGroundLaser : MonoBehaviour
{
    [SerializeField] private float laserSpeed = 1;

    private bool playerTrsCheck = false;

    private LineRenderer line;

    private Transform p1;
    private Transform playerTrs;

    //¾È¾¸

    void Start()
    {
        line = GetComponent<LineRenderer>();
        playerTrs = GameManager.instance.GetPlayerTransform;
    }

    void Update()
    {
        p1Move();
    }

    private void p1Move()
    {
        if (playerTrsCheck == false)
        {
            p1.transform.rotation = Quaternion.LookRotation(playerTrs.position - transform.position);
            playerTrsCheck = true;
        }

        p1.transform.position += transform.forward * Time.deltaTime * laserSpeed;
    }

    

}
