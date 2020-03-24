using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text;

public class CenterAngleSlice : MonoBehaviour
{
    public const int CircleSegmentCount = 32;

    public const int CircleVertexCount = CircleSegmentCount + 2;
    public const int CircleIndexCount = CircleSegmentCount * 3;

    public float minDegree;
    public float maxDegree;
    public float centerDegree = 45f;

    public Mesh testMesh;
    public GameObject testObject;

    public List<float> DegreeList;

    private float averageDegree;

    public float radius;

    public List<Vector3> vertexList;

    [Range (0f,360f)]
    public float rotationValue;

    public Color color;


    // Start is called before the first frame update
    void Start()
    {
        //CalcCenterDegrees(rotationValue);

        GenerateSlice(rotationValue, radius);
    }

    // Update is called once per frame
    void Update()
    {
        //List<Vector3> testVerts = new List<Vector3>();

        //vertexList.Clear();

        //CalcDegrees();

        //testObject.GetComponent<MeshFilter>().mesh = GenerateCircle(radius, vertexList);

        //testVerts.Clear();
        CalcCenterDegrees(rotationValue);
        GenerateSlice(rotationValue, radius);
    }

    public void GenerateSlice()
    {
        vertexList = new List<Vector3>();

        DegreeList = new List<float>();

        CalcDegrees();

        testObject.GetComponent<MeshFilter>().mesh = GenerateCircle(2f, vertexList);

        Debug.Log("ANGLE TESTING VERTEX COUNT: " + vertexList.Count);

        transform.rotation = Quaternion.Euler(new Vector3(0, rotationValue, 0));
    }

    public void GenerateSlice(float rotation)
    {
        vertexList = new List<Vector3>();

        DegreeList = new List<float>();

        //CalcDegrees();
        CalcDegrees(rotation);

        testObject.GetComponent<MeshFilter>().mesh = GenerateCircle(2f, vertexList);

        Debug.Log("ANGLE TESTING VERTEX COUNT: " + vertexList.Count);

        //transform.rotation = Quaternion.Euler(new Vector3(0, rotation, 0));
    }

    public void GenerateSlice(float rotation, float radius)
    {
        vertexList = new List<Vector3>();

        DegreeList = new List<float>();

        //CalcDegrees();
        //CalcDegrees(rotation);
        CalcCenterDegrees(rotation);

        testObject.GetComponent<MeshFilter>().mesh = GenerateCircle(radius, vertexList);


    }



    public void CalcDegrees()
    {
        DegreeList.Clear();
        float startDegree = 0f;

        averageDegree = (maxDegree - minDegree) / CircleVertexCount;

        IEnumerable<double> testDouble = LinSpace(0, maxDegree, CircleVertexCount, true);
        List<float> testFloats = new List<float>();

        foreach (double value in testDouble)
        {
            testFloats.Add((float)value);
        }

        for (int i = 0; i < CircleVertexCount; ++i)
        {
            DegreeList.Add(testFloats[i]);

            startDegree += averageDegree;
        }

        DegreeList[0] = minDegree;
        DegreeList[1] = minDegree;
        DegreeList[DegreeList.Count - 1] = maxDegree;

    }

    public void CalcDegrees(float rotateValue)
    {
        DegreeList.Clear();
        float startDegree = 0f;

        averageDegree = (maxDegree - minDegree) / CircleVertexCount;

        IEnumerable<double> testDouble = LinSpace(0 + rotateValue, maxDegree + rotateValue, CircleVertexCount, true);
        List<float> testFloats = new List<float>();

        foreach (double value in testDouble)
        {
            testFloats.Add((float)value);
        }

        for (int i = 0; i < CircleVertexCount; ++i)
        {
            DegreeList.Add(testFloats[i]);

            startDegree += averageDegree;
        }

        DegreeList[0] = rotateValue;
        DegreeList[1] = rotateValue;
        DegreeList[DegreeList.Count - 1] = maxDegree + rotateValue;

    }

    public void CalcCenterDegrees(float rotateValue)
    {
        DegreeList.Clear();
        float startDegree = 0f;

        averageDegree = (maxDegree - minDegree) / CircleVertexCount;


        minDegree = centerDegree - (rotateValue / 2);
        maxDegree = centerDegree + (rotateValue / 2);

        IEnumerable<double> testDouble = LinSpace(minDegree, maxDegree, CircleVertexCount, true);

        List<float> testFloats = new List<float>();

        foreach (double value in testDouble)
        {
            testFloats.Add((float)value);
        }

        for (int i = 0; i < CircleVertexCount; ++i)
        {
            DegreeList.Add(testFloats[i]);

            startDegree += averageDegree;
        }

        DegreeList[0] = minDegree;
        DegreeList[1] = minDegree;
        DegreeList[DegreeList.Count - 1] = maxDegree;
    }


    public static IEnumerable<double> LinSpace(double start, double stop, int num, bool endpoint = true)
    {
        var result = new List<double>();
        if (num <= 0)
        {
            return result;
        }

        if (endpoint)
        {
            if (num == 1)
            {
                return new List<double>() { start };
            }

            var step = (stop - start) / ((double)num - 1.0d);
            result = Arange(0, num).Select(v => (v * step) + start).ToList();
        }
        else
        {
            var step = (stop - start) / (double)num;
            result = Arange(0, num).Select(v => (v * step) + start).ToList();
        }

        return result;
    }
    public static IEnumerable<double> Arange(double start, int count)
    {
        return Enumerable.Range((int)start, count).Select(v => (double)v);
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < DegreeList.Count; ++i)
        {
            float xPos = Mathf.Cos(DegreeList[i] * Mathf.Deg2Rad * -1) * radius;
            float zPos = Mathf.Sin(DegreeList[i] * Mathf.Deg2Rad * -1) * radius;

            Vector3 vertLocation = new Vector3(xPos + transform.position.x, transform.position.y, zPos + transform.position.z);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(vertLocation, .05f);
        }
    }



    private Mesh GenerateCircle(float rad, List<Vector3> vertices)
    {

        var circle = new Mesh();

        var indices = new int[CircleIndexCount];

        var angle = 0f;

        vertices.Add(Vector3.zero);

        Debug.Log("VertexCount: " + CircleVertexCount);

        for (int i = 1; i < CircleVertexCount; ++i)
        {
            angle = DegreeList[i] * Mathf.Deg2Rad * -1;

            vertices.Add(new Vector3(Mathf.Cos(angle) * rad, 0f, Mathf.Sin(angle) * rad));

            if (i > 1)
            {
                var j = (i - 2) * 3;
                indices[j + 0] = 0;
                indices[j + 1] = i - 1;
                indices[j + 2] = i;
            }


        }

        vertexList = vertices;

        Color[] colors = new Color[vertices.Count];

        for (int i = 0; i < vertices.Count; i++)
        {
            colors[i] = color;
        }


        circle.SetVertices(vertices);
        circle.colors = colors;
        circle.SetIndices(indices, MeshTopology.Triangles, 0);
        circle.RecalculateBounds();
        circle.RecalculateNormals();

        return circle;
    }

}
