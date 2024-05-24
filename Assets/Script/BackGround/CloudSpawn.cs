using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawn : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_listCloud;
    [SerializeField] private Transform m_trsSpawn;
    [SerializeField] private Transform m_trsRemove;
    [SerializeField]private float m_spawnTime;
    private float m_Timer = 500;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(new Vector2(m_trsSpawn.position.x, -2), new Vector2(m_trsSpawn.position.x, 6));
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        m_Timer += Time.deltaTime;
        if (m_Timer >= m_spawnTime)
        {
            spawnCloud();
        }
    }

    private void spawnCloud()
    {
        int iRand = Random.Range(0, m_listCloud.Count);
        GameObject objCloud = m_listCloud[iRand];

        float yPos = Random.Range(-2, 6);
        Vector3 cloudPos = m_trsSpawn.position;
        cloudPos.y = yPos;

        GameObject obj = Instantiate(objCloud, cloudPos, Quaternion.identity, transform);
        m_Timer = 0;
    }

    public Transform GetEndTrs()
    {
        return m_trsRemove;
    }
}
