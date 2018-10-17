using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrackSpeed{Whole = 1, Half = 2, Quarter = 4, Eighth = 8, Sixteenth = 16};

[CreateAssetMenu (menuName = "Create/New Track", fileName = "Track.asset")]
public class Track : ScriptableObject {
    public int BPM;
    public TrackSpeed TrackSpeed = TrackSpeed.Quarter;
    public AudioClip Clip;

    public string Seed;
    public List<Beat> Beats;

    
}
