using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BowEquipment : MonoBehaviour
{
    [SerializeField]
    private Text text;
    private WeaponDepot inside;

    private void OnTriggerEnter(Collider other)
    {
        inside = other.GetComponent<WeaponDepot>();
        if (inside != null)
            text.gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (inside != null)
            text.gameObject.SetActive(false);
        inside = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (inside != null)
            {
                inside.Disarming();
                inside.EquipBow();
            }
        }
    }
}
