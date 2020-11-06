using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    private float prevHeight;
    private float minHeight;
    private float camOffset;
    
    private Transform playerTransform;
    private GameObject activeCamera;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = player.transform;
        camOffset = transform.position.y;
        player.GetComponent<Ball>().OnBigImpact += CameraShake;
        activeCamera = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // only move camera downwards
        if (playerTransform.position.y < minHeight)
        {
            minHeight = playerTransform.position.y;
            transform.position = new Vector3(transform.position.x, playerTransform.position.y + camOffset, transform.position.z);
        }

    }

    private void CameraShake()
    {
        StartCoroutine(CameraShaker(0.3f, 0.3f));
    }

    IEnumerator CameraShaker(float duration, float magnitude)
    {
        Vector3 originalPos = activeCamera.transform.localPosition;
        float elapsed = 0.0f;
        while(elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            activeCamera.transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }
        activeCamera.transform.localPosition = originalPos;
    }
}
