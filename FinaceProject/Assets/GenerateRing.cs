using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

public class GenerateRing : MonoBehaviour
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

    public List<Vector3> vertices;

    public float rotationValue;

    public Color color;

    public List<Vector3> innerRing;
    public List<Vector3> outerRing;

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
            float xValue = Mathf.Cos(DegreeList[i] * Mathf.Deg2Rad * -1) * radius;
            float zValue = Mathf.Sin(DegreeList[i] * Mathf.Deg2Rad * -1) * radius;

            Vector3 vertLocation = new Vector3(xValue + transform.position.x, transform.position.y, zValue + transform.position.z);
            Gizmos.DrawSphere(vertLocation, .05f);
        }

        for (int i = 0; i < DegreeList.Count; ++i)
        {
            float xValue = Mathf.Cos(DegreeList[i] * Mathf.Deg2Rad * -1) * radius * 1.5f;
            float zValue = Mathf.Sin(DegreeList[i] * Mathf.Deg2Rad * -1) * radius * 1.5f;

            Vector3 vertLocation = new Vector3(xValue + transform.position.x, transform.position.y, zValue + transform.position.z);
            Gizmos.DrawSphere(vertLocation, .05f);
        }
    }


    public void GenerateRings()
    {
        innerRing.Clear();
        outerRing.Clear();
        for (int i = 0; i < DegreeList.Count; i++)
        {
            float xValue = Mathf.Cos(DegreeList[i] * Mathf.Deg2Rad * -1) * radius;
            float zValue = Mathf.Sin(DegreeList[i] * Mathf.Deg2Rad * -1) * radius;

            //Vector3 vertLocation = new Vector3(xValue + transform.position.x, transform.position.y, zValue + transform.position.z);
            Vector3 vertLocation = new Vector3(xValue + testObject.transform.localPosition.x, testObject.transform.localPosition.y, zValue + testObject.transform.localPosition.z);

            innerRing.Add(vertLocation);
        }

        for (int i = 0; i < DegreeList.Count; i++)
        {
            float xValue = Mathf.Cos(DegreeList[i] * Mathf.Deg2Rad * -1) * radius * 1.5f;
            float zValue = Mathf.Sin(DegreeList[i] * Mathf.Deg2Rad * -1) * radius * 1.5f;

            //Vector3 vertLocation = new Vector3(xValue + transform.position.x, transform.position.y, zValue + transform.position.z);
            Vector3 vertLocation = new Vector3(xValue + testObject.transform.localPosition.x, testObject.transform.localPosition.y, zValue + testObject.transform.localPosition.z);

            outerRing.Add(vertLocation);
        }
    }

    public Mesh RingConnector(List<Vector3> bottom, List<Vector3> top)
    {
        Mesh wrap = new Mesh();

        int[] indices = new int[CircleIndexCount * 4];

        for (int i = 0; i < CircleVertexCount - 2; i++)
        //for (int i = 0; i < 4; i++)
        {

            vertices.Add(bottom[i + 1]);
            vertices.Add(bottom[i + 2]);
            vertices.Add(top[i + 2] );
            vertices.Add(top[i + 1] );


            int j = i * 4;

            indices[j + 0] = j + 0;
            indices[j + 1] = j + 1;
            indices[j + 2] = j + 2;
            indices[j + 3] = j + 3;

        }


        // create new colors array where the colors will be created.
        Color[] colors = new Color[vertices.Count];

        for (int i = 0; i < vertices.Count; i++)
        {
            colors[i] = color;
        }


        wrap.SetVertices(vertices);
        wrap.SetIndices(indices, MeshTopology.Quads, 0);
        wrap.colors = colors;
        wrap.RecalculateBounds();
        wrap.RecalculateNormals();

        return wrap;
    }




    // Start is called before the first frame update
    void Start()
    {
        CalcDegrees();
        GenerateRings();

        testObject.GetComponent<MeshFilter>().mesh = RingConnector(outerRing, innerRing);

    }

    // Update is called once per frame
    void Update()
    {
    }
}
