using UnityEngine;

public class ColliderTrigger : MonoBehaviour
{
    private void OnCollisionEnter(Collision other){
        if(other.transform.tag == "Player"){
            Transform player = other.transform;
            while(player.gameObject.name != "Player"){
                player = player.transform.parent;
            }
            PlayerStats script = player.gameObject.GetComponent<PlayerStats>();
            script.PlayerHPUpdate(-1);
        }
    }
}
