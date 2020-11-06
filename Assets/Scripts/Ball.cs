using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameObject particles;

    [Header("Materials")]
    public Material normalMaterial;
    public Material fastMaterial;

    public float bounceForce = 1;
    
    private Rigidbody rb;
    private Renderer rendr;
    private TrailRenderer tr;

    //Trail sizes
    private AnimationCurve smallCurve;
    private AnimationCurve bigCurve;

    //Trail colors
    private Gradient slowTrailGradient;
    private Gradient fastTrailGradient;


    [HideInInspector]
    public Action<bool> OnEndingReached;
    private int holesInArow = 0;
    // Start is called before the first frame update
    void Start()
    {
        rendr = GetComponent<Renderer>();
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
        col.transform.parent.gameObject.SetActive(false);
        holesInArow = 0;
        rendr.material = normalMaterial;
        tr.time = 0.3f;
        tr.widthCurve = smallCurve;
        tr.colorGradient = slowTrailGradient;
        BigPaintSplatter(col.GetContact(0).point, col.gameObject.transform);
    }

    private GameObject PaintSplatter(Vector3 pos, Transform parent)
    {
        GameObject splatter = Instantiate(particles, pos, Quaternion.identity, parent);
        splatter.GetComponent<ParticleSystem>().Play();
        Destroy(splatter, 4);
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
                rendr.material = fastMaterial;
                tr.time = 0.5f;
                tr.widthCurve = bigCurve;
                tr.colorGradient = fastTrailGradient;
            }
        }        
    }
}
