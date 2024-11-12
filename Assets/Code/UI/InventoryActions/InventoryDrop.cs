using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryDrop", menuName = "Inventory Actions/InventoryDrop", order = 2)]
public class InventoryDrop : InventoryAction
{
    public override void InventoryAct(Inventory inventory){
        ItemSlot item = inventory.GetSelected();
        Vector3 modifiedPosition = inventory.GetWorld().GetPlayer().transform.position;
        modifiedPosition.y -= 0.5f;
        // Debug.Log("Attempting to drop "+item.GetName());
        // Debug.Log("Using prefab: "+item.GetPrefab().name);
        BasicItem newItemObject = Instantiate(item.GetPrefab(), modifiedPosition, Quaternion.identity).GetComponent<BasicItem>();
        newItemObject.SetInventoryRef(inventory);
        newItemObject.prefab = item.GetPrefab();
        inventory.DeleteItem();
    }
}
