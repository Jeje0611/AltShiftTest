using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
#region Static Variables

    private static Slot currentHoveredSlot;


#endregion


    public Vector2 coordinates;

    [SerializeField]
    Animator anim;

    [SerializeField]
    RectTransform parentCollectable;

    Collectable colectable;

    public void Init(GridData.SlotData slotData)
    {
        coordinates = new Vector2(slotData.q, slotData.r);
        gameObject.name = "Slot : " + coordinates.x + " " + coordinates.y;
    }

    public void Init(Vector2 coordonate)
    {
        coordinates = coordonate;
        gameObject.name = "Slot : " + coordinates.x + " " + coordinates.y;
    }

    public void UnitArrived(Unit unit)
    {
        if (colectable != null)
            colectable.TakeIt();
    }

    public bool HaveCollectable()
    {
        return colectable != null;
    }

    public void NewCollectable(Collectable newCollectible)
    {
        this.colectable = newCollectible;
        colectable.transform.SetParent(parentCollectable);
        colectable.transform.position = parentCollectable.position;

    }

    public void OnFocus(bool focus)
    {
        anim.SetBool("Focus", focus);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Unit.currentUnitSelect != null)
        {
            currentHoveredSlot = this;
            GridManager.Instance.GoToSlot(Unit.currentUnitSelect, currentHoveredSlot);
            Unit.currentUnitSelect = null;
        }
       
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Unit.currentUnitSelect != null)
        {
            GridManager.Instance.PathHightlight(Unit.currentUnitSelect, this);
        }
    }
}
