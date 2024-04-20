using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Launcher
{
    public Transform objectParent;
    //�� �߻�븦 ������ �ִ� ����
    protected Unit parent;
    //�߻��
    public Transform launcher;
    //�Ѿ��� ������ ��ġ
    public Transform muzzle;
    //�Ѿ��� ������ ����
    public float angle;
    //�߻�� ���� ����
    protected float mistake = 0;
    public float Mistake { get => mistake; set => mistake = value; }
    //���� �߻���� �ɸ��� �ð�
    protected float firerate;
    public float FireRate { get { return firerate; } set { firerate = value; } }
    //�߻� Ÿ�̸�
    protected Coroutine firerateCoroutine;
    //���� �߻��� ������ ȣ���ϴ� �ݹ��Լ� (�׳� �Ѿ��� �߻��ϴ� �׼���)
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
    /// ���ؾ� �ϴ� ������ ����󿡼� �ָ��Ҷ�
    /// </summary>
    /// <param name="screen"></param>
    public virtual void Direction_Calculation_Screen(Vector3 screen)
    {
        //������ ��ġ�� ��ũ����ġ�� ��ȯ�ؼ� ������ ���
        //raycast�� �浹�� ���� �ʴ� ��쿡 ������ ������ �ʱ⿡
        //ScreenToWorld�� ���̸� ��������ϴ� ������ �ֱ⿡ ���̰� �ٸ� �� ����
        angle = AngleCalculator.ScreenAngleCalculate(screen, Camera.main.WorldToScreenPoint(launcher.position));

        //localEulerAnlge�� ���ٰ� ������ �߻� ���⸶�� ������ ������ �ٸ�
        //local��ǥ�� �ƴ� world��ǥ�� �����
        launcher.eulerAngles = new Vector3(90, angle, 0);
    }

    /// <summary>
    /// ���ؾ� �ϴ� ������ ����󿡼� �ָ����� ������
    /// </summary>
    /// <param name="screen"></param>
    public virtual void Direction_Calculation(Vector3 position)
    {
        //������ ��ġ�� ��ũ����ġ�� ��ȯ�ؼ� ������ ���
        //raycast�� �浹�� ���� �ʴ� ��쿡 ������ ������ �ʱ⿡
        //ScreenToWorld�� ���̸� ��������ϴ� ������ �ֱ⿡ ���̰� �ٸ� �� ����
        angle = AngleCalculator.WorldAngleCalculate(position, launcher.position);

        //localEulerAnlge�� ���ٰ� ������ �߻� ���⸶�� ������ ������ �ٸ�
        //local��ǥ�� �ƴ� world��ǥ�� �����
        launcher.eulerAngles = new Vector3(90, angle, 0);
    }

    //�÷��̾ Ŭ���Ҷ����� ȣ���� �Լ�
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
