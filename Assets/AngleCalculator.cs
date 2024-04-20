using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AngleCalculator
{
    //��ũ�� ��ġ�� �� ��ġ�� �޾Ƽ� ������ ������
    public static float ScreenAngleCalculate(Vector3 mousePosition, Vector3 objectPosition)
    {
        // 2. ���콺 ��ġ�� ���ϴ� ���� ���͸� ����մϴ�.
        Vector3 direction = mousePosition - objectPosition;

        // 3. ���� ���͸� ����Ͽ� ������Ʈ�� �ٶ󺸴� ������ ����մϴ�. (��ũ���� xy�� ��ġ�� ��Ÿ��)
        // x,y �� ���ڸ� �ݴ�� ������ ����Ƽ �������� ������ �߳��� ( ������ �ݴ�鼭 -90������ ������ ���·� ���� ����)
        return Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
    }

    //���� ��ġ�� �� ��ġ�� �޾Ƽ� ������ ������
    public static float WorldAngleCalculate(Vector3 look, Vector3 Criteria)
    {
        // 2. ���콺 ��ġ�� ���ϴ� ���� ���͸� ����մϴ�.
        Vector3 direction = look - Criteria;
         
        // 3. ���� ���͸� ����Ͽ� ������Ʈ�� �ٶ󺸴� ������ ����մϴ�. (����� xz�� ��ġ�� ��Ÿ��)
        // x,y �� ���ڸ� �ݴ�� ������ ����Ƽ �������� ������ �߳��� ( ������ �ݴ�鼭 -90������ ������ ���·� ���� ����)
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

    //���⺤�͸� ������ ��������
    public static float DirAngle(Vector2 dir)
    {
        float angleInR = Mathf.Atan2(dir.x, dir.y);
        return angleInR * (180 / Mathf.PI);
    }


}
