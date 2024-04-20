using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tower : Unit
{
    public Player player;
    public Launcher[] launcher;
    public Transform objectParent;

    private Coroutine operationCoroutine = null;
    //��ü
    public Transform body;
    public Transform[] muzzles;
    public bool bodyRocation;
    private float timer;
    private Coroutine bodyCoroutine = null;
    //���� ���ݱ��� �ɸ��� �ð�
    public float rate = 0.3f;
    //��ǥ ���� (�� �� ������ ���߰ڴ�)
    public float desired;
    //ȸ�� �ӵ�
    public float rotationSpeed;
    //���� �� ���� (�� �̰����� �����̴�)
    private float rotation;


    protected new void Start()
    {
        base.Start();
        launcher = new Launcher[muzzles.Length];
        for (int i = 0; i < muzzles.Length; i++)
        {
            launcher[i] = new Launcher(this, body, muzzles[i], 0, rate, objectParent);
        }

        if (player == null)
        {
            //�÷��̾� ã��
        }
        else
        {
            //����
            Operation();
        }
    }

    private void Update()
    {
        if (player == null)
        {
            OperationStop();
        }
        else
            �÷��̾��ã��();
    }

    //player�� �ִٸ� ������ ã�°���
    public void �÷��̾��ã��()
    {
        if (player != null)
        {
            float angle = AngleCalculator.WorldAngleCalculate(transform.position, player.transform.position);
            desired = angle;
        }
    }

    //�۵�
    public void Operation()
    {
        operationCoroutine = StartCoroutine(OperationCoroutine());
    }
    //�۵� ����
    public void OperationStop()
    {
        if (operationCoroutine != null)
            StopCoroutine(operationCoroutine);
    }
    //�ڵ� �߻�
    private IEnumerator OperationCoroutine()
    {
        while (true)
        {
            for (int i = 0; i < launcher.Length; i++)
            {
                launcher[i].angle = muzzles[i].eulerAngles.y;
                launcher[i].Fire();
            }
            yield return null;
        }
    }

    //��ü ȸ��
    private void BodyRotation()
    {
        body.transform.localEulerAngles = new Vector3(0, rotation, 0);
    }

    /// <summary>
    /// ��ü ȸ��
    /// ���ϴ� ���� (desired)�� timer���� �̵��ϱ�
    /// </summary>
    /// <param name="timer"></param>
    /// <returns></returns>
    private IEnumerator BodyRotationCoroutine(float timer)
    {
        float elapsedTime = 0f;  // ��� �ð�
        float startAngle = rotation;  // ���� ����
        float endAngle = desired;   // ��ǥ ����
        float moveDuration = timer;  // �̵��� �ɸ��� �ð� (��)

        while (true)
        {
            elapsedTime += Time.deltaTime;

            // �ð��� moveDuration�� �ʰ��ϸ�, �� �̻� ������Ʈ���� ����
            if (elapsedTime >= moveDuration)
            {
                break;
            }

            // Lerp�� ����Ͽ� ���� ���� ���
            float t = elapsedTime / moveDuration;
            float currentAngle = Mathf.LerpAngle(startAngle, endAngle, t);

            // ������Ʈ�� ȸ����ŵ�ϴ�.
            rotation = currentAngle;

            if (bodyRocation)
                BodyRotation();

            yield return null;
        }
    }

    ////���ٷ� �߻�
    //private void LauncherLine()
    //{
    //    Bullet b = GetBullet();
    //    b.gameObject.SetActive(true);
    //    b.transform.position = muzzle.position;
    //    b.transform.eulerAngles = new Vector3(0, launcher.eulerAngles.y + UnityEngine.Random.Range(-mistake, mistake), 0);
    //    b.Straight();

    //    FireCallback();
    //}

    //ȸ���ϸ鼭 �߻�
    private void LauncherRotation()
    {
        for (int i = 0; i < muzzles.Length; i++)
        {
            launcher[i].angle = muzzles[i].eulerAngles.y;
            launcher[i].Fire();
        }

        //�� ȸ���ڵ�
        if (bodyCoroutine != null)
        {
            StopCoroutine(bodyCoroutine);
        }
        //������ ȸ���� ����� �ض�
        desired = rotation + (rotationSpeed * timer);
        bodyCoroutine = StartCoroutine(BodyRotationCoroutine(timer));
    }

    ////��������� �߻�
    //private void LauncherCircle() 
    //{
    //    for(int i = 0; i < 36; i ++)
    //    {
    //        Bullet b = GetBullet();
    //        b.gameObject.SetActive(true);
    //        b.transform.position = body.position;
    //        rotation = i * 10;
    //        b.transform.localEulerAngles = new Vector3(0, rotation, 0);
    //        b.Straight();
    //    }
    //}
}
