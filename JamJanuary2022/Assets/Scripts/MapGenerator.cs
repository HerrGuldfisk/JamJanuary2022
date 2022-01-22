using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class MapGenerator : MonoBehaviour
{
    [SerializeField] GameObject soundPopPrefab;

    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    public static int xSize = 100;
    public static int zSize = 100;

    public GameObject[] players;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        if (players != null)
        {
            Debug.Log(players);
            players = GameObject.FindGameObjectsWithTag("Player");
        }

        CreateInitShape();
        //UpdateMesh();
    }

    private void Update()
    {
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
        //StartCoroutine(CreateNewShapeEnum(playerPos, enemyPos, vertices, triangles));
        //UpdateMesh();
        GameObject.Instantiate(soundPopPrefab, enemyPos, Quaternion.identity);
    }

    void CreateNewShape(Vector3 pPos, Vector3 ePos, Vector3[] prevVertices)
    {
        Vector3Int roundpPos = new Vector3Int(Mathf.RoundToInt(pPos.x), Mathf.RoundToInt(pPos.y), Mathf.RoundToInt(pPos.z));
        Vector3Int roundePos = new Vector3Int(Mathf.RoundToInt(ePos.x), Mathf.RoundToInt(ePos.y), Mathf.RoundToInt(ePos.z));

        ePos.y = roundePos.y;

        float dist = Vector3.Distance(ePos, pPos);
        int raiseArea = 1 + Mathf.RoundToInt(dist/10);
        float raiseHeight = 1f + dist/4f;

        for (var i = 0; i < prevVertices.Length; i++)
        {
            if (prevVertices[i] == new Vector3(roundePos.x, prevVertices[i].y, roundePos.y))
            {
                for (int u = roundePos.x - raiseArea; u < roundePos.x + raiseArea; u++)
                {
                    for (int v = roundePos.z - raiseArea; v < roundePos.z + raiseArea; v++)
                    {
                        for (var j = 0; j < prevVertices.Length; j++)
                        {
                            /*if (prevVertices[j] == new Vector3(u, ePos.y, v))
                            {
                                if (roundpPos != new Vector3(u, ePos.y, v))
                                {
                                    prevVertices[j].y += raiseHeight;
                                }
                                else {
                                    prevVertices[j].y += raiseHeight;
                                }
                            }*/
                            if (prevVertices[j] == new Vector3(u, prevVertices[j].y, v))
                            {
                                prevVertices[j].y += raiseHeight;
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

    IEnumerator CreateNewShapeEnum(Vector3 pPos, Vector3 ePos, Vector3[] prevVertices, int[] prevTriangles)
    {
        Vector3Int roundpPos = new Vector3Int(Mathf.RoundToInt(pPos.x), Mathf.RoundToInt(pPos.y), Mathf.RoundToInt(pPos.z));
        Vector3Int roundePos = new Vector3Int(Mathf.RoundToInt(ePos.x), Mathf.RoundToInt(ePos.y) - 1, Mathf.RoundToInt(ePos.z));

        float dist = Vector3.Distance(ePos, pPos);
        int raiseArea = Mathf.RoundToInt(dist / 10);
        int raiseHeight = Mathf.RoundToInt(dist / 10);

        for (var i = 0; i < prevVertices.Length; i++)
        {
            if (prevVertices[i] == roundePos)
            {
                for (int u = roundePos.x - raiseArea; u < roundePos.x + raiseArea; u++)
                {
                    for (int v = roundePos.z - raiseArea; v < roundePos.z + raiseArea; v++)
                    {
                        for (var j = 0; j < prevVertices.Length; j++)
                        {
                            if (prevVertices[j] == new Vector3(u, roundePos.y, v))
                            {
                                if (roundpPos != new Vector3(u, roundePos.y, v))
                                {
                                    Debug.Log("Player pos: " + roundpPos + " " + "Enemy pos: " + new Vector3(u, roundePos.y, v));
                                    prevVertices[j].y += raiseHeight;
                                }
                                else
                                {
                                    Debug.Log("Player is here!");
                                }
                            }
                        }
                    }
                }
            }
        }


        //triangles = new int[xSize * zSize * 6];
        triangles = prevTriangles;

        int vert = 0;
        int tris = 0;

        /*
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

                yield return new WaitForSeconds(0.00001f);
            }
            vert++;
        }*/

        for (int z = roundePos.z - raiseArea; z < roundePos.z + raiseArea; z++)
        {
            for (int x = roundePos.x - raiseArea; x < roundePos.x + raiseArea; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;

                yield return new WaitForSeconds(0.1f);
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
                float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 2f;
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