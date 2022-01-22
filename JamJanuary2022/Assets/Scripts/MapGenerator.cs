using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class MapGenerator : MonoBehaviour
{

    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    public int xSize = 1000;
    public int zSize = 1000;

    public GameObject[] players;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        if (players == null)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
        }

        CreateInitShape();
        UpdateMesh();
    }

    public void OnKill(Vector3 enemyPos)
    {
        Vector3 playerPos = Vector3.zero;

        foreach (GameObject player in players)
        {
            playerPos = player.transform.position;
        }

        CreateNewShape(playerPos, enemyPos, vertices);
        UpdateMesh();
    }

    void CreateNewShape(Vector3 pPos, Vector3 ePos, Vector3[] prevVertices)
    {
        Vector3Int roundePos = new Vector3Int(Mathf.RoundToInt(ePos.x), Mathf.RoundToInt(ePos.y) - 1, Mathf.RoundToInt(ePos.z));

        for (var i = 0; i < prevVertices.Length; i++)
        {
            if (prevVertices[i] == roundePos)
            {
                for (int u = roundePos.x - 3; u < roundePos.x + 3; u++)
                {
                    for (int v = roundePos.z - 3; v < roundePos.z + 3; v++)
                    {
                        for (var j = 0; j < prevVertices.Length; j++)
                        {
                            if (prevVertices[j] == new Vector3(u, roundePos.y, v))
                            {
                                prevVertices[j].y++;
                            }
                        }
                    }
                }
            }
        }


        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    void CreateInitShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                //float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 2f;
                float y = 0f;
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        mesh.RecalculateBounds();
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
    }
}