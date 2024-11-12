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
    public GameObject inventoryObject;
    [SerializeField]
    public int heal;
    [SerializeField]
    private Sprite sprite;
    private Inventory inventory;
    private bool collected = true;
    [SerializeField]
    public GameObject prefab;
    [SerializeField]
    public InventoryAction action;
    void Start()
    {
        //prefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(transform.gameObject);
        if(prefab != null) print("Found prefab: "+prefab.name);
        //else print("Didn't find prefab");

        if(inventoryObject != null) inventory = inventoryObject.GetComponent<Inventory>();
        StartCoroutine(waiter());
    }

    // Update is called once per frame
    void Update()
    {
        /*if(prefab == null){
            print("Trying again to find prefab");
            prefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(transform.gameObject);
        } else print("Found prefab: "+prefab.name);*/
    }

    public void SetInventoryRef(Inventory inventory) {this.inventory = inventory;}

    void OnTriggerEnter(Collider other){
        if(other.tag == "Player" && !collected){
            collected = true;
            if(inventory.AddItem(itemName, count, sprite, prefab, action)){
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
