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
        if(CanCraft(inventory, world.GetCosts(outputName)) && world.GetPlayer().IsGrounded()){
            foreach((string name, int count) in inventory.GetWorld().GetCosts(outputName)){
                inventory.ConsumeItem(name, count);
            }
            BasicMovement player = world.GetPlayer();
            Vector3 modifiedPosition = player.transform.position;
            Vector3 tempForward = player.transform.forward;
            tempForward.y = 0;
            modifiedPosition.y -= 0.75f;
            modifiedPosition += tempForward;
            Structure structure = Instantiate(world.GetPrefab(outputName), modifiedPosition, Quaternion.identity).GetComponent<Structure>();
            structure.Setup(inventory);
        } else {
            // Debug.Log("Crafting failed! Displaying error");
            string error;
            if(!world.canBuild) error = "Make space in front of you to place a "+outputName+"!";
            else if(!world.GetPlayer().IsGrounded()) error = "You must be on the ground to place a "+outputName+"!";
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
