using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossType2Patten2 : MonoBehaviour
{
    [SerializeField] private float speed=120;

    void Update()
    {
        transform.Rotate(new Vector3(0,1,0) * speed * Time.deltaTime);
    }
}
