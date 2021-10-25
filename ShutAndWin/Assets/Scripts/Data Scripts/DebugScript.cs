using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
    public void Save()
    {
        StatsManager.instance.SaveProgress();
    }

    public void Load()
    {
        StatsManager.instance.LoadProgress();
    }
}
