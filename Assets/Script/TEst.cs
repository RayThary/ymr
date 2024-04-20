using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TEst : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("1");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("2");
    }

    void Start()
    {
       
    }

    void Update()
    {
        if (Input.anyKeyDown == true)
        {
            SceneManager.LoadScene(1);
        }
    }

}
