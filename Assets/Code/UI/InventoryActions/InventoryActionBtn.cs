using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class InventoryActionBtn : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField]
    private Inventory inventory;
    private GameObject highlight;
    [SerializeField]
    private TMP_Text label;
    [SerializeField]
    private bool allowRecipe;
    public bool highlighted;
    public InventoryAction action;

    // Start is called before the first frame update
    void Start(){
        highlight = transform.GetChild(0).gameObject;
    }

    void Update(){
    }

    public void EndHighlight() {
        highlighted = false;
        highlight.SetActive(false);
    }
    
    public void ChangeLabel(string s) {label.text = s;}
    private bool CanInteract() {
        if(action == null) return false;
        if(!allowRecipe && (inventory.GetSelectedR() != null || inventory.GetSelected() == null)) return false;
        if(allowRecipe && (inventory.GetSelected() == null && inventory.GetSelectedR() == null)) return false;
        // print(inventory.GetSelected());
        // if(inventory.GetSelected().IsEmpty()) return false;
        return true;
    }

    public void OnPointerClick(PointerEventData eventData){
        if(eventData.button == PointerEventData.InputButton.Left){
            if(CanInteract()) action.InventoryAct(inventory);
        }
    }

    public void OnPointerEnter(PointerEventData eventData){
        if(CanInteract()){
            highlighted = true;
            highlight.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData){
        if(CanInteract()){
            highlighted = false;
            highlight.SetActive(false);
        }
    }
}
