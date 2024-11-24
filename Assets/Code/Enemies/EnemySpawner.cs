using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    void Update()
    {
        if(Input.GetKeyDown("o")){
            Instantiate(enemy, this.transform.position, Quaternion.identity, null);
        }
    }
}
