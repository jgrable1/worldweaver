using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class World : MonoBehaviour
{
    private bool canMove, canLook, notifUse, notifOpen;
    private List<string> moveRestrictor, lookRestrictor;
    [SerializeField]
    private TMP_Text notificationText;
    private float notifWait, timer;
    void Start()
    {
        canMove = true;
        canLook = true;
        moveRestrictor = new List<string>();
        lookRestrictor = new List<string>();
    }

    void Update()
    {
        if(notifUse){
            if(notifOpen){
                if(timer <= notifWait){
                    print(notificationText.transform.position.x);
                    if(notificationText.transform.position.x > 540){
                        notificationText.transform.Translate(new Vector3(-200, 0, 0)*Time.deltaTime);
                    } else{
                        //notificationText.transform.position.x = 220;
                        timer += Time.deltaTime;
                    }
                } else notifOpen = false;
            } else{
                if(notificationText.transform.position.x < 740){
                        notificationText.transform.Translate(new Vector3(200, 0, 0)*Time.deltaTime);
                } else{
                    //notificationText.transform.position.x = 420;
                    notifUse = false;
                }
            }
        }
    }

    public void RestrictMovement(bool restrict, string source){
        print(source+ " disabling movement.");
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

    public void SendNotification(string notif, float waitTime){
        notificationText.text = notif;
        notifUse = true;
        notifOpen = true;
        notifWait = waitTime;
    }
}
