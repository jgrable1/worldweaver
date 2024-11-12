using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryCraft", menuName = "Inventory Actions/InventoryCraft", order = 3)]
public class InventoryCraft : InventoryAction
{
    string[] costNames;
    int[] costCounts;
   public override void InventoryAct(Inventory inventory){
        itemAction = false;
        RecipeSlot recipe = inventory.GetSelectedR();
        if(CanCraft(inventory)){
            for(int i = 0; i < costNames.Length; i++){
                inventory.ConsumeItem(costNames[i], costCounts[i]);
            }
            inventory.AddItem(recipe.GetName(), recipe.GetProducedCount(), recipe.GetSprite(), recipe.GetPrefab(), recipe.GetAction());
        } else {
            // Debug.Log("Crafting failed! Displaying error");
            inventory.DisplayError("You do not have the required materials to craft "+recipe.GetName()+"!");
        }
        
    }
    bool CanCraft(Inventory inventory){
        RecipeSlot recipe = inventory.GetSelectedR();
        string[] costs = recipe.GetCosts();
        string name;
        int count;
        List<string> nameList = new List<string>();
        List<int> countList = new List<int>();
        for(int i = 0; i < costs.Length; i++){
            name = costs[i].Substring(0, costs[i].Length-2);
            nameList.Add(name);
            count = int.Parse(costs[i].Substring(costs[i].Length-2));
            countList.Add(count);
            Debug.Log("Checking if Player has "+count+" of "+name);
            if(inventory.CountItem(name) < count) return false;
            Debug.Log("They do!");
        }
        costNames = nameList.ToArray();
        costCounts = countList.ToArray();
        return true;
    }
}
