using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour {

	private ScoreSystem ScoreSystem;

	egGame game;
	// Use this for initialization
	void Start () {
        ScoreSystem = GameObject.Find("GameLogic").GetComponent<ScoreSystem>();
		game = GameObject.Find ("GameLogic").GetComponent<egGame> ();
	}

	void OnCollisionEnter(Collision collision)
    {
		Debug.LogWarning ("Collectible hit!");
        UpdatePlayerScore();
		
		Destroy(gameObject);
		
    }

    void UpdatePlayerScore()
    {
        ScoreSystem.UpdateScore(2);
    }
}
