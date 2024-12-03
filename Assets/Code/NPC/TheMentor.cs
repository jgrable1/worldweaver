using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TheMentor : TemplateNPC
{

    private bool axeGift = false;

    // Start is called before the first frame update
    void Start(){
        tmp = GetComponentInChildren<TextMeshPro>();

        npcTurn = false;
        textTurn = false;

        dialogue = new string[] {
            "Talk (E)",
            "Greetings, I'll keep this brief as time is limited.",
            "Far off to my left, over the big green hill, there lies an encampment.",
            "That encampment houses the evil goblins. You must stop them.",
            "Take this wooden axe, get started by chopping down trees.",
            "Then make a pickaxe out of wood and start mining the nodes you see in the area.",
            "Eventually you'll use these resources to craft a sword to fight them with.",
            "Stone you'll find in nodes mostly at the base of mountains and hills.",
            "Iron and coal however are mostly found much higher up, in the tallest mountains.",
            "And before you ask, I tried hitting them with the axe. It didn't work, not sure why.",
            "Oh and also, some of the other villagers have provided some of their lifeforce.",
            "They manifest as white spheres spread throughout the village, they can heal you.",
            "Anyways, good luck."
        };

        methods = new System.Action[13];
        methods[4] = WoodAxeCheck;
    }

    void WoodAxeCheck(){
        // print("WoodAxeCheck Called");
        if(!axeGift){
            World world = player.GetComponent<BasicMovement>().worldObject.GetComponent<World>();
            BasicItem woodAxe = world.GetPrefab("Wooden Axe").GetComponent<BasicItem>();
            world.GetInventory().AddItem("Wooden Axe", 1, woodAxe.GetSprite(), woodAxe.action);
            axeGift = true;
            dialogue[4] = "Use that wooden axe I gave you to chop down some trees.";
        }
        methods[4] = null;
    }
}
