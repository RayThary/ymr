using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float speed;

    public float x;
    Vector3 point;//¿øÁ¡

    void Start()
    {
        point = transform.position;

    }
    public float rotationSpeed = 100f;

    void Update()
    {

        //transform.position += transform.forward * Time.deltaTime * speed * 0.5f;
        //transform.Rotate(new Vector3(0, 1, 0) * rotationSpeed * Time.deltaTime);
        //speed += Time.deltaTime * x;
        //if (speed < 8)
        //{
        //}
        
        Vector3 axis = Vector3.up;
        transform.RotateAround(point, axis,rotationSpeed * Time.deltaTime);

        if (Vector3.Distance(point, transform.position) < speed)
        { 
            transform.position -= transform.forward * Time.deltaTime * speed;
        }
    }

    

}
