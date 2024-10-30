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

    public GameObject highlight;
    public bool highlighted;

    void Start() {
        inventory = transform.parent.parent.parent.gameObject.GetComponent<Inventory>();
        highlight = transform.GetChild(0).gameObject;
        if(itemName == null){
            countText.enabled = false;
            spriteV.enabled = false;
        }
    }

    public bool AddItem(string name, int count, Sprite sprite){
        this.itemName = name;
        this.count = count;
        this.sprite = sprite;
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
    }

    public bool IsEmpty() {return empty;}
    public bool IsFull() {return full;}
    public string GetName() {return itemName;}

    public void OnPointerClick(PointerEventData eventData){
        if(eventData.button == PointerEventData.InputButton.Left){
            inventory.DeselectLastSlot();
            highlighted = true;
            highlight.SetActive(true);
            if(!empty) inventory.DescribeItem(itemName, sprite);
        }
        if(eventData.button == PointerEventData.InputButton.Right){

        }

    }
}
