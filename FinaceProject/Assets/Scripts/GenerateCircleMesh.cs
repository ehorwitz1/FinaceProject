using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCircleMesh : MonoBehaviour
{
    public GameObject startingObject;
    public GameObject topObject;


    public List<Vector3> vertices;

    public float radius;

    private const int CircleSegmentCount = 64;
    //public const int CircleSegmentCount = 12;

    private const int CircleVertexCount = CircleSegmentCount + 2;
    private const int CircleIndexCount = CircleSegmentCount * 3;

    // Start is called before the first frame update
    void Start()
    {
        startingObject = this.gameObject;
        vertices = new List<Vector3>(CircleVertexCount);

        startingObject.GetComponent<MeshFilter>().mesh = GenerateCircle();
        topObject.GetComponent<MeshFilter>().mesh = GenerateCircle(4f);


    }

    // Update is called once per frame
    void Update()
    {
        startingObject.GetComponent<MeshFilter>().mesh = GenerateCircle();
        //showColumn();
    }



    private  Mesh GenerateCircle()
    {

        var circle = new Mesh();

        var indices = new int[CircleIndexCount];

        var segmentWidth = Mathf.PI * 2f / CircleSegmentCount;

        //Debug.Log("Segment Width " + segmentWidth);

        var angle = 0f;

        vertices.Clear();

        vertices.Add(Vector3.zero);

        for (int i = 1; i < CircleVertexCount; ++i)
        {

            vertices.Add(new Vector3(Mathf.Cos(angle)*radius, 0f, Mathf.Sin(angle)*radius));

            angle -= segmentWidth;
            //Debug.Log("Angle Index: " + i + " " + angle + " " + Mathf.Cos(angle) + " " + Mathf.Sin(angle));

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
            if( (float)i / vertices.Count < .33f)
            {
                colors[i] = Color.red;
            }
            else if ((float)i / vertices.Count >= .33f && (float)i / vertices.Count <= .66f)
            {
                colors[i] = Color.green;
            }
            else
            {
                colors[i] = Color.blue;
            }
            
        }
            
        
        circle.SetVertices(vertices);
        circle.colors = colors;
        circle.SetIndices(indices, MeshTopology.Triangles, 0);
        circle.RecalculateBounds();
        circle.RecalculateNormals();

        return circle;
    }


    private Mesh GenerateCircle(float rad)
    {

        var circle = new Mesh();

        var indices = new int[CircleIndexCount];

        var segmentWidth = Mathf.PI * 2f / CircleSegmentCount;

        //Debug.Log("Segment Width " + segmentWidth);

        var angle = 0f;


        
        vertices.Add(Vector3.zero);

        for (int i = 1; i < CircleVertexCount; ++i)
        {
            vertices.Add(new Vector3(Mathf.Cos(angle) * rad, 0f, Mathf.Sin(angle) * rad));

            angle -= segmentWidth;
            //Debug.Log("Angle Index: " + i + " " + angle + " " + Mathf.Cos(angle) + " " + Mathf.Sin(angle));

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
            if ((float)i / vertices.Count < .33f)
            {
                colors[i] = Color.red;
            }
            else if ((float)i / vertices.Count >= .33f && (float)i / vertices.Count <= .66f)
            {
                colors[i] = Color.green;
            }
            else
            {
                colors[i] = Color.blue;
            }

        }


        circle.SetVertices(vertices);
        circle.colors = colors;
        circle.SetIndices(indices, MeshTopology.Triangles, 0);
        circle.RecalculateBounds();
        circle.RecalculateNormals();

        return circle;
    }


    //Pass in 2 lists of vertices (bottom and top references)
    //Connect bottom row verts with top row verts using Mesh Topology quads
    public void GenerateColumns()
    {
        //b0-b1
        //b1-t1
        //t1-t0
        //t0-b0

    }

    public void showColumn()
    {

        for (int i = 0; i < vertices.Count; i++)
        {
            if ((float)i / vertices.Count < .33f)
            {
                Debug.DrawRay(vertices[i], Vector3.up * 10, Color.red);
            }
            else if ((float)i / vertices.Count >= .33f && (float)i / vertices.Count <= .66f)
            {
                Debug.DrawRay(vertices[i], Vector3.up * 10, Color.green);
            }
            else
            {
                Debug.DrawRay(vertices[i], Vector3.up * 10, Color.blue);
            }

        }
    }

    private void OnDrawGizmos()
    {
        float i = 0;
        foreach(Vector3 vect in vertices)
        {

            if(i/CircleSegmentCount <= .33f)
            {
                //Debug.Log("i Percentage: " + i / CircleSegmentCount + " Yellow");

                Gizmos.color = Color.red;
                Gizmos.DrawSphere(vect, .05f);
            }
            else if((i/CircleSegmentCount > .33f) && (i / CircleSegmentCount <= .66f))
            {
                //Debug.Log("i Percentage: " + i / CircleSegmentCount + " Red");
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(vect, .05f);
            }
            else
            {
                //Debug.Log("i Percentage: " + i / CircleSegmentCount + " Blue");
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(vect, .05f);
            }
            
            i++;
        }
    }

}
