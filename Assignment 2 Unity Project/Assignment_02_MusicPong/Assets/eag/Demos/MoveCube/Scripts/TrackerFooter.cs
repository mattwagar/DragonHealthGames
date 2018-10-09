using UnityEngine;
using System.Collections;
using FullSerializer;
using System;
using System.Collections.Generic;

/// <summary>
/// Footer message that will be saved in the session json file
/// </summary>
public class FooterMessage : EnableSerializableValue
{

    /// <summary>
    /// abstraction of the skeletonData bone structure
    /// </summary>
    public struct BoneData
    {
        public string boneName;
        // public Vector3 bonePosition; //starting position of the bone
        public Vector3 bonePositionMin;
        public Vector3 bonePositionMax;
    }

    /// <summary>
    /// abstraction of the sekeletonData joint structure
    /// </summary>
    public struct JointData
    {
        public string jointName;
        // public float jointAngle; //starting position of the joint
        public float jointAngleMin;
        public float jointAngleMax;
    }

    /// <summary>
    /// the join of bone and joint represtn the skeletonData
    /// </summary>
    public struct structSkeletonData
    {
        public List<BoneData> bonesData;
        public List<JointData> jointsData;
    }
    /// <summary>
    /// struct that contains the detailed score points, it change in every game
    /// </summary>
    public struct ScoreDetails
    {
       
    }


    [fsProperty]
    public int score;

    [fsProperty]
    public structSkeletonData skeletonData;

    [fsProperty]
    public ScoreDetails scoreDetails;

    /// <summary>
    /// Game length (milliseconds)
    /// </summary>
    [fsProperty]
    int gameLength;

    [fsProperty]
    public string playerPerformance;

	[fsProperty]
	public string playerNotes;

    GameObject player;
    GameObject playerAvatar;
	GameObject gameOverPanel;

    void Awake()
    {
        skeletonData = new structSkeletonData();
        skeletonData.bonesData = new List<BoneData>();
        skeletonData.jointsData = new List<JointData>();
    }

    public FooterMessage()
	{
		Awake ();
		/*
        Collecting information from other scripts to write in the footer message
        */
		player = GameObject.Find ("Player");
		//GameObject TimerObject = GameObject.Find ("Timer");
		/*
        Player performances changes on each game
         */
//        if (player != null)
		{
			//pointsSystem Scorer = player.GetComponent<pointsSystem>();
			egGame Scorer = GameObject.Find ("GameLogic").GetComponent<egGame> (); //placeholder, change it with the scipt where the score is kept
           
			if (Scorer == null) {
				score = 1; //Scorer.points; //zero score won't show footer info on web portal
				playerPerformance = "Good"; // Resume the performace
				gameLength = 1000;
			} else {
				gameLength = (int) Scorer.duration * 1000;
				score = Scorer.score; //GameObject.Find ("GameOverPanel").GetComponent<GameOverPanel> ().score;
				playerPerformance = ((score < 50) ? "Good" : "Great!"); // Resume the performace
				GameObject gop = GameObject.Find ("GameOverPanel");
				if (gop !=null)
				playerNotes = gop.GetComponent<GameOverPanel> ().notes.text;
			}
		}

//        if (TimerObject != null)
		{
			//egGame Timer = null; //placeholder, change it with the scipt where the game time is kept
            //if (Timer != null)
			{
				// We transform the game lenght to milliseconds
				/*gameLength = Int32.Parse(Timer.gameLengthInMinutes)*60000;
                gameLength += Int32.Parse(Timer.gameLengthInSeconds)*1000;*/
//                gameLength = (int)((RangeParameter)ParameterHandler.Instance.AllParameters[0].GetParameter(egParameterStrings.PHASE_TIMER)).Value * 60 * 1000;
			}
		}
		GetSkeletonData ();
	}

	/// <summary>
	/// Gets the skeleton data.
	/// </summary>
	void GetSkeletonData()
	{
		playerAvatar = GameObject.Find ("Tracking Avatar");
        /*
        Collecting information from the SkeletonData.cs about the avatar angles and bones (min, max)
         */
        if (playerAvatar != null)
        {
            SkeletonData SkeletonStats = playerAvatar.GetComponent<SkeletonData>();
            if (SkeletonStats != null)
            {
				Debug.Log ("Footer has skeleton data");
                BoneData bone = new BoneData();
                JointData joint = new JointData();
                //skeletonData.bonesData = new List<BoneData>();
                //skeletonData.jointsData = new List<JointsData>();
                for (int i = 0; i < SkeletonStats.boneNames.Length; i++)
                {
                    bone.boneName = SkeletonStats.boneNames[i];
                    //bone.bonePosition = SkeletonStats.bonePos[i];
                    bone.bonePositionMax = SkeletonStats.bonePosMax[i];
                    bone.bonePositionMin = SkeletonStats.bonePosMin[i];
                    //Debug.Log("bone="+bone);
                    //Debug.Log("boneName="+bone.boneName);
                    skeletonData.bonesData.Add(bone);
                }
                for (int i = 0; i < SkeletonStats.jointNames.Length; i++)
                {
                    joint.jointName = SkeletonStats.jointNames[i];
                    //joint.jointAngle = SkeletonStats.jointAng[i];
                    joint.jointAngleMax = SkeletonStats.jointAngMax[i];
                    joint.jointAngleMin = SkeletonStats.jointAngMin[i];
                    skeletonData.jointsData.Add(joint);
                }
            }
        }
    }

    public string Serialize()
    {
		Debug.Log ("Footer Serialize");
        return JSONSerializer.Serialize(typeof(FooterMessage), this);
    }

    public object Deserialize(string json)
    {
		Debug.Log ("Footer Deserialize");
        return JSONSerializer.Deserialize(typeof(FooterMessage), json);
    }
}

/// <summary>
/// Tracker message for the end of the game with some information about the game stats and player achievements.
///  This is needed for the summary info for the web portal session
/// Added as component in scene's Tracker Object (Tracker,TrackerHeader, ParameterTracker, TrackerFooter)
/// </summary>
// Serialized data... will get used someday, but the warnings are unncessary
#pragma warning disable 414
public class TrackerFooter : TrackerComponent
{
    // Use this for initialization
    FooterMessage fm;
    void Awake()
    {
//        fm = new FooterMessage();
        TrackModuleOnEvent(new TrackerModule("Footer", FooterMessage));
    }


    EnableSerializableValue FooterMessage()
    {
		Debug.Log ("creating Footer message");
        return new FooterMessage();
    }
}
