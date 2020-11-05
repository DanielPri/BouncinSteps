using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("GameObject Dependencies")]
    public GameObject Structure;
    public GameObject Pillar;
    public List<GameObject> RingPrefabs;
    public GameObject FinalRing;
    public Transform Camera;

    [Header("Level Settings")]
    public int initalQtyOfRings =7;
    public float ringOffset = 5;
    public int LevelLength = 50;

    private Queue<GameObject> ActiveRings;
    private Queue<GameObject> ActivePillars;

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
            Destroy(ActivePillars.Dequeue());
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
        ActivePillars = new Queue<GameObject>();
        LevelLength += level;
        for(int i=0; i < initalQtyOfRings; i++)
        {
            AddRingToStructure(RingPrefabs[Random.Range(0, RingPrefabs.Count)]);
        }
    }

    /// <summary>
    /// Instantiate ring and pillar
    /// </summary>
    /// <param name="ringType"></param>
    private void AddRingToStructure(GameObject ringType)
    {
        GameObject ring = Instantiate(ringType, Structure.transform, false);
        GameObject pillar = Instantiate(Pillar, Structure.transform, false);
        ActiveRings.Enqueue(ring);
        ActivePillars.Enqueue(pillar);

        // update variables needed for next rings
        generatedRingsQty++;
        prevRingPosition = ring.transform.position = new Vector3(0, prevRingPosition.y - ringOffset, 0);
        ring.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        pillar.transform.position = new Vector3(0, prevRingPosition.y, 0);
    }
}

