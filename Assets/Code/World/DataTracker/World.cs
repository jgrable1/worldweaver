using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
public class World : MonoBehaviour
{
    private bool canMove, canLook, notifUse, notifOpen, staminaUse, shownStaminaTip, playerTool;
    private List<string> moveRestrictor, lookRestrictor;
    private List<(string, float)> notificationQueue;
    [SerializeField]
    private TMP_Text notificationText;
    [SerializeField]
    private TMP_Text instructions;
    private float notifWait, timer, staminaConsumption;
    [SerializeField]
    private BasicMovement player;
    private Dictionary<string, string> descriptions = new Dictionary<string, string>();
    private Dictionary<string, (string, int)[]> costs = new Dictionary<string, (string, int)[]>();
    [SerializeField]
    private string[] prefabNames;
    [SerializeField]
    private GameObject[] prefabsList;
    [SerializeField]
    private Inventory inventory;
    private Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
    private Dictionary<string, int> tools = new Dictionary<string, int>();
    private List<string> seenItems = new List<string>();
    
    void Start()
    {
        staminaConsumption = -0.5f;
        canMove = true;
        canLook = true;
        staminaUse = true;
        shownStaminaTip = false;
        playerTool = false;
        moveRestrictor = new List<string>();
        lookRestrictor = new List<string>();
        notificationQueue = new List<(string, float)>();
        ReadItemData("Assets/Data/ItemData.txt", true);
        ReadItemData("Assets/Data/RecipeCosts.txt", false);
        inventory.SetLocks(costs);
        for(int i = 0; i < prefabNames.Length; i++){
            prefabs.Add(prefabNames[i], prefabsList[i]);
        }
        prefabNames = null;
        prefabsList = null;
        tools.Add("Wooden Axe", 0); tools.Add("Wooden Pickaxe", 1); tools.Add("Wooden Sword", 2);
        tools.Add("Stone Axe", 3); tools.Add("Stone Pickaxe", 4); tools.Add("Stone Sword", 5);
        tools.Add("Iron Axe", 6); tools.Add("Iron Pickaxe", 7); tools.Add("Iron Sword", 8);
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
            if(!shownStaminaTip){
                QueueNotification("You ran out of stamina! You'll need to recover a little bit before you can use anymore.", 2.0f);
                shownStaminaTip = true;
            }
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

    public void NewItem(string itemName){
        seenItems.Add(itemName);
        // print("New Item: "+itemName+" obtained. Checking locked recipes");
        int remainingLocks;
        foreach(KeyValuePair<string, (string, int)[]> entry in costs){
            // print("Checking recipe for "+entry.Key);
            remainingLocks = inventory.RemainingLocks(entry.Key);
            if(remainingLocks > 0){
                foreach((string a, int b) in entry.Value){
                    if(a == itemName){
                        print("Found ingredient "+a+" of "+entry.Key+"! Removing 1 lock.");
                        inventory.RemoveLock(entry.Key);
                        break;
                    }
                }
            } // else print("Already unlocked");
        }
    }

    public bool CanMove() {return canMove;}
    public bool CanLook() {return canLook;}
    public bool CanUseStamina() {return staminaUse;}
    public bool HasSeenItem(string itemName) {return seenItems.Contains(itemName);}
    public float GetDeltaStamina() {return staminaConsumption;}
    public BasicMovement GetPlayer() {return player;}
    public string GetDescription(string itemName) {return descriptions[itemName];}
    public (string, int)[] GetCosts(string itemName) {return (itemName!="Unknown"?costs[itemName]:null);}
    public GameObject GetPrefab(string prefabName) {return prefabs[prefabName];}
    public Inventory GetInventory() {return inventory;}
    public string GetPlayerAction() {return (playerTool?inventory.GetPlayerTool():null);}
    public void PlayerEquip(bool b, string toolName) {
        playerTool = b;
        if(b) GetPlayer().transform.GetChild(0).GetChild(2).GetChild(0).GetChild(tools[toolName]).gameObject.SetActive(true);
        else GetPlayer().transform.GetChild(0).GetChild(2).GetChild(0).GetChild(tools[toolName]).gameObject.SetActive(false);
    }

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

    private void ReadItemData(string fileName, bool descriptions){
        try{
            StreamReader reader = new StreamReader(fileName);
            string line;
            line = reader.ReadLine();

            if(descriptions){
                string itemName, description;
                while (line != null){
                    itemName = line.Substring(0, line.IndexOf(";"));
                    description = line.Substring(line.IndexOf(";")+2);
                    // print("Looking for \\n in string");
                    description = description.Replace("\\n", "\n");
                    this.descriptions.Add(itemName, description);
                    line = reader.ReadLine();
                }
            } else{
                string itemName, remainder, curr;
                List<(string, int)> list = new List<(string, int)>();
                while (line != null){
                    itemName = line.Substring(0, line.IndexOf(";"));
                    remainder = line.Substring(line.IndexOf(";")+2);
                    if(itemName != "Unknown"){
                        while(remainder != null){
                            if(remainder.IndexOf(";") != -1){
                                curr = remainder.Substring(0, remainder.IndexOf(";"));
                                remainder = remainder.Substring(remainder.IndexOf(";")+2);
                            } else {
                                curr = remainder;
                                remainder = null;
                            }
                            // print(curr);
                            list.Add((curr.Substring(0, curr.Length-2), int.Parse(curr.Substring(curr.Length-2))));     
                        }
                        costs.Add(itemName, list.ToArray());
                        list.Clear();
                    }
                    line = reader.ReadLine();
                }
            }
            
        } catch(Exception e){
            Debug.LogException(e, this);
        }
    }
}
