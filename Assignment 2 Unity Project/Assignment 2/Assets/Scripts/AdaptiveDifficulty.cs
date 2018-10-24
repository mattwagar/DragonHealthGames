using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class AdaptiveDifficulty : MonoBehaviour {

    public bool isAdaptiveOn = false;
    public Track track;
    public BeatmapManager beatmap;
    public GameObject tiles;


    [Range(0, 5)] public int adaptDifficulty = 5;
    [Range(0.1f, 2.0f)] public float tileSize = 1.0f;

    private int score;
    private int beatTracker;

    // Use this for initialization
    void Start () {
        
        //print each beat location
        /*foreach (Beat b in track.Beats){
            Debug.Log(b.BeatLocation.ToString());
        }*/



    }

    //get value from slider on update 
    //resizes the grid and ceiling parent object using the slider value
    public void AdaptTiles(float tileSz){
        tileSize = tileSz;
        Vector3 temp = tiles.transform.localScale;

        temp.x = tileSz;
        temp.z = tileSz;
        tiles.transform.localScale = temp;

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
