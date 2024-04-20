using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Unit unit;
    public float damage;
    public float timer;
    public float speed; 

    protected Coroutine straight = null;
    protected Coroutine timerCoroutine = null;


    public virtual void Straight()
    {
        if(straight != null)
        {
            StopCoroutine(straight);
        }
        straight = StartCoroutine(StraightC());
        TimerStart();
    }

    public void TimerStart()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        timerCoroutine = StartCoroutine(OffTimer(timer));
    }

    protected IEnumerator StraightC()
    {
        while(true)
        {
            transform.Translate(speed *  Time.deltaTime * transform.forward, Space.World);

            yield return null;
        }
    }

    protected IEnumerator OffTimer(float timer)
    {
        yield return new WaitForSeconds(timer);
        timerCoroutine = null;
        if(straight != null)
        {
            StopCoroutine(straight);
        }
        gameObject.SetActive(false);
    }

    public virtual void Judgment(Collider other)
    {
        if (other.GetComponent<Unit>() != null && unit != null)
        {
            other.GetComponent<Unit>().Hit(unit, damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (1 << other.gameObject.layer == LayerMask.GetMask("Wall") || 1 << other.gameObject.layer == LayerMask.GetMask("Player"))
        {
            if (straight != null)
            {
                StopCoroutine(straight);
            }

            Judgment(other);

            PoolingManager.Instance.RemovePoolingObject(gameObject);
        }
    }
}
