using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    public GameObject StaticObject;
    public GameObject DestructObject;
    
    public float explosionForce = 1.5f;
    public float upwardsExplosionModifier = 0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // replace the platform with a destroyed version
    public void Break(Vector3 explosionCenter)
    {
        if(StaticObject != null)
        {
            StaticObject.SetActive(false);
            DestructObject.SetActive(true);
            foreach (Rigidbody rb in DestructObject.GetComponentsInChildren<Rigidbody>())
            {
                StartCoroutine(PlaceExplosion(rb, explosionCenter));
            }
        }
        else
        {
            MeshRenderer[] activeChildren = GetComponentsInChildren<MeshRenderer>();
            foreach(MeshRenderer child in activeChildren)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Destructable");
                Rigidbody rb = child.gameObject.AddComponent<Rigidbody>();
                StartCoroutine(PlaceExplosion(rb, explosionCenter));
            }

        }
    }

    IEnumerator PlaceExplosion(Rigidbody rb, Vector3 center)
    {
        while(rb == null)
        {
            yield return null;
        }
        rb.AddExplosionForce(explosionForce, center, 0, upwardsExplosionModifier, ForceMode.Impulse);
    }
}
