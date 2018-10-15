using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
	
	public Track Track;
	
	private List<Beat> beats
	{
		get{return Track.Beats;}
	} 
	void Start()
	{
		initializeBeats();
	}

	void initializeBeats(){
		float distanceX = Mathf.Abs(MinPositionX) + Mathf.Abs(MaxPositionX);
		float distanceZ = Mathf.Abs(MinPositionZ) + Mathf.Abs(MaxPositionZ);
		 
		for(int i = 0; i < beats.Count; i++)
		{
			setTimeline(beats[i], i);
		} 
	}

	void setTimeline(Beat beat, int index){
		ControlTrack track = TimelineAsset.CreateTrack<ControlTrack>(null, "Collectible_"+index);

		// TimelineAsset collectibleTimelineAsset = CollectiblePrefab.GetComponent<PlayableDirector>().playableAsset as TimelineAsset;
		PlayableDirector collectiblePlayableDirector = CollectiblePrefab.GetComponent<PlayableDirector>();
		// List<TrackAsset> trackAssets = collectibleTimelineAsset.GetOutputTracks().ToList();

		TimelineClip clip = track.CreateDefaultClip();
		clip.duration = collectiblePlayableDirector.duration;

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
			// ControlPlayableAsset
			break;
		}

		controlPlayableAsset.prefabGameObject = CollectiblePrefab;
		controlPlayableAsset.updateParticle = false;
		controlPlayableAsset.updateITimeControl = false;
		controlPlayableAsset.updateDirector = true;

		


		// controlPlayableAsset.CreatePlayable<PrefabControlPlayable>();
		
		// for(int i = 0; i < trackAssets.Count; i++)
		// {
		// 		TimelineClip clip = track.CreateDefaultClip();
		// 		ControlPlayableAsset controlPlayableAsset = clip.asset as ControlPlayableAsset;

		// 		controlPlayableAsset.prefabGameObject



			// List<TimelineClip> clips = trackAssets[i].GetClips().ToList();
			// for(int j = 0; j < clips.Count;  j++)
			// {
			// 	TimelineClip clip = track.CreateDefaultClip();
			// 	AnimationPlayableAsset animationPlayableAsset = clip.asset as AnimationPlayableAsset;
				
			// 	animationPlayableAsset.clip = clips[j].animationClip;
			// }
		// }
	}

}
