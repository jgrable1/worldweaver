using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyMove : MonoBehaviour
{
    public int currHP;
    Rigidbody RigidbodyComponent;
    public GameObject PlayerObject;
    /*private void Awake(){
        RigidbodyComponent = GetComponent<Rigidbody>();
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
    }*/
    private void Start(){
        RigidbodyComponent = GetComponent<Rigidbody>();
        StartCoroutine(Chase());
    }

    IEnumerator Chase(){
        Vector3 v = ((PlayerObject.transform.position - this.transform.position).normalized)*5;
        v.y = 0;
        RigidbodyComponent.AddForce(v*30, ForceMode.Acceleration);
        RigidbodyComponent.transform.rotation = Quaternion.LookRotation(v);
        yield return new WaitForSeconds(0.2f); // Rechecks player position every second.
        StartCoroutine(Chase());
    }

    public void UpdateEnemyHealth(int HP){
        if(Input.GetKeyDown("b")){
            currHP -= HP;
        }
        if(currHP == 0){
            Destroy(gameObject);
        }
    }

    private void Update(){
        UpdateEnemyHealth(1);
    }
}
