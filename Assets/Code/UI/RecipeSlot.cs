using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RecipeSlot : MonoBehaviour, IPointerClickHandler
{

    [SerializeField]
    private bool empty;
    [SerializeField]
    private string itemName;
    [SerializeField]
    private int produceCount;
    [SerializeField]
    private Sprite sprite;

    [SerializeField]
    public Image spriteV;
    [SerializeField]
    private string[] costs;
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private InventoryAction action;
    private Inventory inventory;

    public GameObject highlight;
    public bool highlighted;
    public int id;

    void Start() {
        inventory = transform.parent.parent.parent.parent.gameObject.GetComponent<Inventory>();
        highlight = transform.GetChild(0).gameObject;
        spriteV.sprite = sprite;
    }

    public string GetName() {return itemName;}
    public int GetProducedCount() {return produceCount;}
    public Sprite GetSprite() {return sprite;}
    public GameObject GetPrefab() {return prefab;}
    public InventoryAction GetAction() {return action;}
    public string[] GetCosts() {return costs;}

    public void OnPointerClick(PointerEventData eventData){
        if(eventData.button == PointerEventData.InputButton.Left){
            inventory.DeselectLastSlot();
            highlighted = true;
            highlight.SetActive(true);
            if(!empty) inventory.DescribeCraft(this);
        }
        if(eventData.button == PointerEventData.InputButton.Right){

        }

    }
}
