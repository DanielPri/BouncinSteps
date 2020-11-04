using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuStructure : MonoBehaviour
{
    public float rotationSpeed = 1;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
