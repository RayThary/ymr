using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombing : MonoBehaviour
{
    private Player player;

    private bool oneCheck = false;
    private float timer = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player.Hit(null, 1);
        }
    }
    void Start()
    {
        player = GameManager.instance.GetPlayer;
    }

    void Update()
    {
        if (oneCheck)
        {
            timer += Time.deltaTime;
            if (timer >= 0.7f)
            {
                oneCheck = false;
                timer = 0;
                PoolingManager.Instance.RemovePoolingObject(gameObject);
            }
        }
    }

    public void SetSpawnCheck()
    {
        oneCheck = true;
    }
    
}
