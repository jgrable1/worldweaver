using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryHeal", menuName = "Inventory Actions/InventoryHeal", order = 2)]
public class InventoryHeal : InventoryAction
{
    [SerializeField]
    protected int cost = 1;
    [SerializeField]
    protected int heal = 1;

    public override void InventoryAct(Inventory inventory){
        useCount = cost;
        PlayerStats stats = inventory.GetWorld().GetPlayer().gameObject.GetComponent<PlayerStats>();
        if(stats.health == stats.GetMaxHealth()) inventory.DisplayError("You are already at max health!");
        else{
            base.InventoryAct(inventory);
            if(canAct) stats.PlayerHPUpdate(heal);
            else{
                if(inventory.GetSelected() != null){
                    inventory.DisplayError("You don't have enough "+inventory.GetSelected().GetName()+"!");
                }
            }
        }
        
    }
}
