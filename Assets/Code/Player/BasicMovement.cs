using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    private CharacterController controller;
    private Rigidbody body;
    private Vector3 playerVelocity;
    private bool groundedPlayer = true;
    public float playerSpeed = 5.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    private float lookSpeed = 2.0f;
    private float lookX = 0f;
    private float lookY = 0f;
    private bool wallJump = false;
    public bool lockMovement = false;
    public bool lockCamera = false;

    private void Start(){
        controller = gameObject.GetComponent<CharacterController>();
        body = gameObject.GetComponent<Rigidbody>();
    }

    void Update(){
        if(!lockMovement){
            // Basic WASD Movement
            Vector3 move = transform.right*Input.GetAxis("Horizontal") + transform.forward*Input.GetAxis("Vertical");
            move.y = 0;
            move.Normalize();
            controller.Move(move * playerSpeed * Time.deltaTime);

            // Jump
            if (Input.GetButtonDown("Jump") && groundedPlayer)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
                groundedPlayer = false;
            }

            // Toggle Wall Jump
            if (Input.GetButtonDown("WallJump")){
                print("WallJump Toggled");
                wallJump = !wallJump;
            }
        }

        // Gravity
        playerVelocity.y += gravityValue * Time.deltaTime;  
        controller.Move(playerVelocity * Time.deltaTime);
        
        if(!lockCamera){
            // Camera Movement
            float h = lookSpeed * Input.GetAxis("Mouse X");
            float v = lookSpeed * Input.GetAxis("Mouse Y");

            lookY += h;
            if(Mathf.Abs(lookX - v) <= 50) lookX -= v;

            transform.localEulerAngles = new Vector3(lookX, lookY, 0);
        }

        if(transform.position.y < -50){
            transform.position = new Vector3(10, 10, 10);
        }
    }

    void OnCollisionStay(Collision collisionInfo) {
        //print("Collision Stay Called on "+collisionInfo.gameObject.tag);
        if (collisionInfo.gameObject.tag == "Ground") {
            if(playerVelocity.y < 0 && collisionInfo.contacts[0].normal.y > 0){
                playerVelocity.y = 0f;
                groundedPlayer = true;
            }
        } else if (collisionInfo.gameObject.tag == "Wall" && wallJump){
            groundedPlayer = true;
        }
    }
    void OnCollisionExit(Collision collisionInfo) {
        //print("Collision Exit Called on "+collisionInfo.gameObject.tag);
        if (collisionInfo.gameObject.tag == "Ground") {
            groundedPlayer = false;
        }
    }
    void OnCollisionEnter(Collision collisionInfo) {
        //print("Collision Enter Called on "+collisionInfo.gameObject.tag);
        if (collisionInfo.gameObject.tag == "Ground") {
             if(playerVelocity.y < 0 && collisionInfo.contacts[0].normal.y > 0){
                playerVelocity.y = 0f;
                groundedPlayer = true;
            } else if(collisionInfo.contacts[0].normal.y == 0 && wallJump) groundedPlayer = true;
        } else if (collisionInfo.gameObject.tag == "Wall" && wallJump){
            groundedPlayer = true;
        }
    }

}
