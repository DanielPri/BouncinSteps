﻿using UnityEngine;

public class ParticleEventCatcher : MonoBehaviour
{
    public bool isDone;

    private void OnParticleSystemStopped()
    {
        isDone = true;
    }
}
