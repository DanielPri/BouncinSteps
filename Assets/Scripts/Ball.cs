using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Action<bool> OnEndingReached;

    public float bounceForce = 1;
    
    private Rigidbody rb;

    private int holesInArow = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(holesInArow < 3)
        {
            if(collision.gameObject.tag == "Floor")
            {
                rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
                holesInArow = 0;
            }
        }
        if (collision.gameObject.tag == "Death")
        {
            print("Game Over!");
            OnEndingReached?.Invoke(false);
        }
        else if(collision.gameObject.tag == "Finish")
        {
            print("Victory!");
            OnEndingReached?.Invoke(true);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Hole")
        {
            Destroy(other.gameObject);
            holesInArow++;
        }
        
    }
}
