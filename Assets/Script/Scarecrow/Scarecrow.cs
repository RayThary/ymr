using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarecrow : Unit
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Hit(Unit unit, float figure)
    {
        base.Hit(unit, figure);
        if(stat.HP <= 0)
        {
            stat.RecoveryHP(stat.MAXHP, null);
        }
        spriteRenderer.transform.localScale = new Vector3(stat.HP, 1, 1);
    }
}
