using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Unity.VisualScripting;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Transform camTransform;
    float speed = 4f;
    private Vector3 velocity;
    public float gravity = -19.62f;
    float jumpHeight = 1.2f;
    [Header("Ground Check Settings")]
public Transform groundCheck;
public float groundDistance = 0.4f;
public LayerMask groundMask; 
    void Start()
    {
       controller = GetComponent<CharacterController>();
       camTransform = Camera.main.transform;
    }

    void Update()
    {
        //GroundCheck
        bool isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;}
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 forward = camTransform.forward;
        Vector3 right = camTransform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 move = (forward * z) + (right * x);

        controller.Move(move * speed * Time.deltaTime);

        //jumplogic
       if (Input.GetButtonDown("Jump") && isGrounded) {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
       }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        
    }

}
