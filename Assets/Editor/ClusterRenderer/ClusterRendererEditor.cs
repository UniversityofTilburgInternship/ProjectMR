using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(ClusterRenderer))]
public class ClusterRendererEditor : Editor
{
	private SerializedProperty m_CameraRig;
	private SerializedProperty m_OverrideNodeIndex;
	private SerializedProperty m_ForcedIndex;

	private bool m_DisplayClusterGizmo = false;
	private bool m_HeadTrackerAttached = false;

	private int m_PreviewingIndex = -1;
	private float m_OriginalMainCameraNearClip = 0.0f;
	private float m_OriginalMainCameraFarClip = 0.0f;


	public void OnEnable ()
	{
		m_OriginalMainCameraNearClip = Camera.main.nearClipPlane;
		m_OriginalMainCameraFarClip = Camera.main.farClipPlane;

        m_CameraRig = serializedObject.FindProperty("m_Rig");
		m_OverrideNodeIndex = serializedObject.FindProperty("m_OverrideNodeIndex");
		m_ForcedIndex = serializedObject.FindProperty("m_ForcedIndex");

		ClusterRenderer clusterRenderer = target as ClusterRenderer;
		if (clusterRenderer.GetComponent ("HeadTracker") != null)
		{
			m_HeadTrackerAttached = true;
			m_DisplayClusterGizmo = false;
		}
	}

	public void OnDisable ()
	{
		ResetCamera ();
	}

    public override void OnInspectorGUI()
    {
		serializedObject.Update();

        EditorGUI.PropertyField(new Rect(0, 0, 10, 10), m_CameraRig);

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();

		if (GUILayout.Button(new GUIContent("Export Settings", "Export cluster rig to a target location as a .cfg file.")))
		{
			ClusterRenderer renderer = target as ClusterRenderer;
			renderer.rig.Export(target.name);
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);
		EditorGUI.BeginDisabledGroup (m_HeadTrackerAttached);
		m_DisplayClusterGizmo = EditorGUILayout.Toggle("Cluster Gizmo", m_DisplayClusterGizmo);
		EditorGUI.EndDisabledGroup ();
		if (GUI.changed)
		{
			SceneView.RepaintAll();
		}

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(m_OverrideNodeIndex);

		EditorGUI.BeginDisabledGroup(!m_OverrideNodeIndex.boolValue);
		EditorGUIUtility.labelWidth = 80;
		EditorGUILayout.PropertyField(m_ForcedIndex);
		EditorGUI.EndDisabledGroup();

		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		serializedObject.ApplyModifiedProperties();
    }

	private void ResetCamera ()
	{
		Camera.main.ResetAspect ();
		Camera.main.ResetProjectionMatrix ();
		Camera.main.nearClipPlane = m_OriginalMainCameraNearClip;
		Camera.main.farClipPlane = m_OriginalMainCameraFarClip;
		Camera.main.transform.localRotation = Quaternion.identity;
	}

	public void OnSceneGUI()
	{
		if (!m_DisplayClusterGizmo)
			return;

		ClusterRenderer renderer = target as ClusterRenderer;

		Color orgHandlesColor = Handles.color;
		Handles.color = Color.green;
		CameraRig rig = renderer.rig;

		DrawClusterGizmo(rig, renderer);

		Handles.color = orgHandlesColor;
	}
	private void DrawClusterGizmo(CameraRig rig, ClusterRenderer clusterRenderer)
	{
		int totalNodeCount;
		if (rig.clusterType == CameraRig.ClusterType.Wall)
			totalNodeCount = rig.wallParams.cols * rig.wallParams.rows;
		else
			totalNodeCount = rig.caveParams.sides + (rig.caveParams.hasFloor ? 1 : 0) + (rig.caveParams.hasSky ? 1 : 0);

		for (int i = 0; i < totalNodeCount; ++i)
		{
			if (m_PreviewingIndex != -1 && m_PreviewingIndex != i)
				continue;

			float nearLeft = 0.0f;
			float nearRight = 0.0f;
			float nearBottom = 0.0f;
			float nearTop = 0.0f;

			// Get the nears
			rig.CalculateCameraProjectionFrustum (i, ref nearLeft, ref nearRight, ref nearBottom, ref nearTop);

			float ratio = rig.farClip / rig.nearClip;

			// Calculate the far
			float farLeft = nearLeft * ratio;
			float farRight = nearRight * ratio;
			float farBottom = nearBottom * ratio;
			float farTop = nearTop * ratio;

			// Points
			Vector3[] near = new Vector3[4];
			near[0] = new Vector3 (nearLeft, nearBottom, rig.nearClip);
			near[1] = new Vector3 (nearRight, nearBottom, rig.nearClip);
			near[2] = new Vector3 (nearRight, nearTop, rig.nearClip);
			near[3] = new Vector3 (nearLeft, nearTop, rig.nearClip);

			Vector3[] far = new Vector3[4];
			far[0] = new Vector3 (farLeft, farBottom, rig.farClip);
			far[1] = new Vector3 (farRight, farBottom, rig.farClip);
			far[2] = new Vector3 (farRight, farTop, rig.farClip);
			far[3] = new Vector3 (farLeft, farTop, rig.farClip);

			// Local to world transform (ignore scale of transform)
			Matrix4x4 localToWorld;
			if (rig.clusterType == CameraRig.ClusterType.Wall)
			{
				localToWorld = Matrix4x4.TRS(clusterRenderer.transform.position, clusterRenderer.transform.rotation, Vector3.one);
			}
			else
			{
				Quaternion q = clusterRenderer.transform.rotation * (m_PreviewingIndex == -1 ? Quaternion.Euler (rig.CalculateRotationForCaveNode (i)) : Quaternion.identity);
				localToWorld = Matrix4x4.TRS (clusterRenderer.transform.position, q, Vector3.one);
			}

			// transform point to world
			for (int p = 0; p < 4; ++p)
			{
				near[p] = localToWorld.MultiplyPoint (near[p]);
				far[p] = localToWorld.MultiplyPoint (far[p]);
			}

			// Draw the frustum gizmo
			for (int p = 0; p < 4; ++p)
			{
				Handles.DrawLine (near[p], near[(p + 1) % 4]);
				Handles.DrawLine (far[p], far[(p + 1) % 4]);
				Handles.DrawLine (near[p], far[p]);
			}

			// Label to show which index of node this screen belongs to
			Handles.Label(Vector3.Lerp(far[1], far[3], 0.9f), "" + i);

			// Button to activate preview
			Vector3 midPoint = Vector3.Lerp (far[0], far[2], 0.5f);
			float handleSize = HandleUtility.GetHandleSize(midPoint) * 0.1f;
			if (Handles.Button(midPoint, Quaternion.identity, handleSize, handleSize, Handles.DotCap))
			{
				if (m_PreviewingIndex == i)
				{
					m_PreviewingIndex = -1;
					ResetCamera ();
				}
				else
				{
					m_PreviewingIndex = i;
					rig.SetupCamera (Camera.main, i);
				}
			}
		}
	}
}
