using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryDrop", menuName = "Inventory Actions/InventoryDrop", order = 2)]
public class InventoryDrop : InventoryAction
{
    public override void InventoryAct(Inventory inventory){
        World world = inventory.GetWorld();
        Vector3 modifiedPosition = world.GetPlayer().transform.position;
        modifiedPosition.y -= 0.5f;
        // Debug.Log("Attempting to drop "+item.GetName());
        // Debug.Log("Using prefab: "+world.GetPrefab(item.GetName());
        BasicItem newItemObject = Instantiate(world.GetPrefab(inventory.GetSelected().GetName()), modifiedPosition, Quaternion.identity).GetComponent<BasicItem>();
        newItemObject.SetInventoryRef(inventory);
        inventory.DeleteItem();
    }
}
