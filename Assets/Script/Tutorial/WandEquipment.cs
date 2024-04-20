using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandEquipment : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        WeaponDepot weaponDepot = other.GetComponent<WeaponDepot>();
        if (weaponDepot != null)
        {
            weaponDepot.Disarming();
            weaponDepot.EquipWand();
        }
    }
}
