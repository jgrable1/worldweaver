using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    [SerializeField]
    private BasicItem resource;
    [SerializeField]
    private World world;
    [SerializeField]
    private char requiredTool; // A for Axe, P for Pickaxe, S for Sword


    void Start(){

    }

    void OnTriggerEnter(Collider collisionInfo) {
        // print("Collision Enter Called on "+collisionInfo.gameObject.tag);
        if (collisionInfo.gameObject.tag == "PlayerTool") HandleToolCollision();
    }

    public void HandleToolCollision(){
        string action = world.GetPlayerAction();
        if(action != null && action.Substring(action.IndexOf(" ")+1)[0] == requiredTool){
            resource.AddItemBypass(world.GetInventory(), ModifyCount(action.Substring(0, action.IndexOf(" "))));
        }
    }



    private int ModifyCount(string toolTier){
        // print(toolTier);
        if(toolTier == "Wooden") return 1;
        else if(toolTier == "Stone") return 2;
        else return 3;
    }
}