using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolCollider : MonoBehaviour
{
    void OnCollisionEnter(Collision collisionInfo) {
        print("Collision Enter Called on "+collisionInfo.gameObject.name);
        if (collisionInfo.gameObject.tag == "Resource") {
            collisionInfo.gameObject.GetComponent<ResourceNode>().HandleToolCollision();
        } else if (false){
            
        }
    }
}
