using System;
using System.Collections;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    [SerializeField]
    private BasicItem resource;
    [SerializeField]
    private World world;
    [SerializeField]
    private char requiredTool; // A for Axe, P for Pickaxe, S for Sword
    [SerializeField]
    private int minTier = 1; // 1 for Wood, 2 for Stone, 3 for Iron
    private bool canHarvest = true;
    private bool weakToolNotif = true;


    void Start(){

    }

    void OnTriggerEnter(Collider collisionInfo) {
        // print("Collision Enter Called on "+collisionInfo.gameObject.tag);
        if (collisionInfo.gameObject.tag == "PlayerTool" && canHarvest) HandleToolCollision();
    }

    void OnTriggerStay(Collider collisionInfo) {
        if (collisionInfo.gameObject.tag == "PlayerTool" && canHarvest) HandleToolCollision();
    }

    public void HandleToolCollision(){
        string action = world.GetPlayerAction();
        if(action != null && action.Substring(action.IndexOf(" ")+1)[0] == requiredTool){
            if(GetTier(action) < minTier && weakToolNotif){
                world.QueueNotification("You need at least a"+(requiredTool == 'I'?"n ":" ")+MinToolString()+" to harvest this resource!", 2f);
                weakToolNotif = false;
                StartCoroutine(NotifWaiter());
            } else if(weakToolNotif) {
                resource.AddItemBypass(world.GetInventory(), GetTier(action)-(minTier-1));
                canHarvest = false;
                StartCoroutine(HarvestWaiter());
            }
        }
    }

    private int GetTier(string tool){
        if(tool[0] == 'I') return 3;
        if(tool[0] == 'S') return 2;
        else return 1;
    }

    private string MinToolString(){
        string result = "";
        if(minTier == 1) result += "Wooden ";
        else if(minTier == 2) result += "Stone ";
        else result += "Iron ";
        
        if(requiredTool == 'A') result += "Axe";
        else if(requiredTool == 'P') result += "Pickaxe";
        else if(requiredTool == 'S') result += "Sword";

        return result;
    }

    IEnumerator HarvestWaiter() {yield return new WaitForSeconds(0.75f); canHarvest = true;}
    IEnumerator NotifWaiter() {yield return new WaitForSeconds(5f); weakToolNotif = true;}

}