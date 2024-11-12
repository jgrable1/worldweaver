using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private bool invincible = false;
    public int health;
    public float stamina;
    public PlayerStatUI playerHPBar, playerStaminaBar;
    private int playerMaxHP = 10; // HP of the player
    private float playerMaxStamina = 10.0f;
    [SerializeField]
    private World world;
    void Start()
    {
        // health = PlayerPrefs.GetInt("PlayerHP");
        playerStaminaBar.updateStat(stamina);
        playerHPBar.updateStat(health);
    }

    void Update() {
        PlayerStaminaUpdate(-world.GetDeltaStamina()*Time.deltaTime);
    }

    public void PlayerStaminaUpdate(float change){
        stamina = stamina + change;
        if(stamina > playerMaxStamina)
            stamina = playerMaxStamina;
        if(stamina <= 0.0f) world.LockStamina(true);
        else if(stamina > 1.0f) world.LockStamina(false);
        playerStaminaBar.updateStat(stamina);
    }

    public void PlayerHPUpdate(int HP) {
        if(!invincible || HP > 0){
            health = health + HP;
            if(health > playerMaxHP)
                health = playerMaxHP;
            playerHPBar.updateStat(health);
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
