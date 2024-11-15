using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BasicItem : MonoBehaviour
{
    [SerializeField]
    private string itemName;
    [SerializeField]
    private int count;
    [SerializeField]
    private Inventory inventory;
    [SerializeField]
    public int heal;
    [SerializeField]
    private Sprite sprite;
    private bool collected = true;
    [SerializeField]
    public InventoryAction action;

    void Start(){
        StartCoroutine(waiter());
    }

    public void SetInventoryRef(Inventory inventory) {this.inventory = inventory;}

    void OnTriggerEnter(Collider other){
        if(other.tag == "Player" && !collected){
            collected = true;
            if(inventory.AddItem(itemName, count, sprite, action)){
                Destroy(gameObject);
                Transform player = other.transform;
                while(player.gameObject.name != "Player"){
                    player = player.transform.parent;
                }
                PlayerStats script = player.GetComponent<PlayerStats>();
                script.PlayerHPUpdate(heal);
            }
            else StartCoroutine(waiter());
        }
    }

    IEnumerator waiter(){
        yield return new WaitForSeconds(3);
        collected = false;
    }
}
