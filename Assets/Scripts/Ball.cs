using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Action OnVictoryReached;

    public float bounceForce = 1;
    
    private Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
        }
        if(collision.gameObject.tag == "Finish")
        {
            print("Victory!");
            OnVictoryReached?.Invoke();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Hole")
        {
            Destroy(other.gameObject);
        }
        

    }
}
