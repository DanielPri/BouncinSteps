using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Materials")]
    public Material normalMaterial;
    public Material fastMaterial;

    public float bounceForce = 1;
    
    private float ringOffset;
    private float NextRingHeight;    

    private Rigidbody rb;
    private Renderer _renderer;

    //Particles
    private BallParticles ballParticles;
    private SpeedParticles speedParticles;

    //Trail
    private BallTrail ballTrail;
    
    [HideInInspector]
    public Action<bool> OnEndingReached;
    [HideInInspector]
    public Action OnBigImpact;
    [HideInInspector]
    public Action PassedRing;
    [HideInInspector]
    public int holesInArow = 0;


    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
        ballParticles = GetComponent<BallParticles>();
        ballTrail = GetComponentInChildren<BallTrail>();
        speedParticles = GetComponentInChildren<SpeedParticles>();
    }

    private void FixedUpdate()
    {
        // Check if ball passed a ring
        if(transform.position.y < NextRingHeight)
        {
            NextRingHeight -= ringOffset;
            PassRing();
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            if(holesInArow < 3)
            {
                rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
                holesInArow = 0;
                speedParticles.changeEmitAmount(false);
                ballParticles.SmallPaintSplatter(collision.GetContact(0).point);
            }
            else
            {
                BreakThrough(collision);
            }
        }
        if (collision.gameObject.tag == "Death")
        {
            if(holesInArow >= 3)
            {
                BreakThrough(collision);
            }
            else
            {
                print("Game Over!");
                OnEndingReached?.Invoke(false);
            }
        }
        else if(collision.gameObject.tag == "Finish")
        {
            print("Victory!");
            OnEndingReached?.Invoke(true);
        }
    }

    private void BreakThrough(Collision col)
    {
        OnBigImpact?.Invoke();
        col.transform.parent.gameObject.GetComponent<Ring>().Break(transform.position);
        holesInArow = 0;
        speedParticles.changeEmitAmount(false);
        _renderer.material = normalMaterial;
        ballParticles.BigPaintSplatter(col.GetContact(0).point);

        ballTrail.setSmall();
    }
    
    private void PassRing()
    {
        holesInArow++;
        PassedRing?.Invoke();
        speedParticles.changeEmitAmount(true);
        if (holesInArow == 3)
        {
            _renderer.material = fastMaterial;
            ballTrail.setBig();
        }  
    }
    
    public void SetRingOffset(float value)
    {
        ringOffset = value;
        NextRingHeight = -ringOffset;
    }
}
