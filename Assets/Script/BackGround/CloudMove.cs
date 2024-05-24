using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMove : MonoBehaviour
{
    [SerializeField] private float m_CloudSpeed;

    private CloudSpawn cloudspawn;
    private Transform m_trsEnd;
    private Rigidbody2D m_rigd2d;

    void Start()
    {
        m_rigd2d = GetComponent<Rigidbody2D>();
        cloudspawn = GetComponentInParent<CloudSpawn>();
        m_trsEnd = cloudspawn.GetEndTrs();
    }

    
    void Update()
    {
        cloudMove();
    }

    private void cloudMove()
    {
        m_rigd2d.velocity = new Vector2(m_CloudSpeed, m_rigd2d.velocity.y);
        if (transform.position.x > m_trsEnd.position.x)
        {
            Destroy(gameObject);
        }

    }
}
