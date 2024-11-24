using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    private Rigidbody body;
    [SerializeField]
    private BasicMovement player;
    [SerializeField]
    private World world;
    float dashCooldown, dashLimit, dashSpeed;
    bool dashing = false;
    Vector3 dir;
    public PlayerStatUI dashBar;

    void Start()
    {
        body = gameObject.GetComponent<Rigidbody>();
        dashCooldown = 0.25f;
        dashLimit = 1.25f; // Can dash every dashLimit-0.25 seconds
        dashSpeed = player.playerSpeed*6f; // Dash Speed = 6x current player walking speed
    }

    void Update()
    {
        if(dashCooldown < dashLimit){
            dashCooldown += Time.deltaTime;
        }

        if(dashing && dashCooldown >= 0.25f){
            dashing = false;
            world.RestrictMovement(false, "Dash");
            dashBar.updateDash(true);
        }


        if(world.CanMove()){
            if(Input.GetButtonDown("Dash") && dashCooldown >= dashLimit){
                world.RestrictMovement(true, "Dash");
                dashCooldown = 0f;
                dashing = true;
                dashBar.updateDash(false);
                dir = transform.right*Input.GetAxis("Horizontal") + transform.forward*Input.GetAxis("Vertical");
                dir.y = 0;
                if(dir == Vector3.zero){
                    print("No direction, assigning facing direction.");
                    dir = transform.forward;
                    dir.y = 0;
                }
                dir.Normalize();
            }
        }
    }
    void FixedUpdate(){
        if(dashing) body.AddForce(dir*dashSpeed*10*(player.IsGrounded()?0.6f:0.2f), ForceMode.Acceleration);
    }
}
