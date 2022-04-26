using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 moveVector;
    private float speed = 3.0f;
    private float verticalVelocity = 0.0f;
    private float gravity = 10.0f;
    private float animationDuration = 3.0f;
    private float startTime;
    private bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        startTime = Time.time;
    }
    // Update is called once per frame
    void Update()
    {
        if(isDead)
            return; 

        if(Time.time - startTime < animationDuration)
        {
            controller.Move(Vector3.forward*speed*Time.deltaTime);
            return;
        }
        moveVector = Vector3.zero;
        if(controller.isGrounded)
        {
            verticalVelocity = -0.5f;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
        // X - Kanan dan Kiri
        moveVector.x = Input.GetAxisRaw("Horizontal")*speed;
        if(Input.GetMouseButton(0))
        {
            //percabangan agar program tahu apakah kita touch layar sebalah kanan
            if(Input.mousePosition.x > Screen.width/2)
                moveVector.x = speed;
            else
                moveVector.x = -speed;
        }
        // Y - Atas dan Bawah
        moveVector.y = verticalVelocity;
        // Z - Maju Dan Mundur
        moveVector.z = speed;
        controller.Move(moveVector*Time.deltaTime);

    }
    public void SetSpeed(float modifier)
    {
        speed = 5.0f + modifier;
    }

    //Dijalankan saat kapsul Player menyentuh sesuatu
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Obstacles")
            Death();
    }

    private void Death()
    {
        isDead = true;
        GetComponent<Score>().OnDeath();
    }
}
