using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotorSwipe : MonoBehaviour
{
    private const float LANE_DISTANCE = 1.25f;
    private const float TURN_SPEED = 0.05f;
    //Movement nya 
    private CharacterController controller;
    private Vector3 moveVector;
    private float jumpForce = 7.5f;
    private float gravity = 10.0f;
    private float verticalVelocity;
    private float speed = 3.0f;
    private int desiredLane = 1; // 0 = kiri, 1= tengah, 2=kanan
    private float animationDuration = 3.0f;
    private float startTime;
    private bool isDead = false;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        startTime = Time.time;
    }

    private void Update()
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


        //mendapatkan input di lane mana player harus berada
        if(MobileInput.Instance.SwipeLeft)
            MoveLane(false);
        if(MobileInput.Instance.SwipeRight)
            MoveLane(true);

        //hitung dimana seharusnya kedepannya
        Vector3 targetPosition = transform.position.z * Vector3.forward;
        if(desiredLane == 0)
            targetPosition += Vector3.left * LANE_DISTANCE;
        else if(desiredLane ==2)
            targetPosition += Vector3.right * LANE_DISTANCE;

        //kalkulasi move disini
        moveVector = Vector3.zero;
        moveVector.x = (targetPosition - transform.position).x * (speed*1.5f);
        
        //kalkulasi Y
        if(IsGrounded())//Kalau ditanah
        {
            verticalVelocity = -0.1f;
            if(MobileInput.Instance.SwipeUp)
            {
                //Lompat
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            verticalVelocity -= (gravity * Time.deltaTime);
            //fastfall jika saat lompat langsung turun
            if(MobileInput.Instance.SwipeDown)
            {
                verticalVelocity = -jumpForce;
            }
        }
        
        moveVector.y = verticalVelocity;
        moveVector.z = speed;

        //move player nya
        controller.Move(moveVector * Time.deltaTime);

        //rotasi sedikit untuk pindah lane
        Vector3 dir = controller.velocity;
        dir.y = 0;
        transform.forward = Vector3.Lerp(transform.forward,dir,TURN_SPEED); 

    }

    public void SetSpeed(float modifier)
    {
        speed = 3.0f + (modifier/1.5f);
    }

    private void MoveLane(bool goingRight)
    {
        desiredLane += (goingRight) ? 1 : -1;
        desiredLane = Mathf.Clamp(desiredLane, 0, 2);

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

    private bool IsGrounded()
    {
        Ray groundRay = new Ray(new Vector3(controller.bounds.center.x,(controller.bounds.center.y - controller.bounds.extents.y) + 0.2f,controller.bounds.center.z),Vector3.down);
        Debug.DrawRay(groundRay.origin,groundRay.direction,Color.cyan,1.0f);
        
        return Physics.Raycast(groundRay,0.2f + 0.1f);
    }
}
