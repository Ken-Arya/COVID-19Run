using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInput : MonoBehaviour
{
    private const float DEADZONE = 5.0F;
    public static MobileInput Instance{ set; get; }
    private bool tap, swipeLeft, swipeRight, swipeUp, swipeDown;
    private Vector2 swipeDelta, startTouch;

    public bool Tap{get{return tap;}}
    public Vector2 SwipeDelta{get{return swipeDelta;}}
    public bool SwipeLeft{get{return swipeLeft;}}
    public bool SwipeRight{get{return swipeRight;}}
    public bool SwipeUp{get{return swipeUp;}}
    public bool SwipeDown{get{return swipeDown;}}

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        //Reset semua boolean
        tap = swipeLeft = swipeRight = swipeDown = swipeUp = false;
    
        //cek input di PC
        #region Standalone Inputs
        if(Input.GetMouseButtonDown(0))
        {
            tap=true;
            startTouch = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            startTouch = swipeDelta = Vector2.zero;
        }
        #endregion

        //Cek Input di Mobile
        #region Standalone Inputs
        if(Input.touches.Length != 0)
        {
            if(Input.touches[0].phase == TouchPhase.Began)
            {
                tap=true;
                startTouch = Input.mousePosition;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                startTouch = swipeDelta = Vector2.zero;
            }    
        }
        #endregion
    
        //kalkulasi distance
        swipeDelta = Vector2.zero;
        if(startTouch != Vector2.zero)
        {
            //cek di mobile 
            if(Input.touches.Length != 0)
            {
                swipeDelta = Input.touches[0].position - startTouch;
            }
            //cek dengan standalone
            else if(Input.GetMouseButton(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
            }
        }

        //cek kalau move drag swipe nya sedikit(deadzone piksel)
        if(swipeDelta.magnitude > DEADZONE)
        {
            //ini ada swipe yang dikonfirmasi dengan besaran derajat
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            //cek kalau nilai nya minus dan dijadikan postiif
            if(Mathf.Abs(x) > Mathf.Abs(y))
            {
                //kanan atau kiri
                if(x<0)
                    swipeLeft = true;
                else   
                    swipeRight = true;
            }
            else
            {
                //atas atau bawah
                if(x<0)
                    swipeDown = true;
                else   
                    swipeUp = true;
            }

            startTouch = swipeDelta = Vector2.zero;
        }

    }
}
