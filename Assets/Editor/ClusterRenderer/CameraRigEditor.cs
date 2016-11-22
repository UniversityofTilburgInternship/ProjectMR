using UnityEditor;
using UnityEngine;
using System.Collections;

using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.IO; 
using System;

[CustomPropertyDrawer(typeof(CameraRig))]
public partial class CameraRigEditor : PropertyDrawer {

	SerializedProperty clusterType;
	// wall props
	SerializedProperty rows;
	SerializedProperty cols;
	// cave props
	SerializedProperty totalSides;
	SerializedProperty hasSky;
	SerializedProperty hasFloor;
	SerializedProperty indexSky;
	SerializedProperty indexFloor;

	SerializedProperty fov;
	SerializedProperty nearClip;
	SerializedProperty farClip;
	SerializedProperty screenAspectWidth;
	SerializedProperty screenAspectHeight;

	SerializedProperty horizontalSeam;
	SerializedProperty verticalSeam;

	SerializedProperty indexOffset;

	SerializedProperty masterNodeIntegrated;
	SerializedProperty masterNodeIndex;

	// for cave sky and floor index errors
	bool indexError = false;

	void FindAllProperty(SerializedProperty property)	
	{
		if (clusterType != null)
			return;

		clusterType = property.FindPropertyRelative("clusterType");

		rows = property.FindPropertyRelative("wallParams.rows");
		cols = property.FindPropertyRelative("wallParams.cols");

		totalSides = property.FindPropertyRelative("caveParams.sides");
		hasSky = property.FindPropertyRelative("caveParams.hasSky");
		hasFloor = property.FindPropertyRelative("caveParams.hasFloor");
		indexSky = property.FindPropertyRelative("caveParams.indexSky");
		indexFloor = property.FindPropertyRelative("caveParams.indexFloor");

		fov = property.FindPropertyRelative("fov");
		nearClip = property.FindPropertyRelative("nearClip");
		farClip = property.FindPropertyRelative("farClip");
		screenAspectHeight = property.FindPropertyRelative("screenAspectHeight");
		screenAspectWidth = property.FindPropertyRelative("screenAspectWidth");

		indexOffset = property.FindPropertyRelative("cameraIndexOffset");

		horizontalSeam = property.FindPropertyRelative("horizontalSeam");
		verticalSeam = property.FindPropertyRelative("verticalSeam");

		masterNodeIntegrated = property.FindPropertyRelative("masterNodeIntegrated");
		masterNodeIndex = property.FindPropertyRelative("masterNodeIndex");
	}

	private void MakeTwinProperty(string label, GUIContent subLableContent1, GUIContent subLableContent2, SerializedProperty prop1, SerializedProperty prop2, int labelWidth = 48)
	{
		float lineHeight = 16f;
		Rect r = EditorGUILayout.GetControlRect(true, lineHeight * 2f);
		r.height = lineHeight;
		// Main clipping planes label
		GUI.Label (r, new GUIContent (label));
		// Make labels for near and far control appear righht in front of the number fields.
		r.xMin += EditorGUIUtility.labelWidth - 1; // Shift one pixel to align text with left edge of other controls above and below
		EditorGUIUtility.labelWidth = labelWidth;
		// Near and far clip plane controls
		EditorGUI.PropertyField (r, prop1, subLableContent1);
		r.y += lineHeight;
		EditorGUI.PropertyField (r, prop2, subLableContent2);
		// Reset label width
		EditorGUIUtility.labelWidth = 0;
	}

	private void MakeCheckboxAndIndex(ref SerializedProperty hasObjectSP, ref SerializedProperty objectIndexSP, GUIContent toggleContent, string indexTooltip, int minIndexOffset = 0)
	{
		// get if there is that object and index for it
		EditorGUILayout.BeginHorizontal();
		hasObjectSP.boolValue = EditorGUILayout.Toggle(toggleContent, hasObjectSP.boolValue);
		
		// let user input index
		EditorGUI.BeginDisabledGroup(!hasObjectSP.boolValue);

		EditorGUILayout.LabelField(new GUIContent("Node Index", indexTooltip), GUILayout.MaxWidth(75));
		string indexString = (objectIndexSP.intValue).ToString();
		indexString = EditorGUILayout.TextField(indexString);
		EditorGUI.EndDisabledGroup();
		
		if (hasObjectSP.boolValue)
		{
			// check if number is valid
			int indexInt = objectIndexSP.intValue;
			if (!int.TryParse(indexString, out indexInt))
				indexInt = objectIndexSP.intValue;
			else
			{
				int minIndex = indexOffset.intValue + minIndexOffset;
				// cannot be lesser than minimum index
				if (indexInt < minIndex)
				{
					indexInt = objectIndexSP.intValue;
					if (hasObjectSP == hasSky || hasObjectSP == hasFloor)
						indexError = true;
				}
				else
				{
					// if it was just changed
					if ((objectIndexSP.intValue != indexInt) &&
						// if other value's index is also more than or equal minimum index
						( ( hasObjectSP == hasSky && indexFloor.intValue >= minIndex ) || 
						( hasObjectSP == hasFloor && indexSky.intValue >= minIndex ) ) )
						indexError = false;
					// success value
					objectIndexSP.intValue = indexInt;
				}
			}
		}
		EditorGUILayout.EndHorizontal ();
	}

	//public override void OnInspectorGUI() {
	public override void  OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		FindAllProperty (property);

		EditorGUILayout.BeginVertical();
		EditorGUILayout.LabelField(new GUIContent("Cluster Settings", "Options for controlling the settings for a cluster rig."), EditorStyles.boldLabel);

		int indexOffsetPrevious = indexOffset.intValue;		// for cave index error message use

        	// Only applicable for multiple rig
		//EditorGUILayout.PropertyField (indexOffset, new GUIContent ("Camera Index Offset", "Offset when reading camera indices."));

		EditorGUILayout.PropertyField(clusterType, new GUIContent("Type", "Whether this cluster rig is for a cave or a wall."));

		if((clusterType.enumValueIndex == (int)CameraRig.ClusterType.Cave)) { 
			EditorGUILayout.PropertyField(totalSides, new GUIContent("Sides", "Number of sides in cave cluster."));
			EditorGUILayout.Space();

			// make checkboxes for floor and sky if there are 4 cave walls
			if (totalSides.intValue == 4)
			{
				MakeCheckboxAndIndex(ref hasSky, ref indexSky,
				                     new GUIContent("Sky", "Should this cave cluster contain a node for sky."), "Node index for the sky node.", 4);
				MakeCheckboxAndIndex(ref hasFloor, ref indexFloor,
				                     new GUIContent("Floor", "Should this cave cluster contain a node for floor."), "Node index for the floor node.", 4);
				
				// index for floor cannot be the same as for sky
				if (hasSky.boolValue && hasFloor.boolValue && indexFloor.intValue == indexSky.intValue)
					indexFloor.intValue = indexSky.intValue + 1;


				// show error message for sky and floor if there is any
				if (indexError)
				{
					int minIndex = indexOffset.intValue + totalSides.intValue;
					EditorGUILayout.HelpBox("Index must be " + minIndex + " or higher.", MessageType.Error);

					// remove error message if both sky and floor are turned off
					if ((!hasSky.boolValue && !hasFloor.boolValue) ||
						// or if index offset was changed, and both sky and floor indexes are alright now
						(indexOffset.intValue < indexOffsetPrevious &&
						(indexFloor.intValue >= minIndex) &&
						(indexSky.intValue >= minIndex)))
					{
						indexError = false;
					}
				}
			}
			else
			{
				hasSky.boolValue = false;
				hasFloor.boolValue = false;
			}
		
		} else {
			EditorGUILayout.PropertyField(rows, new GUIContent("Rows", "Number of rows in wall cluster."));
			EditorGUILayout.PropertyField(cols, new GUIContent("Columns", "Number of columns in wall cluster."));
		}

		// toggle box for integrating master node
		MakeCheckboxAndIndex(ref masterNodeIntegrated, ref masterNodeIndex,
							 new GUIContent("Integrate Master Node", "Should the master node be integrated into the cluster."),
							 "Node index for the master node.");

		// other settings
		EditorGUILayout.Space();
        	EditorGUILayout.LabelField(new GUIContent("Screen Settings"), EditorStyles.boldLabel);
		MakeTwinProperty("Screen Margin", new GUIContent("Horizontal", "Horizontal seam for the screen."),
			                			  new GUIContent("Vertical", "Vertical seam for the screen."), horizontalSeam, verticalSeam, 60);
		MakeTwinProperty("Screen Aspect", new GUIContent("Width", "Screen aspect width."),
			                			  new GUIContent("Height", "Screen aspect height."), screenAspectWidth, screenAspectHeight, 60);

		EditorGUILayout.Space();
        	EditorGUILayout.LabelField(new GUIContent("Camera Settings"), EditorStyles.boldLabel);

		// the fov slider
		if ((clusterType.enumValueIndex == (int)CameraRig.ClusterType.Wall))
			EditorGUILayout.PropertyField(fov, new GUIContent("Field of View", "Extent of observable world."));
		MakeTwinProperty("Clipping Planes", new GUIContent("Near", "Where the near clip plane is."),
		                					new GUIContent("Far", "Where the far clip plane is."), nearClip, farClip);

		EditorGUILayout.Space();
		if ((clusterType.enumValueIndex == (int)CameraRig.ClusterType.Cave && totalSides.intValue != 4))	// Give info message for sky and floor
			EditorGUILayout.HelpBox("Sky and Floor options are only available for Caves with 4 sides.", MessageType.Info);
		EditorGUILayout.HelpBox("The above settings apply to individual cameras.", MessageType.Info);

		EditorGUILayout.EndVertical();
	}
}
