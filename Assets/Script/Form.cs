using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossType
{

}

public enum player
{

}

public enum enemyAttackObject
{
    Meteor,
    MeteorObj,
    RotatingSphere,
    BigBullet,
    WindMillPatten,
    HaxagonLaser,
    UpGroundObj,
    CheckedLaserPatten,
    LaserPatten,
    RedButterfly,
    RedButterflyBomb,
}

public enum enemyAttackEffect 
{ 
    BoosEndAttackRange,

}


public enum playerAttackObject
{

}
public enum bullet 
{

}

public enum Tags
{
    Player,
}

public static class GameTags
{
    public static string GetGameTag(Tags _value)
    {
        return _value.ToString();
    }
}
