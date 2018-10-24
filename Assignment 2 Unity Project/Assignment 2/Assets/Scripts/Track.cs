using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrackSpeed{Whole = 1, Half = 2, Quarter = 4, Eighth = 8, Sixteenth = 16};

[CreateAssetMenu (menuName = "Create/New Track", fileName = "Track.asset")]
public class Track : ScriptableObject {
    public int BPM;
    public TrackSpeed TrackSpeed = TrackSpeed.Quarter;
    public AudioClip Clip;

    //variables for the note spawn points
    public bool NorthSpawn = true;
    public bool SouthSpawn = true;
    public bool CenterSpawn = true;
    public bool EastSpawn = true;
    public bool WestSpawn = true;
    public bool ReturnCenter;

    public string Seed;
    public List<Beat> Beats;

    
}
