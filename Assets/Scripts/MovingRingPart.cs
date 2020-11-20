using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingRingPart : MonoBehaviour
{
    public float speed = 1;
    public float fromAngle;
    public float toAngle;

    private float currentY;
    private bool canToggle = true;

    // Update is called once per frame
    void FixedUpdate()
    {
        currentY = transform.rotation.eulerAngles.y;
        RotateSegment();
        if (canToggle)
        {
            if (isNear(currentY, toAngle, 1f) || isNear(currentY, fromAngle, 1f))
            {
                Toggle();
                StartCoroutine(Cooldown(1f));
            }
        }

    }

    void Toggle()
    {
        speed *= -1;
    }

    void RotateSegment()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, currentY + speed * Time.deltaTime, 0);
    }

    bool isNear(float A, float B, float sensitivity)
    {
        if(Mathf.Abs(A-B) < sensitivity)
        {
            return true;
        }
        return false;
    }

    IEnumerator Cooldown(float duration)
    {
        canToggle = false;
        yield return new WaitForSeconds(duration);
        canToggle = true;
    }
}

