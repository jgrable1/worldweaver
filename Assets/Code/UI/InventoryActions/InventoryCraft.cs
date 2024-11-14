using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryCraft", menuName = "Inventory Actions/InventoryCraft", order = 3)]
public class InventoryCraft : InventoryAction
{
   public override void InventoryAct(Inventory inventory){
        itemAction = false;
        RecipeSlot recipe = inventory.GetSelectedR();
        if(CanCraft(inventory, recipe)){
            foreach((string name, int count) in inventory.GetWorld().GetCosts(recipe.GetName())){
                inventory.ConsumeItem(name, count);
            }
            inventory.AddItem(recipe.GetName(), recipe.GetProducedCount(), recipe.GetSprite(), recipe.GetAction());
        } else {
            // Debug.Log("Crafting failed! Displaying error");
            inventory.DisplayError("You do not have the required materials to craft "+recipe.GetName()+"!");
        }
        
    }
    bool CanCraft(Inventory inventory, RecipeSlot recipe){
        foreach((string name, int count) in inventory.GetWorld().GetCosts(recipe.GetName())){
            if(inventory.CountItem(name) < count) return false;
        }
        return true;
    }
}
