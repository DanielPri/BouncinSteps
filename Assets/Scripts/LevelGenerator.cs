using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject Structure;
    public List<GameObject> RingPrefabs;
    public int setupQuantity = 8;
    public float ringOffset = 5;
    // Start is called before the first frame update

    private Vector3 prevRingPosition = Vector3.zero;

    void Start()
    {
        SetupLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetupLevel()
    {
        for(int i=0; i < setupQuantity; i++)
        {
            print("Adding ring!");
            GameObject ring = GetRandomRing();
            prevRingPosition = ring.transform.position = new Vector3(0, prevRingPosition.y - ringOffset, 0);
            ring.transform.rotation = Quaternion.Euler(0,Random.Range(0f,360f),0);
        }
    }

    private GameObject GetRandomRing()
    {
        return Instantiate(RingPrefabs[Random.Range(0, RingPrefabs.Count)], Structure.transform, false);
    }
}
