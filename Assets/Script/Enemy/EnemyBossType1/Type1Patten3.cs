using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Type1Patten3 : MonoBehaviour
{
    [SerializeField]private bool shotStart = false;

    private List<GameObject>blueBullet = new List<GameObject>();
    void Start()
    {

    }

    void Update()
    {
        
        dealyShot();
    }

    private void dealyShot()
    {
        if (shotStart)
        {
            StartCoroutine(s());
            shotStart = false;
        }
    }

    IEnumerator s()
    {
        for(int i = 0; i < 5; i++)
        {
            StartCoroutine(shot());
            yield return new WaitForSeconds(0.8f);
        }
    }

    IEnumerator shot()
    {
        blueBullet.Clear();
        int angle = 0;
        for (int i = 0; i < 4; i++)
        {
            blueBullet.Add(PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.BlueBullet, GameManager.instance.GetEnemyAttackObjectPatten));
            blueBullet[i].transform.position = transform.position;
            blueBullet[i].transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
            angle += 90;
        }

        yield return new WaitForSeconds(0.3f);
        angle = 45;
        for (int j = 4; j < 8; j++)
        {
            blueBullet.Add(PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.BlueBullet, GameManager.instance.GetEnemyAttackObjectPatten));
            blueBullet[j].transform.position = transform.position;
            blueBullet[j].transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
            angle += 90;
        }
        
    }

    public void SetShotStart(bool _value)
    {
        shotStart = _value;

    }

}
