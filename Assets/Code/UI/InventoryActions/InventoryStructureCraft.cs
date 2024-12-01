using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryStructureCraft", menuName = "Inventory Actions/InventoryStructureCraft", order = 4)]
public class InventoryStructureCraft : InventoryAction
{
    [SerializeField]
    private string type, outputName;
    [SerializeField]
    private int outputCount;


   public override void InventoryAct(Inventory inventory){
        World world = inventory.GetWorld();
        if(CanCraft(inventory, world.GetCosts(outputName))){
            foreach((string name, int count) in inventory.GetWorld().GetCosts(outputName)){
                inventory.ConsumeItem(name, count);
            }
            BasicItem item = world.GetPrefab(outputName).GetComponent<BasicItem>();
            inventory.AddItem(outputName, outputCount, item.GetSprite(), item.action);
        } else {
            Debug.Log("Crafting failed! Displaying error");
            string error;
            if(!inventory.GetWorld().canSmelt) error = "You are not at the required structure to craft "+outputName+"!";
            else error = "You do not have the required materials to craft "+outputName+"!";
            inventory.DisplayError(error);
        }
        
    }
    bool CanCraft(Inventory inventory, (string, int)[] costs){
        if(type == "smelt" && !inventory.GetWorld().canSmelt) return false;
        foreach((string name, int count) in costs){
            if(inventory.CountItem(name) < count) return false;
        }
        return true;
    }
}
