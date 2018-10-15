using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Beat))]
public class BeatPropertyDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
		float height = 0;
		SerializedProperty StartPositionX = property.FindPropertyRelative("StartPositionX");
		SerializedProperty StartPositionZ = property.FindPropertyRelative("StartPositionZ");
		// SerializedProperty Curve = property.FindPropertyRelative("Curve");
		SerializedProperty BeatStart = property.FindPropertyRelative("BeatStart");
		SerializedProperty BeatEnd = property.FindPropertyRelative("BeatEnd");
		SerializedProperty AudioLength = property.FindPropertyRelative("AudioLength");


		height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		EditorGUI.PropertyField(new Rect(position.x, position.y+height, position.width, EditorGUIUtility.singleLineHeight), StartPositionX, new GUIContent("Start Position X(%)"));
		height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.PropertyField(new Rect(position.x, position.y+height, position.width, EditorGUIUtility.singleLineHeight), StartPositionZ, new GUIContent("Start Position Z(%)"));
		height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		// EditorGUI.PropertyField(new Rect(position.x, position.y+height, position.width, EditorGUIUtility.singleLineHeight), Curve);
		// height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		// EditorGUI.PropertyField(new Rect(position.x, position.y+height, position.width, EditorGUIUtility.singleLineHeight), BeatLocation);


		EditorGUI.IntSlider(new Rect(position.x, position.y+height, position.width, EditorGUIUtility.singleLineHeight), BeatStart, 0, AudioLength.intValue-1, new GUIContent("Beat Start (0 - "+ (AudioLength.intValue-1) +")"));
        if(BeatStart.intValue > BeatEnd.intValue){BeatEnd.intValue = BeatStart.intValue+1;}
		height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		EditorGUI.IntSlider(new Rect(position.x, position.y+height, position.width, EditorGUIUtility.singleLineHeight), BeatEnd, 1, AudioLength.intValue, new GUIContent("Beat End (1 - "+ (AudioLength.intValue) +")"));
        if(BeatEnd.intValue <= BeatStart.intValue){BeatStart.intValue = BeatEnd.intValue-1;}

    }

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return 60.0f;
	}


	//   public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //  {
    //      float height = 0;
    //      EditorGUI.BeginProperty(position, label, property);
    //      {
    //          Rect titleRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
    //         //  EditorGUI.PropertyField(titleRect, property.FindPropertyRelative("title"));
    //          height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
 
    //          Rect chaptersRect = new Rect(position.x, position.y + height, position.width, EditorGUIUtility.singleLineHeight);
    //          if(chapterList == null)
    //          {
    //              chapterList = BuildChaptersReorderableList(property.FindPropertyRelative("chapters"));
    //          }
    //          chapterList.DoList(chaptersRect);
    //      }
    //      EditorGUI.EndProperty();
    //  }
}
