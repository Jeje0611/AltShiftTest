﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour, IPointerClickHandler
{
#region Static Variables

    public static Unit currentUnitSelect;

    /// <summary>
    /// Neighbours coordonate for axial coordonate
    /// </summary>
    public static Vector2[] neighbours = new Vector2[] {
        new Vector2 (+1, 0),
        new Vector2 (+1, -1),
        new Vector2 (0, -1),
        new Vector2 (-1, 0),
        new Vector2 (-1, +1),
        new Vector2 (0, +1)
    };

#endregion

    /// <summary>
    /// Unit State
    /// </summary>
    public enum UnitState
    {
        Stay,  
        Move
    }


    [SerializeField]
    Animator animator;

    [SerializeField]
    CanvasGroup canvasGroup;

    [SerializeField]
    RectTransform rectTransform;


    /// <summary>
    /// On this Slot
    /// </summary>
    public Slot linkedSlot;

    private UnitState unitState = UnitState.Stay;

    public void OnPointerClick(PointerEventData eventData)
    {  
        if(unitState == UnitState.Stay)
        {
            currentUnitSelect = this;
            animator.SetTrigger("Focus");
        }
        
    }

    // Change Moving State
    public void Moving(bool moving)
    {
        if (moving)
            unitState = UnitState.Move;
        else
            unitState = UnitState.Stay;
        animator.SetBool("Moving", moving);
    }
}


