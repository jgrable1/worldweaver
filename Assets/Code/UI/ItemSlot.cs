using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    private string itemName;
    private int count;
    private Sprite sprite;
    private bool empty = true;
    private bool full = false;

    [SerializeField]
    private TMP_Text countText;
    [SerializeField]
    public Image spriteV;
    private Inventory inventory;
    private GameObject prefab;
    private InventoryAction action;

    public GameObject highlight;
    public bool highlighted;
    public int id;

    void Start() {
        inventory = transform.parent.parent.parent.gameObject.GetComponent<Inventory>();
        highlight = transform.GetChild(0).gameObject;
        if(itemName == null){
            countText.enabled = false;
            spriteV.enabled = false;
        }
    }

    public bool AddItem(string name, int count, Sprite sprite, GameObject prefab, InventoryAction action){
        this.itemName = name;
        this.count = count;
        this.sprite = sprite;
        this.prefab = prefab;
        this.action = action;
        this.empty = false;
        if(count > 32){
            count = 32;
            full = true;
        }
        countText.text = count.ToString();
        countText.enabled = true;
        spriteV.sprite = sprite;
        spriteV.enabled = true;
        return full;
    }

    public int AddCount(int count){
        if(count >= 0){
            if(this.count+count <= 32){
                if(this.count+count == 32) full = true;
                this.count+=count;
                countText.text = this.count.ToString();
                return 0;
            } else {
                count -= (32-this.count);
                this.count = 32;
                countText.text = this.count.ToString();
                return count;
            }
        } else{
            if(this.count+count <= 0){
                itemName = null;
                full = false;
                empty = true;
                int remainder = this.count+count;
                this.count = 0;
                this.sprite = null;
                this.prefab = null;
                countText.text = "";
                countText.enabled = false;
                spriteV.sprite = this.sprite;
                spriteV.enabled = false;
                return -remainder;
            } else{
                this.count += count;
                countText.text = this.count.ToString();
                return 0;
            }
        }
    }

    public bool IsEmpty() {return empty;}
    public bool IsFull() {return full;}
    public string GetName() {return itemName;}
    public Sprite GetSprite() {return sprite;}
    public GameObject GetPrefab() {return prefab;}
    public InventoryAction GetAction() {return action;}
    public int GetCount() {return count;}

    public void OnPointerClick(PointerEventData eventData){
        if(eventData.button == PointerEventData.InputButton.Left){
            inventory.DeselectLastSlot();
            highlighted = true;
            highlight.SetActive(true);
            if(!empty) inventory.DescribeItem(this);
        }
        if(eventData.button == PointerEventData.InputButton.Right){

        }

    }
}
