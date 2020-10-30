using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    private float touchStart = 0;
    private float displacement;
    private float prevRotation;

    public float rotationSpeed = 1;


    void Start()
    {
        prevRotation =  0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                DragStart(touch);
            }
            if(touch.phase == TouchPhase.Moved)
            {
                Dragging(touch);
            }
            if(touch.phase == TouchPhase.Ended)
            {
                DragRelease(touch);
            }
        }
    }

    void DragStart(Touch touch)
    {
        touchStart = touch.position.x;
    }
    void Dragging(Touch touch)
    {
        displacement = (touch.position.x - touchStart) * rotationSpeed;         
        transform.eulerAngles = new Vector3(0, prevRotation - displacement, 0);
    }
    void DragRelease(Touch touch)
    {
        //remember the rotation of the structure for next drag
        prevRotation = transform.rotation.eulerAngles.y;
    }
}
