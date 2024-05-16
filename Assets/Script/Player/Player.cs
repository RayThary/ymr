using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Player : Unit
{
    
    public float MoveSpeed 
    { 
        get { return stat.SPEED; }
        set 
        {
            stat.SPEED = value;
            animator.SetFloat("MoveSpeed", stat.SPEED * 0.2f);
        } 
    }
    float horizontalInput = 0;
    float verticalInput = 0;
    Vector3 moveVelocity;

    //�뽬�� ��Ÿ���� �˷��� �̹���
    private Canvas spaceCanvas;
    private Image spaceImage;
    //�뽬�� ��Ÿ��
    [SerializeField]
    private float spaceCooltime = 3;
    //�뽬�� ������ �Ÿ�
    [SerializeField]
    private float spaceDis;
    //�뽬�� �̵��ð��� �����ð�
    [SerializeField]
    private float travel = 0.1f;
    //�ð��� �� ����
    private float spaceTimer;
    public float SpaceTimer { get { return spaceTimer; } set { spaceTimer = value; } }
    //�÷��̾ ������ �� �ִ���
    private bool canMove = true;
    public bool CanMove { get { return canMove; } set { canMove = value; } }
    
    [SerializeField]
    //�°� ���� �����ð�
    private float hit_invincibility;
    //�����ð��� ��� �ڷ�ƾ
    
    private Coroutine invincibility = null;
    private SpriteAlphaControl spriteAlpha;
    private Animator animator;
    //���� �Լ��� ������ ����
    private Action pull = null;

    private Rigidbody _rigidbody;
    public ComponentController componentController;
    private void Awake()
    {
    }

    protected new void Start()
    {
        base.Start();
        _rigidbody = GetComponent<Rigidbody>();
        spriteAlpha = GetComponent<SpriteAlphaControl>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        componentController = new ComponentController(this);
        DontDestroyOnLoad(this);
        CreatePlayerUI();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    

    private void Update()
    {
        moveVelocity = Vector3.zero;

        //�������� ���
        MoveCalculate();
        //�����ӿ� ���� ���� ���
        
        pull?.Invoke();
        
        //������
        Move();
        MoveAnimation();

        Vector3 look = Camera.main.WorldToScreenPoint(transform.position);
        UnitLook(Input.mousePosition - look);


        if (Input.GetKeyDown(KeyCode.Space))
        {
            Space(moveVelocity);
        }

        if (spaceTimer >= 0)
        {
            spaceTimer -= Time.deltaTime;
            spaceImage.fillAmount = spaceTimer / spaceCooltime;
        }
    }

    private void CreatePlayerUI()
    {
        spaceCanvas = FindObjectOfType<Canvas>();
        spaceImage = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.RushCoolTimer, spaceCanvas.transform).GetComponent<Image>();
        spaceImage.rectTransform.anchoredPosition = new Vector2(-100, 100);
        PlayerHpUI playerHpUI = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.PlayerHpUI, spaceCanvas.transform).GetComponent<PlayerHpUI>();
        playerHpUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(100, 100);
        playerHpUI.Stat = stat;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CreatePlayerUI();
        transform.position = new Vector3(14.5f, 0, 3);
    }

    private void MoveCalculate()
    {
        // ���� �� ���� �Է��� �����ɴϴ�.
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // �Է¿� ���� �̵� ���͸� ����ϴ�.
        Vector3 moveDirection = new(horizontalInput, 0, verticalInput);

        // �̵� ������ ���̸� 1�� ����ȭ�ϰ� �ӵ��� ���մϴ�.
        moveDirection.Normalize();
        moveVelocity = moveDirection * stat.SPEED;
    }

    //���� ����
    Vector3 target;
    //���� �ð�
    float timer;
    //���� ��
    float power;
    public void Pull(Vector3 target, float timer, float power)
    {
        this.target = target;
        this.timer = timer;
        this.power = power;
        pull += PullAction;
    }
    private void PullAction()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            pull -= PullAction;
        }
        Vector3 dir = (target - transform.position).normalized;
        moveVelocity += dir * power;
    }

    private void Move()
    {
        // �̵��� �����մϴ�.
        if (canMove)
        {
            _rigidbody.velocity = (moveVelocity);
        }
    }

    private void MoveAnimation()
    {
        if (moveVelocity == Vector3.zero)
        {
            animator.SetFloat("RunState", 0);
        }
        else if(canMove && moveVelocity != Vector3.zero)
        {
            animator.SetFloat("RunState", 0.5f);
        }
    }

    //�÷��̾�� ������ ��� �����ð��� �ʿ���
    public override void Hit(Unit unit, float figure)
    {
        if (god)
            return;
        componentController.CallHit(unit, ref figure);

        //������� 0���� ũ�ٸ�
        if(figure > 0)
        {
            base.Hit(unit, figure);
            //�÷��̾�� ü���� 0�̵Ǹ� ������ ��������
            if (stat.HP <= 0)
            {
                //���� �ٽý���
                GameManager.instance.PlayerDead();
                return;
            }
            else
            {
                //�÷��̾� �����ð�
                if (invincibility != null && godTimer < hit_invincibility)
                {
                    StopCoroutine(invincibility);
                    invincibility = StartCoroutine(Invincibility(hit_invincibility));
                }
                else if (invincibility == null)
                {
                    invincibility = StartCoroutine(Invincibility(hit_invincibility));
                }
                spriteAlpha.isHit = true;
            }
        }
    }

    //�÷��̾ ��� �������� ���� �¿� ����
    private void UnitLook(Vector3 dir)
    {
        if (dir.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    //�����̽��� ������ �뽬
    public void Space(Vector3 velocity)
    {
        if (velocity != Vector3.zero && spaceTimer <= 0)
        {
            //�̵��� ���� �ڷ�ƾ
            StartCoroutine(SpaceCoroutine(travel, velocity));

            //������ ���� �ڷ�ƾ
            //invincibility �� null���� �ƴ����� �����ð��� �۵������� �Ǵ�

            //�̹� ���� �����϶�
            //���� �� �����ð����� ���� �����ð��� ��ٸ� �����ð��� ���� �� �ʿ䰡 ���ٰ� �����ؼ� && godTimer < travel �ߴµ� �׳� ���ֵ� �������
            if (invincibility != null && godTimer < travel)
            {
                StopCoroutine(invincibility);
                invincibility = StartCoroutine(Invincibility(travel));
            }
            //�������°� �ƴϿ��ٸ� �׳� ����
            else if (invincibility == null)
            {
                invincibility = StartCoroutine(Invincibility(travel));
            }
            //��Ÿ��
            spaceTimer = spaceCooltime;
        }
    }

    //�뽬 �̵�
    private IEnumerator SpaceCoroutine(float t, Vector3 velocity)
    {
        componentController.CallDashStart();
        float timer = 0;
        canMove = false;

        while (true)
        {
            _rigidbody.velocity = (spaceDis * velocity);

            timer += Time.deltaTime;
            if (timer > t)
                break;
            yield return null;
        }
        componentController.CallDashEnd();
        canMove = true;
    }

    private IEnumerator UpGG(Vector3 velocity)
    {
        float timer = 0;
        canMove = false;
        while (true)
        {
            _rigidbody.velocity = (5 * velocity);
            timer += Time.deltaTime;

            if (timer > 0.5f)
                break;
            yield return null;
        }
        _rigidbody.velocity = Vector3.zero;
        canMove = true;
    }

    private IEnumerator Invincibility(float t)
    {
        god = true;
        godTimer = t;
        GetComponent<Collider>().enabled = false;
        while (godTimer > 0)
        {
            godTimer -= Time.deltaTime;
            yield return null;
        }
        GetComponent<Collider>().enabled = true;
        god = false;
        invincibility = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("UpGroundPush"))
        {
            if (!god)
            {
                Vector3 dir = (other.transform.position - transform.position) * -1;
                StartCoroutine(UpGG(UpggDirCalculate(dir)));
            }
        }
    }

    private Vector3 UpggDirCalculate(Vector3 vector)
    {
        float maxComponent = Mathf.Max(Mathf.Abs(vector.x), Mathf.Abs(vector.z));
        
        // ���� ū ���� ������ ������ ��Ҹ� 0���� ����ϴ�.
        Vector3 resultVector = new Vector3();
        if (maxComponent == Mathf.Abs(vector.x))
        {
            if(vector.x > 0)
                resultVector.x = 1;
            else
                resultVector.x = -1;
            resultVector.y = 0;
            resultVector.z = 0;
        }
        else // maxComponent == vector.z
        {
            resultVector.x = 0;
            resultVector.y = 0;
            if(vector.z > 0)
                resultVector.z = 1;
            else
                resultVector.z = -1;
        }
        return resultVector;
    }
}
