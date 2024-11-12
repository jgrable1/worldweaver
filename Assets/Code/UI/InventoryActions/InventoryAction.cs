using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryAction", menuName = "Inventory Actions/InventoryAction", order = 1)]
public class InventoryAction : ScriptableObject
{
    protected bool canAct = true;
    protected int useCount = 0;
    protected bool itemAction = true;
    
    public virtual void InventoryAct(Inventory inventory){
        // print("Base Inventory Action");
        if(itemAction && inventory.GetSelected().GetCount()-useCount >= 0){
            inventory.GetSelected().AddCount(-useCount);
        } else if(itemAction) canAct = false;
    }
}
