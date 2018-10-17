using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BeatLocation{Center=0,North,South,East,West};
[System.Serializable]
public class Beat
{
    // [Range(0,100)] public float StartPositionX;
    // [Range(0,100)] public float StartPositionZ;
    // public AnimationCurve Curve;
    public BeatLocation BeatLocation;
    public int BeatStart;
    public int BeatEnd;
    public int AudioLength;

    public Beat(BeatLocation beatLocation, int beatStart, int beatEnd){
        BeatLocation = beatLocation;
        BeatStart = beatStart;
        BeatEnd = beatEnd;

    }
    // public Track Track;

    // public Beat(Track track){
    //     Track = track;
    // }
}
