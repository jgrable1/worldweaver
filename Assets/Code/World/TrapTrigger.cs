using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            Transform player = other.transform;
            while(player.gameObject.name != "Player"){
                player = player.transform.parent;
            }
            PlayerHP script = player.GetComponent<PlayerHP>();
            script.PlayerHPUpdate(-1);
        }
        
    }
}
