using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GridManager : Singleton<GridManager>
{
    [SerializeField]
    private float timeBeetweenSlot = 1;

    [SerializeField]
    List<Slot> slotsInGrid;

    private static Unit unitMoving;

    public void GoToSlot(Unit unit, Slot slotTarget)
    {
        List<Vector3> path = GetPath(slotTarget,  unit.linkedSlot);
        if (path.Count == 0)
            return;
        unitMoving = unit;
        Tweener tweener = unit.transform.DOPath(path.ToArray(), path.Count * timeBeetweenSlot, PathType.CatmullRom);
        tweener.onWaypointChange += (int index) =>
        {
            if(index > 0)
                NewWaypoint(GetSlotByPosition(path[index-1]));
        };
        tweener.onComplete += UnitArrived;
    }

    private void NewWaypoint(Slot slot)
    {
        unitMoving.linkedSlot = slot;
    }

    private void UnitArrived()
    {
        unitMoving = null;
    }


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
        Debug.Log(coordonates);
        foreach (Vector2 neighbour in Unit.neighbours)
        {
            Slot slot = GetSlotByCoordonates(new Vector2(coordonates.x + neighbour.x, coordonates.y + neighbour.y));
            
            if (slot != null)
            {
                slotsFind.Add(slot);
                Debug.Log(slot.coordinates);
            }
                
        }
        return slotsFind;
    }

    private Slot GetNearestSlot(Vector2 coordonates, Vector2 destination)
    {
        List<Slot> slotAround = GetNeighboursSlotByCoordonates(coordonates);
        Debug.Log(slotAround.Count);
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

}
