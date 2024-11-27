using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyMove : MonoBehaviour
{
    private int currHP, speed;
    private float chaseCooldown;
    private Vector3 v;
    private bool finalBoss, onHitCooldown = false;
    private Rigidbody enemyBody;
    private GameObject PlayerObject;

    private void Awake(){
        enemyBody = GetComponent<Rigidbody>();
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        finalBoss = this.transform.tag == "Final Boss";
        speed = finalBoss? 1000:300;
        currHP = finalBoss? 21:5;
        chaseCooldown = finalBoss? 1:0.2f;
        StartCoroutine(Chase());
    }

    IEnumerator Chase(){
        v = ((PlayerObject.transform.position - this.transform.position).normalized);
        if(this.transform.position.y > 1) enemyBody.drag = 5;
        else enemyBody.drag = 5;
        v.y = 0;
        enemyBody.AddForce(v*speed, ForceMode.Acceleration);
        enemyBody.transform.rotation = Quaternion.LookRotation(v);
        yield return new WaitForSeconds(chaseCooldown); // Rechecks player position after every cooldown.
        StartCoroutine(Chase());
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