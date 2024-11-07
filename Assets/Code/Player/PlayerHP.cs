using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    private bool invincible = false;
    public int health;
    public PlayerHPBar playerHPBar;
    private int playerMaxHP = 10; // HP of the player
    void Start()
    {
        // health = PlayerPrefs.GetInt("PlayerHP");
        playerHPBar.updatePlayerHP(health);
    }

    // Update is called once per frame
    public void PlayerHPUpdate(int HP) {
        if(!invincible){
            health = health + HP;
            if(health > playerMaxHP)
                health = playerMaxHP;
            playerHPBar.updatePlayerHP(health);
            // PlayerPrefs.SetInt("PlayerHP", health);
            // PlayerPrefs.Save();
            if(HP < 0){
                invincible = true;
                StartCoroutine(IFrames());
            }
        }
    }

    IEnumerator IFrames(){
        yield return new WaitForSeconds(3); // Can't take damage for 3 seconds.
        invincible = false;
    }
}
