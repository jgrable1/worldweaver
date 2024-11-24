using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryBuild", menuName = "Inventory Actions/InventoryBuild", order = 5)]
public class InventoryBuild : InventoryAction
{
    [SerializeField]
    private string outputName;


   public override void InventoryAct(Inventory inventory){
        World world = inventory.GetWorld();
        if(CanCraft(inventory, world.GetCosts(outputName))){
            foreach((string name, int count) in inventory.GetWorld().GetCosts(outputName)){
                inventory.ConsumeItem(name, count);
            }
            Vector3 modifiedPosition = world.GetPlayer().transform.position;
            modifiedPosition.y -= 0.75f;
            modifiedPosition.z += 1f;
            Structure structure = Instantiate(world.GetPrefab(outputName), modifiedPosition, Quaternion.identity).GetComponent<Structure>();
            structure.Setup(inventory);
        } else {
            // Debug.Log("Crafting failed! Displaying error");
            string error;
            if(!inventory.GetWorld().canBuild) error = "Make space in front of you to place a "+outputName+"!";
            else error = "You do not have the required materials to build "+outputName+"!";
            inventory.DisplayError(error);
        }
        
    }
    bool CanCraft(Inventory inventory, (string, int)[] costs){
        if(!inventory.GetWorld().canBuild) return false;
        foreach((string name, int count) in costs){
            if(inventory.CountItem(name) < count) return false;
        }
        return true;
    }
}
