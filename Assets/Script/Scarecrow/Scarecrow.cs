using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarecrow : Unit
{

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
    }
}
