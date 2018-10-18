using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreSystem : MonoBehaviour
{

    #region Variables

    public Text scoreTextObject;
    public Text highScoreTextObject;

    private int currentScore;

    #endregion


    private void Start()
    {
        // Sets highScoreTextObject.text to value stored in PlayerPrefs for key "HighScore" with a default value of 0 if nothing currently exsits.
        highScoreTextObject.text = "High Score: " + PlayerPrefs.GetInt("HighScore", 0).ToString();

        // Initializes score at 0 to start game.
        currentScore = 0;
        scoreTextObject.text = "Score: " + currentScore.ToString();
    }

    // Adds whatever the value of additionalScore is to the currentScore
    // Sets highScore value when currentScore exceeds previous highScore value
    public void UpdateScore(int additionalScore)
    {
        currentScore += additionalScore;
        Debug.Log("Player's score has increased by " + additionalScore + ". The total is now: " + currentScore);
        scoreTextObject.text = "Score: " + currentScore.ToString();

        if (currentScore > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
            highScoreTextObject.text = "High Score: " + currentScore.ToString();
        }
    }

    // Removes current highScore value key
    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
    }


}
