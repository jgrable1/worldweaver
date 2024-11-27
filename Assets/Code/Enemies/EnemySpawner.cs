using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    private bool first = true, finalBoss;
    void OnTriggerEnter(Collider other){
        if(other.tag == "Player" && first){
            first = false;
            finalBoss = enemy.transform.tag == "Final Boss";
            Transform player = other.transform;
            while(player.gameObject.name != "Player"){
                player = player.transform.parent;
            }
            Vector3 position = this.transform.position;
            if(!finalBoss){
                position.y += 1;
                position.x -= 2;
                Instantiate(enemy, position, Quaternion.identity, null);
                position.x += 4;
                Instantiate(enemy, position, Quaternion.identity, null);
                position.x -= 2;
                position.z += 2;
                Instantiate(enemy, position, Quaternion.identity, null);
                position.z -= 4;
                Instantiate(enemy, position, Quaternion.identity, null);
            } else {
                position.y += 5;
                position.x -= 4;
                Instantiate(enemy, position, Quaternion.identity, null);
            }
        } 
    }
}
