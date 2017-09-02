using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
