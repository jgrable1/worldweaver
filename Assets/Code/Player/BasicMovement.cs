using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    public GameObject worldObject;
    private World world;
    private Rigidbody body;
    private Transform attachedCamera;
    private Vector3 playerVelocity;
    public LayerMask ground;
    private RaycastHit terrainHit;
    private bool wallJump, groundedPlayer, walking;
    public float playerSpeed = 5.0f;
    private float lookX, lookY, lookSpeed = 0f;
    public bool lockMovement, lockCamera = false;

    private void Start(){
        body = gameObject.GetComponent<Rigidbody>();
        attachedCamera = gameObject.transform.GetChild(1);
        lookSpeed = 2f;
        world = worldObject.GetComponent<World>();
    }

    void Update(){
        if(world.CanMove()){
            // Basic WASD Movement
            playerVelocity = (transform.right*Input.GetAxis("Horizontal") + transform.forward*Input.GetAxis("Vertical")).normalized;
            playerVelocity.y = 0;
            walking = playerVelocity != Vector3.zero;

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

        if(world.CanLook()){
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
        // print(OnSlope());

        body.AddForce(playerVelocity*playerSpeed*10*(groundedPlayer?1:0.2f), ForceMode.Acceleration);
    }

    bool OnSlope(){
        if(Physics.Raycast(transform.position, Vector3.down, out terrainHit, 1)){
            if(terrainHit.normal != Vector3.up) return true;
            else return false;
        } else return false;
    }

    public bool IsGrounded(){ return groundedPlayer;}
    public bool IsWalking(){ return walking;}

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
