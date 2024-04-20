using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackType : IAttackType
{
    protected PoolingManager.ePoolingObject _poolingObject;
    protected NewLauncher _launcher;
    protected float _rate;
    public float Rate { get => Mathf.Max(0.1f, _rate); set { _rate += value; } }
    protected float _timer;
    public float Timer { get { return Mathf.Max(1f, _rate); } set { _timer = value; } }
    protected float _speed = 2;
    public float Speed { get { return _speed; } set { _speed = value; } }
    protected bool shot = true;
    protected Player _player;

    public AttackType(NewLauncher newLauncher , Player player, float rate, float timer)
    {
        _launcher = newLauncher;
        _player = player;
        _rate = rate;
        _timer = timer;
    }

    public virtual void LeftDown()
    {
        if(shot)
        {
            if(Fire())
            {
                shot = false;
                _player.StartCoroutine(FireRate());
                _player.componentController.CallFire();
            }
        }
    }
    public abstract void LeftUp();
    public abstract bool Fire();

    protected IEnumerator FireRate()
    {
        yield return new WaitForSeconds(Rate);
        shot = true;
    }
}
