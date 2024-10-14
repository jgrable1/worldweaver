using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    private CharacterController controller;
    private Rigidbody body;
    private BasicMovement player;
    float dashCooldown, dashLimit, dashSpeed;
    bool dashing = false;
    Vector3 dir;

    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        player = gameObject.GetComponent<BasicMovement>();
        dashCooldown = 0.25f;
        dashLimit = 1.25f; // Can dash every dashLimit-0.25 seconds
        dashSpeed = player.playerSpeed*6f; // Dash Speed = 6x current player walking speed
    }

    void Update()
    {
        if(player == null){
            player = gameObject.GetComponent<BasicMovement>();
        }

        if(dashCooldown < dashLimit){
            dashCooldown += Time.deltaTime;
        }

        if(dashing && dashCooldown < 0.25f){
            controller.Move(dir * dashSpeed * Time.deltaTime);
        } else if(dashing){
            dashing = false;
            player.lockMovement = false;
        }

        if(Input.GetButtonDown("Dash") && dashCooldown >= dashLimit){
            player.lockMovement = true;
            dashCooldown = 0f;
            dashing = true;
            dir = transform.right*Input.GetAxis("Horizontal") + transform.forward*Input.GetAxis("Vertical");
            dir.y = 0;
            if(dir == Vector3.zero){
                print("No direction, assigning facing direction.");
                dir = transform.forward;
                dir.y = 0;
            }
            dir.Normalize();
            controller.Move(dir * dashSpeed * Time.deltaTime);
        }
        
    }
}
