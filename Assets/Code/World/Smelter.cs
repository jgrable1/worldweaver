using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smelter : Structure
{
    [SerializeField]
    private World world;
    void OnTriggerEnter(Collider collisionInfo) {
        // Debug.Log("Trigger entered");
        if(collisionInfo.tag == "Player") world.canSmelt = true;
        // if (collisionInfo.tag == "Player") Debug.Log("Setting canSmelt to True");
    }

    void OnTriggerExit(Collider collisionInfo) {
        if(collisionInfo.tag == "Player") world.canSmelt = false;
    }

    public override void Setup(Inventory inventory){
        this.world = inventory.GetWorld();
    }
}
