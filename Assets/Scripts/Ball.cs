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
    private TrailRenderer tr;

    //Particles
    BallParticles ballParticles;

    //Trail sizes
    private AnimationCurve smallCurve;
    private AnimationCurve bigCurve;

    //Trail colors
    private Gradient slowTrailGradient;
    private Gradient fastTrailGradient;
    
    [HideInInspector]
    public Action<bool> OnEndingReached;
    [HideInInspector]
    public Action OnBigImpact;
    [HideInInspector]
    public Action<bool> PassedRing;
    [HideInInspector]
    public int holesInArow = 0;


    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
        tr = transform.GetChild(0).GetComponent<TrailRenderer>();
        ballParticles = GetComponent<BallParticles>();
        smallCurve = tr.widthCurve;
        bigCurve = InitializeBigCurve();
        slowTrailGradient = tr.colorGradient;
        fastTrailGradient = InitializeTrailGradient();
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

    private Gradient InitializeTrailGradient()
    {
        Gradient gradient = new Gradient();

        gradient.SetKeys(
            new GradientColorKey[] {new GradientColorKey(Color.red,0f), new GradientColorKey(Color.red, 1.0f)},
            new GradientAlphaKey[] {new GradientAlphaKey(0.9f, 0f), new GradientAlphaKey(0.5f, 1f) }
        );

        return gradient;
    }

    private AnimationCurve InitializeBigCurve()
    {
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0f, 0.8f);
        curve.AddKey(0.93f, 0.08f);
        return curve;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            if(holesInArow < 3)
            {
                rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
                holesInArow = 0;
                PassedRing?.Invoke(false);
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
        PassedRing?.Invoke(false);
        _renderer.material = normalMaterial;
        tr.time = 0.3f;
        tr.widthCurve = smallCurve;
        tr.colorGradient = slowTrailGradient;
        ballParticles.BigPaintSplatter(col.GetContact(0).point);
    }
    
    private void PassRing()
    {
        holesInArow++;
        PassedRing?.Invoke(true);
        if (holesInArow == 3)
        {
            _renderer.material = fastMaterial;
            tr.time = 0.5f;
            tr.widthCurve = bigCurve;
            tr.colorGradient = fastTrailGradient;
        }  
    }
    
    public void SetRingOffset(float value)
    {
        ringOffset = value;
        NextRingHeight = -ringOffset;
    }
}
