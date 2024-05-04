using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GroundPatten : MonoBehaviour
{
    [SerializeField] private GameObject nowMap;
    private List<GameObject> maps = new List<GameObject>();

    [SerializeField] private float HrizontalSetUpGroundTime = 1;
    [SerializeField] private bool Horizontal;

    [SerializeField] private float ViticalSetUpGroundTime = 1;
    [SerializeField] private bool Vitical;


    [SerializeField] private float UpSetUpGroundTime = 1;
    [SerializeField] private bool Up;
    [SerializeField] private float RightSetUpGroundTime = 1;
    [SerializeField] private bool Right;


    [SerializeField] private float CloseWallSetUpGroundTime = 1;
    [SerializeField] private int CloseWallRadius = 4;


    private List<GameObject> mapUnder = new List<GameObject>();
    [SerializeField]private List<GameObject> mapUnderTrs = new List<GameObject>();

    private List<(int, int)> listUpground = new List<(int, int)>();
    private void addUpground((int, int) point)
    {
        listUpground.Add(point);
    }
    private void removeUpground((int, int) point)
    {
        listUpground.Remove(point);
    }

    public enum PattenName
    {
        HorizontalAndVerticalPatten,
        WavePattenHrizontal,
        WavePattenVitical,
        OpenWallGroundPatten
    }

    [SerializeField] private PattenName pattenName;

    [SerializeField] private bool pattenStart = false;


    private bool mapChange = true;

    private Transform playerTrs;
    private Stat stat;

    void Start()
    {
        playerTrs = GameManager.instance.GetPlayerTransform;
        stat = GetComponent<Stat>();
    }

    // Update is called once per frame
    void Update()
    {
        nowMapCheck();
        groundPatten();

    }

    private void nowMapCheck()
    {
        if (mapChange)
        {
            //맵이바뀌면 겟스테이지로 현재스테이지를 체크해줘서 리턴을해줘야함 *만들예정
            //nowMpa = maps[GameManager.instance.nowStage()]
            int childCount = nowMap.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                maps.Add(nowMap.transform.GetChild(i).gameObject);
                if (i == childCount - 1)
                {
                    mapUnder = maps.FindAll((x) => x.name.Contains("Tile") == true && x.transform.childCount == 0);
                    mapChange = false;
                }
            }
        }
    }

    private void groundPatten()
    {
        if (pattenStart)
        {
            if (pattenName == PattenName.HorizontalAndVerticalPatten)
            {
                if (Horizontal)
                {
                    horizontalPatten();
                }
                if (Vitical)
                {
                    verticalPatten();
                }

                pattenStart = false;
            }

            if (pattenName == PattenName.WavePattenHrizontal)
            {
                if (Right)
                {
                    StartCoroutine(wavePattenRightStart());
                }
                else
                {
                    StartCoroutine(wavePattenLeftStart());
                }

                pattenStart = false;
            }

            if (pattenName == PattenName.WavePattenVitical)
            {
                if (Up)
                {
                    StartCoroutine(wavePattenUpStart());
                }
                else
                {
                    StartCoroutine(wavePattenDownStart());
                }
                pattenStart = false;
            }

            if (pattenName == PattenName.OpenWallGroundPatten)
            {
                closeWallGroundPatten(CloseWallRadius);//반지름의크기 ex) 4 = 8*8 로만들어진벽
                pattenStart = false;
            }
        }
    }



    private void verticalPatten()
    {
        mapUnderTrs.Clear();
        int add = 3;
        int limitLine = 28;
        int findNum = 1;
        string findText = string.Empty;
        while (true)
        {
            findText = $"{{{findNum},";
            mapUnderTrs.AddRange(mapUnder.FindAll((x) => x.name.Contains(findText) == true));
            findNum += add;
            if (findNum > limitLine)
            {
                break;
            }
        }

        for (int i = 0; i < mapUnderTrs.Count; i++)
        {
            GameObject obj;
            obj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.UpGroundPushObj, GameManager.instance.GetEnemyAttackObjectPatten);

            Vector3 objVec = mapUnderTrs[i].transform.position;
            objVec.y = -1.1f;
            obj.transform.position = objVec;

            UpGround upGround = obj.GetComponent<UpGround>();
            upGround.Horizontal = true;
            DangerZone danger = obj.GetComponentInChildren<DangerZone>();
            danger.SetTime(ViticalSetUpGroundTime);
        }

    }

    private void horizontalPatten()
    {
        mapUnderTrs.Clear();
        int add = 3;
        int limitLine = 28;
        int findNum = 1;
        string findText = string.Empty;
        while (true)
        {
            findText = $",{findNum}}}";
            mapUnderTrs.AddRange(mapUnder.FindAll((x) => x.name.Contains(findText) == true));
            findNum += add;
            if (findNum > limitLine)
            {
                break;
            }
        }

        for (int i = 0; i < mapUnderTrs.Count; i++)
        {
            GameObject obj;

            obj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.UpGroundPushObj, GameManager.instance.GetEnemyAttackObjectPatten);

            Vector3 objVec = mapUnderTrs[i].transform.position;
            objVec.y = -1.1f;
            obj.transform.position = objVec;

            UpGround upGround = obj.GetComponentInChildren<UpGround>();
            upGround.Horizontal = true;

            DangerZone danger = obj.GetComponentInChildren<DangerZone>();
            danger.SetTime(HrizontalSetUpGroundTime);
        }

    }

    IEnumerator wavePattenRightStart()
    {
        mapUnderTrs.Clear();
        for (int i = 1; i < 28; i++)
        {
            GameObject obj;
            List<GameObject> objTrs = new List<GameObject>();
            objTrs.Clear();

            objTrs = mapUnder.FindAll((x) => x.name.Contains($"{{{i},") == true);
            for (int j = 0; j < objTrs.Count; j++)
            {
                obj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.UpGroundPushObj, GameManager.instance.GetEnemyAttackObjectPatten);
                Vector3 objVec = objTrs[j].transform.position;
                objVec.y = -1.1f;
                obj.transform.position = objVec;

                UpGround upGround = obj.GetComponent<UpGround>();
                upGround.Vertical = true;

                DangerZone danger = obj.GetComponentInChildren<DangerZone>();
                danger.SetTime(RightSetUpGroundTime);
            }
            yield return new WaitForSeconds(1.8f);
        }
    }

    IEnumerator wavePattenLeftStart()
    {
        for (int i = 28; i > 0; i--)
        {
            GameObject obj;
            List<GameObject> objTrs = new List<GameObject>();
            objTrs.Clear();

            objTrs = mapUnder.FindAll((x) => x.name.Contains($"{{{i},") == true);
            for (int j = 0; j < objTrs.Count; j++)
            {
                obj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.UpGroundPushObj, GameManager.instance.GetEnemyAttackObjectPatten);
                Vector3 objVec = objTrs[j].transform.position;
                objVec.y = -1.1f;
                obj.transform.position = objVec;

                UpGround upGround = obj.GetComponent<UpGround>();
                upGround.Vertical = true;

                DangerZone danger = obj.GetComponentInChildren<DangerZone>();
                danger.SetTime(RightSetUpGroundTime);//오른쪽에서 시작했다 왼쪽으로갈때 같은시간을쓰는중 나중에 분리해줄필요있을지도?
            }
            yield return new WaitForSeconds(1.8f);
        }
    }

    IEnumerator wavePattenDownStart()
    {
        mapUnderTrs.Clear();
        for (int i = 1; i < 25; i++)
        {
            GameObject obj;
            List<GameObject> objTrs = new List<GameObject>();
            objTrs.Clear();

            objTrs = mapUnder.FindAll((x) => x.name.Contains($",{i}}}") == true);
            for (int j = 0; j < objTrs.Count; j++)
            {
                obj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.UpGroundPushObj, GameManager.instance.GetEnemyAttackObjectPatten);
                Vector3 objVec = objTrs[j].transform.position;
                objVec.y = -1.1f;
                obj.transform.position = objVec;

                UpGround upGround = obj.GetComponent<UpGround>();
                upGround.Horizontal = true;

                DangerZone danger = obj.GetComponentInChildren<DangerZone>();
                danger.SetTime(UpSetUpGroundTime);
            }
            yield return new WaitForSeconds(1f);
        }

      
    }

    IEnumerator wavePattenUpStart()
    {
        for (int i = 25; i > 0; i--)
        {
            GameObject obj;
            List<GameObject> objTrs = new List<GameObject>();
            objTrs.Clear();

            objTrs = mapUnder.FindAll((x) => x.name.Contains($",{i}}}") == true);
            
            for (int j = 0; j < objTrs.Count; j++)
            {
                obj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.UpGroundPushObj, GameManager.instance.GetEnemyAttackObjectPatten);
                Vector3 objVec = objTrs[j].transform.position;
                objVec.y = -1.1f;
                obj.transform.position = objVec;

                UpGround upGround = obj.GetComponent<UpGround>();
                upGround.Horizontal = true;

                DangerZone danger = obj.GetComponentInChildren<DangerZone>();
                danger.SetTime(UpSetUpGroundTime);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void closeWallGroundPatten(int _value)
    {
        mapUnderTrs.Clear();


        Vector3 playerPos = playerTrs.position;
        int x = Mathf.RoundToInt(playerPos.x);
        int z = Mathf.RoundToInt(playerPos.z);


        int rightX = Mathf.Clamp(x + _value, 1, 28);
        int leftX = Mathf.Clamp(x - _value, 1, 28);


        int upZ = Mathf.Clamp(z + _value, 1, 28);
        int downZ = Mathf.Clamp(z - _value, 1, 28);

        Vector3 rightVec = mapUnder.Find((x) => x.name.Contains($"{{{rightX},{upZ}}}") == true).transform.position;
        Vector3 leftVec = mapUnder.Find((x) => x.name.Contains($"{{{leftX},{downZ}}}") == true).transform.position;
        Vector3 upVec = mapUnder.Find((x) => x.name.Contains($"{{{leftX},{upZ}}}") == true).transform.position;
        Vector3 downVec = mapUnder.Find((x) => x.name.Contains($"{{{rightX},{downZ}}}") == true).transform.position;

        List<Vector3> spawnTrs = new List<Vector3>();

        for (int i = 0; i < _value * 2; i++)
        {


            for (int j = 0; j < 4; j++)
            {
                switch (j)
                {
                    case 0:
                        if (spawnTrs.Exists((x) => x == rightVec) == false)
                        {
                            spawnTrs.Add(rightVec);
                        }
                        break;
                    case 1:
                        if (spawnTrs.Exists((x) => x == leftVec) == false)
                        {
                            spawnTrs.Add(leftVec);
                        }
                        break;
                    case 2:
                        if (spawnTrs.Exists((x) => x == upVec) == false)
                        {
                            spawnTrs.Add(upVec);
                        }
                        break;
                    case 3:
                        if (spawnTrs.Exists((x) => x == downVec) == false)
                        {
                            spawnTrs.Add(downVec);
                        }
                        break;
                }
            }

            if (rightVec.z > 1)
            {
                rightVec.z -= 1;
            }

            if (leftVec.z < 28)
            {
                leftVec.z += 1;
            }

            if (upVec.x < 28)
            {
                upVec.x += 1;
            }

            if (downVec.x > 1)
            {
                downVec.x -= 1;
            }
        }

        if (x < _value)
        {
            spawnTrs = spawnTrs.FindAll((x) => x.x > rightX == false);
        }


        if (z < _value)
        {
            spawnTrs = spawnTrs.FindAll((z) => z.z > upZ == false);
        }


        if (x > 28 - _value)
        {
            spawnTrs = spawnTrs.FindAll((x) => x.x < leftX == false);
        }

        if (z > 28 - _value)
        {
            spawnTrs = spawnTrs.FindAll((x) => x.z < downZ == false);
        }


        for (int i = 0; i < spawnTrs.Count; i++)
        {
            GameObject obj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.UpGroundPushObj, GameManager.instance.GetEnemyAttackObjectPatten);

            //spawnTrs[i].y = -1.1f; 이거왜안되지 

            Vector3 objTrs = spawnTrs[i];
            objTrs.y = -1.1f;
            obj.transform.position = objTrs;

            UpGround upGround = obj.GetComponent<UpGround>();
            upGround.SetStopTime(true, 3);

            upGround.CubeWall = true;

            Vector3 upPos = obj.transform.position + new Vector3(0, 0, 1);
            Vector3 downPos = obj.transform.position + new Vector3(0, 0, -1);
            Vector3 rightPos = obj.transform.position + new Vector3(1, 0, 0);
            Vector3 leftPos = obj.transform.position + new Vector3(-1, 0, 0);

            if (spawnTrs.Exists((x) => x == upPos || x == downPos))
            {
                upGround.Vertical = true;
            }
            if (spawnTrs.Exists((x) => x == rightPos || x == leftPos))
            {
                upGround.Horizontal = true;
            }

            DangerZone danger = obj.GetComponentInChildren<DangerZone>();
            danger.SetTime(CloseWallSetUpGroundTime);
        }

    }


    /// <summary>
    /// type 설명
    /// </summary>
    /// <param name="_value">사용할패턴</param>
    /// <param name="_type">_type 가 true 일땐 Horizontal , Up 이 True가된다.</param>
    public void GroundPattenStart(PattenName _value, bool _type)
    {
        if (_value == PattenName.HorizontalAndVerticalPatten)
        {
            pattenName = PattenName.HorizontalAndVerticalPatten;
            if (_type)
            {
                Horizontal = true;
                Vitical = false;
                pattenStart = true;
            }
            else
            {
                Horizontal = false;
                Vitical = true;
                pattenStart = true;
            }
        }
        else if (_value == PattenName.WavePattenHrizontal)
        {
            pattenName = PattenName.WavePattenHrizontal;
            if (_type)
            {
                Right = true;
                pattenStart = true;
            }
            else
            {
                Right = false;
                pattenStart = true;
            }
        }
        else if (_value == PattenName.WavePattenVitical)
        {
            pattenName = PattenName.WavePattenVitical;
            if (_type)
            {
                Up = true;
                pattenStart = true;
            }
            else
            {
                Up = false;
                pattenStart = true;
            }
        }
        else
        {
            Debug.LogError("이상하게넣어줌");
        }
    }

    public void GroundPattenStart(PattenName _value)
    {
        if (_value == PattenName.OpenWallGroundPatten)
        {
            pattenName = PattenName.OpenWallGroundPatten;
            pattenStart = true;
        }
        else
        {
            Debug.LogError("이상하게넣어줌");
        }
    }
}
