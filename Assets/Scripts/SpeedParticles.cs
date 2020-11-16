using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedParticles : MonoBehaviour
{

    ParticleSystem ps;
    Ball ballScript;
    int Intensity = -1;

    private Color defColor;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        ballScript = transform.parent.gameObject.GetComponent<Ball>();
        defColor = ps.main.startColor.color;
    }

    public void changeEmitAmount(bool isPassed)
    {
        if (isPassed)
        {
            Intensity++;
        }
        else
        {
            Intensity = -1;
        }

        var em = ps.emission;
        em.rateOverTimeMultiplier = Intensity * 50;

        var main = ps.main;
        if (Intensity >= 2)
        {
            main.startColor = Color.red;
        }
        else
        {
            main.startColor = defColor;
        }

    }
}
