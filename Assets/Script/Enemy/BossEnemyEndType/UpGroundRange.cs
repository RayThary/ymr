using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpGroundRange : MonoBehaviour
{
    //
    public bool test = false;
    [SerializeField] private float MaxRange = 5;
    private Vector3 originPos;
    private bool objSpawnCheck = false;

    private bool firstSpawnCheck = true;
    
    private List<GameObject> spawnList = new List<GameObject>();

    private float objTimer = 0;
    private float timer = 0;//오브젝트 소환용타이머
    void Start()
    {



    }

    void Update()
    {
        uPGroundCheck();
    }

    private void uPGroundCheck()
    {

        if (test)
        {
            if (firstSpawnCheck)
            {
                objSpawnCheck = true;

                //이부분은 리무브할때 다시 트루줘야함
                firstSpawnCheck = false;
            }

            if (objSpawnCheck == false)
            {
                timer += Time.deltaTime;
            }

            if (timer >= 2)
            {
                objSpawnCheck = true;
            }

        }

        if (objSpawnCheck)
        {
            GameObject Objpos;

            originPos = transform.position;

            float rangeX = Random.Range((MaxRange / 2) * -1, (MaxRange / 2));
            float rangeZ = Random.Range((MaxRange / 2) * -1, (MaxRange / 2));
            Vector3 randomPos = new Vector3(rangeX, 0, rangeZ);

            originPos += randomPos;
            Objpos = PoolingManager.Instance.CreateObject("UpGroundObj", transform);
            Objpos.transform.position = originPos;

            timer = 0;
            objSpawnCheck = false;
        }

        objTimer += Time.deltaTime;
        if (objTimer >= 6)
        {
            objTimer = 0;
            test = false;
        }
    }
}
