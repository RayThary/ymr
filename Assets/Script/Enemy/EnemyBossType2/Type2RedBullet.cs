using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Type2RedBullet : MonoBehaviour
{
    [SerializeField] private float speed;
  
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }
}
