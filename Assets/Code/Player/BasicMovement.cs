using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    private Rigidbody body;
    private Transform attachedCamera;
    private Vector3 playerVelocity;
    private float verticalVelocity;
    private bool groundedPlayer = true;
    public float playerSpeed = 5.0f;
    private float jumpHeight = 1.0f;
    private float lookSpeed = 2.0f;
    private float lookX = 0f;
    private float lookY = 0f;
    private bool wallJump = false;
    public bool lockMovement = false;
    public bool lockCamera = false;
    public LayerMask ground;
    private RaycastHit terrainHit;

    private void Start(){
        //controller = gameObject.GetComponent<CharacterController>();
        body = gameObject.GetComponent<Rigidbody>();
        attachedCamera = gameObject.transform.GetChild(1);
        //body.drag = playerSpeed;
    }

    void Update(){
        if(!lockMovement){
            // Basic WASD Movement
            playerVelocity = (transform.right*Input.GetAxis("Horizontal") + transform.forward*Input.GetAxis("Vertical")).normalized;
            playerVelocity.y = 0;

            // Jump
            if (Input.GetButtonDown("Jump") && groundedPlayer)
            {
                body.AddForce(transform.up*10f, ForceMode.Impulse);
            }

            // Toggle Wall Jump
            if (Input.GetButtonDown("WallJump")){
                print("WallJump Toggled");
                wallJump = !wallJump;
            }
        }

        if(!lockCamera){
            // Camera Movement
            float h = lookSpeed * Input.GetAxis("Mouse X");
            float v = lookSpeed * Input.GetAxis("Mouse Y");

            lookY += h;
            if(Mathf.Abs(lookX - v) <= 50) lookX -= v;
        }
    }

    void FixedUpdate() {
        groundedPlayer = Physics.CheckSphere(transform.position - new Vector3(0, 0.75f, 0), 0.1f, ground);

        if(groundedPlayer) body.drag = playerSpeed;
        else body.drag = playerSpeed/5;
        if(transform.position.y < -50){
            transform.position = new Vector3(10, 10, 10);
        }

        attachedCamera.localEulerAngles = new Vector3(lookX, 0, 0);
        transform.localEulerAngles = new Vector3(0, lookY, 0);

        if(OnSlope()) playerVelocity = Vector3.ProjectOnPlane(playerVelocity, terrainHit.normal);

        body.AddForce(playerVelocity*playerSpeed*10*(groundedPlayer?1:0.2f), ForceMode.Acceleration);
    }

    bool OnSlope(){
        if(Physics.Raycast(transform.position, Vector3.down, out terrainHit, 1)){
            if(terrainHit.normal != Vector3.up) return true;
            else return false;
        } else return false;
    }

    public bool isGrounded(){ return groundedPlayer;}

    /*void OnCollisionStay(Collision collisionInfo) {
        //print("Collision Stay Called on "+collisionInfo.gameObject.tag);
        if (collisionInfo.gameObject.tag == "Ground") {
            if(playerVelocity.y < 0 && collisionInfo.contacts[0].normal.y > 0){
                //print("Velocity fixed");
                playerVelocity.y = 0f;
                groundedPlayer = true;
            }
        } else if (collisionInfo.gameObject.tag == "Wall" && wallJump){
            groundedPlayer = true;
        }
    }
    void OnCollisionExit(Collision collisionInfo) {
        print("Collision Exit Called on "+collisionInfo.gameObject.tag);
        if (collisionInfo.gameObject.tag == "Ground") {
            groundedPlayer = false;
        }
    }
    void OnCollisionEnter(Collision collisionInfo) {
        print("Collision Enter Called on "+collisionInfo.gameObject.tag);
        if (collisionInfo.gameObject.tag == "Ground") {
             if(playerVelocity.y < 0 && collisionInfo.contacts[0].normal.y > 0){
                playerVelocity.y = 0f;
                groundedPlayer = true;
            } else if(collisionInfo.contacts[0].normal.y == 0 && wallJump) groundedPlayer = true;
        } else if (collisionInfo.gameObject.tag == "Wall" && wallJump){
            groundedPlayer = true;
        }
    }*/

}
