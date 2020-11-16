using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrail : MonoBehaviour
{

    private TrailRenderer tr;
    
    //Trail sizes
    private AnimationCurve smallCurve;
    private AnimationCurve bigCurve;

    //Trail colors
    private Gradient slowTrailGradient;
    private Gradient fastTrailGradient;


    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<TrailRenderer>();
        smallCurve = tr.widthCurve;
        bigCurve = InitializeBigCurve();
        slowTrailGradient = tr.colorGradient;
        fastTrailGradient = InitializeTrailGradient();
    }

    public void setSmall()
    {
        tr.time = 0.3f;
        tr.widthCurve = smallCurve;
        tr.colorGradient = slowTrailGradient;
    }

    public void setBig()
    {
        tr.time = 0.5f;
        tr.widthCurve = bigCurve;
        tr.colorGradient = fastTrailGradient;
    }

    private Gradient InitializeTrailGradient()
    {
        Gradient gradient = new Gradient();

        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.red, 0f), new GradientColorKey(Color.red, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(0.9f, 0f), new GradientAlphaKey(0.5f, 1f) }
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
}
