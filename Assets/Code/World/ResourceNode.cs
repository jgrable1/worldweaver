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
    private char requiredTool;


    void Start(){

    }

    void OnTriggerEnter(Collider collisionInfo) {
        print("Collision Enter Called on "+collisionInfo.gameObject.tag);
        //ContactPoint[collisionInfo.contactCount] collisions
        /*for(int i = 0; i < collisionInfo.contactCount; i++){
            print("Point # "+i+": "+collisionInfo.GetContact(i).point);
        }
        print("Points colliding: " + collisionInfo.GetContacts(new ContactPoint[collisionInfo.contactCount]).ToString());
        print("First point that collided: " + collisionInfo.GetContact(0).point);*/
        if (collisionInfo.gameObject.tag == "PlayerTool") HandleToolCollision();
    }

    public void HandleToolCollision(){
        string action = world.GetPlayerAction();
        print(action);
        print(action.Substring(action.IndexOf(" ")+1)[0]);
        if(action != null && action.Substring(action.IndexOf(" ")+1)[0] == requiredTool){
            resource.AddItemBypass(world.GetInventory(), ModifyCount(action.Substring(0, action.IndexOf(" "))));
        }
    }



    private int ModifyCount(string toolTier){
        if(toolTier == "Wood") return 1;
        else if(toolTier == "Stone") return 2;
        else return 3;
    }
}