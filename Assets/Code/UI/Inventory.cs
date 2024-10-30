using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Inventory : MonoBehaviour
{

    public GameObject inventory;
    bool inventoryUp;
    public GameObject worldObject;
    private World world;
    public ItemSlot[] slots;
    [SerializeField]
    private TMP_Text selectedDescription, selectedName;
    [SerializeField]
    private Image selectedImage;

    private Dictionary<string, string> descriptions = new Dictionary<string, string>();
    void Start()
    {
        world = worldObject.GetComponent<World>();
        descriptions.Add("The Sphere", "Ancient artifact of Sir Ligma the 45th, grants 0.000005 additional speed.\n\n Try to prove me wrong.");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Inventory")) Activate(!inventoryUp);
    }

    void Activate(bool b){
        print("Activate called");
        if(!b) DeselectLastSlot();
        inventoryUp = b;
        inventory.SetActive(b);
        world.RestrictMovement(b, "Inventory");
        world.RestrictCamera(b, "Inventory");
        Time.timeScale = (b ? 0 : 1);
    }

    public bool AddItem(string name, int count, Sprite sprite){
        bool foundSlot = false;
        int originalCount = count;
        for(int i = 0; i < slots.Length; i++){
            if(slots[i].IsEmpty()){
                if(!slots[i].AddItem(name, count, sprite)){
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
        if(!foundSlot){
            world.SendNotification("Inventory is Full!", 3.0f);
        } else world.SendNotification("Obtained "+originalCount+" of "+name, 3.0f);
        return foundSlot;
    }

    public void DescribeItem(string name, Sprite sprite){
        selectedImage.enabled = true;
        selectedImage.sprite = sprite;
        selectedDescription.text = descriptions[name];
        selectedName.text = name;
    }
    public void DeselectLastSlot(){
        for(int i = 0; i < slots.Length; i++){
            if(slots[i].highlighted){
                slots[i].highlighted = false;
                slots[i].highlight.SetActive(false);
                break;
            }
        }
        selectedImage.enabled = false;
        selectedDescription.text = "";
        selectedName.text = "";
    }
}
