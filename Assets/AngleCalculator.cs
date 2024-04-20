using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AngleCalculator
{
    //스크린 위치에 두 위치를 받아서 각도를 리턴함
    public static float ScreenAngleCalculate(Vector3 mousePosition, Vector3 objectPosition)
    {
        // 2. 마우스 위치를 향하는 방향 벡터를 계산합니다.
        Vector3 direction = mousePosition - objectPosition;

        // 3. 방향 벡터를 사용하여 오브젝트가 바라보는 각도를 계산합니다. (스크린은 xy로 위치를 나타냄)
        // x,y 의 인자를 반대로 넣으면 유니티 기준으로 각도가 잘나옴 ( 방향이 반대면서 -90도정도 더해진 상태로 답이 나옴)
        return Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
    }

    //월드 위치에 두 위치를 받아서 각도를 리턴함
    public static float WorldAngleCalculate(Vector3 look, Vector3 Criteria)
    {
        // 2. 마우스 위치를 향하는 방향 벡터를 계산합니다.
        Vector3 direction = look - Criteria;
         
        // 3. 방향 벡터를 사용하여 오브젝트가 바라보는 각도를 계산합니다. (월드는 xz로 위치를 나타냄)
        // x,y 의 인자를 반대로 넣으면 유니티 기준으로 각도가 잘나옴 ( 방향이 반대면서 -90도정도 더해진 상태로 답이 나옴)
        return Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
    }

    public static Card[] GetRandomCards(List<Card> list, int count)
    {
        if (list.Count < count)
            return null;

        Card[] selected = new Card[count];
        int index;
        for(int i = 0; i < count; i++)
        {
            index = Random.Range(0, list.Count);
            selected[i] = list[index];
            list.RemoveAt(index);
        }
        return selected;
    }

    //방향벡터를 각도로 리턴해줌
    public static float DirAngle(Vector2 dir)
    {
        float angleInR = Mathf.Atan2(dir.x, dir.y);
        return angleInR * (180 / Mathf.PI);
    }


}
