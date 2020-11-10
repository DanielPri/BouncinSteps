using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level;
    public float rotationSensitivity;

    public PlayerData(int givenLevel, float sens)
    {
        level = givenLevel;
        rotationSensitivity = sens;
        // set up all data members
    }
}
