using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TemplateNPC : MonoBehaviour
{

    protected string[] dialogue;
    protected TextMeshPro tmp;
    protected int interactStage = 0;
    protected bool interactable = false;
    protected int walkSpeed = 2;
    protected float walkWait = 1f;
    private float walkTimer = 0f;
    private bool walking = false;
    // WalkBounds: [x1, x2, z1, z2] x1 < x2, z1 < z2
    protected float[] walkBounds;
    private Vector3 randomMove;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponentInChildren<TextMeshPro>();
        //player = GameObject.FindWithTag("Player");
    }

    void DisplayDialogue(string s, bool cont){
        tmp.text = s+(cont?"\n\n Press E to Continue":"");
    }

    // Update is called once per frame
    void Update()
    {  
        if(player == null){
            print("Trying again to find Player");
            player = GameObject.FindWithTag("Player");
        }
        if(interactStage == 0){
            if(walking){
                transform.position += randomMove*Time.deltaTime;
            }
            walkTimer+=Time.deltaTime;
            Vector3 playerDir = player.transform.position-transform.position;
            playerDir.y = 0;
            tmp.transform.rotation = Quaternion.LookRotation(-playerDir);
        }
        

        if (Input.GetButtonDown("Interact") && interactable){
            interactStage++;
            if(interactStage >= dialogue.Length) interactStage = 0;
            DisplayDialogue(dialogue[interactStage], interactStage != 0);
            Vector3 playerDir = player.transform.position-transform.position;
            playerDir.y = 0;
            transform.rotation = Quaternion.LookRotation(-playerDir);
            tmp.transform.rotation = Quaternion.LookRotation(-playerDir);
        }

        if(walkTimer >= walkWait){
            walking = !walking;
            walkTimer = 0;
            if(walking){
                randomMove = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)).normalized;
                randomMove*=walkSpeed;
                float clampX = Mathf.Clamp(transform.position.x+randomMove.x, walkBounds[0], walkBounds[1]);
                if(clampX == walkBounds[0]) randomMove.x = transform.position.x-walkBounds[0];
                else if(clampX == walkBounds[1]) randomMove.x = walkBounds[1]-transform.position.x;

                float clampZ = Mathf.Clamp(transform.position.z+randomMove.z, walkBounds[2], walkBounds[3]);
                if(clampZ == walkBounds[2]) randomMove.z = transform.position.z-walkBounds[2];
                else if(clampZ == walkBounds[3]) randomMove.z = walkBounds[3]-transform.position.z;
                transform.rotation = Quaternion.LookRotation(randomMove);
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        //print("onTriggerEnter called");
        interactable = true;
        if(interactStage == 0) DisplayDialogue(dialogue[interactStage], interactStage != 0);
    }
    void OnTriggerExit(Collider other){
        interactable = false;
        if(interactStage == 0) tmp.text = "";
    }
}
