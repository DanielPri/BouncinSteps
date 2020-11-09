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

    [HideInInspector]
    public Action<bool> OnEndingReached;
    [HideInInspector]
    public Action OnBigImpact;
    private int holesInArow = 0;
    // Start is called before the first frame update
    void Start()
    {
        ParticlePool = new Pool(contactParticles, 6);
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
                PaintSplatter(collision.GetContact(0).point, collision.gameObject.transform);
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
        col.transform.parent.gameObject.SetActive(false);
        holesInArow = 0;
        _renderer.material = normalMaterial;
        tr.time = 0.3f;
        tr.widthCurve = smallCurve;
        tr.colorGradient = slowTrailGradient;
        BigPaintSplatter(col.GetContact(0).point, col.gameObject.transform);
    }

    private GameObject PaintSplatter(Vector3 pos, Transform parent)
    {
        GameObject splatter = ParticlePool.getObjectFromPool();
        splatter.GetComponent<ParticleEventCatcher>().isDone = false;
        splatter.transform.position = pos;
        splatter.transform.localScale = Vector3.one;
        splatter.GetComponent<ParticleSystem>().Clear();
        splatter.SetActive(true);
        splatter.GetComponent<ParticleSystem>().Play();
        StartCoroutine(WaitAndPool(splatter, 4));
        return splatter;
    }

    private void BigPaintSplatter(Vector3 point, Transform transform)
    {
        ParticleSystem splatter = PaintSplatter(point, null).GetComponent<ParticleSystem>();
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
            other.transform.parent.gameObject.SetActive(false);
            holesInArow++;
            if(holesInArow == 3)
            {
                _renderer.material = fastMaterial;
                tr.time = 0.5f;
                tr.widthCurve = bigCurve;
                tr.colorGradient = fastTrailGradient;
            }
        }        
    }

    private IEnumerator WaitAndPool(GameObject ParticlesToRemove, float time)
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
