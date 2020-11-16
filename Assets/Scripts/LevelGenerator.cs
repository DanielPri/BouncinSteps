using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [Header("GameObject Dependencies")]
    public GameObject Structure;
    public List<GameObject> RingPrefabs;
    public GameObject FinalRing;
    public Transform Camera;

    [Header("Level Settings")]
    public int initalQtyOfRings =7;
    public float ringOffset = 5;
    public int LevelLength = 50;

    private Queue<GameObject> ActiveRings;
    private Queue<GameObject> BreakRings;

    private int generatedRingsQty = 0;
    private bool isEndGenerated = false;
    // Start is called before the first frame update

    private Vector3 prevRingPosition = Vector3.zero;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        HandleNextRing();
    }

    /// <summary>
    /// Remove unseen ring and add next ring until the last ring is reached
    /// </summary>
    private void HandleNextRing()
    {
        // If head of queue is far enough above the camera, destroy it
        if(ActiveRings.Count > 0 && ActiveRings.Peek().transform.position.y - Camera.position.y > 1.5)
        {
            //TODO consider using object pooling instead of destroying
            Destroy(ActiveRings.Dequeue());
            if(generatedRingsQty < LevelLength)
            {
                AddRingToStructure(RingPrefabs[Random.Range(0, RingPrefabs.Count)]);
            }
            else if(!isEndGenerated)
            {
                AddRingToStructure(FinalRing);
                isEndGenerated = true;
            }
            
        }
    }

    /// <summary>
    /// prepares the first few rings (the ones that are visible at first)
    /// </summary>
    public void SetupLevel(int level)
    {
        ActiveRings = new Queue<GameObject>();
        BreakRings = new Queue<GameObject>();
        LevelLength += level;

        // First ring is guaranteed safe, the hole wont be below player
        GameObject firstRing = AddRingToStructure(RingPrefabs[0]);
        firstRing.transform.rotation = Quaternion.Euler(0, Random.Range(-160f,70f), 0);

        // then generate the next initial rings with random rotations
        for (int i=0; i < initalQtyOfRings-1; i++)
        {
            AddRingToStructure(RingPrefabs[Random.Range(0, RingPrefabs.Count)]);
        }
    }

    public void BreakTopRing(Vector3 pos)
    {
        Ring ring = BreakRings.Dequeue().GetComponent<Ring>();
        if (!ring.isBroken)
        {
            ring.Break(pos);
        }

    }

    /// <summary>
    /// Instantiate ring
    /// </summary>
    /// <param name="ringType"></param>
    private GameObject AddRingToStructure(GameObject ringType)
    {
        GameObject ring = Instantiate(ringType, Structure.transform, false);
        ActiveRings.Enqueue(ring);
        BreakRings.Enqueue(ring);

        // update variables needed for next rings
        generatedRingsQty++;
        prevRingPosition = ring.transform.position = new Vector3(0, prevRingPosition.y - ringOffset, 0);
        ring.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        return ring;
    }
}

