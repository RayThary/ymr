using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Launcher
{
    public Transform objectParent;
    //이 발사대를 가지고 있는 유닛
    protected Unit parent;
    //발사대
    public Transform launcher;
    //총알이 나가는 위치
    public Transform muzzle;
    //총알이 나가는 각도
    public float angle;
    //발사시 오차 범위
    protected float mistake = 0;
    public float Mistake { get => mistake; set => mistake = value; }
    //다음 발사까지 걸리는 시간
    protected float firerate;
    public float FireRate { get { return firerate; } set { firerate = value; } }
    //발사 타이머
    protected Coroutine firerateCoroutine;
    //총을 발사할 때마다 호출하는 콜백함수 (그냥 총알을 발사하는 액션임)
    protected Action fireCallback = null;
    protected PoolingManager.ePoolingObject bulletObject;
    public PoolingManager.ePoolingObject BulletPool { get { return bulletObject; } set { bulletObject = value; } }

    public Launcher(Unit unit, Transform launcher, Transform muzzle, float mistake, float firerate, Transform objectParent)
    {
        parent = unit;
        this.launcher = launcher;
        this.muzzle = muzzle;
        this.mistake = mistake;
        this.firerate = firerate;
        this.objectParent = objectParent;
        bulletObject = PoolingManager.ePoolingObject.PlayerBullet;
        FireCallbackAdd(BulletControl);
    }

    /// <summary>
    /// 구해야 하는 각도가 월드상에서 애매할때
    /// </summary>
    /// <param name="screen"></param>
    public virtual void Direction_Calculation_Screen(Vector3 screen)
    {
        //무기의 위치를 스크린위치로 변환해서 각도를 계산
        //raycast는 충돌을 하지 않는 경우에 각도를 구하지 않기에
        //ScreenToWorld는 길이를 정해줘야하는 문제가 있기에 높이가 다를 수 있음
        angle = AngleCalculator.ScreenAngleCalculate(screen, Camera.main.WorldToScreenPoint(launcher.position));

        //localEulerAnlge을 쓰다가 문제가 발생 무기마다 각도의 기준이 다름
        //local좌표가 아닌 world좌표를 써야함
        launcher.eulerAngles = new Vector3(90, angle, 0);
    }

    /// <summary>
    /// 구해야 하는 각도가 월드상에서 애매하지 않을때
    /// </summary>
    /// <param name="screen"></param>
    public virtual void Direction_Calculation(Vector3 position)
    {
        //무기의 위치를 스크린위치로 변환해서 각도를 계산
        //raycast는 충돌을 하지 않는 경우에 각도를 구하지 않기에
        //ScreenToWorld는 길이를 정해줘야하는 문제가 있기에 높이가 다를 수 있음
        angle = AngleCalculator.WorldAngleCalculate(position, launcher.position);

        //localEulerAnlge을 쓰다가 문제가 발생 무기마다 각도의 기준이 다름
        //local좌표가 아닌 world좌표를 써야함
        launcher.eulerAngles = new Vector3(90, angle, 0);
    }

    //플레이어가 클릭할때마다 호출할 함수
    public void Fire()
    {
        FireCallback();
    }

    public virtual void BulletControl()
    {
        Bullet b = GetBullet();
        b.unit = parent;
        b.transform.position = new Vector3(muzzle.position.x, 0.1f, muzzle.position.z) ;
        b.transform.eulerAngles = new Vector3(0, angle + UnityEngine.Random.Range(- mistake, mistake), 0);
        b.Straight();
    }

    public bool CanShot()
    {
        if (firerateCoroutine == null)
            return true;
        return false;
    }

    public void FireCallback()
    {
        if (CanShot())
        {
            fireCallback?.Invoke();
            if(firerate != 0)
                firerateCoroutine = parent.StartCoroutine(FirerateTimerC());
        }
    }

    private IEnumerator FirerateTimerC()
    {
        yield return new WaitForSeconds(firerate);
        firerateCoroutine = null;
    }

    public virtual Bullet GetBullet()
    {
        //Bullet bullet = PoolingManager.Instance.CreateObject(bulletObject, objectParent).transform.GetComponent<Bullet>();
        return PoolingManager.Instance.CreateObject(bulletObject, objectParent).transform.GetComponent<Bullet>();
    }

    public void FireCallbackAdd(Action action)
    {
        if (fireCallback == null)
        {
            fireCallback = action;
            return;
        }

        if (!fireCallback.GetInvocationList().Contains(action))
            fireCallback += action;
    }

    public void FireCallbackRemove(Action action)
    {
        if (fireCallback == null)
        {
            return;
        }

        if (fireCallback.GetInvocationList().Contains(action))
            fireCallback -= action;
    }

   
}
