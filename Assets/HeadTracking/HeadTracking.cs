﻿using UnityEngine;


public class HeadTracking : MonoBehaviour
{
    public float pupilOffCenter   = 0.0f;
    private Vector3 eyePosition = new Vector3(0.0f,1.25f,0.0f), disparity;
    private bool eyeTrackerAdded = false;
    private int[] cameraDirection = new int[4]{0,-1,1,2};
    private int cameraIndex = -1;
    private float farClip  = 1000  , width  = 0.03f  , halfCaveHeight = 1.25f ,
        nearClip = 0.01f , height = 0.014f , halfCaveWidth  = 2.5f ;

    void Awake() {
        cameraIndex = UnityEngine.ClusterNetwork.nodeIndex;
        disparity = new Vector3(pupilOffCenter,0.0f,0.0f);

        if (cameraIndex >= 0) {
            Camera camera = this.gameObject.GetComponent<Camera>();
            camera.ResetProjectionMatrix();
            camera.transform.localEulerAngles = new Vector3 (0.0f, 90.0f*cameraDirection[cameraIndex], 0.0f);
            camera.nearClipPlane = nearClip;
            camera.farClipPlane = farClip;
        }
    }

    void Update() {

        if (!eyeTrackerAdded) {
            UnityEngine.ClusterInput.AddInput ("EyeTracker", "PPT0", "FC-NUC", 0, ClusterInputType.Tracker);
            eyeTrackerAdded = true;
            Debug.Log ("eyeTrackerAdded");
        }

        if (cameraIndex >= 0) {

            if (!UnityEngine.ClusterNetwork.isDisconnected && eyeTrackerAdded) {
                Quaternion trackerRotation = UnityEngine.ClusterInput.GetTrackerRotation ("EyeTracker");
                eyePosition = UnityEngine.ClusterInput.GetTrackerPosition ("EyeTracker") + trackerRotation*disparity;
            } else {
                eyePosition = new Vector3 (0.0f, 1.25f, 0.0f);
            }

            float right = width  * eyePosition [0] / halfCaveWidth;
            float up 	= height * (eyePosition [1] - halfCaveHeight) / halfCaveHeight;
            float front = width  * eyePosition [2] / halfCaveWidth;

            Camera camera = this.gameObject.GetComponent<Camera> ();

            if (cameraIndex < 2) {
                if (cameraIndex == 0) {
                    camera.projectionMatrix = PerspectiveOffCenter (-width - right, width - right, -height - up, height - up, width - front, farClip);
                } else {
                    camera.projectionMatrix = PerspectiveOffCenter (-width - front, width - front, -height - up, height - up, width + right, farClip);
                }
            } else {
                if (cameraIndex == 2) {
                    camera.projectionMatrix = PerspectiveOffCenter (-width + front, width + front, -height - up, height - up, width - right, farClip);
                } else {
                    camera.projectionMatrix = PerspectiveOffCenter (-width + right, width + right, -height - up, height - up, width + front, farClip);
                }
            }
            camera.transform.position = eyePosition;
        }
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
}                                                                                            