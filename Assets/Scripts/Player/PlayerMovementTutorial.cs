using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementTutorial : MonoBehaviour {
    [Header("Movement")]
    public float moveSpeed = 7f;
    public float groundDrag = 15f;

    [Header("Jumping")]
    public float jumpForce = 8f;
    public float jumpCooldown = 0.25f;
    public float airMultiplier = 0.3f;
    bool readyToJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight = 2f;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("References")]
    public Transform orientation;
    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
    }

    private void Update() {
        // 1. Ground Check (Keep this high-frequency in Update)
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();

        // 2. IMMEDIATE DRAG REMOVAL
        // If we press jump, we kill drag IMMEDIATELY so there's no friction against the upward force
        if (grounded && !Input.GetKey(jumpKey)) {
            rb.drag = groundDrag;
        }
        else {
            rb.drag = 0;
        }
    }

    private void FixedUpdate() {
        MovePlayer();
    }

    private void MyInput() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // 3. Jump Trigger
        if (Input.GetKey(jumpKey) && readyToJump && grounded) {
            readyToJump = false;
            Jump();

            // Allows for continuous jumping if key is held
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer() {
        // Calculate movement direction based on where the player is looking
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // On Ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // In Air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl() {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Limit velocity if it exceeds moveSpeed
        if (flatVel.magnitude > moveSpeed) {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump() {
        // 3. INSTANT VELOCITY OVERRIDE
        // Resetting Y to 0 ensures we don't have to 'fight' falling velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Use VelocityChange for an instant "snap" upward
        rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
    }

    private void ResetJump() {
        readyToJump = true;
    }
}