using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{

    private SpriteRenderer spr;

    private float rangeSpeed = 0.5f;
    private float sXY;
    private float Ratio;
    private bool meteorSpawn = false;
    private GameObject shadow;
    private GameObject meteor;
    private Color originalColor;


    [SerializeField] private Unit boss;
    public Unit Boss { set => boss = value; }

    void Start()
    {
        spr = GetComponentInChildren<SpriteRenderer>();
        shadow = spr.gameObject;
        originalColor = spr.color;
    }


    void Update()
    {

        if (meteorSpawn)
        {
            return;
        }

        Ratio += Time.deltaTime * rangeSpeed;
        sXY = Mathf.Lerp(0.2f, 0.1f, Ratio);

        shadow.transform.localScale = new Vector3(sXY, sXY, 0.1f);
        if (sXY <= 0.1f)
        {
            meteor = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.MeteorObj, GameManager.instance.GetEnemyAttackObjectParent);
            meteor.transform.position = new Vector3(transform.position.x, 2, transform.position.z);
            meteor.GetComponent<MeteorMove>().Boss = boss;
            StartCoroutine(shodwFalse());
            meteorSpawn = true;
        }
    }
    IEnumerator shodwFalse()
    {
        yield return new WaitForSeconds(0.3f);

        spr.color = originalColor;
        Ratio = 0;
        sXY = 0.2f;
        meteorSpawn = false;
        PoolingManager.Instance.RemovePoolingObject(gameObject);
    }


}
