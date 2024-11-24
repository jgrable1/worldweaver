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

    public void PlayerHPUpdate(int change) {
        if(!invincible || change > 0){
            health = health + change;
            if(health == 0){
                UnityEditor.EditorApplication.isPlaying = false; // Quits out of the unity editor for test purposes.
                Application.Quit(); // Quits the build when showing off final product.
            }
            if(health > playerMaxHP)
                health = playerMaxHP;
            playerHPBar.updateStat(health);
            if(change < 0){
                invincible = true;
                StartCoroutine(IFrames());
            }
        }
    }
    
    public int GetMaxHealth(){return playerMaxHP;}
    public void SetMaxHealth(int newMax){playerMaxHP = newMax;}
    public float GetMaxStamina(){return playerMaxStamina;}
    public void SetMaxStamina(float newMax){playerMaxStamina = newMax;}

    IEnumerator IFrames(){
        yield return new WaitForSeconds(3); // Can't take damage for 3 seconds.
        invincible = false;
    }
}
