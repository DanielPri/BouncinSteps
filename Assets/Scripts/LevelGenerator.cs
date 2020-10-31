using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject Structure;
    public GameObject Pillar;
    public List<GameObject> RingPrefabs;
    public Transform Camera;
    public int setupQuantity = 8;
    public float ringOffset = 5;

    private Queue<GameObject> ActiveRings;
    private Queue<GameObject> ActivePillars;
    
    // Start is called before the first frame update

    private Vector3 prevRingPosition = Vector3.zero;

    void Start()
    {
        ActiveRings = new Queue<GameObject>();
        ActivePillars = new Queue<GameObject>();
        SetupLevel();
    }

    // Update is called once per frame
    void Update()
    {
        HandleNextRing();
    }

    private void HandleNextRing()
    {
        if(ActiveRings.Peek().transform.position.y - Camera.position.y > 1.5)
        {
            //TODO consider using object pooling
            Destroy(ActiveRings.Dequeue());
            AddRingToStructure();
        }
    }

    void SetupLevel()
    {
        for(int i=0; i < setupQuantity; i++)
        {
            AddRingToStructure();
        }
    }

    private void AddRingToStructure()
    {
        GameObject ring = Instantiate(RingPrefabs[Random.Range(0, RingPrefabs.Count)], Structure.transform, false);
        GameObject pillar = Instantiate(Pillar, Structure.transform, false);
        ActiveRings.Enqueue(ring);
        print("Ring added");
        ActivePillars.Enqueue(pillar);

        prevRingPosition = ring.transform.position = new Vector3(0, prevRingPosition.y - ringOffset, 0);
        ring.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        pillar.transform.position = new Vector3(0, prevRingPosition.y, 0);
    }
}

