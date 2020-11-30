using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class serves as a way to gain a weighted probability.
/// Items with higher probability ranges will be more likely to be selected.
/// </summary>
public class SpawnChance
{
    // int is index of ring, float is its probability
    private Dictionary<int, Range> rings;

    private float chanceSum = 0f;

    // initializes probability range, return the sum of all values
    public float init(List<GameObject> RingPrefabs)
    {
        if (rings == null)
        {
            rings = new Dictionary<int, Range>();
        }
        else
        {
            rings.Clear();
        }

        for (int i = 0; i < RingPrefabs.Count; i++)
        {
            // temp variable
            float chanceValue = RingPrefabs[i].GetComponent<Ring>().spawnChance;

            // The probability range of each item will be from current total + their prob 
            // value, then increment the total for the next item
            Range ringRange = new Range();
            ringRange.min = chanceSum;
            ringRange.max = ringRange.min + chanceValue;

            // Debug.Log("Ring " + i + " Has min - max of " + ringRange.min + " - " + ringRange.max);

            rings.Add(i, ringRange);
            chanceSum += chanceValue;
        }
        return chanceSum;
    }

    // return index of item that contains value in its range
    public int getIndex(float input)
    {
        // can probably be optimised
        // binary search or somethin, but since the list is always gonna be small very minimal returns
        foreach (KeyValuePair<int, Range> ring in rings)
        {
            //  ring.key ring.value
            if (ring.Value.isInRange(input))
            {
                Debug.Log("ring " + ring.Key + " is in range");
                return ring.Key;
            }
        }
        throw new System.Exception("No item contains a range with that value");
    }

    public int getRandomWeightedIndex()
    {
        return getIndex(Random.Range(0, chanceSum));
    }
}