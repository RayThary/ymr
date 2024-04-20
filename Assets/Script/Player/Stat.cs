using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Linq;

public class Stat : MonoBehaviour
{
    private Unit user;
    [SerializeField] string bossName;
    public string BossName => bossName;

    //ü��
    [SerializeField] 
    private float hp;
    public float HP { get { return  hp; } }
    [SerializeField]
    private float maxHp;
    public float MAXHP { get { return maxHp; } set { maxHp += value; if (hp > maxHp) hp = maxHp; } }    
    //ü�� �ڿ� ȸ����
    private float naturalHP;
    public float NaturalHP { get => naturalHP; set => naturalHP = value; }
    private float mp;
    public float MP { get { return mp; } }
    private float maxMp;
    public float MAXMP { get { return maxMp; } }
    private float naturalMP;
    private float defence;
    public float DEFENCE { get { return defence; } }
    private float resistance;
    public float RESISTANCE { get { return resistance; } }
    [SerializeField]
    private float ad;
    public float AD { get { return ad; } set { ad += value; } }
    private float ap;
    public float AP { get { return ap; } }
    private float speed;
    public float SPEED { get { return speed; } set { speed = value; } }
    //
    public float originalHP;
    public float originalNaturalHP;
    public float originalMP;
    public float originalNaturalMP;
    public float originalDefence;
    public float originalResistance;
    public float originalAd;
    public float originalAp;
    public float originalSpeed;

    //Natural_Recovery_HP
    public bool naturalHPing;
    private Coroutine nrhp = null;
    //Natural_Recovery_MP
    public bool naturalMPing;
    private Coroutine nrmp = null;
    //Dot_Poison
    private Coroutine dot_poison = null;
    private Coroutine dot_burn = null;
    private Coroutine dot_shock = null;
    private Coroutine dot_bleeding = null;

    /// <summary>
    /// ������ �ߵ��ϴ� unit = ���� �����ߴ��� float = �����, 0��°�� �׻� ui�� ����� ǥ��
    /// </summary>
    protected Action<Unit, float> hitDelegate = null;
    // ���� ������ �ߵ��ϴ� �Լ��� ���� �����ߴ��� float = �����
    protected Action<Unit, float> attackDelegate = null;

    //color��
    private Color red = new Color(0.5660378f, 0, 0);
    private Color green = new Color(0, 0.5647059f, 0);
    private Color blue = new Color(0, 0.2810156f, 1);

    //�����̻��� �޴��� �ȹ޴���
    public bool poison = true;
    public bool burn = true;
    public bool shock = true;
    public bool bleeding = true;


    //��Ʈ���̳� �ڿ� ȸ���� ���ʿ� �ѹ� �Ͼ��
    private float dotTime = 2;
    private float naturalTime = 2;

    public string SetText()
    {
        string str = "ü�� = " + Mathf.Floor(hp * 100f) / 100f + " / " + Mathf.Floor(maxHp * 100f) / 100f + "\n" +
                    "���� = " + Mathf.Floor(mp * 100f) / 100f + " / " + Mathf.Floor(maxMp * 100f) / 100f + "\n" +
                    "ü����� = " + naturalHP + "\n" +
                    "������� = " + naturalMP + "\n" +
                    "���� = " + defence + "\n" +
                    "���׷� = " + resistance + "\n" +
                    "���ݷ� = " + ad + "\n" +
                    "�ֹ��� = " + ap + "\n" +
                    "�ӵ� = " + speed;
        return str;
    }

    public void Init()
    {
        user = GetComponent<Unit>();
        hp = maxHp = originalHP;
        naturalHP = originalNaturalHP;
        mp = maxMp = originalMP;
        naturalMP = originalNaturalMP;
        defence = originalDefence;
        resistance = originalResistance;
        ad = originalAd;
        ap = originalAp;
        speed = originalSpeed;
    }

    public void MinusHp(float figure)
    {
        hp -= figure;
        Debug.Log($"{user.name}�� ���� ü�� {hp}");
        if(nrhp == null && naturalHPing)
        {
            nrhp = StartCoroutine(Natural_Recovery_HP());
        }
    }

    public void MinusMp(float figure) 
    { 
        mp -= figure;
        if (mp < 0)
            mp = 0;
        if (nrmp == null && naturalMPing)
        {
            nrmp = StartCoroutine(Natural_Recovery_MP());
        }
    }

    public void RecoveryHP(float figure, Unit perpetrator)
    {
        if (hp + figure > maxHp)
            figure = maxHp - hp;
        hp += figure;
    }

    public void RecoveryMP(float figure, Unit perpetrator)
    {
        if (mp + figure > maxMp)
            figure = maxMp - mp;
        mp += figure;
    }

    /// <summary>
    /// ad ������� ���¿� �°� ��������
    /// </summary>
    /// <param name="figure">�����</param>
    /// <param name="penetration">�� ����� (����) </param>
    /// <param name="per">�� ����� (�ۼ�Ʈ)</param>
    /// <returns>���°� ������� ���İ� �����</returns>
    public float Halved_AD(float figure, float penetration, float per)
    {
        float defence = this.defence - penetration;
        defence -= defence * per * 0.01f;
        return (100 / (100 + defence) * figure);
    }
    /// <summary>
    /// ap ������� ���¿� �°� ��������
    /// </summary>
    /// <param name="figure">�����</param>
    /// <param name="penetration">���׷� ����� (����) </param>
    /// <param name="per">���׷� ����� (�ۼ�Ʈ)</param>
    /// <returns>���׷°� ������� ���İ� �����</returns>
    public float Halved_AP(float figure, float penetration, float per)
    {
        float resistance = this.resistance - penetration;
        resistance -= resistance * per * 0.01f;
        return (100 / (100 + resistance) * figure);
    }

    /// <summary>
    /// ���������� ���� �޾��� ��
    /// </summary>
    /// <param name="figure">�����</param>
    /// <param name="penetration">�����</param>
    /// <param name="per">����� (�ۼ�Ʈ)</param>
    /// <param name="perpetrator">���� �����Ѱ���</param>
    public float Be_Attacked_AD(float figure, float penetration, float per, Unit perpetrator)
    {
        float damage = Halved_AD(figure, penetration, per);
        MinusHp(damage);
        HitInvocation(perpetrator, damage);
        //Debug.Log($"{perpetrator.transform.name}���� {figure} ��ŭ ����� ���� ���� ü�� {hp}");
        return damage;
    }
    /// <summary>
    /// ���������� ���� �޾��� ��
    /// </summary>
    /// <param name="figure">�����</param>
    /// <param name="penetration">�����</param>
    /// <param name="per">����� (�ۼ�Ʈ)</param>
    /// <param name="perpetrator">���� �����Ѱ���</param>
    public float Be_Attacked_AP(float figure, float penetration, float per, Unit perpetrator)
    {
        float damage = Halved_AP(figure, penetration, per);
        MinusHp(damage);
        HitInvocation(perpetrator, damage);

        return damage;
    }
    /// <summary>
    /// ���������� ���� �޾��� ��
    /// </summary>
    /// <param name="figure">�����</param>
    /// <param name="perpetrator">���� �����Ѱ���</param>
    public float Be_Attacked_TRUE(float figure, Unit perpetrator)
    {
        MinusHp(figure);
        HitInvocation(perpetrator, figure);

        return figure;
    }

    //�ڿ� ȸ��, max���� �������� ����
    private IEnumerator Natural_Recovery_HP()
    {
        float t = 0;
        while(hp < maxHp)
        {
            t += Time.deltaTime;

            if(t > naturalTime)
            {
                hp += naturalHP;
                t = 0;
            }

            yield return null;
        }
    }
    private IEnumerator Natural_Recovery_MP()
    {
        float t = 0;
        while (mp < maxMp)
        {
            t += Time.deltaTime;

            if (t > naturalTime)
            {
                mp += naturalMP;
                t = 0;
            }

            yield return null;
        }
    }

    //��Ʈ������� ������ �̹� �ߵ����� ��Ʈ������� ���� �� �ߵ���
    //  �� : �����̳� ���׷� ������ ���� ����
    /// <summary>
    /// �� ��Ʈ�� 1�ʸ��� ������� ��
    /// </summary>
    /// <param name="duration">�ߵ� Ƚ��</param>
    /// <param name="figure">1ȸ�� �����</param>
    /// <param name="attack">���ݽ� �ߵ��ϴ� delegate</param>
    /// <param name="perpetrator">���� �ߴ���</param>
    public void Be_Attacked_Poison(int duration, float figure, Unit perpetrator)
    {
        if(poison)
        {
            if (dot_poison != null)
            {
                StopCoroutine(dot_poison);
            }
            dot_poison = StartCoroutine(Dot_Poison(duration, figure));
        }
    }
    private IEnumerator Dot_Poison(int duration, float figure)
    {
        int du = 1;
        float t = 0;
        while (du <= duration)
        {
            yield return null;

            t += Time.deltaTime;

            if(t >= dotTime)
            {
                MinusHp(figure);

                du++;
                t = 0;
            }
        }
        dot_poison = null;
    }
    //ȭ�� : �����̳� ���׷� ������ ���� ����
    /// <summary>
    /// ȭ�� ��Ʈ�� 1�ʸ��� ������� ��
    /// </summary>
    /// <param name="duration">�ߵ� Ƚ��</param>
    /// <param name="figure">1ȭ�� �����</param>
    /// <param name="perpetrator">���� �ߴ���</param>
    public void Be_Attacked_Burn(int duration, float figure, Unit perpetrator)
    {
        if(burn)
        {
            if (dot_burn != null)
            {
                StopCoroutine(dot_burn);
            }
            dot_burn = StartCoroutine(Dot_Burn(duration, figure, perpetrator));
        }
    }
    private IEnumerator Dot_Burn(int duration, float figure, Unit perpetrator)
    {
        int du = 1;
        float t = 0;
        while (du <= duration)
        {
            yield return null;

            t += Time.deltaTime;

            if (t >= dotTime)
            {
                MinusHp(figure);
                du++;
                t = 0;
            }
        }
        dot_burn = null;
    }
    //���� : ap�� ���׷� ������ ����
    /// <summary>
    /// ���� ��Ʈ�� 1�ʸ��� ������� ��
    /// </summary>
    /// <param name="duration">�ߵ� Ƚ��</param>
    /// <param name="figure">1ȸ�� �����</param>
    /// <param name="penetration">�� �����(+)</param>
    /// <param name="per">�� �����(%)</param>
    /// <param name="perpetrator">���� �ߴ���</param>
    public void Be_Attacked_Shock(int duration, float figure, float penetration, float per, Unit perpetrator)
    {
        if(shock)
        {
            if (dot_shock != null)
            {
                StopCoroutine(dot_shock);
            }
            dot_shock = StartCoroutine(Dot_Shock(duration, figure, penetration, per, perpetrator));
        }
    }
    private IEnumerator Dot_Shock(int duration, float figure, float penetration, float per, Unit perpetrator)
    {
        int du = 1;
        float t = 0;    
        while (du <= duration)
        {
            yield return null;

            t += Time.deltaTime;

            if(t >= dotTime)
            {
                MinusHp(figure);
                du++;
                t = 0;
            }
        }
        dot_shock = null;
    }
    //���� : ad�� ���� ������ ����
    /// <summary>
    /// ���� ��Ʈ�� 1�ʸ��� ������� ��
    /// </summary>
    /// <param name="duration">�ߵ� Ƚ��</param>
    /// <param name="figure">1ȸ�� �����</param>
    /// <param name="penetration">�� �����(+)</param>
    /// <param name="per">�� �����(%)</param>
    /// <param name="attack">���ݽ� �ߵ��ϴ� delegate</param>
    /// <param name="perpetrator">���� �ߴ���</param>
    public void Be_Attacked_Bleeding(int duration, float figure, float penetration, float per, Unit perpetrator)
    {
        if(bleeding)
        {
            if (dot_bleeding != null)
            {
                StopCoroutine(dot_bleeding);
            }
            dot_bleeding = StartCoroutine(Dot_Bleeding(duration, figure, penetration, per));
        }
    }
    private IEnumerator Dot_Bleeding(int duration, float figure, float penetration, float per)
    {
        int du = 1;
        float t = 0;
        float damage;
        while (du <= duration)
        {
            yield return null;

            t += Time.deltaTime;

            if(t >= dotTime)
            {
                damage = Halved_AD(figure, penetration, per);
                MinusHp(damage);

                du++;
                t = 0;
            }
        }
        dot_bleeding = null;
    }

    //

    public void AddHit(Action<Unit, float> action)
    {
        if (hitDelegate == null)
        {
            hitDelegate = action;
            return;
        }

        if (!hitDelegate.GetInvocationList().Contains(action))
            //hitDelegate = (Action<Unit, float>)Delegate.Combine(hitDelegate, action);
            hitDelegate += action;
    }
    public void RemoveHit(Action<Unit, float> action)
    {
        if (hitDelegate == null)
        {
            return;
        }

        if (hitDelegate.GetInvocationList().Contains(action))
            hitDelegate -= action;
    }
    public void AddAttack(Action<Unit, float> action)
    {
        if (attackDelegate == null)
        {
            attackDelegate = action;
            return;
        }

        if (!attackDelegate.GetInvocationList().Contains(action))
            attackDelegate += action;

    }
    public void RemoveAttack(Action<Unit, float> action)
    {
        if (attackDelegate == null)
        {
            return;
        }

        if (attackDelegate.GetInvocationList().Contains(action))
            attackDelegate -= action;
    }
    /// <summary>
    /// hitDelegate ���� �ߵ�
    /// </summary>
    /// <param name="perpetrator">���� �� �����ߴ���</param>
    /// <param name="figure">�����</param>
    public void HitInvocation(Unit perpetrator, float figure)
    {
        if (hitDelegate != null)
        {
            if (perpetrator != null)
                hitDelegate(perpetrator, figure);
        }
    }

    /// <summary>
    /// ���� ���������� attackDelegate ���� �ߵ�
    /// </summary>
    /// <param name="perpetrator">���� �����ߴ���</param>
    /// <param name="figure">�����</param>
    public void AttackInvocation(Unit perpetrator, float figure)
    {
        if(attackDelegate != null)
        {
            if (perpetrator != null)
                attackDelegate(perpetrator, figure);
        }
    }
    //maxHP , hp �����ϴºκ��̾�� ����
    public void SetHp(float _value)
    {
        hp = _value;
        maxHp = _value;
    }
}
