using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class bezir : MonoBehaviour
{
    [SerializeField] private float dot = 60;

    [SerializeField] private Transform p1;
    [SerializeField] private Transform p2;
    [SerializeField] private Transform p3;
    
    [SerializeField] private List<Vector3> point = new List<Vector3>();

    LineRenderer line;

    [SerializeField] private GameObject obj;
    [Range(0, 1)] public float value;
    public Vector3 bezier(Vector3 P0, Vector3 P1, Vector3 P2, float t)
    {
        Vector3 m0 = Vector3.Lerp(P0, P1, t);
        Vector3 m1 = Vector3.Lerp(P1, P2, t);

        return Vector3.Lerp(m0, m1, t);
    }
    private void OnDrawGizmos()
    {
        point.Clear();
        Gizmos.color = Color.red;
        for (int i = 0; i < dot; i++)
        {

            float t = i / dot;
            Vector3 p4 = Vector3.Lerp(p1.position, p2.position, t);
            Vector3 p5 = Vector3.Lerp(p2.position, p3.position, t);

            point.Add(Vector3.Lerp(p4, p5, t));
        }

        for (int i = 0; i < point.Count - 1; i++)
        {
            Gizmos.DrawLine(point[i], point[i + 1]);
        }

    }
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = (int)dot;

        for (int i = 0; i < dot; i++)
        {

            float t = i / dot;
            Vector3 p4 = Vector3.Lerp(p1.transform.position, p2.transform.position, t);
            Vector3 p5 = Vector3.Lerp(p2.transform.position, p3.transform.position, t);

            point.Add(Vector3.Lerp(p4, p5, t));
        }

        Vector3[] arrPoint = point.ToArray();
        for (int i = 0; i < point.Count - 1; i++)
        {
            line.SetPositions(arrPoint);
        }
    }

    // Update is called once per frame
    void Update()
    {
        s();
        //obj.transform.position = bezier(p1.transform.position, p2.transform.position, p3.transform.position, value);
    }

    private void s()
    {

        line.positionCount = (int)dot;

        for (int i = 0; i < dot; i++)
        {
            float t;
            t = i / dot;
            Vector3 p4 = Vector3.Lerp(p1.transform.position, p2.transform.position, t);
            Vector3 p5 = Vector3.Lerp(p2.transform.position, p3.transform.position, t);

            Vector3 p6 = Vector3.Lerp(p4, p5, t);
        }

        Vector3[] arrPoint = point.ToArray();
        for (int i = 0; i < point.Count - 1; i++)
        {
            line.SetPositions(arrPoint);
        }
        
    }
}
