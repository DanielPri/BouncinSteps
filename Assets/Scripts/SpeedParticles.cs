using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedParticles : MonoBehaviour
{

    ParticleSystem ps;
    Ball ballScript;


    private Color defColor;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        ballScript = transform.parent.gameObject.GetComponent<Ball>();
        ballScript.PassedRing += changeEmitAmount;
        defColor = ps.main.startColor.color;
    }

    private void changeEmitAmount()
    {
        var em = ps.emission;
        em.rateOverTimeMultiplier = (ballScript.holesInArow - 1) * 50;

        var main = ps.main;
        if (ballScript.holesInArow >= 3)
        {
            main.startColor = Color.red;
        }
        else
        {
            main.startColor = defColor;
        }

    }
}
