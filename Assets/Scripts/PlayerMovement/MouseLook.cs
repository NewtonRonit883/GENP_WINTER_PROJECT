using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Transactions;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public GameObject PlayerGameObject;
    public float Sensitivity_of_mouse = 100f;
    public bool Drunken_Mouse_Enabled = false;
    public float MoveSpeed = 100f;

    public bool Flying_Enabled = false;

    public float Jump_vel = 100f;

    private CharacterController pls;

    private Vector3 grav_vel;
    public float gravity_acc = 9.81f;

    public Transform GroundChecker;
    public LayerMask layermask;
    public float ground_dist = 0.1f;

    float xRot = 0;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pls = PlayerGameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        float MouseX = Input.GetAxis("Mouse X") * Sensitivity_of_mouse * Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * Sensitivity_of_mouse * Time.deltaTime;
        if (Drunken_Mouse_Enabled)
        {
            PlayerGameObject.transform.Rotate(new Vector3(-MouseY, MouseX, 0));
        }
        else
        {
            xRot -= MouseY;
            xRot = Mathf.Clamp(xRot, -90f, 90f);
            transform.localRotation = Quaternion.Euler(xRot, 0f , 0f);
            PlayerGameObject.transform.Rotate(Vector3.up * MouseX);
        }
        
    }

    void FixedUpdate()
    {
        bool IsGrounded = Physics.CheckCapsule(GroundChecker.position, GroundChecker.position - new Vector3(0f, ground_dist, 0f ), 0.4f, layermask);
        
        float verti = Input.GetAxis("Vertical");
        float hori = Input.GetAxis("Horizontal");
        if (Flying_Enabled)
        {
            pls.Move((transform.right * hori + transform.forward * verti) * Time.deltaTime * MoveSpeed);
            return;
        }
        pls.Move((PlayerGameObject.transform.right * hori + PlayerGameObject.transform.forward * verti) * Time.deltaTime * MoveSpeed);
        pls.Move(grav_vel * Time.deltaTime);
        grav_vel.y -= gravity_acc * Time.deltaTime;
        if (IsGrounded && grav_vel.y < 0)
        {
            grav_vel.y = -5f;
        }
        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            grav_vel.y = Jump_vel;
        }

    }
}
