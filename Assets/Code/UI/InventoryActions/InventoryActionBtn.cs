using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryActionBtn : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField]
    private Inventory inventory;
    private GameObject highlight;
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

    public void OnPointerClick(PointerEventData eventData){
        if(eventData.button == PointerEventData.InputButton.Left){
            if(action != null && inventory.GetSelected() != null) action.InventoryAct(inventory);
        }
    }

    public void OnPointerEnter(PointerEventData eventData){
        if(action != null && inventory.GetSelected() != null){
            highlighted = true;
            highlight.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData){
        if(action != null && inventory.GetSelected() != null){
            highlighted = false;
            highlight.SetActive(false);
        }
    }
}
