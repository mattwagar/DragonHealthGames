using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using FullSerializer;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour {
    //private Scorer scoreTracker = new Scorer();
    public egGame gameManager;
    public egGame scorer;
    public egGame points;
	public int score;
    public string[] GameTextStrings;
    void Awake()
    {
        //print("adding scorer!!!");
        //        scoreTracker = this.gameObject.AddComponent<Scorer>() as Scorer;
    }
	[SerializeField]
    private Text scoreText;
   
    [SerializeField]
    public Text notes;

    public void OnEnable() {
        //print("Game Over Panel");
		score=scorer.score;
		scoreText.text = "You Scored: "+ score;
        //waveText.text = string.Format("{0}/{1} waves cleared!", scorer.WavesWon.ToString(), scorer.WavesSeen.ToString());
        transform.DOScale(1f, .2f);		
	}

    public void saveStats() {
        gameManager.Notes = notes.text;
        //GameObject.Find("GameLogic").GetComponent<egGame>().Notes = notes.text; //store here or in game?
    }

	public void Quit() {
        print("Game Over Quit");
        gameManager.PauseGame();
        saveStats();
        gameManager.EndGame();
        Application.Quit();
    }

    public void MainMenu() {
        print("Game Over Main Menu");
        saveStats();
        gameManager.MainMenu();
    }


	/*
	public string Serialize()
	{
		Debug.Log ("Footer Serialize");
		return JSONSerializer.Serialize(typeof(GameOverPanel), this);
	}

	public object Deserialize(string json)
	{
		Debug.Log ("Footer Deserialize");
		return JSONSerializer.Deserialize(typeof(GameOverPanel), json);
	}
*/

}
