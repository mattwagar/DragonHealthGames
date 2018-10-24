using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Linq;

public class BeatmapManager : MonoBehaviour {
	public AudioSource AudioSource;
	public TimelineAsset TimelineAsset;
	public PlayableDirector PlayableDirector;
	public GameObject CollectiblePrefab;
	public GameObject CenterZone;
	public GameObject NorthZone;
	public GameObject SouthZone;
	public GameObject EastZone;
	public GameObject WestZone;

	public GameObject CenterTile;
	public GameObject NorthTile;
	public GameObject SouthTile;
	public GameObject EastTile;
	public GameObject WestTile;

	public Toggle tCenter;
	public Toggle tUp;
	public Toggle tDown;
	public Toggle tLeft;
	public Toggle tRight;

	
	public Track Track;

	
	private List<Beat> beats
	{
		get{return Track.Beats;}
	}
	
	void OnEnable()
	{
		SetEnableGrid();
		Track.GenerateBeatmap();
		SetTrackClip();
		InitializeBeats();
	}
	void Start()
	{
		PlayableDirector.Play();
	}

	void SetTrackClip()
	{
		AudioSource.clip = Track.Clip;
		AudioTrack track = TimelineAsset.CreateTrack<AudioTrack>(null, "TrackAudio");

		track.CreateClip(AudioSource.clip);

	}

	void InitializeBeats(){
		 
		for(int i = 0; i < beats.Count; i++)
		{
			ConvertBeatToControlTrack(beats[i], i);
		}
	}

	void ConvertBeatToControlTrack(Beat beat, int index){
		ControlTrack track = TimelineAsset.CreateTrack<ControlTrack>(null, "Collectible_"+index);

		TimelineAsset timelineAsset = CollectiblePrefab.GetComponent<PlayableDirector>().playableAsset as TimelineAsset;

		TimelineClip clip = track.CreateDefaultClip();
		clip.start = beat.BeatStart * (60f / Track.BPM / ((float)Track.TrackSpeed/4f));
		
		float beatDuration  = ((beat.BeatEnd - beat.BeatStart) * (60f / Track.BPM / ((float)Track.TrackSpeed/4f)));
		float collectibleDuration = (float)timelineAsset.duration;
		
		clip.timeScale = timelineAsset.duration / beatDuration;

		clip.duration = beatDuration;

		ControlPlayableAsset controlPlayableAsset = clip.asset as ControlPlayableAsset;

		controlPlayableAsset.sourceGameObject.exposedName = UnityEditor.GUID.Generate ().ToString ();
		switch(beat.BeatLocation)
		{
			case BeatLocation.Center:
			PlayableDirector.SetReferenceValue(controlPlayableAsset.sourceGameObject.exposedName, CenterZone);
			break;
			case BeatLocation.North:
			PlayableDirector.SetReferenceValue(controlPlayableAsset.sourceGameObject.exposedName, NorthZone);
			break;
			case BeatLocation.South:
			PlayableDirector.SetReferenceValue(controlPlayableAsset.sourceGameObject.exposedName, SouthZone);
			break;
			case BeatLocation.East:
			PlayableDirector.SetReferenceValue(controlPlayableAsset.sourceGameObject.exposedName, EastZone);
			break;
			case BeatLocation.West:
			PlayableDirector.SetReferenceValue(controlPlayableAsset.sourceGameObject.exposedName, WestZone);
			break;
		}

		controlPlayableAsset.prefabGameObject = CollectiblePrefab;
		controlPlayableAsset.updateParticle = false;
		controlPlayableAsset.updateITimeControl = false;
		controlPlayableAsset.updateDirector = true;
	}

	void OnApplicationQuit(){
		List<TrackAsset> tracks = TimelineAsset.GetOutputTracks().ToList();
		for(int i = tracks.Count-1; i >= 0; i--)
		{
			TimelineAsset.DeleteTrack(tracks[i]);
		}
	}

	public void PauseTimeline(){
		PlayableDirector.Pause();
	}

	public void UnPauseTimeline(){
		SetEnableGrid();
		
		TimelineAsset timelineAsset = CollectiblePrefab.GetComponent<PlayableDirector>().playableAsset as TimelineAsset;

		List<TrackAsset> tracks = TimelineAsset.GetOutputTracks().ToList();
		for(int i = tracks.Count-1; i >= 0; i--)
		{
				TimelineAsset.DeleteTrack(tracks[i]);
		}
		
		double tempTime = PlayableDirector.time;
		Track.GenerateBeatmap();
		SetTrackClip();
		InitializeBeats();
		PlayableDirector.time = tempTime;
		PlayableDirector.Play();
	}


	public void SetEnableGrid()
	{
		if(tCenter.isOn){
			CenterTile.SetActive(true);
		
		} else {
			CenterTile.SetActive(false);
		}
		if(tUp.isOn){
			NorthTile.SetActive(true);
			
		} else {
			NorthTile.SetActive(false);
		}
		if(tDown.isOn){
			SouthTile.SetActive(true);
			
		} else {
			SouthTile.SetActive(false);
		}
		if(tLeft.isOn){
			EastTile.SetActive(true);
			
		} else {
			EastTile.SetActive(false);
		}
		if(tRight.isOn){
			WestTile.SetActive(true);
			
		} else {
			WestTile.SetActive(false);
		}
		Debug.LogWarning("tCenter.isOn"+tCenter.isOn);
		Track.setSpawnTiles(tCenter.isOn, tUp.isOn, tDown.isOn, tLeft.isOn, tRight.isOn);
	}


}
