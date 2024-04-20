using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComponentGuided : IComponentObject
{
    private ComponentObject componentBullet;
    private Transform _canvas;
    private float _senser;
    private float _radius;

    public void Fire(ComponentObject bullet)
    {
        _canvas = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.GuidedUI, null).transform;
        _canvas.transform.SetParent(bullet.transform, false);
        _senser = _canvas.GetChild(0).GetComponent<Image>().fillAmount * 360;
        _radius = _canvas.GetChild(0).GetComponent<Image>().rectTransform.rect.width * 0.5f;
        componentBullet = bullet;
    }

    public void Update()
    {
        componentBullet.transform.eulerAngles += new Vector3(0, Rotate(10, FindTarget()) * 0.1f, 0);
    }

    public void Enter(Collider other)
    {

    }

    private Transform FindTarget()
    {
        Collider[] colliders = new Collider[10];
        int count = Physics.OverlapSphereNonAlloc(componentBullet.transform.position, _radius, colliders, LayerMask.GetMask("Enemy"));
        for(int i = 0; i < count; i++)
        {
            if (GetAngle(colliders[i].transform) < _senser)
            {
                return colliders[i].transform;
            }
        }

        return null;
    }

    private float Rotate(float max, Transform target)
    {
        if(target == null)
            return 0;

        Vector3 dir = (target.position - componentBullet.transform.position).normalized;
        // 내적(dot)을 통해 각도를 구함. (Acos로 나온 각도는 방향을 알 수가 없음)
        float dot = Vector3.Dot(componentBullet.transform.forward, dir);
        if (dot < 1.0f)
        {
            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

            // 외적을 통해 각도의 방향을 판별.
            Vector3 cross = Vector3.Cross(componentBullet.transform.forward, dir);
            // 외적 결과 값에 따라 각도 반영
            if (cross.y < 0)
            {
                angle = componentBullet.transform.rotation.eulerAngles.z - Mathf.Min(max, angle);
            }
            else
            {
                angle = componentBullet.transform.rotation.eulerAngles.z + Mathf.Min(max, angle);
            }
            return angle;
        }
        return 0;
    }

    private float GetAngle(Transform target)
    {
        Vector3 dir = (target.position - componentBullet.transform.position).normalized;
        float dot = Vector3.Dot(componentBullet.transform.forward, dir);
        return Mathf.Acos(dot) * Mathf.Rad2Deg;
    }
}
