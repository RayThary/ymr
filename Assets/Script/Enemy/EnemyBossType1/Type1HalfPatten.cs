using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Type1HalfPatten : MonoBehaviour
{
    private List<Vector3> startTrsList = new List<Vector3>();

    private List<Vector3> midTrsList = new List<Vector3>();
    private Vector3 targetTrs;//도착지점

    [SerializeField] private float speed;
     private float value = 0;
     private List<GameObject> bulletObj = new List<GameObject>();
    private bool initPatten = false;
    private bool pattenStart = false;

    private Vector3 bezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        Vector3 P1 = Vector3.Lerp(p0, p1, t);
        Vector3 P2 = Vector3.Lerp(p1, p2, t);

        return Vector3.Lerp(P1, P2, t);
    }

    void Start()
    {
        startTrsList.Clear();
        startTrsList.Add(Vector3.zero);
        startTrsList.Add(new Vector3(0, 0, 29));//첫좌표 위로
        startTrsList.Add(new Vector3(29, 0, 29));//첫좌표 대각선
        startTrsList.Add(new Vector3(29, 0, 0));//첫좌표 오른쪽

        targetTrs = GameManager.instance.GetPlayerTransform.position;

    }

    void Update()
    {
        setPatten();
        bulletMove();
    }

    private void setPatten()
    {
        if (initPatten == true)
        {
            
            for (int i = 0; i < startTrsList.Count; i++)
            {
                bulletObj.Add(PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.CurveBullet, GameManager.instance.GetEnemyAttackObjectPatten));
                bulletObj[i].transform.position = startTrsList[i];
                bulletObj[i].GetComponentInChildren<TrailRenderer>().Clear();
            }
            setPostion();
            initPatten = false;
            pattenStart = true;
        }
    }

    private void bulletMove()
    {
        if (pattenStart == false)
        {
            return;
        }

        for (int i = 0; i < startTrsList.Count; i++)
        {
            bulletObj[i].transform.position = bezier(startTrsList[i], midTrsList[i], targetTrs, value);
        }
        value += Time.deltaTime * speed * 0.1f;
        if (value >= 1)
        {
            for(int i =0;i<startTrsList.Count;i++)
            {
                PoolingManager.Instance.RemovePoolingObject(bulletObj[i]);
            }
            pattenStart = false;
        }
    }

    private void setPostion()
    {
        for (int i = 0; i < startTrsList.Count; i++)
        {
            midTrsList.Add(Vector3.Lerp(startTrsList[i], targetTrs, 0.5f));
            float x = 0;
            float z = 0;
            if (i == 0)
            {
                x = Random.Range(startTrsList[i].x, targetTrs.x);
                z = Random.Range(targetTrs.z, 30);
            }
            else if (i == 1)
            {
                x = Random.Range(targetTrs.x, 30);
                z = Random.Range(targetTrs.z, startTrsList[i].z);
            }
            else if (i == 2)
            {
                x = Random.Range(targetTrs.x, 30);
                z = Random.Range(targetTrs.z, 0);
            }
            else if (i == 3)
            {
                x = Random.Range(0, targetTrs.x);
                z = Random.Range(startTrsList[i].z, targetTrs.z);
            }
            midTrsList[i] = new Vector3(x, 0, z);
        }
    }
    public void SetHalfPatten()
    {
        

        bulletObj.Clear();
        initPatten = true;
    }
}
