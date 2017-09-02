using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GridManager : Singleton<GridManager>
{
#region Static variable
    private static Unit unitMoving;
#endregion

    [SerializeField]
    private float timeBeetweenSlot = 1;

    [SerializeField]
    GridData gridData;

    /// <summary>
    /// All slot in this grid
    /// </summary>
    [SerializeField]
    List<Slot> slotsInGrid;


    [SerializeField]
    Collectable collectablePrefab;

    [SerializeField]
    Slot slotPrefab;

    [SerializeField]
    Unit unit;

    [SerializeField]
    RectTransform slotParent;

    [SerializeField]
    private float timeBeetweenCollectibleMin = 5, timeBeetweenCollectibleMax = 10;

    [SerializeField]
    private float timeBeetweenNewSlotMin = 5, timeBeetweenNewSlotMax = 10;

    private float timerNextCollectible;
    private float timerNextSlot;
    private int slotX;
    private int slotY;
    private Vector3 qVector;
    private Vector3 rVector;

    private void Start()
    {
        slotsInGrid = new List<Slot>();
        timerNextCollectible = Time.time + Random.Range(timeBeetweenCollectibleMin, timeBeetweenCollectibleMax);
        timerNextSlot = Time.time + Random.Range(timeBeetweenNewSlotMin, timeBeetweenNewSlotMax);
        Load();
    }

    /// <summary>
    /// Load Map
    /// </summary>
    void Load()
    {
        slotX = Mathf.RoundToInt(slotPrefab.GetComponentInChildren<RectTransform>().sizeDelta.x);
        slotY = Mathf.RoundToInt(slotPrefab.GetComponentInChildren<RectTransform>().sizeDelta.y);

        qVector = new Vector3((float)slotX * -3f / 4, (float)slotY / -2, 0);
        rVector = new Vector3(0, (float)-slotY, 0);
        foreach (GridData.SlotData slotData in gridData.allSlots)
        {
            Slot slot = Instantiate<Slot>(slotPrefab, slotParent);
            slot.Init(slotData);
            slot.transform.localPosition = slotData.q * qVector + slotData.r * rVector;
            slotsInGrid.Add(slot);
        }
        unit.linkedSlot = slotsInGrid[0];


    }

    /// <summary>
    /// Create a new slot
    /// </summary>
    void NewSlot()
    {
        List<Vector2> neighboursNotUse = GetNeighboursEmptySlotByCoordonates(slotsInGrid[Random.Range(0, slotsInGrid.Count - 1)].coordinates);
        if(neighboursNotUse.Count > 0)
        {
            Vector2 coordonate = neighboursNotUse[Random.Range(0, neighboursNotUse.Count)];
            Slot slot = Instantiate<Slot>(slotPrefab, slotParent);
            slot.Init(coordonate);
            slot.transform.localPosition = coordonate.x * qVector + coordonate.y * rVector;
            slotsInGrid.Add(slot);
        }
        
    }

    private void Update()
    {
        if(timerNextCollectible < Time.time)
        {
            timerNextCollectible = Time.time + Random.Range(timeBeetweenCollectibleMin, timeBeetweenCollectibleMax);
            CreateNewCollectable();
        }
        if (timerNextSlot < Time.time)
        {
            timerNextSlot = Time.time + Random.Range(timeBeetweenNewSlotMin, timeBeetweenNewSlotMax);
            NewSlot();
        }
       
    }

    /// <summary>
    /// Generate a new collectable
    /// </summary>
    void CreateNewCollectable()
    {
        Slot slotEmpty = FindSlotWithoutCollectable();
        if(slotEmpty != null)
        {
            Collectable collectable = Instantiate<Collectable>(collectablePrefab);
            slotEmpty.NewCollectable(collectable);
        }
    }


    /// <summary>
    /// Move a unit to a destination
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="slotTarget">Destination</param>
    public void GoToSlot(Unit unit, Slot slotTarget)
    {
        List<Vector3> path = GetPath(slotTarget,  unit.linkedSlot);
        if (path.Count == 0)
            return;
        unitMoving = unit;
        unitMoving.Moving(true);
        Tweener tweener = unit.transform.DOPath(path.ToArray(), path.Count * timeBeetweenSlot, PathType.CatmullRom);
        tweener.onWaypointChange += (int index) =>
        {
            if(index > 0)
                NewWaypoint(GetSlotByPosition(path[index-1]));
        };
        tweener.onComplete += UnitArrived;
    }

    /// <summary>
    /// Hightlight a futur path
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="slotTarget"></param>
    public void PathHightlight(Unit unit, Slot slotTarget)
    {
        List<Slot> slotPath = GetSlotPath(slotTarget, unit.linkedSlot);
        foreach (Slot slot in slotsInGrid)
        {
            slot.OnFocus(slotPath.Contains(slot));
        }
    }


    /// <summary>
    /// Arrived on a new Waypoint
    /// </summary>
    /// <param name="slot"></param>
    private void NewWaypoint(Slot slot)
    {
        unitMoving.linkedSlot = slot;
        slot.UnitArrived(unitMoving);
    }

    /// <summary>
    /// Unit arrived
    /// </summary>
    private void UnitArrived()
    {
        unitMoving.Moving(false);
        unitMoving = null;
        foreach (Slot slot in slotsInGrid)
        {
            slot.OnFocus(false);
        }
    }

    /// <summary>
    /// Generate a path
    /// </summary>
    /// <param name="targetSlot"> Destination Point </param>
    /// <param name="originSlot"> Start point </param>
    /// <returns></returns>
    private List<Vector3> GetPath(Slot targetSlot, Slot originSlot)
    {
        List<Vector3> coordonates = new List<Vector3>();
        Slot slotNextSlot = originSlot;
        int i = 0;
        while (slotNextSlot != targetSlot && i < 50)
        {
            i++;
            Slot slotReturn = GetNearestSlot(slotNextSlot.coordinates, targetSlot.coordinates);
            if(slotReturn && slotReturn != slotNextSlot)
            {
                coordonates.Add(slotReturn.transform.position);
                slotNextSlot = slotReturn;
            }      
        }
        return coordonates;
    }

    private List<Slot> GetSlotPath(Slot targetSlot, Slot originSlot)
    {
        List<Slot> slots = new List<Slot>();
        Slot slotNextSlot = originSlot;
        int i = 0;
        while (slotNextSlot != targetSlot && i < 50)
        {
            i++;
            Slot slotReturn = GetNearestSlot(slotNextSlot.coordinates, targetSlot.coordinates);
            if (slotReturn && slotReturn != slotNextSlot)
            {
                slots.Add(slotReturn);
                slotNextSlot = slotReturn;
            }
        }
        return slots;
    }

    private Slot GetSlotByCoordonates(Vector2 coordonates)
    {
        return slotsInGrid.Find(slot => slot.coordinates.Equals(coordonates));
    }

    private Slot GetSlotByPosition(Vector3 position)
    {
        return slotsInGrid.Find(slot => slot.transform.position == position);
    }

    private List<Slot> GetNeighboursSlotByCoordonates(Vector2 coordonates)
    {
        List<Slot> slotsFind = new List<Slot>();
        foreach (Vector2 neighbour in Unit.neighbours)
        {
            Slot slot = GetSlotByCoordonates(new Vector2(coordonates.x + neighbour.x, coordonates.y + neighbour.y));
            
            if (slot != null)
            {
                slotsFind.Add(slot);
            }
                
        }
        return slotsFind;
    }

    private List<Vector2> GetNeighboursEmptySlotByCoordonates(Vector2 coordonates)
    {
        List<Vector2> slotsFind = new List<Vector2>();
        foreach (Vector2 neighbour in Unit.neighbours)
        {
            Slot slot = GetSlotByCoordonates(new Vector2(coordonates.x + neighbour.x, coordonates.y + neighbour.y));

            if (slot == null)
            {
                slotsFind.Add(new Vector2(coordonates.x + neighbour.x, coordonates.y + neighbour.y));
            }

        }
        return slotsFind;
    }

    private Slot GetNearestSlot(Vector2 coordonates, Vector2 destination)
    {
        List<Slot> slotAround = GetNeighboursSlotByCoordonates(coordonates);
        Slot nearestSlot = null;
        float smallestDistance = float.MaxValue;
        foreach (Slot slot in slotAround)
        {
            float distance = Vector2.Distance(slot.coordinates, destination);
            if(distance < smallestDistance)
            {
                smallestDistance = distance;
                nearestSlot = slot;
            }
        }
        return nearestSlot;
    }


    private Slot FindSlotWithoutCollectable()
    {
        var slots = slotsInGrid.FindAll(slot => !slot.HaveCollectable());
        if (slots.Count > 0)
            return slots[Random.Range(0, slots.Count - 1)];
        else
            return null;
    }
}
