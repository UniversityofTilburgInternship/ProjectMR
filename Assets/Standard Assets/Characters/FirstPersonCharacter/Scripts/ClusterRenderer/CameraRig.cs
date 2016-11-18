using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using System;

[System.Serializable]
public class WallParams {
	public int rows = 1;
	public int cols = 1;
}

[System.Serializable]
public class CaveParams {
	public int sides = 1;
	public bool hasSky = false;
	public bool hasFloor = false;
	public int indexSky = 4;
	public int indexFloor = 4;
}

[System.Serializable]
public class CameraRig {

	public enum ClusterType {
		Cave, Wall
	}

	public WallParams wallParams;
	public CaveParams caveParams;
	public float nearClip = 0.3f;
	public float farClip = 1000;
	public float fov = 60;
	public ClusterType clusterType;
	public int cameraIndexOffset = 0;
	public float horizontalSeam = 0;
	public float verticalSeam = 0;

	public float screenAspectWidth = 16;
	public float screenAspectHeight = 9;

	public bool masterNodeIntegrated = false;
	public int masterNodeIndex = 0;

	/**
	 * The goal is to subdivide the original projection into smaller off-centered projections 
	 * based on row and col counts. The original projection is calculated based on fov, nearclip and aspect ratio.
	 * 
	 * The easiest way is to specify the true aspect ratio of the final rig say 16x9
	 * And divide that equally by the number of rows and columns specified. 
	 * It is up to the user to ensure that the number of rows and cols actually represent the aspect ratio
	 * 
	 * The calculation:
	 * 
	 * Given T, L= -T*aspect
	 * Given cameraIndex = i
	 * 
	 * w=(R-L)/c, h=(T-B)/r
	 * x=i%c, y=floor(i/c)
	 * 
	 * t=T-(y*h), b=t-h, l=L+(x*w), r=l+w
	 *
	 * For PowerWall, the rows and columns determine how the offcenter are calculated.
	 * Each quad is 2 units width and height. So a center quad is
	 *        -----------1,1
	 *        |           |
	 *        |           |
	 *        |           |
	 *      -1,-1----------
	 * 
	 * Margins for WALLs are added in during the computation of the projection matrix.
	 * 
	 * 
	 * For CAVE, we could have odd and even number of screens. 
	 * For odd we could position screen 0 as center 31024 or 01234 or -2,-1,0,1,2
	 * For even screens, we could force a center screen. 0,1 or -1,0,1,2
	 * 
	 * sides != screens. You can have 3 screens showing parts of a 8 sided simulation
	 * This gives you a 45deg angel between the screens
	 * 
	 * Or you could have a 3 screens with 4 sided simulation which means the screen are at 90deg with each other.
	 * 
	 * Formula to convert camera index to rig index the arrangement is 31024
	 * f(odd n)  = ceil(n/2)
	 * f(even n) = -n/2
	 * f(n) = (1-2*(n&1))*ceil(n*0.5);
	 * i = 0, h = 0
	 * i = 1, h = 1
	 * i = 2, h = -1
	 * i = 3, h = 2
	 * i = 4, h = -2
	 * i = 5, h = 3
	 * i = 6, h = -3
	 * i = 7, h = 4
	 * i = 8, h = -4
	 * 
	 * 
	 * A sky or floor is only applicable for caves with 4 walls, as the screen can only be a square or rectangle.
	 * If we are doing sky or floor, we could still calculate the vertical fov for cave walls first.
	 * For a room with 4 walls and/or 1 sky and/or 1 floor, the fov for the sky or floor is simply
	 * (360 - (2*fovVert)) / 2
	 * = 180 - fovVert
	 *
	 *          180-
	 *       \fovVert /
	 *        \      /
	 *         \    /
	 *          \  / 
	 *   fovVert \/  fovVert (for cave walls)
	 *           /\
	 *          /  \
	 *         /    \
	 *        /      \
	 *       /  180-  \
	 *        fovVert 
	 *
	 * After computing the fov, we will rotate the camera to face the floor or sky.
	 * For floor and sky, the aspect ratios are 1, as all 4 walls around them will have the same length.
	 * Margins for CAVEs are subtracted from the horizontal and vertical field of views during calculation of
	 * values for the projection matrix.
	 *
	 */
	 
	public void SetupCamera(Camera camera, int cameraIndex) {

		// the index relative to this rig. Support multi-rig per scene
		cameraIndex -= cameraIndexOffset;
		// reset this incase we were wall once
		camera.ResetProjectionMatrix();

		// this must be reset
		camera.transform.localRotation = Quaternion.identity;

		if (clusterType == ClusterType.Cave)
		{
			camera.transform.localEulerAngles = CalculateRotationForCaveNode (cameraIndex);
		}

		float left = 0.0f, right = 0.0f, bottom = 0.0f, top = 0.0f;
		CalculateCameraProjectionFrustum(cameraIndex, ref left, ref right, ref bottom, ref top);
		camera.projectionMatrix = PerspectiveOffCenter( left, right, bottom, top, nearClip, farClip);
		camera.nearClipPlane = nearClip;
		camera.farClipPlane = farClip;
	}

	public void CalculateCameraProjectionFrustum (int cameraIndex,
		ref float outLeft,
		ref float outRight,
		ref float outBottom,
		ref float outTop)
	{
		if (clusterType == ClusterType.Wall)
		{
			float totalScreenAspectWidth = wallParams.cols * (screenAspectWidth + horizontalSeam);
			float totalScreenAspectHeight = wallParams.rows * (screenAspectHeight + verticalSeam);

			// aspect ratio of whole image
			float aspect = totalScreenAspectWidth / totalScreenAspectHeight;
			float halfFov = Mathf.Deg2Rad * (fov * 0.5f);
			// overall top computed from half fov,
			// then overall left computed by multiplying aspect ratio
			float top = Mathf.Tan (halfFov) * nearClip;
			float left = -top * aspect;
			// x and y offsets of current camera based on camera index
			int x = cameraIndex % wallParams.cols;
			int y = (int)(Mathf.Floor (cameraIndex / wallParams.cols));

			float l, t, w, h;

			if (totalScreenAspectWidth > totalScreenAspectHeight)
			{
				float screenAspect = screenAspectHeight / screenAspectWidth;
				// width and height of single screen
				w = (top * aspect * 2) / wallParams.cols;
				h = w * screenAspect;
				float b = (2 * top - h * wallParams.rows) * 0.5f;
				l = left + x * w;
				t = top - b - y * h;

			}
			else
			{
				float screenAspect = screenAspectWidth / screenAspectHeight;
				// height and width of single screen
				h = (top * 2) / wallParams.rows;
				w = h * screenAspect;
				float b = ((2 * top * aspect) - (w * wallParams.cols)) * 0.5f;
				l = left + b + x * w;
				t = top - y * h;
			}

			// use percentage of seam over aspect width or height
			// multipled by width or height to find offset
			float horizontalOffset = (horizontalSeam / screenAspectWidth) * w;
			float verticalOffset = (verticalSeam / screenAspectHeight) * h;

			outLeft = l + horizontalOffset;
			outRight = l + w - horizontalOffset;
			outBottom = t - h + verticalOffset;
			outTop = t - verticalOffset;
		}
		else if (clusterType == ClusterType.Cave)
		{

			float aspect = screenAspectWidth / screenAspectHeight;
			float hfov = CalculateCaveHorizontalFOV ();

			// the tangent of half the vertical fov is the tangent of half the horizontal fov divided by the aspect ratio
			// using half the horizontal fov, we can use the tangent function to find a factor of the horizontal side,
			// i.e. (opposite/adjacent), where opposite = the horizontal side.
			// we can apply the aspect ratio to that factor to find a factor of the vertical side
			// following that, we can use the atan function to get half the vertical fov, which will be used in our
			// calculation later to find the sides of the projection matrix.
			float tanVert = (Mathf.Tan (hfov * Mathf.Deg2Rad * 0.5f)) / aspect;
			float vfov = (Mathf.Atan (tanVert) * Mathf.Rad2Deg * 2);

			if (IsSky (cameraIndex) || IsFloor (cameraIndex))
			{
				vfov = 180.0f - vfov;
				// for floor and sky, aspect ratio is 1 for final screen,
				// as all 4 sides will be same length
				aspect = 1;
			}
			vfov -= verticalSeam;

			// compute projection matrix's top if we are doing a cave wall
			// also applies to sky/floor as top/right will be the same due to aspect ratio
			float t = Mathf.Tan (Mathf.Deg2Rad * vfov * 0.5f) * nearClip;

			outLeft = -t * aspect;
			outRight =  t * aspect;
			outBottom = -t;
			outTop = t;

		}
	}

	public Vector3 CalculateRotationForCaveNode (int cameraIndex)
	{
		// rotation index for cave walls
		float rotIndex = (1 - 2 * (cameraIndex & 1)) * Mathf.Ceil (cameraIndex * 0.5f);

		// rotate up or down for sky or floor
		if (IsSky (cameraIndex))
			return new Vector3 (-90.0f, 0);
		else if (IsFloor (cameraIndex))
			return new Vector3 (90.0f, 0);
		else
			// is cave wall, so rotate according to rotation index
			return new Vector3 (0, rotIndex * CalculateCaveHorizontalFOV ());
	}

	public float CalculateCaveHorizontalFOV ()
	{
		return (360.0f / caveParams.sides) - horizontalSeam;
	}

	public bool IsSky (int cameraIndex)
	{
		return (caveParams.hasSky && cameraIndex == caveParams.indexSky);
	}
	public bool IsFloor (int cameraIndex)
	{
		return (caveParams.hasFloor && cameraIndex == caveParams.indexFloor);
	}

	private Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far) {
		float x =  (2.0f * near) / (right - left);
		float y =  (2.0f * near) / (top - bottom);
		float a =  (right + left) / (right - left);
		float b =  (top + bottom) / (top - bottom);
		float c = -(far + near) / (far - near);
		float d = -(2.0f * far * near) / (far - near);
		float e = -1.0f;
		Matrix4x4 m  = new Matrix4x4();
		m[0,0] = x;  m[0,1] = 0;  m[0,2] = a;  m[0,3] = 0;
		m[1,0] = 0;  m[1,1] = y;  m[1,2] = b;  m[1,3] = 0;
		m[2,0] = 0;  m[2,1] = 0;  m[2,2] = c;  m[2,3] = d;
		m[3,0] = 0;  m[3,1] = 0;  m[3,2] = e;  m[3,3] = 0;
		return m;
	}

#if UNITY_EDITOR
	public string Export (string rigName)
	{
		// use C#'s XmlSerializer to serialize the inspected object into a string,
		// then save it to an external .cfg file
		StringWriter sw = new StringWriter();
		XmlSerializer xmlSer = new XmlSerializer(this.GetType());
		xmlSer.Serialize(sw, this);

		// Save file to a directory user specified
		string path = EditorUtility.SaveFilePanel(
			"Export rig file as .cfg",
			"",
			rigName + ".cfg",
			"cfg");

		// Convert to bytes
		if (path.Length != 0)
		{
			byte[] b = Encoding.ASCII.GetBytes(sw.ToString());
			File.WriteAllBytes(path, b);
			// Tell Unity to scan for modified or new assets
			AssetDatabase.Refresh();
			return path;
		}
		else
			return null;
	}
#endif
}

