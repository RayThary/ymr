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
    //몸체
    public Transform body;
    public Transform[] muzzles;
    public bool bodyRocation;
    private float timer;
    private Coroutine bodyCoroutine = null;
    //다음 공격까지 걸리는 시간
    public float rate = 0.3f;
    //목표 각도 (난 이 각도로 가야겠다)
    public float desired;
    //회전 속도
    public float rotationSpeed;
    //현재 내 각도 (난 이각도인 상태이다)
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
            //플레이어 찾기
        }
        else
        {
            //공격
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
            플레이어각도찾기();
    }

    //player가 있다면 각도를 찾는거임
    public void 플레이어각도찾기()
    {
        if (player != null)
        {
            float angle = AngleCalculator.WorldAngleCalculate(transform.position, player.transform.position);
            desired = angle;
        }
    }

    //작동
    public void Operation()
    {
        operationCoroutine = StartCoroutine(OperationCoroutine());
    }
    //작동 중지
    public void OperationStop()
    {
        if (operationCoroutine != null)
            StopCoroutine(operationCoroutine);
    }
    //자동 발사
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

    //몸체 회전
    private void BodyRotation()
    {
        body.transform.localEulerAngles = new Vector3(0, rotation, 0);
    }

    /// <summary>
    /// 몸체 회전
    /// 원하는 각도 (desired)로 timer동안 이동하기
    /// </summary>
    /// <param name="timer"></param>
    /// <returns></returns>
    private IEnumerator BodyRotationCoroutine(float timer)
    {
        float elapsedTime = 0f;  // 경과 시간
        float startAngle = rotation;  // 시작 각도
        float endAngle = desired;   // 목표 각도
        float moveDuration = timer;  // 이동에 걸리는 시간 (초)

        while (true)
        {
            elapsedTime += Time.deltaTime;

            // 시간이 moveDuration을 초과하면, 더 이상 업데이트하지 않음
            if (elapsedTime >= moveDuration)
            {
                break;
            }

            // Lerp를 사용하여 현재 각도 계산
            float t = elapsedTime / moveDuration;
            float currentAngle = Mathf.LerpAngle(startAngle, endAngle, t);

            // 오브젝트를 회전시킵니다.
            rotation = currentAngle;

            if (bodyRocation)
                BodyRotation();

            yield return null;
        }
    }

    ////한줄로 발사
    //private void LauncherLine()
    //{
    //    Bullet b = GetBullet();
    //    b.gameObject.SetActive(true);
    //    b.transform.position = muzzle.position;
    //    b.transform.eulerAngles = new Vector3(0, launcher.eulerAngles.y + UnityEngine.Random.Range(-mistake, mistake), 0);
    //    b.Straight();

    //    FireCallback();
    //}

    //회전하면서 발사
    private void LauncherRotation()
    {
        for (int i = 0; i < muzzles.Length; i++)
        {
            launcher[i].angle = muzzles[i].eulerAngles.y;
            launcher[i].Fire();
        }

        //몸 회전코드
        if (bodyCoroutine != null)
        {
            StopCoroutine(bodyCoroutine);
        }
        //다음에 회전을 여기로 해라
        desired = rotation + (rotationSpeed * timer);
        bodyCoroutine = StartCoroutine(BodyRotationCoroutine(timer));
    }

    ////원모양으로 발사
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
