using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class AdaptiveDifficulty : MonoBehaviour {

    public bool isAdaptiveOn = false;
    public Track track;
    public BeatmapManager beatmap;


    [Range(0, 5)] public int adaptDifficulty = 5;
    

    private int score;
    private int beatTracker;

    // Use this for initialization
    void Start () {
        
        //print each beat location
        /*foreach (Beat b in track.Beats){
            Debug.Log(b.BeatLocation.ToString());
        }*/



    }

    //update the score
    public void UpdateScore(int currentScore) {
        score = currentScore;
    }

    //add a point to the beatTracker variable to get currect difficulty
    //check if it is equal than the wanted streak # and adaptDifficulty
    public void AddBeatTracker() {
        beatTracker++;
        Debug.Log(beatTracker);

        if ((isAdaptiveOn) && (beatTracker == adaptDifficulty)) {
            AdaptDifficulty();
        }
    }

    //reset the beatTracker variable to 0 if you miss a beat
    public void ResetBeatTracker() {
        beatTracker = 0;
    }

    //code to slowly add more notes
    void AdaptDifficulty() {
        
    }

	// Update is called once per frame
	void Update () {
		
	}




}
