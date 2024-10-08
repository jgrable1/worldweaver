using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    private CharacterController controller;
    private Rigidbody body;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 5.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    private float lookSpeed = 2.0f;
    float lookX = 0f;
    float lookY = 0f;

    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        body = gameObject.AddComponent<Rigidbody>();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }


    
        Vector3 move = transform.right*Input.GetAxis("Horizontal") + transform.forward*Input.GetAxis("Vertical");
        move.y = 0;
        move.Normalize();
        print(move.ToString("G"));
        controller.Move(move * playerSpeed * Time.deltaTime);

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        float h = lookSpeed * Input.GetAxis("Mouse X");
        float v = lookSpeed * Input.GetAxis("Mouse Y");

        lookY = lookY + h;
        lookX = lookX - v;

        transform.localEulerAngles = new Vector3(lookX, lookY, 0);
    }
}
