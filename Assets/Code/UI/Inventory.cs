using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Inventory : MonoBehaviour
{

    public GameObject inventory;
    private bool inventoryUp;
    [SerializeField]
    private World world;
    public ItemSlot[] slots;
    public RecipeSlot[] recipes;
    private Dictionary<string, int> recipeIDs = new Dictionary<string, int>();
    [SerializeField]
    private TMP_Text selectedDescription, selectedName, errorDisplay;
    [SerializeField]
    private Image selectedImage;
    private int selectedItem;
    private int selectedRecipe;
    public InventoryAction craftingAction;
    public Sprite lockedSprite;
    public int equipped;

    void Awake(){
        for(int i = 0; i < slots.Length; i++) slots[i].id = i;
        for(int i = 0; i < recipes.Length; i++){
            recipes[i].id = i;
            // print("Setting recipe for "+recipes[i].GetName()+" to id "+i);
            recipeIDs.Add(recipes[i].GetName(), i);
        }
        
    }

    /*void Start()
    {
        
    }*/

    public void SetLocks(Dictionary<string, (string, int)[]> costs){
        foreach(KeyValuePair<string, (string, int)[]> entry in costs){
            // print(entry.Key+" has "+entry.Value.Length+" locks.");
            recipes[recipeIDs[entry.Key]].remainingLocks = entry.Value.Length;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Inventory")) Activate(!inventoryUp);
    }

    void Activate(bool b){
        // print("Activate called");
        if(!b) DeselectLastSlot();
        inventoryUp = b;
        inventory.SetActive(b);
        world.RestrictMovement(b, "Inventory");
        world.RestrictCamera(b, "Inventory");
        Time.timeScale = (b ? 0 : 1);
    }

    public bool AddItem(string name, int count, Sprite sprite, InventoryAction action){
        bool foundSlot = false;
        int originalCount = count;
        int slotIndex = FindExistingSlot(name);
        if(!world.HasSeenItem(name)) world.NewItem(name);
        while(slotIndex != -1){
            // print("Found existing slot "+(slotIndex+1)+" containing "+name);
            count = slots[slotIndex].AddCount(count);
            if(count <= 0){
                foundSlot = true;
                break;
            } else{
                slotIndex = FindExistingSlot(name);
            }
        }
        if(!foundSlot){
            for(int i = 0; i < slots.Length; i++){
                if(slots[i].IsEmpty()){
                    if(!slots[i].AddItem(name, count, sprite, action)){
                        foundSlot = true;
                        break;
                    } else{
                        count -= 32;
                        if(count <= 0) {
                            foundSlot = true;
                            break;
                        }
                    }
                } else if(!slots[i].IsFull() && slots[i].GetName() == name){
                    int remainder = slots[i].AddCount(count);
                    if(remainder == 0){
                        foundSlot = true;
                        break;
                    } else{
                        count = remainder;
                    }
                    
                }
            }
        }
        
        if(!foundSlot){
            world.QueueNotification("Inventory is Full!", 3.0f);
        } else world.QueueNotification("Obtained "+originalCount+" of "+name, 3.0f);
        return foundSlot;
    }

    private void UnlockRecipe(string name) {recipes[recipeIDs[name]].Unlock();}
    public int RemainingLocks(string name) {return recipes[recipeIDs[name]].remainingLocks;}
    public void RemoveLock (string name) {
        if(recipes[recipeIDs[name]].remainingLocks == 1) UnlockRecipe(name);
        recipes[recipeIDs[name]].remainingLocks--;
    }

    public void DescribeItem(ItemSlot item){
        selectedImage.enabled = true;
        selectedImage.sprite = item.GetSprite();
        selectedDescription.text = world.GetDescription(item.GetName());
        selectedName.text = item.GetName();
        selectedItem = item.id;
        selectedRecipe = -1;
        transform.GetChild(0).GetChild(1).GetChild(4).GetComponent<InventoryActionBtn>().action = item.GetAction();
        string actName = item.GetAction().actionName;
        if(actName == "Equip" && item.id == equipped) actName = "Unequip";
        transform.GetChild(0).GetChild(1).GetChild(4).GetComponent<InventoryActionBtn>().ChangeLabel(actName);
    }
    public void DescribeCraft(RecipeSlot recipe){
        selectedImage.enabled = true;
        selectedImage.sprite = recipe.GetSprite();
        string recipeName = (recipe.locked ? "Unknown" : recipe.GetName());
        selectedDescription.text = world.GetDescription(recipeName)+"\n\n"+CostString(world.GetCosts(recipeName));
        selectedName.text = recipeName;
        selectedRecipe = recipe.id;
        selectedItem = -1;
        transform.GetChild(0).GetChild(1).GetChild(3).gameObject.SetActive(false);
        transform.GetChild(0).GetChild(1).GetChild(4).GetComponent<InventoryActionBtn>().ChangeLabel("Craft");
        if(!recipe.locked) transform.GetChild(0).GetChild(1).GetChild(4).GetComponent<InventoryActionBtn>().action = craftingAction;
    }

    public string CostString((string, int)[] costs){
        if(costs == null) return "";
        string result = "Costs ";
        int curr = 0;
        foreach((string a, int b) in costs){
            if(curr+1 == costs.Length && curr != 0) result += "and ";
            result += b+" "+a+", ";
            curr++;
        }
        return result;
    }

    public void DeselectLastSlot(){
        for(int i = 0; i < slots.Length; i++){
            if(slots[i].highlighted){
                slots[i].highlighted = false;
                slots[i].highlight.SetActive(false);
                break;
            }
        }
        for(int i = 0; i < recipes.Length; i++){
            if(recipes[i].highlighted){
                recipes[i].highlighted = false;
                recipes[i].highlight.SetActive(false);
                break;
            }
        }
        selectedImage.enabled = false;
        selectedDescription.text = "";
        selectedName.text = "";
        selectedItem = -1;
        selectedRecipe = -1;
        errorDisplay.transform.gameObject.SetActive(false);

        transform.GetChild(0).GetChild(1).GetChild(3).GetComponent<InventoryActionBtn>().EndHighlight();
        transform.GetChild(0).GetChild(1).GetChild(3).gameObject.SetActive(true);
        transform.GetChild(0).GetChild(1).GetChild(4).GetComponent<InventoryActionBtn>().EndHighlight();
        transform.GetChild(0).GetChild(1).GetChild(4).GetComponent<InventoryActionBtn>().ChangeLabel("Use");
    }

    public void ConsumeItem(string name, int count){
        for(int i = 0; i < slots.Length; i++){
            if(slots[i].GetName() == name){
                count = -slots[i].AddCount(-count);
                if(count == 0) return;
            }
        }
    }

    public int CountItem(string name){
        int total = 0;
        for(int i = 0; i < slots.Length; i++){
            if(slots[i].GetName() == name){
                total += slots[i].GetCount();
            }
        }
        return total;
    }

    public int FindExistingSlot(string name){
        for(int i = 0; i < slots.Length; i++){
            if(slots[i].GetName() == name && slots[i].GetCount() < 32) return i;
        }
        return -1;
    }

    public void DeleteItem() {
        // print("Before: "+slots[selectedItem].GetCount());
        slots[selectedItem].AddCount(-slots[selectedItem].GetCount());
        // print("After: "+slots[selectedItem].GetCount());
        DeselectLastSlot();
    }

    public void DisplayError(string error){
        errorDisplay.text = error;
        errorDisplay.transform.gameObject.SetActive(true);
        // StartCoroutine(waiter());
    }

    public ItemSlot GetSelected() {return ((selectedItem != -1)?(slots[selectedItem]) : null);}
    public RecipeSlot GetSelectedR() {return (selectedRecipe != -1 ? recipes[selectedRecipe] : null);}
    public World GetWorld() {return world;}
    public bool InventoryActive() {return inventoryUp;}
    public string GetPlayerTool() {return ((equipped != -1)?slots[equipped].GetName():null);}
    public void EquipSelected() {
        world.PlayerEquip(equipped != selectedItem, GetSelected().GetName());
        equipped = ((equipped == selectedItem)?-1:selectedItem);
    }

    /*IEnumerator waiter(){
        yield return new WaitForSeconds(5);
        errorDisplay.transform.gameObject.SetActive(false);
    }*/
}
