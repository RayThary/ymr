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

    //대쉬의 쿨타임을 알려줄 이미지
    private Canvas spaceCanvas;
    private Image spaceImage;
    //대쉬의 쿨타임
    [SerializeField]
    private float spaceCooltime = 3;
    //대쉬가 움직일 거리
    [SerializeField]
    private float spaceDis;
    //대쉬의 이동시간과 무적시간
    [SerializeField]
    private float travel = 0.1f;
    //시간을 잴 변수
    private float spaceTimer;
    public float SpaceTimer { get { return spaceTimer; } set { spaceTimer = value; } }
    //플레이어가 움직일 수 있는지
    private bool canMove = true;
    public bool CanMove { get { return canMove; } set { canMove = value; } }
    
    [SerializeField]
    //맞고 나서 무적시간
    private float hit_invincibility;
    //무적시간을 재는 코루틴
    
    private Coroutine invincibility = null;
    private SpriteAlphaControl spriteAlpha;
    private Animator animator;
    //당기는 함수를 저장할 변수
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

        //움직임을 계산
        MoveCalculate();
        //움직임에 당기는 힘을 계산
        
        pull?.Invoke();
        
        //움직임
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
        // 수평 및 수직 입력을 가져옵니다.
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // 입력에 따라 이동 벡터를 만듭니다.
        Vector3 moveDirection = new(horizontalInput, 0, verticalInput);

        // 이동 벡터의 길이를 1로 정규화하고 속도를 곱합니다.
        moveDirection.Normalize();
        moveVelocity = moveDirection * stat.SPEED;
    }

    //당기는 방향
    Vector3 target;
    //당기는 시간
    float timer;
    //당기는 힘
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
        // 이동을 적용합니다.
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

    //플레이어는 맞을때 잠시 무적시간이 필요함
    public override void Hit(Unit unit, float figure)
    {
        if (god)
            return;
        componentController.CallHit(unit, ref figure);

        //대미지가 0보다 크다면
        if(figure > 0)
        {
            base.Hit(unit, figure);
            //플레이어는 체력이 0이되면 게임이 끝난거임
            if (stat.HP <= 0)
            {
                //게임 다시시작
                GameManager.instance.PlayerDead();
                return;
            }
            else
            {
                //플레이어 무적시간
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

    //플레이어가 어디를 보는지에 따라 좌우 반전
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

    //스페이스를 누르면 대쉬
    public void Space(Vector3 velocity)
    {
        if (velocity != Vector3.zero && spaceTimer <= 0)
        {
            //이동에 관한 코루틴
            StartCoroutine(SpaceCoroutine(travel, velocity));

            //무적에 관한 코루틴
            //invincibility 가 null인지 아닌지로 무적시간이 작동중인지 판단

            //이미 무적 상태일때
            //새로 들어갈 무적시간보다 현재 무적시간이 길다면 무적시간을 새로 할 필요가 없다고 생각해서 && godTimer < travel 했는데 그냥 없애도 상관없음
            if (invincibility != null && godTimer < travel)
            {
                StopCoroutine(invincibility);
                invincibility = StartCoroutine(Invincibility(travel));
            }
            //무적상태가 아니였다면 그냥 시작
            else if (invincibility == null)
            {
                invincibility = StartCoroutine(Invincibility(travel));
            }
            //쿨타임
            spaceTimer = spaceCooltime;
        }
    }

    //대쉬 이동
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
        
        // 가장 큰 값을 제외한 나머지 요소를 0으로 만듭니다.
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
