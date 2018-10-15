using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrackSpeed{Whole, Half, Quarter, Eighth, Sixteenth};

[CreateAssetMenu (menuName = "Create/New Track", fileName = "Track.asset")]
public class Track : ScriptableObject {
    public int BPM;
    public TrackSpeed TrackSpeed = TrackSpeed.Quarter;
    public AudioClip Clip;
    public List<Beat> Beats;

    
}
