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

    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponentInChildren<TextMeshPro>();
    }

    void DisplayDialogue(string s, bool cont){
        tmp.text = s+(cont?"\n\n Press E to Continue":"");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact") && interactable){
            interactStage++;
            if(interactStage >= dialogue.Length) interactStage = 0;
            DisplayDialogue(dialogue[interactStage], interactStage != 0);
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
