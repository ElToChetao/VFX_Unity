using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteStuffGenerator : MonoBehaviour
{
    private Mesh mesh;
    private MeshRenderer render;

    public float radius = 3f;
    public float height = 0.5f;
    public float angle = 30f;
    public int numRectangles = 10;
    public float heightOffset = 0.0f;

    private float speed;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        render = GetComponent<MeshRenderer>();
    }
    private void OnEnable()
    {
        speed = Random.Range(360f, 720f);
        float timeToDestroy = 360f / speed;
        int rand = Random.Range(0, 2);
        if(rand == 0)
        {
            speed *= -1;
        }
        GeneratePolygon();
        Invoke("Destroy", timeToDestroy);
    }
    private void Destroy()
    {
        gameObject.SetActive(false);
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
    private void Update()
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }

    private void GeneratePolygon()
    {
        numRectangles = Random.Range(7, 12);
        radius = Random.Range(5.0f, 6.0f);
        height = Random.Range(0.1f, 0.4f);

        Vector3[,] circVerts = new Vector3[numRectangles, 4];

        float currentAngle = 0;
        Vector3 firstVert = CreateVert(currentAngle, radius - 0.25f, height / 2);

        currentAngle += angle * 2;
        for (int i = 0; i < numRectangles; i++)
        {
            currentAngle += angle;

            circVerts[i, 0] = CreateVert(currentAngle, radius, height + Random.Range(-heightOffset, heightOffset));
            circVerts[i, 1] = CreateVert(currentAngle, radius - 0.5f, height + Random.Range(-heightOffset, heightOffset));
            circVerts[i, 2] = CreateVert(currentAngle, radius - 0.5f, Random.Range(-heightOffset, heightOffset));
            circVerts[i, 3] = CreateVert(currentAngle, radius, Random.Range(-heightOffset, heightOffset));
        }
        currentAngle += angle * 3;

        Vector3 lastVert = CreateVert(currentAngle, radius - 0.25f, height / 2);

        for (int i = 0; i < 4; i++)
        {
            GenerateEdge(firstVert, circVerts[0, i]);
            for (int j = 0; j < numRectangles - 1; j++)
            {
                GenerateEdge(circVerts[j, i], circVerts[j + 1, i]);
            }
            GenerateEdge(circVerts[numRectangles - 1, i], lastVert);
        }

        Vector3[] finalVerts = new Vector3[numRectangles * 4 + 2];

        int index = 0;
        finalVerts[index] = firstVert;

        for (int i = 0; i < numRectangles; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                index++;
                finalVerts[index] = circVerts[i, j];
            }
        }
        index++;
        finalVerts[index] = lastVert;

        int[] triangles = new int[numRectangles * 8 * 3];

        int n = 1;
        int triIndex = 0;
        for (int i = 0; i < 4; i++)
        {

            triangles[triIndex] = 0;
            triIndex++;
            if (n + 1 > 4)
            {
                triangles[triIndex] = 1;
            }
            else
            {
                triangles[triIndex] = n + 1;
            }
            triIndex++;

            triangles[triIndex] = n;
            triIndex++;

            n++;
        }
        int currentMin = 1;
        int currentMax = 4;
        for (int i = 0; i < (numRectangles - 1); i++)
        {
            for (int j = 0; j < 4; j++)
            {
                //first triangle
                triangles[triIndex] = currentMin + j;
                triIndex++;
                if (currentMin + j + 1 > currentMax)
                {
                    triangles[triIndex] = currentMin;
                }
                else
                {
                    triangles[triIndex] = currentMin + j + 1;
                }
                triIndex++;
                triangles[triIndex] = currentMin + j + 4;
                triIndex++;

                //second triangle
                triangles[triIndex] = currentMin + j + 4;
                triIndex++;

                if(currentMin + j + 1 > currentMax)
                {
                    triangles[triIndex] = currentMin;
                    triIndex++;
                    triangles[triIndex] = triangles[triIndex] = currentMin + 4;
                }
                else
                {
                    triangles[triIndex] = currentMin + j + 1;
                    triIndex++;
                    triangles[triIndex] = triangles[triIndex] = currentMin + j + 4 + 1;
                }
                triIndex++;
            }
            currentMin += 4;
            currentMax += 4;
        }

        for (int i = 0; i < 4; i++)
        {
            triangles[triIndex] = currentMin + i;
            triIndex++;

            if (currentMin + 1 + i > currentMax)
            {
                triangles[triIndex] = currentMin;
            }
            else
            {
                triangles[triIndex] = currentMin + 1 + i;
            }
            triIndex++;

            triangles[triIndex] = finalVerts.Length - 1;
            triIndex++;

            n++;
        }

        UpdateMesh(finalVerts, triangles);
    }

    private Vector3 CreateVert(float angle, float radius, float height)
    {
        angle *= Mathf.Deg2Rad;

        Vector3 vert;
        vert.x = radius * Mathf.Cos(angle);
        vert.y = height;
        vert.z = radius * Mathf.Sin(angle);

        return vert;
    }

    private void UpdateMesh(Vector3[] verts, int[] tri)
    {
        mesh.Clear();
        mesh.vertices = verts;
        mesh.triangles = tri;
        render.material.color = Color.white;
    }
    private void GenerateEdge(Vector3 a, Vector3 b)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Destroy(go.GetComponent<BoxCollider>());

        Vector3 middle = (a + b) / 2;
        go.transform.SetParent(transform);

        float dist = Vector3.Distance(a, b);
        go.transform.localScale = new Vector3(0.04f, 0.04f, dist);
        go.transform.localPosition = middle;

        Vector3 worldA = transform.TransformPoint(a);
        go.transform.LookAt(worldA);

        go.GetComponent<MeshRenderer>().material.color = Color.black;
    }
}
