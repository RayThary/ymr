using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : AttackType
{
    float _maxChargingTimer = 2;
    float _minChargingTimer = 0.5f;
    float _chargingTimer;
    Transform _muzzle;
    ParticleSystem _particle;

    public FireBall(NewLauncher newLauncher, Player player, Transform muzzle, float rate, float timer) : base(newLauncher, player, rate, timer)
    {
        _muzzle = muzzle;
    }

    public override bool Fire()
    {
        ComponentBullet bullet = CreateBullet();
        bullet.transform.position = _muzzle.position;
        bullet.transform.eulerAngles = _muzzle.eulerAngles;

        _player.componentController.CallAddComponent(bullet);

        bullet.Fire(_player, _speed, _timer);

        RemoveParticelSystem();

        _chargingTimer = 0;
        _player.componentController.CallFire();
        return true;
    }

    public override void LeftDown()
    {
        _chargingTimer += Time.deltaTime;
        CreateParticleSystem();
            
        if (_chargingTimer >= _maxChargingTimer)
        {
            Fire();
        }
    }

    public override void LeftUp()
    {
        if(_chargingTimer >= _minChargingTimer)
        {
            Fire();
        }
        _chargingTimer = 0;
        RemoveParticelSystem();
    }

    private void CreateParticleSystem()
    {
        if (_particle == null)
        {
            _particle = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.FireballParticle, _player.transform).GetComponent<ParticleSystem>();
            _particle.transform.localPosition = Vector3.zero;
            _particle.transform.localScale = new Vector3(1, 1, 1);
            _particle.Play();
        }
    }

    private void RemoveParticelSystem()
    {
        if (_particle != null)
        {
            _particle.Stop();
            PoolingManager.Instance.RemovePoolingObject(_particle.gameObject);
            _particle = null;
        }
    }

    private ComponentBullet CreateBullet()
    {
        return PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.FireBall, null).GetComponent<ComponentBullet>();
    }
}
