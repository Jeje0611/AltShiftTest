using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour, IPointerClickHandler
{
#region Static Variables

    public static Unit currentUnitSelect;

    public static Vector2[] neighbours = new Vector2[] {
        new Vector2 (+1, 0),
        new Vector2 (+1, -1),
        new Vector2 (0, -1),
        new Vector2 (-1, 0),
        new Vector2 (-1, +1),
        new Vector2 (0, +1)
    };

    public static Vector2[] round = new Vector2[] {
        new Vector2 (-1, 0),
        new Vector2 (1, -1),
        new Vector2 (+1, 0),
        new Vector2 (0, 1),
        new Vector2 (-1, 1),
        new Vector2 (-1, 0)
    };

#endregion

    [SerializeField]
    Animator animator;

    [SerializeField]
    CanvasGroup canvasGroup;

    [SerializeField]
    RectTransform rectTransform;

    public Slot linkedSlot;

    public void OnPointerClick(PointerEventData eventData)
    {  
        currentUnitSelect = this;
        Debug.Log(currentUnitSelect.linkedSlot.coordinates);
    }
}


