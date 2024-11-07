using System.Collections;
using System.Collections.Generic;
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
    private bool collected = false;
    void Start()
    {
        inventory = inventoryObject.GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        if(other.tag == "Player" && !collected){
            collected = true;
            if(inventory.AddItem(itemName, count, sprite)){
                Destroy(gameObject);
                Transform player = other.transform;
                while(player.gameObject.name != "Player"){
                    player = player.transform.parent;
                }
                PlayerHP script = player.GetComponent<PlayerHP>();
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
