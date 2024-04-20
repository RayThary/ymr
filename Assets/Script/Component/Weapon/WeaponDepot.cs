using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//player�� �����ϰ� 
public class WeaponDepot : MonoBehaviour
{
    [SerializeField]
    private bool isTestPlayer = false; //���߿� �����ּ���
    private NewLauncher _launcher;
    public NewLauncher Launcher { get => _launcher; }
    [SerializeField]
    private Transform muzzle;
    public Transform Muzzle { get => muzzle; }  
    private Player player;
    [SerializeField]
    private Transform bow;
    [SerializeField]
    private Transform wand;


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        _launcher = new NewLauncher();
        //launcher.AddAttackType(new Shot(launcher, player, muzzle, PoolingManager.ePoolingObject.ComponuntBulle, 45, 1.5f, 10));
        //_launcher.AddAttackType(new FireBall(_launcher, player, muzzle, 1, 10));
        //FireB fireB = new FireB(player);
        //fireB.Activation();


        //player.Hit(null, 10);

        if (isTestPlayer)
        {
            EquipBow();
        }
    }

    // Update is called once per frame
    void Update()
    {
        muzzle.eulerAngles = new Vector3(0, Direction_Calculation_Screen(Input.mousePosition), 0);
        if(Input.GetMouseButton(0))
        {
            _launcher.LeftDown();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            _launcher.LeftUp();
        }
    }

    //��� ���� ����
    public void Disarming()
    {
        _launcher = new NewLauncher();
    }

    //��� ī�� ����
    public void CardDisarming()
    {
        for(int i = 0; i < CardManager.Instance.selectCards.Count; i++)
        {
            CardManager.Instance.selectCards[i].Deactivation();
        }
        CardManager.Instance.selectCards.Clear();
    }
    public void EquipBow()
    {
        _launcher.AddAttackType(new Shot(_launcher, player, muzzle, PoolingManager.ePoolingObject.ComponuntBulle, 0, 1, 10));
        bow.gameObject.SetActive(true);
        wand.gameObject.SetActive(false);
    }
    public void EquipWand()
    {
        _launcher.AddAttackType(new Ignite(_launcher, player, PoolingManager.ePoolingObject.Flame, 1, 5));
        wand.gameObject.SetActive(true);
        bow.gameObject.SetActive(false);
    }

    public Shot AddShot(PoolingManager.ePoolingObject ePoolingObject, float angle, float rate, float timer)
    {
        Shot shot = new Shot(_launcher, player, muzzle, ePoolingObject, angle, rate, timer);
        _launcher.AddAttackType(shot);
        return shot;
    }
    public void RemoveShot(Shot shot)
    {
        _launcher.RemoveAttackType(shot);
    }

    /// <summary>
    /// ���ؾ� �ϴ� ������ ����󿡼� �ָ��Ҷ�
    /// </summary>
    /// <param name="screen"></param>
    public float Direction_Calculation_Screen(Vector3 screen)
    {
        //������ ��ġ�� ��ũ����ġ�� ��ȯ�ؼ� ������ ���
        //raycast�� �浹�� ���� �ʴ� ��쿡 ������ ������ �ʱ⿡
        //ScreenToWorld�� ���̸� ��������ϴ� ������ �ֱ⿡ ���̰� �ٸ� �� ����
        return AngleCalculator.ScreenAngleCalculate(screen, Camera.main.WorldToScreenPoint(transform.position));
    }
}

//
//
//
public class ComponentController
{
    List<IComponentController> components = new List<IComponentController>();
    Player _player;
    public ComponentController(Player player)
    {
        _player = player;
    }
    public void CallAttack(Unit unit)
    {
        for (int i = 0; i < components.Count; i++)
        {
            components[i].Attack(unit);
        }
    }
    public void CallFire()
    {
        for (int i = 0; i < components.Count; i++)
        {
            components[i].Fire();
        }
    }
    public void CallHit(Unit unit, ref float figure)
    {
        for (int i = 0; i < components.Count; i++)
        {
            components[i].Hit(unit, ref figure);
        }
    }
    public void CallDashStart()
    {
        for (int i = 0; i < components.Count; i++)
        {
            components[i].DashStart();
        }
    }
    public void CallDashEnd()
    {
        for (int i = 0; i < components.Count; i++)
        {
            components[i].DashEnd();
        }
    }
    public void CallKill(Unit unit)
    {
        for (int i = 0; i < components.Count; i++)
        {
            components[i].Kill(unit);
        }
    }
    public void CallAddComponent(ComponentObject componentObject)
    {
        for(int i = 0; i < components.Count; i++)
        {
            components[i].AddComponent(componentObject);
        }
    }

    public void AddComponent(IComponentController component)
    {
        components.Add(component);
        component.Install(_player);
    }
    public void RemoveComponent(IComponentController component)
    {
        components.Remove(component);
        component.Uninstall(_player);
    }
    public bool ContainComponent(IComponentController component)
    {
        return components.Contains(component);
    }
}
public interface IComponentController
{
    void Fire();
    void Hit(Unit unit, ref float figure);
    void DashStart();
    void DashEnd();
    void Kill(Unit unit);
    void Attack(Unit unit);
    void AddComponent(ComponentObject componentObject);
    void Install(Player player);
    void Uninstall(Player player);
}


//�Ѿ��� ���Ϳ��� �¾Ƶ� ������� �ʵ��� ��
public class ComponentPenetrationCreator : IComponentController
{
    public void AddComponent(ComponentObject componentObject)
    {
        componentObject.AddComponent(new ComponentPenetration());
    }

    public void Attack(Unit unit) { }
    public void DashEnd() { }
    public void DashStart() { }
    public void Fire() { }
    public void Hit(Unit unit, ref float figure) { }
    public void Kill(Unit unit) { }
    public void Install(Player player) { }
    public void Uninstall(Player player) { }    
}
//3��° ���ݸ��� ���Ϳ��� ���������� ���ڰ� ��ġ��
public class ComponentMineCreator : IComponentController
{
    int count = 0;
    public void AddComponent(ComponentObject componentObject)
    {
        count++;
        if (count == 3)
        {
            count = 0;
            componentObject.AddComponent(new ComponentMine());
        }
    }
    public void Attack(Unit unit) { }
    public void DashEnd() { }
    public void DashStart() { }
    public void Fire() { }
    public void Hit(Unit unit, ref float figure) { }
    public void Kill(Unit unit) { }
    public void Install(Player player) { }
    public void Uninstall(Player player) { }
}
//�� ���ݸ��� ü���� ȸ����
public class ComponentDrainCreator : IComponentController
{
    ComponentObject ComponentObject;
    public void AddComponent(ComponentObject componentObject)
    {
        ComponentObject = componentObject;
    }
    public void Attack(Unit unit) 
    {
        ComponentObject.Player.STAT.RecoveryHP(1, null);
        Debug.Log("Attack���� ���� ȸ��");
    }
    public void DashEnd() { }
    public void DashStart() { }
    public void Fire() { }
    public void Hit(Unit unit, ref float figure) { }
    public void Kill(Unit unit) { }
    public void Install(Player player) { }
    public void Uninstall(Player player) { }
}
//�� ���ݸ��� ������
public class ComponentExplosionCreator : IComponentController
{
    public void AddComponent(ComponentObject componentObject)
    {
        componentObject.AddComponent(new ComponentExplosion());
    }
    public void Attack(Unit unit) { }
    public void DashEnd() { }
    public void DashStart() { }
    public void Fire() { }
    public void Hit(Unit unit, ref float figure) { }
    public void Kill(Unit unit) { }
    public void Install(Player player) { }
    public void Uninstall(Player player) { }
}
//�Ѿ��� ��븦 �Ѿư�
public class ComponentGuidedCreator : IComponentController
{
    public void AddComponent(ComponentObject componentObject)
    {
        componentObject.AddComponent(new ComponentGuided());
    }
    public void Attack(Unit unit) { }
    public void DashEnd() { }
    public void DashStart() { }
    public void Fire() { }
    public void Hit(Unit unit, ref float figure) { }
    public void Kill(Unit unit) { }
    public void Install(Player player) { }
    public void Uninstall(Player player) { }
}
//�� ������ ��븦 �ߵ���Ŵ
public class ComponentPoisonCreator : IComponentController
{
    private Player _player;
    private float _damage = 1;
    private int _duration = 5;
    public void AddComponent(ComponentObject componentObject) { }
    public void Attack(Unit unit)
    {
        unit.HitDot(_DOT.POISON, _duration, _damage, _player);
    }
    public void DashEnd() { }
    public void DashStart() { }
    public void Fire() { }
    public void Hit(Unit unit, ref float figure) { }
    public void Kill(Unit unit) { }
    public void Install(Player player) { _player = player; }
    public void Uninstall(Player player) { }
}
//�̵��ӵ��� ������
public class ComponentFastCreator : IComponentController
{
    public void AddComponent(ComponentObject componentObject) { }
    public void Attack(Unit unit) { }
    public void DashEnd() { }
    public void DashStart() { }
    public void Fire() { }
    public void Hit(Unit unit, ref float figure) { }
    public void Kill(Unit unit) { }
    public void Install(Player player) 
    {
        player.MoveSpeed += 1;
        Debug.Log("�ӵ� ����");
    }
    public void Uninstall(Player player)
    {
        player.MoveSpeed -= 1;
        Debug.Log("�ӵ� ����");
    }
}
//
//
//

//������ ��ó ������ �̵���
public class ComponentMoveCreator : IComponentController
{
    public void AddComponent(ComponentObject componentObject)
    {
        componentObject.AddComponent(new ComponentMove());
    }
    public void Attack(Unit unit) { }
    public void DashEnd() { }
    public void DashStart() { }
    public void Fire() { }
    public void Hit(Unit unit, ref float figure) { }
    public void Kill(Unit unit) { }
    public void Install(Player player) { }
    public void Uninstall(Player player) { }
}
//
//
//

//������ ������� �ѹ� ������
public class ComponentDefenseCreator : IComponentController
{
    Player _player;
    GameObject sprite;
    public void AddComponent(ComponentObject componentObject) { }
    public void Attack(Unit unit) { }
    public void DashEnd() { }
    public void DashStart() { }
    public void Fire() { }
    public void Hit(Unit unit, ref float figure) 
    {
        figure = 0;
        _player.componentController.RemoveComponent(this);
    }
    public void Install(Player player) 
    { 
        _player = player;
        sprite = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.Defence, _player.transform);
        sprite.transform.localPosition = new Vector3(0, 0.4f, 0);
    }
    public void Kill(Unit unit) { }
    public void Uninstall(Player player)
    {
        PoolingManager.Instance.RemovePoolingObject(sprite);
        sprite = null;
    }
}

//
//Swing
//
//������ ���� �Ѿ� ����
public class ComponentCutCreator : IComponentController
{
    public void AddComponent(ComponentObject compObject)
    {
        compObject.AddComponent(new ComponentCut());
    }
    public void Attack(Unit unit) { }
    public void DashEnd() { }
    public void DashStart() { }
    public void Fire() { }
    public void Hit(Unit unit, ref float figure) { }
    public void Kill(Unit unit) { }
    public void Install(Player player) { }
    public void Uninstall(Player player) { }
}
//���ݹ����� ��뿡�� 1 ������� ������
public class ComponentThornCreator : IComponentController
{
    float _damage = 1;
    Player _player;
    public void AddComponent(ComponentObject componentObject) { }

    public void Attack(Unit unit) { }

    public void DashEnd() { }

    public void DashStart() { }

    public void Fire() { }

    public void Hit(Unit unit, ref float figure)
    {
        unit.Hit(_player, _damage);
    }

    public void Install(Player player) { _player = player; }

    public void Kill(Unit unit) { }

    public void Uninstall(Player player) { }
}
//�ڿ�ȸ�� Ȱ��ȭ
public class ComponentNaturalCreator : IComponentController
{
    public void AddComponent(ComponentObject componentObject) { }
    public void Attack(Unit unit) { }
    public void DashEnd() { }
    public void DashStart() { }
    public void Fire() { }
    public void Hit(Unit unit, ref float figure) { }
    public void Kill(Unit unit) { }
    public void Install(Player player) 
    {
        player.STAT.NaturalHP = 1;
    }
    public void Uninstall(Player player)
    {
        player.STAT.NaturalHP = 0;
    }
}
//���ݽ� ���ݹ������� �뽬
public class ComponentDashCreator : IComponentController
{
    Player _player;
    public void AddComponent(ComponentObject componentObject) { }
    public void Attack(Unit unit) { }
    public void DashEnd() { }
    public void DashStart() { }
    public void Fire() 
    {
        Vector2 dir = (Input.mousePosition - Camera.main.WorldToScreenPoint( _player.transform.position)).normalized;
        float timer = _player.SpaceTimer;
        _player.SpaceTimer = 0;
        _player.Space(new Vector3(dir.x, 0, dir.y) * 6);
        _player.SpaceTimer = timer;
    }
    public void Hit(Unit unit, ref float figure) { }
    public void Install(Player player) { _player = player; }
    public void Kill(Unit unit) { }
    public void Uninstall(Player player) { }
}

//��ø� �Ҷ����� ������ 1ȸ ��ȭ��
public class ComponentEnhanceCreator : IComponentController
{
    Player _player;
    public void AddComponent(ComponentObject componentObject) { }
    public void Attack(Unit unit) { }
    public void DashEnd()
    {
        _player.componentController.AddComponent(new Enhance(_player));
    }
    public void DashStart() { }
    public void Fire() { }
    public void Hit(Unit unit, ref float figure) { }
    public void Kill(Unit unit) { }
    public void Install(Player player) { _player = player; }
    public void Uninstall(Player player) { }
}

//������ 1ȸ ��ȭ��
public class Enhance : IComponentController
{
    Player _player;

    public Enhance(Player player)
    {
        _player = player;
    }
    public void AddComponent(ComponentObject componentObject)
    {
        componentObject.AddComponent(new ComponentPain());
    }
    public void Attack(Unit unit) { }
    public void DashEnd() { }
    public void DashStart() { }
    public void Fire()
    {
        _player.componentController.RemoveComponent(this);
    }
    public void Hit(Unit unit, ref float figure) { }
    public void Kill(Unit unit) { }
    public void Install(Player player) { }
    public void Uninstall(Player player) { }
}


//���濡�� �߰� ������� ��
public class ComponentPain : IComponentObject
{
    ComponentObject ComponentObject;
    float _damage;

    public ComponentPain()
    {
        _damage = 1;
    }
    public ComponentPain(float damage)
    {
        _damage = damage;
    }
    public void Enter(Collider other)
    {
        other.GetComponent<Unit>()?.Hit(ComponentObject.Player, _damage);
    }

    public void Fire(ComponentObject componentObject)
    {
        ComponentObject = componentObject;
    }

    public void Update()
    {

    }
}
