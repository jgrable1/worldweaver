using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MeleeEnemyMove : MonoBehaviour
{
    private int currHP;
    private Vector3 v;
    private bool finalBoss, onHitCooldown = false;
    private Rigidbody enemyBody;
    private GameObject PlayerObject;
    private NavMeshAgent agent;

    private void Awake(){
        enemyBody = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        finalBoss = this.transform.tag == "Final Boss";
        currHP = finalBoss? 21:5;
    }

    void Update(){
        agent.destination = PlayerObject.transform.position;
    }

    public void UpdateEnemyHealth(int HP){
        if(Input.GetMouseButtonDown(0) && !onHitCooldown){
            onHitCooldown = true;
            v = ((PlayerObject.transform.position - this.transform.position).normalized);
            v.y = -0.5f;
            enemyBody.AddForce(-v*500, ForceMode.Acceleration);
            currHP -= HP;
            StartCoroutine(IFrames());
        }
        if(currHP <= 0){
            Destroy(gameObject);
            if(finalBoss){
                SceneManager.LoadScene("WinScreen");
            }
        }
    }

    void OnTriggerStay(Collider other) {
        if (other.gameObject.name == "Wooden Sword") UpdateEnemyHealth(1);
        else if (other.gameObject.name == "Stone Sword") UpdateEnemyHealth(2);
        else if (other.gameObject.name == "Iron Sword") UpdateEnemyHealth(3);
    }

    IEnumerator IFrames(){
        yield return new WaitForSeconds(0.75f); // Cooldown damage of one second.
        onHitCooldown = false;
    }
}