using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreSystem : MonoBehaviour {

    public Text scoreTextObject;
    private int currentScore;

    private void Start()
    {
        currentScore = 0;
        scoreTextObject.text = "Score: " + currentScore.ToString();
    }

    public void UpdateScore(int additionalScore)
    {
        currentScore += additionalScore;
        scoreTextObject.text = "Score: " + currentScore.ToString();
    }

}
