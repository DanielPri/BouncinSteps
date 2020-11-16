using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallParticles : MonoBehaviour
{
    public GameObject contactParticles;
    [Header("Small Particle qty")]
    public short smallParticlesMin = 5;
    public short smallParticlesMax = 20;
    [Header("Large Particle qty")]
    public short largeParticlesMin = 40;
    public short largeParticlesMax = 60;

    private Pool ParticlePool;
    
    //Particle colors
    private Color SmallParticlesColor;

    // Start is called before the first frame update
    void Start()
    {
        ParticlePool = new Pool(contactParticles, 6);
        var main = contactParticles.GetComponent<ParticleSystem>().main;
        SmallParticlesColor = main.startColor.color;
    }

    public GameObject SetupParticles(GameObject particleGameObject, Vector3 pos)
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

    public GameObject SmallPaintSplatter(Vector3 pos)
    {
        // Setup a particle from the pool
        ParticleSystem splatter = SetupParticles(ParticlePool.getObjectFromPool(), pos).GetComponent<ParticleSystem>();

        // set its color
        var main = splatter.main;
        main.startColor = SmallParticlesColor;

        //set its burst quantity range
        splatter.emission.SetBurst(0, new ParticleSystem.Burst(0f, smallParticlesMin, smallParticlesMax));

        // set its shape
        var shape = splatter.shape;
        shape.shapeType = ParticleSystemShapeType.Cone;

        return splatter.gameObject;
    }

    public void BigPaintSplatter(Vector3 pos)
    {
        ParticleSystem splatter = SetupParticles(ParticlePool.getObjectFromPool(), pos).GetComponent<ParticleSystem>();

        var main = splatter.main;
        main.startColor = Color.red;

        splatter.emission.SetBurst(0, new ParticleSystem.Burst(0f, largeParticlesMin, largeParticlesMax));

        var shape = splatter.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
    }

    private IEnumerator WaitAndPool(GameObject ParticlesToRemove)
    {
        if (ParticlesToRemove != null)
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
