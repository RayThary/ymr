using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    private static CardManager _instance;
    public static CardManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CardManager>();
                if (_instance == null)
                {
                    GameObject go = new("CardManager");
                    _instance = go.AddComponent<CardManager>();
                }
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }


    public CardVeiw[] view;
    public List<Card> publicCards = new();
    public List<Card> selectCards = new();
    private Player player;

    private void Awake()
    {
    }

    private void Start()
    {
        player = GameManager.instance.GetPlayer;
        for (int i = 0; i < view.Length; i++)
        {
            view[i].Init(this);
        }
        publicCards.Add(new QuickAttackCard(player));
        publicCards.Add(new PenetrationCard(player));
        publicCards.Add(new MineCard(player));
        publicCards.Add(new DrainCard(player));
        publicCards.Add(new ExplosionCard(player));
        publicCards.Add(new GuidedCard(player));
        publicCards.Add(new PoisonCard(player));
        publicCards.Add(new FastCard(player));
        publicCards.Add(new HealthCard(player));
        publicCards.Add(new MoveCard(player));
        publicCards.Add(new CutCard(player));
        publicCards.Add(new ThornCard(player));
        publicCards.Add(new NaturalCard(player));
        publicCards.Add(new DashCard(player));
        publicCards.Add(new EnhanceCard(player));
        publicCards.Add(new MineMachineCard(player));
        publicCards.Add(new GunMachineCard(player));
        publicCards.Add(new FlameMachineCard(player));
        publicCards.Add(new DefenceMachineCard(player));
        publicCards.Add(new Evolution(player));
        //�Ѹ� ����������
        publicCards.Add(new ShotgunCard(player));
        publicCards.Add(new Quickly(player));
        //������
        publicCards.Add(new FireB(player));
    }

    public void ViewCards()
    {
        List<Card> list = publicCards.Except(selectCards).ToList();
        Card[] cards = AngleCalculator.GetRandomCards(list, view.Length);
        if (cards == null)
            return;
        for (int i = 0; i < view.Length; i++)
        {
            view[i].gameObject.SetActive(true);
            view[i].card = cards[i];
            view[i].transform.GetChild(0).GetComponent<Text>().text = view[i].card.ToString();
        }
    }

    public void Selected(Card card)
    {
        selectCards.Add(card);
        card.Activation();
        for (int i = 0; i < view.Length; i++)
        {
            view[i].gameObject.SetActive(false);
            view[i].card = null;
        }
        GameManager.instance.CardSelected();
    }
}


[System.Serializable]
public class QuickAttackCard : Card
{
    public QuickAttackCard(Player player) : base(player)
    {
        exp = "���ݼӵ��� �������ϴ�";
    }

    public override void Activation()
    {
        for (int i = 0; i < user.GetComponent<WeaponDepot>().Launcher.AttackTypes.Count; i++)
        {
            user.GetComponent<WeaponDepot>().Launcher.AttackTypes[i].Rate = -0.1f;
        }
    }

    public override void Deactivation()
    {
        for (int i = 0; i < user.GetComponent<WeaponDepot>().Launcher.AttackTypes.Count; i++)
        {
            user.GetComponent<WeaponDepot>().Launcher.AttackTypes[i].Rate = 0.1f;
        }
    }
}

[System.Serializable]
public class PenetrationCard : Card
{
    public PenetrationCard(Player player) : base(player)
    {
        exp = "������ ��󿡰� ���������� ���̻� ������� �ʽ��ϴ�.";
    }
    public override void Activation()
    {
        user.componentController.AddComponent(new ComponentPenetrationCreator());
    }
    public override void Deactivation()
    {
        user.componentController.RemoveComponent(new ComponentPenetrationCreator());
    }
}

[System.Serializable]
public class MineCard : Card
{
    public MineCard(Player player) : base(player)
    {
        exp = "��󿡰� ���ݸ��� 3��°���� ���ڸ� ��ȯ�մϴ�.";
    }
    public override void Activation()
    {
        user.componentController.AddComponent(new ComponentMineCreator());
    }

    public override void Deactivation()
    {
        user.componentController.RemoveComponent(new ComponentMineCreator());
    }
}

[System.Serializable]
public class DrainCard : Card
{
    public DrainCard(Player player) : base(player)
    {
        exp = "������ ������ ������ ü���� 1 ȸ���մϴ�.";
    }

    public override void Activation()
    {
        user.componentController.AddComponent(new ComponentDrainCreator());
    }

    public override void Deactivation()
    {
        user.componentController.RemoveComponent(new ComponentDrainCreator());
    }
}

[System.Serializable]
public class ExplosionCard : Card
{
    public ExplosionCard(Player player) : base(player)
    {
        exp = "�� ������ ������ ����ŵ�ϴ�.";
    }

    public override void Activation()
    {
        user.componentController.AddComponent(new ComponentExplosionCreator());
    }

    public override void Deactivation()
    {
        user.componentController.RemoveComponent(new ComponentExplosionCreator());
    }
}

[System.Serializable]
public class GuidedCard : Card
{
    public GuidedCard(Player player) : base(player)
    {
        exp = "������ ��븦 ���� ȸ���մϴ�.";
    }

    public override void Activation()
    {
        user.componentController.AddComponent(new ComponentGuidedCreator());
    }

    public override void Deactivation()
    {
        user.componentController.RemoveComponent(new ComponentGuidedCreator());
    }
}

[System.Serializable]
public class PoisonCard : Card
{
    public PoisonCard(Player player) : base(player)
    {
        exp = "������ ������ ������ ��󿡰� �����¸� �ο��մϴ�.";
    }

    public override void Activation()
    {
        user.componentController.AddComponent(new ComponentPoisonCreator());
    }
    public override void Deactivation()
    {
        user.componentController.RemoveComponent(new ComponentPoisonCreator());
    }
}

[System.Serializable]
public class FastCard : Card
{
    public FastCard(Player player) : base(player)
    {
        exp = "�̵��ӵ��� ����մϴ�.";
    }

    public override void Activation()
    {
        user.componentController.AddComponent(new ComponentFastCreator());
    }
    public override void Deactivation()
    {
        user.componentController.RemoveComponent(new ComponentFastCreator());
    }
}

[System.Serializable]
public class HealthCard : Card
{
    public HealthCard(Player player) : base(player)
    {
        exp = "�ִ� ü���� ����մϴ�.";
    }
    public override void Activation()
    {
        user.STAT.MAXHP = 10;
    }
    public override void Deactivation()
    {
        user.STAT.MAXHP = -10;
    }
}

[System.Serializable]
public class MoveCard : Card
{
    public MoveCard(Player player) : base(player)
    {
        exp = "������ ��븦 ���� �̵��մϴ�.";
    }

    public override void Activation()
    {
        user.componentController.AddComponent(new ComponentMoveCreator());
    }
    public override void Deactivation()
    {
        user.componentController.RemoveComponent(new ComponentMoveCreator());
    }
}

[System.Serializable]
public class CutCard : Card
{
    public CutCard(Player player) : base(player)
    {
        exp = "������ ������ ������ ���۴ϴ�.";
    }

    public override void Activation()
    {
        user.componentController.AddComponent(new ComponentCutCreator());
    }
    public override void Deactivation()
    {
        user.componentController.RemoveComponent(new ComponentCutCreator());
    }
}

[System.Serializable]
public class ThornCard : Card
{
    public ThornCard(Player player) : base(player)
    {
        exp = "������ ���� �� ��뿡�� ������� 1 �ݴϴ�.";
    }

    public override void Activation()
    {
        user.componentController.AddComponent(new ComponentThornCreator());
    }
    public override void Deactivation()
    {
        user.componentController.RemoveComponent(new ComponentThornCreator());
    }
}


[System.Serializable]
public class NaturalCard : Card
{
    public NaturalCard(Player player) : base(player)
    {
        exp = "�ڿ�ȸ���� �����մϴ�.";
    }

    public override void Activation()
    {
        user.componentController.AddComponent(new ComponentNaturalCreator());
    }
    public override void Deactivation()
    {
        user.componentController.RemoveComponent(new ComponentNaturalCreator());
    }
}

[System.Serializable]
public class DashCard : Card
{
    public DashCard(Player player) : base(player)
    {
        exp = "���� �� ���� �������� �뽬�մϴ�.";
    }

    public override void Activation()
    {
        user.componentController.AddComponent(new ComponentDashCreator());
    }
    public override void Deactivation()
    {
        user.componentController.RemoveComponent(new ComponentDashCreator());
    }
}

[System.Serializable]
public class EnhanceCard : Card
{
    public EnhanceCard(Player player) : base(player)
    {
        exp = "�뽬 �� ������ 1ȸ ��ȸ�˴ϴ�.";
    }

    public override void Activation()
    {
        user.componentController.AddComponent(new ComponentEnhanceCreator());
    }
    public override void Deactivation()
    {
        user.componentController.RemoveComponent(new ComponentEnhanceCreator());
    }
}

[System.Serializable]
public class MineMachineCard : Card
{
    MineMachine machine;
    public MineMachineCard(Player player) : base(player)
    {
        exp = "�÷��̾ ����ٴϸ� ���ڸ� ��ġ�մϴ�.";
    }

    public override void Activation()
    {
        machine = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.MineMachinePet, null).GetComponent<MineMachine>();
        if (machine != null)
        {
            machine.Master = user;
            machine.transform.position = user.transform.position;
        }
    }
    public override void Deactivation()
    {
        if (machine != null)
        {
            machine.Master = null;
            PoolingManager.Instance.RemovePoolingObject(machine.gameObject);
        }
    }
}

[System.Serializable]
public class GunMachineCard : Card
{
    GunMachine machine;
    public GunMachineCard(Player player) : base(player)
    {
        exp = "�÷��̾ ����ٴϸ� ������ ���� �߻��մϴ�.";
    }

    public override void Activation()
    {
        machine = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.GunMachinePet, null).GetComponent<GunMachine>();
        if (machine != null)
        {
            machine.Master = user;
            machine.transform.position = user.transform.position;
        }
    }
    public override void Deactivation()
    {
        if (machine != null)
        {
            machine.Master = null;
            PoolingManager.Instance.RemovePoolingObject(machine.gameObject);
        }
    }
}

[System.Serializable]
public class FlameMachineCard : Card
{
    FlameMachine machine;
    public FlameMachineCard(Player player) : base(player)
    {
        exp = "������ �ٰ��� �����մϴ�.";
    }

    public override void Activation()
    {
        machine = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.FlameMachinePet, null).GetComponent<FlameMachine>();
        if (machine != null)
        {
            machine.Master = user;
            machine.transform.position = user.transform.position;
        }
    }
    public override void Deactivation()
    {
        if (machine != null)
        {
            machine.Master = null;
            PoolingManager.Instance.RemovePoolingObject(machine.gameObject);
        }
    }
}

[System.Serializable]
public class DefenceMachineCard : Card
{
    DefenceMachine machine;
    public DefenceMachineCard(Player player) : base(player)
    {
        exp = "�����ð����� �÷��̾�� ��ȣ���� �ɾ��ݴϴ�.";
    }

    public override void Activation()
    {
        machine = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.DefenceMachinePet, null).GetComponent<DefenceMachine>();
        if (machine != null)
        {
            machine.Master = user;
            machine.transform.position = user.transform.position;
        }
    }
    public override void Deactivation()
    {
        if (machine != null)
        {
            machine.Master = null;
            PoolingManager.Instance.RemovePoolingObject(machine.gameObject);
        }
    }
}
[System.Serializable]
public class Evolution : Card
{
    public Evolution(Player player) : base(player)
    {
        exp = "ü�� 10���� ���ݷ� 2����.";
    }

    public override void Activation()
    {
        user.STAT.MAXHP = 10;
        user.STAT.AD = 2;
    }

    public override void Deactivation()
    {
        user.STAT.MAXHP = -10;
        user.STAT.AD = -2;
    }
}

//Ȱ

[System.Serializable]
public class ShotgunCard : Card
{
    WeaponDepot weaponDepot;
    PoolingManager.ePoolingObject ePoolingObject;

    Shot[] shots;
    float angle;
    float rate;
    float timer;
    public ShotgunCard(Player player) : base(player)
    {
        weaponDepot = player.GetComponent<WeaponDepot>();
        ePoolingObject = PoolingManager.ePoolingObject.ComponuntBulle;
        shots = new Shot[2];
        angle = 30;
        rate = 1;
        timer = 5;
        exp = "��Ÿ��� ª�� ���ݼӵ��� ���� ������ 2�� �߰��˴ϴ�.";
    }

    public override void Activation()
    {
        shots[0] = weaponDepot.AddShot(ePoolingObject, angle, rate, timer);
        shots[1] = weaponDepot.AddShot(ePoolingObject, -angle, rate, timer);
    }

    public override void Deactivation()
    {
        weaponDepot.RemoveShot(shots[0]);
        weaponDepot.RemoveShot(shots[1]);
    }
}
[System.Serializable]
public class Quickly : Card
{
    WeaponDepot depot;
    public Quickly(Player player) : base(player)
    {
        exp = "���ݼӵ��� 0.1�� �������ϴ�. �Ѿ��� �ӵ��� �������ϴ�.";
        depot = player.GetComponent<WeaponDepot>();
    }

    public override void Activation()
    {
        if (!depot)
        {
            for (int i = 0; i < depot.Launcher.AttackTypes.Count; i++)
            {
                depot.Launcher.AttackTypes[i].Rate = -0.1f;
                depot.Launcher.AttackTypes[i].Speed += 2f;
            }
        }
    }

    public override void Deactivation()
    {
        if (!depot)
        {
            for (int i = 0; i < depot.Launcher.AttackTypes.Count; i++)
            {
                depot.Launcher.AttackTypes[i].Rate = 0.1f;
                depot.Launcher.AttackTypes[i].Speed -= 2f;
            }
        }
    }
}

// ������

[System.Serializable]
public class FireB : Card
{
    FireBall fireBall;
    public FireB(Player player) : base(player)
    {
        exp = "���ο� ������ ���̾� ���� �߰��˴ϴ�";
        //���̾�� ���ݼӵ��� �ƹ� �ǹ̰� ���� (rate)
        fireBall = new FireBall(player.GetComponent<WeaponDepot>().Launcher, player, player.GetComponent<WeaponDepot>().Muzzle, 1f, 10);
        fireBall.Speed = 5;
    }

    public override void Activation()
    {
        user.GetComponent<WeaponDepot>().Launcher.AddAttackType(fireBall);
    }

    public override void Deactivation()
    {
        user.GetComponent<WeaponDepot>().Launcher.RemoveAttackType(fireBall);
    }
}