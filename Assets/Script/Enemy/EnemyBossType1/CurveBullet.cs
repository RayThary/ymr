using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveBullet : MonoBehaviour
{
    private Player player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(GameTags.GetGameTag(Tags.Player)))
        {
            player.Hit(null, 1);
        }   
    }
    void Start()
    {
        player = GameManager.instance.GetPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
