using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable Object with default map information
/// </summary>
[CreateAssetMenu()]
public class GridData : ScriptableObject
{
    [System.Serializable]
    public class SlotData
    {
        public int q;
        public int r;
    }


    public List<SlotData> allSlots;
}
