using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler
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

    void Start()
    {
        gameObject.name = "Slot : " + coordinates.x +" "+ coordinates.y;
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


    public void OnPointerClick(PointerEventData eventData)
    {
        if (Unit.currentUnitSelect != null)
        {
            currentHoveredSlot = this;
            GridManager.Instance.GoToSlot(Unit.currentUnitSelect, currentHoveredSlot);
        }
       
    }
}
