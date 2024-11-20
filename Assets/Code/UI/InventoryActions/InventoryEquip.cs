using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryEquip", menuName = "Inventory Actions/InventoryEquip", order = 3)]
public class InventoryEquip : InventoryAction
{
    public override void InventoryAct(Inventory inventory){
        Debug.Log("Equipping "+inventory.GetSelected().GetName());
        inventory.EquipSelected();
    }
}
