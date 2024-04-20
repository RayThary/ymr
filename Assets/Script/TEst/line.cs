using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class line : MonoBehaviour
{
    public LineRenderer m_line;
    public float speed;

    public Transform m_trs1;
    public Transform m_trs2;


    void Start()
    {
        //m_trs1 = transform.GetChild(0);
        //m_trs2 = transform.GetChild(1);

        m_line= GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
        m_line.SetPosition(0, m_trs1.position);
        m_line.SetPosition(1, m_trs2.position);
    }

    private void pMove()
    {
        
        m_trs1.position += transform.forward * Time.deltaTime * speed;
        m_trs1.Rotate(new Vector3(0, 1, 0) * 100 * Time.deltaTime);
        speed += Time.deltaTime;

        m_trs2.position += transform.forward * Time.deltaTime * speed;
        m_trs2.Rotate(new Vector3(0, 1, 0) * 100 * Time.deltaTime);
        speed += Time.deltaTime;

    }
}
