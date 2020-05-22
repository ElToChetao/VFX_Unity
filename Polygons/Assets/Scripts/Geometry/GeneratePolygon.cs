using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratePolygon : MonoBehaviour
{
    private Mesh mesh;
    private MeshRenderer render;
    private float time;

    [Range(1, 5)]
    public float radius = 1;

    [Range(0.2f, 5)]
    public float maxHeight;

    private float lastHeight;
    private Vector3 newScale;
    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        render = GetComponent<MeshRenderer>();
    }
    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, newScale, Time.deltaTime * time);
    }
    void OnEnable()
    {
        newScale = Vector3.one * Random.Range(0.7f, 1.7f);
        UpdateFigure();
        time = Random.Range(8f, 15f);
        Invoke("Destroy", 2);
    }
    private void Destroy()
    {
        gameObject.SetActive(false);
        transform.position = Vector3.zero;
        transform.eulerAngles = Vector3.zero;
        transform.localScale = Vector3.one;
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void UpdateFigure()
    {
        GenerateRandomMesh();
    }

    private void UpdateMesh(Vector3[] verts, int[] tri)
    {
        mesh.Clear();
        mesh.vertices = verts;
        mesh.triangles = tri;
        render.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        transform.localScale = Vector3.zero;
    }

    private void GenerateRandomMesh()
    {
        int numVertices = Random.Range(4, 8);

        Vector3[] vertices = new Vector3[numVertices * 2 + 2];
        float[] angles = new float[numVertices];

        for (int i = 0; i < numVertices; i++)
        {
            int offset = 360 / numVertices;

            angles[i] = offset * i;
            angles[i] += Random.Range(-offset / 4, offset / 4);
            angles[i] *= Mathf.Deg2Rad;
        }
        Vector3[] lowVertices = LowPrismaVertices(numVertices + 1, angles);

        for (int i = 0; i < numVertices; i++)
        {
            int offset = 360 / numVertices;

            angles[i] = offset * i;
            angles[i] += Random.Range(-offset / 4, offset / 4);
            angles[i] *= Mathf.Deg2Rad;
        }
        Vector3[] upVertices = UpPrismaVertices(numVertices + 1, angles);

        PiramidCover(numVertices, lowVertices);
        PiramidCover(numVertices, upVertices);
        MiddleCover(numVertices, lowVertices, upVertices);

        int index = 0;
        foreach (Vector3 v in upVertices)
        {
            vertices[index] = v;
            index++;
        }
        foreach (Vector3 v in lowVertices)
        {
            vertices[index] = v;
            index++;
        }

        int[] triangles = new int[numVertices * 12];
        int j = 0;
        int n = numVertices + 1;
        int last = vertices.Length - 1;
        for (int i = 0; i < triangles.Length; i += 12)
        {
            triangles[i + 7] = j;
            triangles[i + 8] = n - 1;
            triangles[i + 11] = last;


            triangles[i] = j;
            triangles[i + 2] = j + n;
            triangles[i + 3] = j + n;
            triangles[i + 9] = j + n;

            if (j + n >= numVertices * 2)
            {
                triangles[i + 5] = n;
                triangles[i + 10] = n;
            }
            else
            {
                triangles[i + 5] = j + n + 1;
                triangles[i + 10] = j + n + 1;
            }

            j++;

            if (j >= n - 1)
            {
                triangles[i + 1] = 0;
                triangles[i + 4] = 0;
                triangles[i + 6] = 0;
            }
            else
            {
                triangles[i + 6] = j;
                triangles[i + 1] = j;
                triangles[i + 4] = j;
            }

        }

        UpdateMesh(vertices, triangles);
    }

    private void MiddleCover(int numVertices, Vector3[] lowVertices, Vector3[] upVertices)
    {
        for (int i = 0; i < numVertices; i++)
        {
            GenerateEdge(upVertices[i], lowVertices[i]);
        }
        for (int i = 0; i < numVertices; i++)
        {
            if (i == numVertices - 1)
            {
                GenerateEdge(lowVertices[i], upVertices[0]);
            }
            else
            {
                GenerateEdge(lowVertices[i], upVertices[i + 1]);
            }
        }
    }

    private void PiramidCover(int numVertices, Vector3[] upVertices)
    {
        for (int i = 0; i < numVertices; i++)
        {
            GenerateEdge(upVertices[i], upVertices[numVertices]);
        }
        for (int i = 0; i < numVertices; i++)
        {
            if (i == numVertices - 1)
            {
                GenerateEdge(upVertices[i], upVertices[0]);
            }
            else
            {
                GenerateEdge(upVertices[i], upVertices[i + 1]);
            }
        }
    }

    private void GenerateEdge(Vector3 a, Vector3 b)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Destroy(go.GetComponent<BoxCollider>());

        Vector3 middle = (a + b) / 2;
        go.transform.SetParent(transform);
        
        float dist = Vector3.Distance(a, b);
        go.transform.localScale = new Vector3(0.04f, 0.04f, dist);
        go.transform.position = middle;

        Vector3 worldA = transform.TransformPoint(a);
        go.transform.LookAt(worldA);

        go.GetComponent<MeshRenderer>().material.color = Color.black;
        
    }
    private Vector3[] UpPrismaVertices(int numVertices, float[] angles)
    {
        Vector3[] upVertices = new Vector3[numVertices];

        float maxHeight = lastHeight;
        for (int i = 0; i < upVertices.Length - 1; i++)
        {
            float randRadius = Random.Range(radius, radius * 1.5f);
            float x = randRadius * Mathf.Cos(angles[i]);
            float z = randRadius * Mathf.Sin(angles[i]);

            float randY = lastHeight + Random.Range(1f, 3f);

            if (randY > maxHeight)
            {
                maxHeight = randY;
            }

            upVertices[i] = new Vector3(x, randY, z);
        }
        lastHeight = maxHeight;

        float angle = Random.Range(1, 360) * Mathf.Deg2Rad;
        float r = Random.Range(0f, radius);
        Vector3 pos;
        pos.x = r * Mathf.Cos(angle);
        r = Random.Range(0f, radius);
        pos.z = r * Mathf.Sin(angle);
        pos.y = lastHeight + Random.Range(1f, 3f);

        upVertices[numVertices - 1] = pos;
        return upVertices;
    }

    private Vector3[] LowPrismaVertices(int numVertices, float[] angles)
    {
        Vector3[] lowVertices = new Vector3[numVertices];

        float angle = Random.Range(1, 360) * Mathf.Deg2Rad;
        float r = Random.Range(0f, radius);

        Vector3 pos;
        pos.x = r * Mathf.Cos(angle);
        r = Random.Range(0f, radius);
        pos.z = r * Mathf.Sin(angle);
        pos.y = Random.Range(-3f, -1f);
        lastHeight = pos.y;

        lowVertices[numVertices - 1] = pos;

        float maxHeight = lastHeight;
        for (int i = 0; i < lowVertices.Length - 1; i++)
        {
            float randRadius = Random.Range(radius, radius * 1.5f);
            float x = randRadius * Mathf.Cos(angles[i]);
            float z = randRadius * Mathf.Sin(angles[i]);

            float randY = lastHeight + Random.Range(1f, 3f) + 1;
            if(randY > maxHeight)
            {
                maxHeight = randY;
            }
            lowVertices[i] = new Vector3(x, randY, z);
        }
        lastHeight = maxHeight;
        return lowVertices;
    }
}
