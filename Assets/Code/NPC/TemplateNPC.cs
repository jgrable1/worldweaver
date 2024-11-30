using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TemplateNPC : MonoBehaviour
{

    protected string[] dialogue;
    protected TextMeshPro tmp;
    protected int interactStage = 0;
    protected bool interactable = false, npcTurn, textTurn;
    protected int walkSpeed = 2;
    protected float walkWait = 1f;
    private float walkTimer = 0f;
    private bool walking = false;
    // WalkBounds: [x1, x2, z1, z2] x1 < x2, z1 < z2
    protected float[] walkBounds;
    protected System.Action[] methods;
    private Vector3 randomMove;
    [SerializeField]
    protected GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        npcTurn = true;
        textTurn = true;
        tmp = GetComponentInChildren<TextMeshPro>(); 
    }

    void DisplayDialogue(string s, bool cont, System.Action method){
        tmp.text = s+(cont?"\n Press E to Continue":"");
        if(method != null) method();
    }

    // Update is called once per frame
    void Update(){
        if(player == null){
            player = GameObject.FindWithTag("Player");
        }
        Vector3 playerDir = transform.position-player.transform.position;
        if(interactStage == 0){
            if(walking){
                transform.position += randomMove*Time.deltaTime;
            }
            walkTimer+=Time.deltaTime;
            //Vector3 playerDir = player.transform.position-transform.position;
            //playerDir.y = 0;
            if(textTurn) tmp.transform.rotation = Quaternion.LookRotation(playerDir);
        }

        if (Input.GetButtonDown("Interact") && interactable){
            interactStage++;
            if(interactStage >= dialogue.Length) interactStage = 0;
            DisplayDialogue(dialogue[interactStage], interactStage != 0, (methods != null?methods[interactStage]:null));
            Vector3 tempPlayerDir = playerDir;
            tempPlayerDir.y = 0;
            if(npcTurn) transform.rotation = Quaternion.LookRotation(tempPlayerDir);
            if(textTurn) tmp.transform.rotation = Quaternion.LookRotation(playerDir);
        }

        if(walkTimer >= walkWait && walkBounds != null){
            walking = !walking;
            walkTimer = 0;
            if(walking){
                randomMove = new Vector3(UnityEngine.Random.Range(-1, 1), 0, UnityEngine.Random.Range(-1, 1)).normalized;
                randomMove*=walkSpeed;
                
                float clampX = Mathf.Clamp(transform.position.x+randomMove.x, walkBounds[0], walkBounds[1]);
                if(clampX == walkBounds[0]){
                    if(transform.position.x < walkBounds[0]) randomMove.x = walkBounds[0] + 0.5f;
                    else randomMove.x = transform.position.x-walkBounds[0];
                } else if(clampX == walkBounds[1]){
                    if(transform.position.x > walkBounds[1]) randomMove.x = walkBounds[1] - 0.5f;
                    else randomMove.x = transform.position.x-walkBounds[1];
                }

                float clampZ = Mathf.Clamp(transform.position.z+randomMove.z, walkBounds[2], walkBounds[3]);
                if(clampZ == walkBounds[2]){
                    if(transform.position.z < walkBounds[2]) randomMove.z = walkBounds[2] + 0.5f;
                    else randomMove.z = transform.position.z-walkBounds[2];
                } else if(clampZ == walkBounds[3]){
                    if(transform.position.z > walkBounds[3]) randomMove.z = walkBounds[3] - 0.5f;
                    else randomMove.z = transform.position.z-walkBounds[3];
                }

                if(npcTurn) transform.rotation = Quaternion.LookRotation(randomMove);
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        //print("onTriggerEnter called");
        interactable = true;
        if(interactStage == 0) DisplayDialogue(dialogue[interactStage], interactStage != 0, (methods != null?methods[interactStage]:null));
    }
    void OnTriggerExit(Collider other){
        interactable = false;
        if(interactStage == 0) tmp.text = "";
    }
}
