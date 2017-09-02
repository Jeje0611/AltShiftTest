using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    /// <summary>
    /// Score to Add
    /// </summary>
    [SerializeField]
    int scoreValue = 100;

    /// <summary>
    /// Method called when the collectible taked
    /// </summary>
    public virtual void TakeIt()
    {
        ScoreManager.Instance.AddScore(scoreValue);
        DestroyCollectible();
    }

    public void DestroyCollectible()
    {
        Destroy(gameObject);
    }
}
