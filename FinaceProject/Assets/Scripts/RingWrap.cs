using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingWrap : MonoBehaviour
{
    public const int CircleSegmentCount = 32;

    public const int CircleVertexCount = CircleSegmentCount + 2;
    public const int CircleIndexCount = CircleSegmentCount * 3;


    public GameObject wrapObject;

    public GameObject top;
    public GameObject bottom;

    public List<Vector3> vertices;

    List<Vector3> topVerts;
    List<Vector3> bottomVerts;

    public Vector3 heightDiff;

    public Color color;

    // Start is called before the first frame update
    void Start()
    {
        topVerts = new List<Vector3>();
        bottomVerts = new List<Vector3>();
        vertices = new List<Vector3>();



        Invoke("SetWrap", 1f);

    }

    public void SetWrap()
    {
        heightDiff = top.transform.position - bottom.transform.position;

        topVerts = top.GetComponent<GenerateRing>().outerRing;
        bottomVerts = bottom.GetComponent<GenerateRing>().outerRing;



        wrapObject.GetComponent<MeshFilter>().mesh = GenerateWrap();
    }

    // Update is called once per frame
    void Update()
    {
        //SetWrap();
    }


    public Mesh GenerateWrap()
    {
        Mesh wrap = new Mesh();

        int[] indices = new int[CircleIndexCount * 4];

        for (int i = 0; i < CircleVertexCount - 2; i++)
        //for (int i = 0; i < 1; i++)
        {

            vertices.Add(bottomVerts[i + 1]);
            vertices.Add(bottomVerts[i + 2]);
            vertices.Add(topVerts[i + 2] + heightDiff);
            vertices.Add(topVerts[i + 1] + heightDiff);


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



    public Mesh GenerateWrap(List<Vector3> bottom, List<Vector3> top)
    {
        Mesh wrap = new Mesh();

        List<Vector3> vertices = new List<Vector3>();

        int[] indices = new int[CircleIndexCount * 4];

        Debug.Log("bottom Count" + bottom.Count);

        for (int i = 0; i < CircleVertexCount - 2; i++)
        {

            vertices.Add(bottom[i + 1]);
            vertices.Add(bottom[i + 2]);
            vertices.Add(top[i + 2]);
            vertices.Add(top[i + 1]);


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

            if ((float)i < (float)vertices.Count * (1f / 3f))
            {
                colors[i] = Color.red;
            }
            else if ((float)i >= (float)vertices.Count * (1f / 3f) && (float)i <= (float)vertices.Count * (2f / 3f))
            {
                colors[i] = Color.green;
            }
            else
            {
                colors[i] = Color.blue;
            }

        }


        wrap.SetVertices(vertices);
        wrap.SetIndices(indices, MeshTopology.Quads, 0);
        wrap.colors = colors;
        wrap.RecalculateBounds();
        wrap.RecalculateNormals();


        return wrap;
    }
}
