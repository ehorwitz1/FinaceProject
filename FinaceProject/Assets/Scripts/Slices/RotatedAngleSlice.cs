using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text;

public class RotatedAngleSlice : MonoBehaviour
{
    public const int CircleSegmentCount = 32;

    public const int CircleVertexCount = CircleSegmentCount + 2;
    public const int CircleIndexCount = CircleSegmentCount * 3;

    public float minDegree;
    public float maxDegree;

    public Mesh testMesh;
    public GameObject testObject;

    public List<float> DegreeList;

    private float averageDegree;

    public float radius;

    public List<Vector3> vertexList;

    public float rotationValue;

    public Color color;


    // Start is called before the first frame update
    void Start()
    {

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
        CalcDegrees(rotation);

        testObject.GetComponent<MeshFilter>().mesh = GenerateCircle(radius, vertexList);


    }

    // Update is called once per frame
    void Update()
    {
        //List<Vector3> testVerts = new List<Vector3>();

        //vertexList.Clear();

        //CalcDegrees();

        //testObject.GetComponent<MeshFilter>().mesh = GenerateCircle(radius, vertexList);

        //testVerts.Clear();
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
            Vector3 vertLocation = new Vector3(Mathf.Cos(DegreeList[i] * Mathf.Deg2Rad * -1) * radius, 0, Mathf.Sin(DegreeList[i] * Mathf.Deg2Rad * -1) * radius);
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


    private Mesh TestGenerate(float rad, List<Vector3> vertices)
    {
        var circle = new Mesh();

        var indices = new int[CircleIndexCount];


        var segmentWidth = Mathf.PI * 2f / CircleSegmentCount;

        var angle = 0f;



        vertices.Add(Vector3.zero);

        for (int i = 1; i < CircleVertexCount; ++i)
        {
            angle = DegreeList[i - 1] * Mathf.Deg2Rad;

            Debug.Log("Degree List: " + DegreeList[i - 1] + "Degree List Size: " + DegreeList.Count);

            vertices.Add(new Vector3(Mathf.Cos(angle) * rad, 0f, Mathf.Sin(angle) * rad));



            //angle -= segmentWidth;

            if (i > 1)
            {
                var j = (i - 2) * 3;
                indices[j + 0] = 0;
                indices[j + 1] = i - 1;
                indices[j + 2] = i;
            }


        }


        Color[] colors = new Color[vertices.Count];

        for (int i = 0; i < vertices.Count; i++)
        {
            colors[i] = Color.red;
        }



        circle.SetVertices(vertices);
        circle.colors = colors;
        circle.SetIndices(indices, MeshTopology.Triangles, 0);
        circle.RecalculateBounds();
        circle.RecalculateNormals();

        return circle;
    }


}
