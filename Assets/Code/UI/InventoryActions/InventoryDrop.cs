using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryDrop", menuName = "Inventory Actions/InventoryDrop", order = 2)]
public class InventoryDrop : InventoryAction
{
    public override void InventoryAct(Inventory inventory){
        World world = inventory.GetWorld();
        if(world.GetPlayer().IsGrounded()){
            Vector3 modifiedPosition = world.GetPlayer().transform.position;
            ItemSlot item = inventory.GetSelected();
            modifiedPosition.y -= 0.5f;
            Debug.Log("Inventory Drop Called");
            BasicItem newItemObject = Instantiate(world.GetPrefab(item.GetName()), modifiedPosition, Quaternion.identity).GetComponent<BasicItem>();
            newItemObject.SetInventoryRef(inventory);
            newItemObject.SetCount(item.GetCount());
            inventory.DeleteItem();
        } else inventory.DisplayError("You must be grounded to drop an item!");
        
    }
}
