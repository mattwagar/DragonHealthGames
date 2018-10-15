using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public enum BeatType{Linear};
[System.Serializable]
public class Beat
{
    [Range(0,100)] public float StartPositionX;
    [Range(0,100)] public float StartPositionZ;
    // public AnimationCurve Curve;
    public int BeatStart;
    public int BeatEnd;
    public int AudioLength;
    // public Track Track;

    // public Beat(Track track){
    //     Track = track;
    // }
}
