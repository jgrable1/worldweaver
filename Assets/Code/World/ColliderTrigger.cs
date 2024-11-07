using UnityEngine;

public class ColliderTrigger : MonoBehaviour
{
    private void OnCollisionEnter(Collision other){
        Transform player = other.transform;
        while(player.gameObject.name != "Player"){
            player = player.transform.parent;
        }
            PlayerHP script = player.gameObject.GetComponent<PlayerHP>();
            script.PlayerHPUpdate(-1);
        }
}
