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
	
	public Track Track;
	
	private List<Beat> beats
	{
		get{return Track.Beats;}
	} 

	public float MinPositionX;
	public float MaxPositionX;

	public float MinPositionZ;
	public float MaxPositionZ;

	void Start()
	{
		initializeBeats();
	}

	void initializeBeats(){
		float distanceX = Mathf.Abs(MinPositionX) + Mathf.Abs(MaxPositionX);
		float distanceZ = Mathf.Abs(MinPositionZ) + Mathf.Abs(MaxPositionZ);
		 
		for(int i = 0; i < beats.Count; i++)
		{
			float StartPositionX = (beats[i].StartPositionX * 0.01f * distanceX) - (distanceX * 0.5f);
			float StartPositionZ = (beats[i].StartPositionZ * 0.01f * distanceZ) - (distanceZ * 0.5f);
			GameObject Collectible = Instantiate(CollectiblePrefab.gameObject, new Vector3(StartPositionX,10,StartPositionZ), Quaternion.identity);
			Collectible.transform.parent = transform;
			setTimeline(Collectible, i);
		} 
	}

	void setTimeline(GameObject Collectible, int index){
		AnimationTrack track = TimelineAsset.CreateTrack<AnimationTrack>(null, "Collectible_"+index);

		TimelineAsset collectibleTimelineAsset = Collectible.GetComponent<PlayableDirector>().playableAsset as TimelineAsset;
		List<TrackAsset> trackAssets = collectibleTimelineAsset.GetOutputTracks().ToList();

		for(int i = 0; i < trackAssets.Count; i++)
		{
			List<TimelineClip> clips = trackAssets[i].GetClips().ToList();
			for(int j = 0; j < clips.Count;  j++)
			{
				TimelineClip clip = track.CreateDefaultClip();
				AnimationPlayableAsset animationPlayableAsset = clip.asset as AnimationPlayableAsset;
				
				animationPlayableAsset.clip = clips[j].animationClip;
			}
		}
	}

}
