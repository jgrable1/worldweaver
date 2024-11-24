using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smelter : Structure
{
    [SerializeField]
    private World world;
    void OnTriggerEnter(Collider collisionInfo) {
        if(collisionInfo.tag == "Player") world.canSmelt = true;
    }

    void OnTriggerExit(Collider collisionInfo) {
        if(collisionInfo.tag == "Player") world.canSmelt = false;
    }

    public override void Setup(Inventory inventory){
        this.world = inventory.GetWorld();
    }
}
