using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using eaglib;
//EG REQUIRED
using enableGame;
using UnityEngine.Networking;

/// <summary>
/// Demo Game.  Moves cube lef and right using 2 suki profiles.
/// Demonstrates network connection to EAG launcher, using SUKI, binding parameters to variables.
/// Recording data is in Tracker scene object (not here) which sets up tracker, header, and footer.
/// 
/// </summary>
public class egGame : MonoBehaviour {

    // player note from the game over panel
    // in Citadel we store the game information such as score and stats in different placese, we may want to have a script 
    // or a singleton that store all those information (so the tracker footer can retrieve those information directly by it)
    public string Notes;   //Player notes to be recorded in footer for portal
	public int score = 0;  // only non-zero scores show up on portal

	public string MenuScene = "eag_MainMenu";
	public string GameScene = "eag_KickCubeGame";

	public Transform PlayerObject;  //the player object
	float startTime; //start time of game

	public static bool isPaused;

	void Awake () {
		//PlayerPrefs.SetString(egParameterStrings.LAUNCHER_ADDRESS,"10.0.0.62");
		egAwake ();
		Physics.gravity = new Vector3 (0.0f, Gravity, 0.0f);

	}
	
	// Update is called once per frame
	void Update () {
		egUpdate ();
	}

	bool gameStarted = false;
	bool gameOver = false;
	public float duration = 0;

	/// <summary>
	/// Main game loop. Checks SUKI Input, updates game time, etc.
	/// </summary>
    private void FixedUpdate()
    {
		if (gameOver)
			return;
		if (!gameStarted) {  //is level just starting?
			gameStarted = true;
			startTime = Time.time;
			egBeginSession (); 
		}
		duration = Time.time - startTime;			
		if (duration >= GameLength) {  //is game time over?
			EndGame();
		}
		//Get translated game input from SUKI
		egGetSukiInput ();
    }

    /// <summary>
    /// Pauses game time and audio
    /// </summary>
	public void PauseGame() {
		//print("Game is Paused...");
		isPaused = true;
		Time.timeScale = 0;
		Time.fixedDeltaTime = 0;
		AudioListener.volume = 0;
	}

    /// <summary>
    /// Unpauses game time and audio
    /// </summary>
	public void UnPauseGame (){
		//print("Unpause");
		isPaused = false;
		Time.timeScale = 1.0f;
		Time.fixedDeltaTime = 0.02f;
		AudioListener.volume = 1.0f;
	}

    /// <summary>
    /// Quit the current gamesession (tracking).
    /// </summary>
	public void EndGame() {
		PauseGame ();
		gameOver = true;
        UnPauseGame();  //must start up unity time again so DOTweens work
		egEndSession ();
	}

    /// <summary>
    /// End the session and Load the main menu scene.
    /// </summary>
	public void MainMenu()
	{
		print ("MainMenu");
        UnPauseGame();  //must start up unity time again so DOTweens work
        egEndSession();

		#if UNITY_ANDROID
		SceneManager.LoadScene(MenuScene);
		#else
		SceneManager.LoadScene(MenuScene);
		#endif
	}

    /// <summary>
    /// End current session and Reload game
    /// </summary>
	public void ReloadGame ()
	{
		print ("ReloadGame");
        UnPauseGame();  //must start up unity time again so DOTweens work
        egEndSession();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

    /// <summary>
    /// Loads next game level.
    /// </summary>
	public void NextGame ()
	{
		print ("NextGame");
        UnPauseGame();  //must start up unity time again so DOTweens work
        egEndSession ();
        SceneManager.LoadScene(GameScene);
	}

	///////////////////////////////////////////////////////////////////////////////
	///////////////////////////////////////////////////////////////////////////////
	/// BEGIN ENABLEGAMES REQUIRED CODE
	/// </summary>
	/// 
	/// 
	/// 
	public SkeletonData Skeleton;  		//holds the body data for the avatar
	public NetworkSkeleton netskeleton; //connects avatar to EAG launcher
	private Suki.SukiInput suki = null; //maps avatar body data to game input

	//egFloat,etc. are custom variables that can be attached to parameters in the settings menu and portal
	//They are attached to the parameters in the egAwake function below.
	egFloat Speed=1.0f;		//speed of player
	egFloat Gravity=-1.0f;	//falling cylinder's gravity (-1.0 is unity default)
	egInt GameLength=300; 	//in seconds

	// Use this for initialization
	void egAwake () {
		print ("egAwake");
		// initialize SUKI
		if (Skeleton == null)
			Skeleton = GameObject.Find ("Tracking Avatar").GetComponent<SkeletonData> ();
		if (netskeleton == null)
			netskeleton = GameObject.Find ("Tracking Avatar").GetComponent<NetworkSkeleton> ();
		suki = Suki.SukiInput.Instance;
		suki.Skeleton = Skeleton;
		print ("egAwake:Trying to connect...");

		// connect the client skeleton to the server skeleton (running in the enablegames launcher app)
		string address = PlayerPrefs.GetString(egParameterStrings.LAUNCHER_ADDRESS);
		print ("Address= " + address);
		NetworkClientConnect.Instance.Connect (address);
		print ("egAwake:after connect.");

		// Bind Speed to the variable "STARTING SPEED" from the settings menu
		//NOTE:Binding will be skipped if ParameterHandler not loaded (i.e. running this scene 
		//without first running MainMenu scene)
		//Also, parameters must be added to DefaultParameters.json file (located in StreamingAssets folder).
		VariableHandler.Instance.Register (ParameterStrings.STARTING_SPEED, Speed);
		VariableHandler.Instance.Register (ParameterStrings.GRAVITY, Gravity);
		VariableHandler.Instance.Register (egParameterStrings.GAME_LENGTH, GameLength);
		print ("Speed=" + Speed);
		print ("Gravity=" + Gravity);
		print ("GameLength=" + GameLength);
	}

	// Update is called once per frame
	void egUpdate () {
		// Return to main menu
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			EndGame ();
		}
	}

	void egBeginSession()
	{
		Tracker.Instance.BeginTracking ();
	}

	void egEndSession()
	{
		Tracker.Instance.Interrupt((int)egEvent.Type.CustomEvent, "GameEnd");
		Tracker.Instance.StopTracking();
		NetworkClientConnect.Instance.Disconnect(); // this will disconnect form the avatar server! remember to disconnect each time you change the time scale or you change scene
	}

	float timeSinceLastLaneMove = 0f;
	/// <summary>
	/// Main game loop. Checks SUKI Input, updates game time, etc.
	/// </summary>
	private void egGetSukiInput()
	{
		//print ("egGetSukiInput-------------------");
		timeSinceLastLaneMove += Time.deltaTime;
		/*
			float duration = Time.time - startTime;
			if (duration >= GameLength)  //is game time over?
				showGameOverPanel ();
			timeSinceLastLaneMove += Time.deltaTime;
			*/

		//Get translated game input from SUKI
		// no-op if SUKI is not currently giving us input data

		/*NO LONGER NEED NETSKELETON TO KNOW IF CONNECTED..USES MOVEMENT FROM T-POSE INSTEAD (suki.Updating)
		//print("Game:FixedUpdate:" + suki.Updating);
		if (netskeleton && netskeleton.moving)
		{
			print ("netskel moving:" + suki.Skeleton.Moving);
			suki.Skeleton.moving = true;
			suki.Skeleton.resetMinMax = true;
		}
		*/
		if (!suki.Updating)
		{
				print("Game:suki not updating.");
			return;
		}
//		return;
		///
		/// Read the various Suki inputs (depending on what suki file was loaded)
		/// Below contains examples for different types of input, including joint angles, bone positions, etc.
		/// 
		/// 
		// read the placement range input and move the cube
		//elbow angle suki schema profile is set as "placement", but probably should use better name.
		//In X-Z movement, can be used for Z-movement together with "joystick" for X
		if (suki.RangeExists("placement"))  
		{
			// we can use a range value as a placement to move left and right
			float range = suki.GetRange("placement");
			// convert 0f to 1f to -1f to 1f
			float xPercent = (range * 2) - 1f;
			//print("placement mode:" + range + ":" + xPercent);
			// add a deadzone of +/- %
			float deadzone = 0.2f;
			if (xPercent > -deadzone && xPercent < deadzone)
			{
				xPercent = 0f;
			}
			// move the object
            Vector3 pos = PlayerObject.transform.localPosition;  //REPLACE PlayerObject with whatever object or vector you want to be updated
			pos.x = pos.x + (xPercent * Speed/40); // we use speed as a position scaler
			PlayerObject.transform.localPosition = pos;
		}
		//shoulder profile is set as "joystick"
		//In X-Z movement, can be used for X-movement togther with "placement" for Z.
		if (suki.RangeExists("joystick"))
		{
			// we can use a range value as a placement to move left and right
			float range = suki.GetRange("joystick");
			// convert 0f to 1f to -1f to 1f
			float xPercent = (range * 2) - 1f;
			//print("joysick mode:" + range + ":" + xPercent);
			// move the object
			float deadzone = 0.2f;
			if (xPercent > -deadzone && xPercent < deadzone)
			{
				xPercent = 0f;
			}

			Vector3 pos = PlayerObject.transform.localPosition; //REPLACE PlayerObject with whatever object or vector you want to be updated
			pos.x = pos.x + (xPercent * Speed/40); // we use speed as position scaler
			PlayerObject.transform.localPosition = pos;
		}

		//moving in discrete steps/lanes
		if (suki.SignalExists("moveLeft") && suki.SignalExists("moveRight"))
		{
			// we can use a pair of triggers to move left or move right
			bool moveLeft = suki.GetSignal("moveLeft");
			bool moveRight = suki.GetSignal("moveRight");

			Vector3 pos = PlayerObject.transform.localPosition;

			// only if there is a direction to move, and it's been some time since our last move
			// Instead of changing the speed of the movement here we change the pause between movements
			if ((!moveLeft && !moveRight) || (moveLeft && moveRight) || (timeSinceLastLaneMove < 1 / Speed)) // we use speed as a time scaler
			{
				return;
			}
			else if (moveLeft)
			{
				pos.x = (pos.x - 0.2f);
			}
			else if (moveRight)
			{
				pos.x = (pos.x + 0.2f);
			}
			PlayerObject.transform.localPosition = pos; //REPLACE PlayerObject with whatever object or vector you want to be updated
			timeSinceLastLaneMove = 0f;

		}
		//using foot or hand x-y position to control player position
		//You could also use each independently as Kollect does to control the hand/footprints.
		if (suki.Location2DExists ("leftfoot") || suki.Location2DExists ("rightfoot") || suki.Location2DExists ("lefthand") || suki.Location2DExists ("righthand")) {
			Vector2 fpos;
			if (suki.Location2DExists ("leftfoot"))
				fpos = suki.GetLocation2D ("leftfoot");
			else if (suki.Location2DExists ("rightfoot"))
				fpos = suki.GetLocation2D ("rightfoot");
			else if (suki.Location2DExists ("lefthand"))
				fpos = suki.GetLocation2D ("lefthand");
			else if (suki.Location2DExists ("righthand"))
				fpos = suki.GetLocation2D ("righthand");
			else
				fpos = new Vector2 ();
			Vector3 pos = PlayerObject.transform.localPosition; //REPLACE PlayerObject with whatever object or vector you want to be updated
			// convert 0f to 1f to -1f to 1f
			float xPercent = (fpos.x * 2) - 1f;
			float yPercent = (fpos.y * 2) - 1f;
			float weight = 10f;
			pos.x = (pos.x * (weight-1) + (xPercent * Speed*4))/weight; // we use speed as position scaler
			pos.y = (pos.y * (weight-1) + (yPercent * Speed*4))/weight; // we use speed as position scaler
			//pos.x = pos.x + (fpos.x * Speed/40); // we use speed as position scaler
			//PlayerObject.transform.position = Vector3.Lerp(LeftFoot.transform.position, new Vector3(newX, newY, newZ), 1f);
			PlayerObject.transform.localPosition = pos;
		}
		checkRange ();
	}
	void checkRange()
	{
		float maxX=4f, maxY= 3f;
		Vector3 pos = PlayerObject.transform.localPosition;  //REPLACE PlayerObject with whatever object or vector you want to be updated
		if (pos.x> maxX)
			pos.x=maxX;
		if (pos.x< -maxX)
			pos.x= -maxX;
		if (pos.y> maxY)
			pos.y=maxY;
		if (pos.y< -maxY)
			pos.y= -maxY;
		PlayerObject.transform.localPosition = pos;

	}
		///
		/// END ENABLEGAMES REQUIRED CODE
		///////////////////////////////////////////////////////////////////////////////


}
