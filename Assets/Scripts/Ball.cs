using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameObject contactParticles;

    [Header("Materials")]
    public Material normalMaterial;
    public Material fastMaterial;

    public float bounceForce = 1;
    
    private Rigidbody rb;
    private Renderer _renderer;
    private TrailRenderer tr;

    //Trail sizes
    private AnimationCurve smallCurve;
    private AnimationCurve bigCurve;

    //Trail colors
    private Gradient slowTrailGradient;
    private Gradient fastTrailGradient;

    private Pool ParticlePool;

    //Particle colors
    private Color SmallParticlesColor;


    [HideInInspector]
    public Action<bool> OnEndingReached;
    [HideInInspector]
    public Action OnBigImpact;
    [HideInInspector]
    public int holesInArow = 0;
    [HideInInspector]
    public Action PassedRing;


    // Start is called before the first frame update
    void Start()
    {
        ParticlePool = new Pool(contactParticles, 6);
        var main = contactParticles.GetComponent<ParticleSystem>().main;
        SmallParticlesColor = main.startColor.color;
        _renderer = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
        tr = transform.GetChild(0).GetComponent<TrailRenderer>();
        smallCurve = tr.widthCurve;
        bigCurve = InitializeBigCurve();
        slowTrailGradient = tr.colorGradient;
        fastTrailGradient = InitializeTrailGradient();
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
                PassedRing?.Invoke();
                SmallPaintSplatter(collision.GetContact(0).point);
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
        PassedRing?.Invoke();
        _renderer.material = normalMaterial;
        tr.time = 0.3f;
        tr.widthCurve = smallCurve;
        tr.colorGradient = slowTrailGradient;
        BigPaintSplatter(col.GetContact(0).point);
    }

    private GameObject SmallPaintSplatter(Vector3 pos)
    {
        // Setup a particle from the pool
        ParticleSystem splatter = SetupParticles(ParticlePool.getObjectFromPool(), pos).GetComponent<ParticleSystem>();

        // set its color
        var main = splatter.main;
        main.startColor = SmallParticlesColor;

        //set its burst quantity range
        splatter.emission.SetBurst(0, new ParticleSystem.Burst(0f, 15, 20));

        // set its shape
        var shape = splatter.shape;
        shape.shapeType = ParticleSystemShapeType.Cone;
    
        return splatter.gameObject;
    }

    private void BigPaintSplatter(Vector3 pos)
    {
        ParticleSystem splatter = SetupParticles(ParticlePool.getObjectFromPool(), pos).GetComponent<ParticleSystem>();

        var main = splatter.main;
        main.startColor = Color.red;

        splatter.emission.SetBurst(0, new ParticleSystem.Burst(0f, 40, 60));

        var shape = splatter.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Hole")
        {
            other.transform.parent.gameObject.GetComponent<Ring>().Break(transform.position);
            holesInArow++;
            PassedRing?.Invoke();
            if (holesInArow == 3)
            {
                _renderer.material = fastMaterial;
                tr.time = 0.5f;
                tr.widthCurve = bigCurve;
                tr.colorGradient = fastTrailGradient;
            }
        }        
    }

    private GameObject SetupParticles(GameObject particleGameObject, Vector3 pos)
    {
        particleGameObject.GetComponent<ParticleEventCatcher>().isDone = false;
        particleGameObject.transform.position = pos;
        particleGameObject.transform.localScale = Vector3.one;
        particleGameObject.GetComponent<ParticleSystem>().Clear();

        particleGameObject.SetActive(true);

        particleGameObject.GetComponent<ParticleSystem>().Play();
        StartCoroutine(WaitAndPool(particleGameObject));
        return particleGameObject;
    }

    private IEnumerator WaitAndPool(GameObject ParticlesToRemove)
    {
        if(ParticlesToRemove != null)
        {
            while (!ParticlesToRemove.GetComponent<ParticleEventCatcher>().isDone)
            {
                yield return null;
            }
            ParticlesToRemove.GetComponent<ParticleSystem>().Stop();
            ParticlePool.addObjectToPool(ParticlesToRemove);
        }
    }
}
