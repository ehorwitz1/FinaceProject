using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateStack : MonoBehaviour
{
    public GameObject emptyMesh;

    List<MeshHolder> totalMeshes;
    List<GameObject> totalObjects;

    public float rad;

    public const int CircleSegmentCount = 128;

    public const int CircleVertexCount = CircleSegmentCount + 2;
    public const int CircleIndexCount = CircleSegmentCount * 3;

    MeshHolder bottomMesh;
    MeshHolder topMesh;

    public class MeshHolder
    {
        public List<Vector3> verts;

        public Mesh shapeMesh;

        public Vector3 offset;

        public MeshHolder()
        {
            verts = new List<Vector3>();
            shapeMesh = new Mesh();
        }

        public MeshHolder(Vector3 off)
        {
            verts = new List<Vector3>();
            shapeMesh = new Mesh();
            offset = off;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        totalMeshes = new List<MeshHolder>();
        totalObjects = new List<GameObject>();

        for (int i = 0; i < 5; i++)
        {
            MeshHolder tempMesh = new MeshHolder(new Vector3(0, i * 3, 0));
            tempMesh.shapeMesh = GenerateCircle(2 + i%2, tempMesh.verts, tempMesh.offset);
            totalMeshes.Add(tempMesh);

            GameObject pieSlice = Instantiate(emptyMesh);
            pieSlice.transform.position = new Vector3(0, i * 3, 0);
            totalObjects.Add(pieSlice);

            //totalMeshes.Add());
        }

        for (int i = 0; i < totalObjects.Count; i++)
        {
            totalObjects[i].GetComponent<MeshFilter>().mesh = totalMeshes[i].shapeMesh;
        }

        for (int i = 0; i < totalObjects.Count-1; i++)
        {
            GameObject testWrap = Instantiate(emptyMesh);
            testWrap.GetComponent<MeshFilter>().mesh = GenerateWrap(totalMeshes[i], totalMeshes[i + 1]);
        }


        //GameObject testWrap1 = Instantiate(emptyMesh);
        //testWrap1.GetComponent<MeshFilter>().mesh = GenerateWrap(totalMeshes[0], totalMeshes[1]);

        //GameObject testWrap2 = Instantiate(emptyMesh);
        //testWrap2.GetComponent<MeshFilter>().mesh = GenerateWrap(totalMeshes[1], totalMeshes[2]);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Mesh GenerateWrap(MeshHolder bottom, MeshHolder top)
    {
        Mesh wrap = new Mesh();

        List<Vector3> vertices = new List<Vector3>();

        int[] indices = new int[CircleIndexCount * 4];

        for (int i = 0; i < CircleVertexCount - 2; i++)
        {

            vertices.Add(bottom.verts[i + 1]);
            vertices.Add(bottom.verts[i + 2]);
            vertices.Add(top.verts[i + 2]);
            vertices.Add(top.verts[i + 1]);


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
            /*
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
            */
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

    private Mesh GenerateCircle(float rad, List<Vector3> vertices, Vector3 offset)
    {

        var circle = new Mesh();

        var indices = new int[CircleIndexCount];

        var segmentWidth = Mathf.PI * 2f / CircleSegmentCount;


        var angle = 0f;



        //vertices.Add(Vector3.zero);
        vertices.Add(offset);


        for (int i = 1; i < CircleVertexCount; ++i)
        {
            vertices.Add(new Vector3(Mathf.Cos(angle) * rad + offset.x, offset.y, Mathf.Sin(angle) * rad + offset.z));

            angle -= segmentWidth;

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

            //Debug.Log("Vert Count: " + vertices.Count + " Vert Count * (1/3) " + (float)vertices.Count * (1f / 3f));

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


        circle.SetVertices(vertices);
        circle.colors = colors;
        circle.SetIndices(indices, MeshTopology.Triangles, 0);
        circle.RecalculateBounds();
        circle.RecalculateNormals();

        return circle;
    }

    private Mesh GenerateCircle(float rad, List<Vector3> vertices)
    {

        var circle = new Mesh();

        var indices = new int[CircleIndexCount];

        var segmentWidth = Mathf.PI * 2f / CircleSegmentCount;

        var angle = 0f;



        vertices.Add(Vector3.zero);

        for (int i = 1; i < CircleVertexCount; ++i)
        {
            vertices.Add(new Vector3(Mathf.Cos(angle) * rad, 0f, Mathf.Sin(angle) * rad));

            angle -= segmentWidth;

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



    public void showColumn(MeshHolder bot, MeshHolder top)
    {

        for (int i = 0; i < bot.verts.Count; i++)
        {
            if ((float)i / bot.verts.Count < .33f)
            {
                Debug.DrawRay(bot.verts[i], top.verts[i] * 5, Color.red);
            }
            else if ((float)i / bot.verts.Count >= .33f && (float)i / bot.verts.Count <= .66f)
            {
                Debug.DrawRay(bot.verts[i], top.verts[i] * 5, Color.green);
            }
            else
            {
                Debug.DrawRay(bot.verts[i], top.verts[i] * 5, Color.blue);
            }

        }
    }
}
