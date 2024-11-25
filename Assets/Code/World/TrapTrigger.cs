using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            Transform player = other.transform;
            while(player.gameObject.name != "Player"){
                player = player.transform.parent;
            }
            PlayerStats script = player.GetComponent<PlayerStats>();
            script.PlayerHPUpdate(-1);
        } 
    }
}
