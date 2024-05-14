using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public enum ePoolingObject
    {
        PlayerBullet,
        EnemyBullet,
        Meteor,
        MeteorObj,
        RotatingSphere,
        BigBullet,
        WindMillPatten,
        HaxagonLaser,
        BossEndAttackRange,
        BlueBullet,
        UpGroundObj,
        UpGroundPushObj,
        CheckedLaserPatten,
        LaserPatten,
        RedButterfly,
        RedButterflyBomb,
        Type2RedBullet,
        RhombusLaser,
        UpGroundLaserObj,
        BlueBombing,
        BombingObj,
        ComponuntBulle,
        Mine,
        Explosion,
        GuidedUI,
        Flame,
        FireballParticle,
        Defence,
        JinBossNoamlBullet,
        JinBossCannonBullet,
        JinBossBounceBullet,
        DefenceMachinePet,
        FlameMachinePet,
        GunMachinePet,
        MineMachinePet,
        PullObject,
        Type1Patten3,
        CurveBulletPatten,
        CurveBullet,
        FireBall,
        RushCoolTimer,
        PlayerHpUI,
        CardSelectWindow,
        CardButton,
        SFXAuiodSource,
    }

    [System.Serializable]
    public class cPoolingObject
    {
        public GameObject obj;
        public int count;
        [TextArea] public string description;
    }

    [SerializeField] private List<cPoolingObject> m_listPoolingObj;

    public enum soundPoolingClip
    {

    }

    //  오브젝트에 사운드클립을 박아넣고 모든클립은 재생끝났을때 자동리무브 해주는걸로 만들예정

    [System.Serializable]
    public class cPoolingClip 
    {
        public AudioClip clip;
        public int count;
    }

    [SerializeField] private List<cPoolingClip> m_listSoundClip;

    public static PoolingManager Instance;
    private void OnValidate()
    {


    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
        initPoolingParents();
        initPoolingChild();
    }

    private void Start()
    {

    }



    private void initPoolingParents()
    {
        List<string> listParentName = new List<string>();

        int pCount = transform.childCount;
        int cCount = transform.childCount;
        for (int iNum = 0; iNum < pCount; ++iNum)
        {
            string name = transform.GetChild(iNum).name;
            listParentName.Add(name);
        }

        pCount = m_listPoolingObj.Count;
        for (int iNum = 0; iNum < pCount; ++iNum)
        {
            if (m_listPoolingObj[iNum].obj == null)
            {
                continue;
            }

            cPoolingObject data = m_listPoolingObj[iNum];

            string name = data.obj.name;
            bool exist = listParentName.Exists(x => x == name);
            if (exist == true)
            {
                listParentName.Remove(name);
            }
            else
            {
                GameObject objParent = new GameObject();
                objParent.transform.SetParent(transform);
                objParent.name = name;
            }
        }

       


        pCount = listParentName.Count;
        for (int iNum = pCount - 1; iNum > -1; --iNum)
        {
            GameObject obj = transform.Find(listParentName[iNum]).gameObject;
            Destroy(obj);
        }
    }

    private void initPoolingChild()
    {
        int pCount = m_listPoolingObj.Count;
        for (int iNum = 0; iNum < pCount; ++iNum)
        {
            if (m_listPoolingObj[iNum].obj == null)
            {
                continue;
            }

            cPoolingObject objPooing = m_listPoolingObj[iNum];
            GameObject obj = m_listPoolingObj[iNum].obj;
            string name = obj.name;
            Transform parent = transform.Find(name);

            int objCount = parent.childCount;

            for (int idNum = objCount - 1; idNum > -1; --idNum)
            {
                Destroy(transform.GetChild(idNum).gameObject);
            }

            if (objCount < objPooing.count)
            {
                int diffcount = objPooing.count - objCount;
                for (int icNum = 0; icNum < diffcount; ++icNum)
                {
                    GameObject cObj = createObject(name);
                    cObj.transform.SetParent(parent);
                }
            }
        }
    }

    private GameObject createObject(string _name)
    {
        GameObject obj = m_listPoolingObj.Find(x => x.obj.name == _name).obj;
        GameObject iobj = Instantiate(obj);
        iobj.SetActive(false);
        iobj.name = _name;
        return iobj;
    }

    public GameObject CreateObject(ePoolingObject _value, Transform _parent)
    {
        string findObjectName = _value.ToString().Replace("_", " ");
        return getPoolingObject(findObjectName, _parent);
    }

    public GameObject CreateObject(string _name, Transform _parent)
    {
        return getPoolingObject(_name, _parent);
    }



    private GameObject getPoolingObject(string _name, Transform _parent)
    {
        Transform parent = transform.Find(_name);

        if (parent == null)
        {
            Debug.LogError("프리팹에 오브젝트가 들어가 있지 않은것 같습니다.");
            return null;
        }

        GameObject returnValue = null;
        if (parent.childCount > 0)
        {
            returnValue = parent.GetChild(0).gameObject;
        }
        else
        {
            returnValue = createObject(_name);
        }
        returnValue.transform.SetParent(_parent);
        returnValue.SetActive(true);
        return returnValue;
    }

    public void RemovePoolingObject(GameObject _obj)
    {
        string name = _obj.name;
        Transform parent = transform.Find(name);

        cPoolingObject poolingObj = m_listPoolingObj.Find(x => x.obj.name == name);

        int poolingCount = poolingObj.count;

        if (parent.childCount < poolingCount)//부족했을때
        {
            _obj.transform.SetParent(parent);
            _obj.SetActive(false);
            _obj.transform.position = Vector3.zero;
        }
        else
        {
            Destroy(_obj);
        }
    }

    public void RemoveAllPoolingObject(GameObject _obj)
    {
        int parentCount = _obj.transform.childCount;
        for (int i = parentCount - 1; i> 0; i--)
        {
            Transform trsObj = _obj.transform.GetChild(i);

            if (trsObj == null)
            {
                Debug.Log("수정필요");
                break;
            }
            string name = trsObj.name;

            Transform parent = transform.Find(name);

            cPoolingObject poolingObj = m_listPoolingObj.Find(x => x.obj.name == name);

            int poolingCount = poolingObj.count;

            if (parent.childCount < poolingCount)
            {
                trsObj.SetParent(parent);
                trsObj.gameObject.SetActive(false);
                trsObj.position = Vector3.zero;

            }
            else
            {
                Destroy(_obj);
            }
        }
    }

    


    
}
