using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor (typeof (Track))]
public class TrackEditor : Editor {

	SerializedProperty _BPM;
	SerializedProperty _TrackSpeed;
	SerializedProperty _clip;
	SerializedProperty _seed;
    SerializedProperty _NorthSpawn;
    SerializedProperty _SouthSpawn;
    SerializedProperty _CenterSpawn;
    SerializedProperty _EastSpawn;
    SerializedProperty _WestSpawn;

	private ReorderableList _myList;

	void OnEnable()
    {
        _BPM = serializedObject.FindProperty("BPM");
        _TrackSpeed = serializedObject.FindProperty("TrackSpeed");
        _clip = serializedObject.FindProperty("Clip");
        _seed = serializedObject.FindProperty("Seed");


        _NorthSpawn = serializedObject.FindProperty("NorthSpawn");
        _SouthSpawn = serializedObject.FindProperty("SouthSpawn");
        _CenterSpawn = serializedObject.FindProperty("CenterSpawn");
        _EastSpawn = serializedObject.FindProperty("EastSpawn");
        _WestSpawn = serializedObject.FindProperty("WestSpawn");


        // Beats = serializedObject.FindProperty("Beats");

        _myList = new ReorderableList (serializedObject, serializedObject.FindProperty ("Beats"), true, true, true, true);
		_myList.drawHeaderCallback = rect => {
			EditorGUI.LabelField (rect, "Beats", EditorStyles.boldLabel);
		};

		_myList.elementHeight = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) *4;
 
		_myList.drawElementCallback = 
			(Rect rect, int index, bool isActive, bool isFocused) => {
			var element = _myList.serializedProperty.GetArrayElementAtIndex (index);
			EditorGUI.LabelField (rect, "Beat "+index, EditorStyles.boldLabel);
			EditorGUI.PropertyField (new Rect (rect.x+15, rect.y, rect.width-15, EditorGUIUtility.singleLineHeight), element);
		};

	
    }

	public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(_BPM);
        EditorGUILayout.PropertyField(_TrackSpeed);
        EditorGUILayout.PropertyField(_clip);
        EditorGUILayout.PropertyField(_seed);
        EditorGUILayout.PropertyField(_NorthSpawn, true);
        EditorGUILayout.PropertyField(_SouthSpawn, true);
        EditorGUILayout.PropertyField(_CenterSpawn, true);
        EditorGUILayout.PropertyField(_EastSpawn, true);
        EditorGUILayout.PropertyField(_WestSpawn, true);


        Track track = (Track) target;

		for(int i = 0; i < track.Beats.Count; i ++)
		{
			track.Beats[i].AudioLength = (int)((float)track.BPM * track.Clip.length / 60f) * (int)track.TrackSpeed / 4;
		}

		if(GUILayout.Button("Procedurally Generate Beatmap"))
        {
            GenerateBeatmap(track);
        }

		if(GUILayout.Button("Clear Beatmap List"))
        {
			track.Beats.Clear();
        }

		EditorGUILayout.LabelField ("(Beats are "+ (60f / track.BPM / ((float)track.TrackSpeed/4f)) +" seconds or "+((60f / track.BPM / ((float)track.TrackSpeed/4f)) / 0.0416666666666667f)+" frames)", EditorStyles.boldLabel);

		_myList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();

		
    }

	public void GenerateBeatmap(Track track)
	{
		track.Beats.Clear();

		float fseed = StringToFloat(track.Seed);

		Debug.LogWarning("fseed " + fseed);

		float perlin = Mathf.PerlinNoise(fseed, fseed);

		Debug.LogWarning("perlin " + perlin);

		int audioLength = (int)((float)track.BPM * track.Clip.length / 60f) * (int)track.TrackSpeed / 4;


        //Debug.Log(audioLength);
		for(int i = 0; i < audioLength; i++)
		{
			bool isUniqueEnd = false;


            //had to comment out the perlin code and seed code for functionality, sorry matt
            //BeatLocation beatLocation = (BeatLocation)((int)(perlin * 7 * i) % 5);


            //based on the publick bool, if they are off continue to find a random number to spawn the notes with
            BeatLocation beatLocation = (BeatLocation)Random.Range(0,5);

            if (!_NorthSpawn.boolValue && !_SouthSpawn.boolValue && !_CenterSpawn.boolValue && !_EastSpawn.boolValue)
            {
                while (((int)beatLocation == 1) || (int)beatLocation == 2 || (int)beatLocation == 0 || (int)beatLocation == 3)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!_NorthSpawn.boolValue && !_WestSpawn.boolValue && !_CenterSpawn.boolValue && !_EastSpawn.boolValue)
            {
                while (((int)beatLocation == 1) || (int)beatLocation == 4 || (int)beatLocation == 0 || (int)beatLocation == 3)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!_NorthSpawn.boolValue && !_WestSpawn.boolValue && !_CenterSpawn.boolValue && !_SouthSpawn.boolValue)
            {
                while (((int)beatLocation == 1) || (int)beatLocation == 4 || (int)beatLocation == 0 || (int)beatLocation == 2)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!_NorthSpawn.boolValue && !_WestSpawn.boolValue && !_EastSpawn.boolValue && !_SouthSpawn.boolValue)
            {
                while (((int)beatLocation == 1) || (int)beatLocation == 4 || (int)beatLocation == 3 || (int)beatLocation == 2)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!_SouthSpawn.boolValue && !_WestSpawn.boolValue && !_CenterSpawn.boolValue && !_EastSpawn.boolValue)
            {
                while (((int)beatLocation == 2) || (int)beatLocation == 4 || (int)beatLocation == 0 || (int)beatLocation == 3)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!_NorthSpawn.boolValue && !_SouthSpawn.boolValue && !_CenterSpawn.boolValue)
            {
                while (((int)beatLocation == 1) || (int)beatLocation == 2 || (int)beatLocation == 0)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!_NorthSpawn.boolValue && !_EastSpawn.boolValue && !_CenterSpawn.boolValue)
            {
                while (((int)beatLocation == 1) || (int)beatLocation == 3 || (int)beatLocation == 0)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!_NorthSpawn.boolValue && !_WestSpawn.boolValue && !_CenterSpawn.boolValue)
            {
                while (((int)beatLocation == 1) || (int)beatLocation == 4 || (int)beatLocation == 0)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!_NorthSpawn.boolValue && !_WestSpawn.boolValue && !_EastSpawn.boolValue)
            {
                while (((int)beatLocation == 1) || (int)beatLocation == 4 || (int)beatLocation == 3)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!_SouthSpawn.boolValue && !_WestSpawn.boolValue && !_EastSpawn.boolValue)
            {
                while (((int)beatLocation == 2) || (int)beatLocation == 4 || (int)beatLocation == 3)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!_SouthSpawn.boolValue && !_CenterSpawn.boolValue && !_EastSpawn.boolValue)
            {
                while (((int)beatLocation == 2) || (int)beatLocation == 0 || (int)beatLocation == 3)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!_SouthSpawn.boolValue && !_WestSpawn.boolValue && !_CenterSpawn.boolValue)
            {
                while (((int)beatLocation == 2) || (int)beatLocation == 4 || (int)beatLocation == 0)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!_CenterSpawn.boolValue && !_WestSpawn.boolValue && !_EastSpawn.boolValue)
            {
                while (((int)beatLocation == 0) || (int)beatLocation == 4 || (int)beatLocation == 3)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!_NorthSpawn.boolValue && !_SouthSpawn.boolValue)
            {
                while (((int)beatLocation == 1) || (int)beatLocation == 2)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!_NorthSpawn.boolValue && !_CenterSpawn.boolValue)
            {
                while (((int)beatLocation == 1) || (int)beatLocation == 0)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!_NorthSpawn.boolValue && !_EastSpawn.boolValue)
            {
                while (((int)beatLocation == 1) || (int)beatLocation == 3)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!_NorthSpawn.boolValue && !_WestSpawn.boolValue)
            {
                while (((int)beatLocation == 1) || (int)beatLocation == 4)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!_SouthSpawn.boolValue && !_WestSpawn.boolValue)
            {
                while (((int)beatLocation == 2) || (int)beatLocation == 4)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!_SouthSpawn.boolValue && !_EastSpawn.boolValue)
            {
                while (((int)beatLocation == 2) || (int)beatLocation == 3)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!_CenterSpawn.boolValue && !_WestSpawn.boolValue)
            {
                while (((int)beatLocation == 0) || (int)beatLocation == 4)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!_CenterSpawn.boolValue && !_EastSpawn.boolValue)
            {
                while (((int)beatLocation == 0) || (int)beatLocation == 3)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!_CenterSpawn.boolValue && !_SouthSpawn.boolValue)
            {
                while (((int)beatLocation == 0) || (int)beatLocation == 2)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!_WestSpawn.boolValue && !_EastSpawn.boolValue)
            {
                while (((int)beatLocation == 4) || (int)beatLocation == 3)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!_EastSpawn.boolValue)
            {
                while ((int)beatLocation == 3)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!_SouthSpawn.boolValue)
            {
                while ((int)beatLocation == 2)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!_CenterSpawn.boolValue)
            {
                while ((int)beatLocation == 0)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!_WestSpawn.boolValue)
            {
                while ((int)beatLocation == 4)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!_NorthSpawn.boolValue)
            {
                while ((int)beatLocation == 1)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }











            //BeatLocation beatLocation = (BeatLocation)((int) (perlin * 7 * i) % 5);
            //Debug.Log((int)beatLocation);
            // Debug.LogWarning(((int)(perlin * i)));
            // if(((int)(perlin * 199 * i)) % 3 != 1){
            int beatStart = i;
				// int beatEnd = (beatStart+1) + ((int)(perlin * 9967 * i) % ((int)track.TrackSpeed * 2));
				int beatEnd = (beatStart + 1);
				track.Beats.Add(new Beat(beatLocation, beatStart, beatEnd));
			// }
		}
	}

	public float StringToFloat(string s)
	{
		float value = 1;

		for(int i = 0; i < s.Length; i++)
		{
			value += i;
			if(i % 2 == 0){
				value *= (float)s[i];
			} else {
				value /= (float)s[i];
			}
		}

		return value;
	}

}
