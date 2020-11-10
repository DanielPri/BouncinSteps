using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreYouSure : MonoBehaviour
{
    public Action<bool> finished;
    public void setResetData()
    {
        finished?.Invoke(true);
        gameObject.SetActive(false);
    }

    public void Cancel()
    {
        finished?.Invoke(false);
        gameObject.SetActive(false);
    }
}
