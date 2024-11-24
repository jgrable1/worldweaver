using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildDetection : MonoBehaviour
{
    [SerializeField]
    private World world;
    int colliding = 0;
    bool timerRunning, timerCheck;
    void OnTriggerEnter(Collider collider){
        // print(collider.name+" currently blocking build");
        if(collider.tag != "PlayerTool") colliding++;
        world.canBuild = false;
    }

    void OnTriggerStay(Collider collider){
        if(timerRunning && timerCheck){
            timerCheck = false;
        }
    }

    void Update(){
        if(!timerRunning){
            timerRunning = true;
            timerCheck = true;
            StartCoroutine(waiter());
        }
    }

    void OnTriggerExit(Collider collider){
        // print(collider.name+" no longer blocking build");
        if(collider.tag != "PlayerTool") colliding--;
        if(colliding <= 0){
            colliding = 0;
            world.canBuild = true;
        }
    }

    IEnumerator waiter() {
        yield return new WaitForSeconds(0.5f);
        if(timerCheck){
            colliding = 0;
            world.canBuild = true;
        }
        timerRunning = false;

    }
}
