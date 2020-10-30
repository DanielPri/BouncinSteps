using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private float prevHeight;
    private float minHeight;
    private float camOffset;
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        camOffset = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        // only move camera downwards
        if (player.position.y < minHeight)
        {
            minHeight = player.position.y;
            transform.position = new Vector3(transform.position.x, player.position.y + camOffset, transform.position.z);
        }

    }
}
