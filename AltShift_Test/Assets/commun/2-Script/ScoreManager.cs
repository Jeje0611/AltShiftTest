using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Score Management (UI + Score)
/// </summary>
public class ScoreManager : Singleton<ScoreManager>
{

    [SerializeField]
    Text textScore;

    private int score = 10;

	// Use this for initialization
	void Start ()
    {
        score = 0;
    }
	

    /// <summary>
    /// Add score on a total score
    /// </summary>
    /// <param name="scoreAdded">score to add
	public void AddScore(int scoreAdded)
    {
        score += scoreAdded;
        SetUI();
    }


    /// <summary>
    /// UI Score Management
    /// </summary>
    void SetUI()
    {
        textScore.text = score.ToString();
    }
}
