using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum _DOT
{
    POISON,
    BURN,
    SHOCK,
    BLEEDING
}

public abstract class Unit : MonoBehaviour
{
    protected Stat stat;
    public Stat STAT {  get { return stat; } }
    protected bool god = false;
    [SerializeField]
    protected float godTimer = 0;

    protected void Start()
    {
        stat = GetComponent<Stat>(); 
        stat.Init();
    }


    public virtual void Hit(Unit unit, float figure)
    {
        if(god)
        {
            return;
        }
        stat.Be_Attacked_TRUE(figure, unit);
        if(unit != null)
            unit.stat.AttackInvocation(this, figure);
    }
    public void HitDot(_DOT dot, int duration, float figure, Unit perpetrator)
    {
        switch(dot)
        {
            case _DOT.POISON:
                stat.Be_Attacked_Poison(duration, figure, perpetrator);
                break;
            case _DOT.BURN:
                stat.Be_Attacked_Burn(duration, figure, perpetrator);   
                break;
            case _DOT.SHOCK:
                stat.Be_Attacked_Shock(duration, figure, 0, 0, perpetrator);  
                break;
            case _DOT.BLEEDING:
                stat.Be_Attacked_Bleeding(duration, figure, 0, 0, perpetrator);   
                break;
        }
    }

    private Coroutine slow = null;
    public void Slow(float timer, float power)
    {
        if (slow != null)
            StopCoroutine(slow);
        power = Mathf.Clamp(power, 0, 1);
        StartCoroutine(SlowCoroutine(timer));
        stat.SPEED = stat.SPEED * power;
    }
    
    private IEnumerator SlowCoroutine(float timer)
    {
        yield return new WaitForSeconds(timer);
        stat.SPEED = stat.originalSpeed;
    }
}

//내가 누군가를 맞춰서 대미지를 넣고싶다면 상대 Unit.Hit(자신, 대미지)를 호출해야함