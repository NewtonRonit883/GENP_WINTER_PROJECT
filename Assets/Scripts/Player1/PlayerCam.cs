using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX,sensY;
    public Transform orientation;
    float xRotation,yRotation;

    private void Start()
    {
        Cursor.lockState= CursorLockMode.Locked;
        Cursor.visible = true;

    }
    private void Update()
    {
        float mouseX=Input.GetAxisRaw("Mouse X")*Time.deltaTime*sensX;
        float mouseY=Input.GetAxisRaw("Mouse Y")*Time.deltaTime*sensY;

        yRotation+=mouseX;
        xRotation-=mouseY;
        xRotation=Mathf.Clamp(xRotation,-90f,90f);
        transform.rotation =Quaternion.Euler(xRotation,yRotation,0);
        orientation.rotation =Quaternion.Euler(0,yRotation,0);
    }
}
