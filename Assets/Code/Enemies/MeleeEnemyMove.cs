using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyMove : MonoBehaviour
{
    public int currHP, speed = 5;
    private Vector3 v;
    private bool onCooldown = false;
    Rigidbody enemyBody;
    GameObject PlayerObject;

    private void Awake(){
        enemyBody = GetComponent<Rigidbody>();
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(Chase());
    }

    IEnumerator Chase(){
        v = ((PlayerObject.transform.position - this.transform.position).normalized);
        if(v.y <= 0) enemyBody.drag = speed;
        else enemyBody.drag = speed/5;
        enemyBody.AddForce(v * 150, ForceMode.Acceleration);
        enemyBody.transform.rotation = Quaternion.LookRotation(v);
        yield return new WaitForSeconds(0.2f); // Rechecks player position every 0.2 seconds.
        StartCoroutine(Chase());
    }

    public void UpdateEnemyHealth(int HP){
        if(Input.GetMouseButtonDown(0) && !onCooldown){
            onCooldown = true;
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
        onCooldown = false;
    }
}