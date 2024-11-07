using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class World : MonoBehaviour
{
    private bool canMove, canLook, notifUse, notifOpen, staminaUse, shownStaminaTip;
    private List<string> moveRestrictor, lookRestrictor;
    private List<(string, float)> notificationQueue;
    [SerializeField]
    private TMP_Text notificationText;
    [SerializeField]
    private TMP_Text instructions;
    private float notifWait, timer, staminaConsumption;
    
    void Start()
    {
        staminaConsumption = -0.5f;
        canMove = true;
        canLook = true;
        staminaUse = true;
        shownStaminaTip = false;
        moveRestrictor = new List<string>();
        lookRestrictor = new List<string>();
        notificationQueue = new List<(string, float)>();
        //notificationText.transform.position = new Vector3((Screen.width/2)+200, (-Screen.height/2)+50, 0);
        //instructions.transform.position = new Vector3((-Screen.width/2), (Screen.height/2), 0);
    }

    void Update(){
        if(notifUse){
            if(notifOpen){
                if(timer <= notifWait){
                    //print(notificationText.transform.position.x);
                    if(notificationText.transform.position.x > (Screen.width)-(Screen.width/8)){
                        notificationText.transform.Translate(new Vector3(-(Screen.width/4), 0, 0)*Time.deltaTime);
                    } else{
                        //notificationText.transform.position.x = 220;
                        timer += Time.deltaTime;
                    }
                } else notifOpen = false;
            } else{
                if(notificationText.transform.position.x < (Screen.width)+(Screen.width/8)){
                        notificationText.transform.Translate(new Vector3((Screen.width/4), 0, 0)*Time.deltaTime);
                } else{
                    //notificationText.transform.position.x = 420;
                    notifUse = false;
                }
            }
        } else{
            if(notificationQueue.Count > 0) SendNextNotification();
        }
    }

    public void LockStamina(bool restrict){
        staminaUse = !restrict;
        if(!staminaUse){
            staminaConsumption = -1f;
            if(!shownStaminaTip) QueueNotification("You ran out of stamina! You'll need to recover a little bit before you can use anymore.", 2.0f);
        }
    }
    public void ChangeStaminaConsumption(float change){staminaConsumption += change;}

    public void RestrictMovement(bool restrict, string source){
        print(source+ " "+(restrict?"dis":"en")+"abling movement.");
        if(canMove && restrict){
            canMove = false;
            moveRestrictor.Add(source);
        } else if(!canMove && restrict){
            if(moveRestrictor.IndexOf(source) == -1){
                moveRestrictor.Add(source);
            }
        } else if(!canMove && !restrict){
            moveRestrictor.RemoveAt(moveRestrictor.IndexOf(source));
            if(moveRestrictor.Count < 1) canMove = true;
        } else print("Error: canMove can't be turned on when it's already on!");
    }

    public void RestrictCamera(bool restrict, string source){
        if(canLook && restrict){
            canLook = false;
            lookRestrictor.Add(source);
        } else if(!canLook && restrict){
            if(lookRestrictor.IndexOf(source) == -1){
                lookRestrictor.Add(source);
            }
        } else if(!canLook && !restrict){
            lookRestrictor.RemoveAt(lookRestrictor.IndexOf(source));
            if(lookRestrictor.Count < 1) canLook = true;
        } else print("Error: canLook can't be turned on when it's already on!");
    }

    public bool CanMove() {return canMove;}
    public bool CanLook() {return canLook;}
    public bool CanUseStamina() {return staminaUse;}
    public float GetDeltaStamina() {return staminaConsumption;}

    public void QueueNotification(string notif, float waitTime) {notificationQueue.Add((notif, waitTime));}
    private void SendNextNotification(){
        (string, float) notif = notificationQueue[0];
        notificationText.text = notif.Item1;
        notifUse = true;
        notifOpen = true;
        notifWait = notif.Item2;
        timer = 0.0f;
        notificationQueue.RemoveAt(0);
    }
}
