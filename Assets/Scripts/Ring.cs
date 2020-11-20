using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    public List<GameObject> StaticObjects;
    public GameObject DestructObject;
    
    public float explosionForce = 1.5f;
    public float upwardsExplosionModifier = 0f;

    public bool isBroken = false;

    // replace the platform with a destroyed version
    public void Break(Vector3 explosionCenter)
    {
        if(StaticObjects.Count > 0)
        {
            foreach(GameObject staticObject in StaticObjects)
            {
                print("Deactivating!");
                staticObject.SetActive(false);
            }
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
                Rigidbody rb;
                child.gameObject.layer = LayerMask.NameToLayer("Destructable");
                rb = child.gameObject.GetComponent<Rigidbody>();
                if (rb == null)
                {
                     rb = child.gameObject.AddComponent<Rigidbody>();
                }
                StartCoroutine(PlaceExplosion(rb, explosionCenter));
            }

        }

        isBroken = true;
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
