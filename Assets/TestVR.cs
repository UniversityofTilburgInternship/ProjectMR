using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class TestVR : MonoBehaviour {

        private readonly Vector3 _point1 = new Vector3(12.06f,3.48f,14.6f);
        private readonly Vector3 _point2 = new Vector3(12.06f,3.48f,-8.85f);
        private readonly Vector3 _point3 = new Vector3(-11.54f,3.48f,-8.85f);
        private readonly Vector3 _point4 = new Vector3(-11.54f,3.48f,14.6f);
        public readonly List<Vector3> Points = new List<Vector3>();

        private void Awake()
        {
            Points.Add(_point1);
            Points.Add(_point2);
            Points.Add(_point3);
            Points.Add(_point4);
        }


        IEnumerator Start()
        {
            while (true) {
                yield return StartCoroutine(MoveObject(transform, Points[0], Points[1], 3.0f, Color.red));
                yield return StartCoroutine(MoveObject(transform, Points[1], Points[2], 3.0f,Color.green));
                yield return StartCoroutine(MoveObject(transform, Points[2], Points[3], 3.0f,Color.yellow));
                yield return StartCoroutine(MoveObject(transform, Points[3], Points[0], 3.0f,Color.blue));
            }
        }

        private IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time, Color color)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = color;
            var i= 0.0f;
            var rate= 0.3f/time;
            while (i < 1.0f) {
                i += Time.deltaTime * rate;
                thisTransform.position = Vector3.Lerp(startPos, endPos, i);
                yield return null;
            }
        }
    }
}
                                               