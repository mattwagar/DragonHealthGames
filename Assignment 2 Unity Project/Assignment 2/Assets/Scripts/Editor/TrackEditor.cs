﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor (typeof (Track))]
public class TrackEditor : Editor {

	SerializedProperty _BPM;
	SerializedProperty _TrackSpeed;
	SerializedProperty _clip;

	private ReorderableList _myList;

	void OnEnable()
    {
        _BPM = serializedObject.FindProperty("BPM");
        _TrackSpeed = serializedObject.FindProperty("TrackSpeed");
        _clip = serializedObject.FindProperty("Clip");
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

		Track track = (Track) target;
		switch(track.TrackSpeed){
			case TrackSpeed.Whole:
			EditorGUILayout.LabelField ("(Beats are "+ (60f / track.BPM * 4) +" seconds or "+((60f / track.BPM * 4) * 100)+" frames)", EditorStyles.boldLabel);
			break;
			case TrackSpeed.Half:
			EditorGUILayout.LabelField ("(Beats are "+ (60f / track.BPM * 2) +" seconds or "+((60f / track.BPM * 2) * 100)+" frames)", EditorStyles.boldLabel);
			break;
			case TrackSpeed.Quarter:
			EditorGUILayout.LabelField ("(Beats are "+ (60f / track.BPM ) +" seconds or "+((60f / track.BPM) * 100)+" frames)", EditorStyles.boldLabel);
			break;
			case TrackSpeed.Eighth:
			EditorGUILayout.LabelField ("(Beats are "+ (60f / track.BPM / 2) +" seconds or "+((60f / track.BPM / 2) * 100)+" frames)", EditorStyles.boldLabel);
			break;
			case TrackSpeed.Sixteenth:
			EditorGUILayout.LabelField ("(Beats are "+ (60f / track.BPM / 4) +" seconds or "+((60f / track.BPM / 4) * 100)+" frames)", EditorStyles.boldLabel);
			break;
			
		}

		_myList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();

		for(int i = 0; i < track.Beats.Count; i ++)
		{
			track.Beats[i].AudioLength = (int)((float)track.BPM * track.Clip.length / 60f);

			switch(track.TrackSpeed){
				case TrackSpeed.Whole:
				track.Beats[i].AudioLength /= 4;
				break;
				case TrackSpeed.Half:
				track.Beats[i].AudioLength /= 2;
				break;
				case TrackSpeed.Eighth:
				track.Beats[i].AudioLength *= 2;
				break;
				case TrackSpeed.Sixteenth:
				track.Beats[i].AudioLength *= 4;
				break;
				
			}
		}
		
		
    }

}
