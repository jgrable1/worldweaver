using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    private bool first = true;

    void OnTriggerEnter(Collider other){
        if(other.tag == "Player" && first){
            first = false;
            Transform player = other.transform;
            while(player.gameObject.name != "Player"){
                player = player.transform.parent;
            }
            Vector3 position = this.transform.position;
            position.x -= 2;
            position.y = 1; 
            Instantiate(enemy, position, Quaternion.identity, null);
            position.x += 4;
            Instantiate(enemy, position, Quaternion.identity, null);
            position.x -= 2;
            position.z += 2;
            Instantiate(enemy, position, Quaternion.identity, null);
            position.z -= 4;
            Instantiate(enemy, position, Quaternion.identity, null);
        } 
    }
}
