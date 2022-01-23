using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class MapGenerator : MonoBehaviour
{
    #region Standard

    [SerializeField] GameObject soundPopPrefab;

    Mesh mesh;
    Mesh oldMesh;
    MeshFilter meshFilter;

    Vector3[] vertices;
    int[] triangles;

    public static int xSize = 30;
    public static int zSize = 30;

    public GameObject[] players;

    #endregion

    #region Added

    Mesh originalMesh;
    Mesh clonedMesh;

    [HideInInspector]
    public int targetIndex;

    [HideInInspector]
    public Vector3 targetVertex;

    [HideInInspector]
    public Vector3[] originalVertices;

    [HideInInspector]
    public Vector3[] modifiedVertices;

    [HideInInspector]
    public Vector3[] normals;

    [HideInInspector]
    public bool isMeshReady = false;
    public bool isEditMode = true;
    public bool showTransformHandle = true;
    public bool isCloned = false;
    public List<int> selectedIndices = new List<int>();
    public float pickSize = 0.01f;

    //Deforming Settings
    public float radiusOfEffect = 0.3f;
    public float pullValue = 1.3f;

    //Animation Settings
    public float duration = 1.2f;
    int currentIndex = 0;
    bool isAnimate = false;
    float startTime = 0f;
    float runTime = 0f;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        if (players != null)
        {
            Debug.Log(players);
            players = GameObject.FindGameObjectsWithTag("Player");
        }

        CreateInitShape();
        UpdateMesh();

        //Init();
    }

    public void Init()
    {
        meshFilter = GetComponent<MeshFilter>();
        isMeshReady = false;

        currentIndex = 0;

        if (isEditMode)
        {
            originalMesh = meshFilter.sharedMesh;
            clonedMesh = new Mesh();
            clonedMesh.name = "clone";
            clonedMesh.vertices = originalMesh.vertices;
            clonedMesh.triangles = originalMesh.triangles;
            clonedMesh.normals = originalMesh.normals;
            meshFilter.mesh = clonedMesh;

            originalVertices = clonedMesh.vertices;
            normals = clonedMesh.normals;
            Debug.Log("Init & Cloned");
        }
        else
        {
            originalMesh = meshFilter.mesh;
            originalVertices = originalMesh.vertices;
            normals = originalMesh.normals;
            modifiedVertices = new Vector3[originalVertices.Length];
            for (int i = 0; i < originalVertices.Length; i++)
            {
                modifiedVertices[i] = originalVertices[i];
            }

            StartDisplacement();
        }

    }

    /*protected void FixedUpdate() //1
    {
        if (!isAnimate) //2
        {
            return;
        }

        runTime = Time.time - startTime; //3

        Debug.Log("Running Update");

        if (runTime < duration)  //4
        {
            Vector3 targetVertexPos =
                meshFilter.transform.InverseTransformPoint(targetVertex);
            DisplaceVertices(targetVertexPos, pullValue, radiusOfEffect);
        }
        else //5
        {
            currentIndex++;
            if (currentIndex < selectedIndices.Count) //6
            {
                StartDisplacement();
            }
            else //7
            {
                originalMesh = GetComponent<MeshFilter>().mesh;
                isAnimate = false;
                isMeshReady = true;
            }
        }
    }
    */
    public void OnKill(Vector3 enemyPos)
    {
        Vector3 playerPos = Vector3.zero;

        foreach (GameObject player in players)
        {
            playerPos = player.transform.position;
        }

        Vector3Int roundpPos = new Vector3Int(Mathf.RoundToInt(playerPos.x), Mathf.RoundToInt(playerPos.y), Mathf.RoundToInt(playerPos.z));
        Vector3Int roundePos = new Vector3Int(Mathf.RoundToInt(enemyPos.x), Mathf.RoundToInt(enemyPos.y), Mathf.RoundToInt(enemyPos.z));

        CreateNewShape(playerPos, enemyPos, vertices);
        UpdateMesh();

        GameObject.Instantiate(soundPopPrefab, enemyPos, Quaternion.identity);
    }

    public void DoAction(int index, Vector3 localPos)
    {
        PullSimilarVertices(index, localPos);
    }

    private void PullSimilarVertices(int index, Vector3 newPos)
    {
        Vector3 targetVertexPos = vertices[index];
        List<int> relatedVertices = FindRelatedVertices(targetVertexPos, false);
        foreach (int i in relatedVertices)
        {
            vertices[i] = newPos;
        }
        clonedMesh.vertices = vertices; //4
        clonedMesh.RecalculateNormals();
    }

    // returns List of int that is related to the targetPt.
    private List<int> FindRelatedVertices(Vector3 targetPt, bool findConnected)
    {
        // list of int
        List<int> relatedVertices = new List<int>();

        int idx = 0;
        Vector3 pos;

        // loop through triangle array of indices
        for (int t = 0; t < triangles.Length; t++)
        {
            // current idx return from tris
            idx = triangles[t];
            // current pos of the vertex
            pos = vertices[idx];
            // if current pos is same as targetPt
            if (pos == targetPt)
            {
                // add to list
                relatedVertices.Add(idx);
                // if find connected vertices
                if (findConnected)
                {
                    // min
                    // - prevent running out of count
                    if (t == 0)
                    {
                        relatedVertices.Add(triangles[t + 1]);
                    }
                    // max 
                    // - prevent runnign out of count
                    if (t == triangles.Length - 1)
                    {
                        relatedVertices.Add(triangles[t - 1]);
                    }
                    // between 1 ~ max-1 
                    // - add idx from triangles before t and after t 
                    if (t > 0 && t < triangles.Length - 1)
                    {
                        relatedVertices.Add(triangles[t - 1]);
                        relatedVertices.Add(triangles[t + 1]);
                    }
                }
            }
        }
        // return compiled list of int
        return relatedVertices;
    }

    void DisplaceVertices(Vector3 targetVertexPos, float force, float radius)
    {
        Vector3 currentVertexPos = Vector3.zero;
        float sqrRadius = radius * radius; //1

        for (int i = 0; i < modifiedVertices.Length; i++) //2
        {
            currentVertexPos = modifiedVertices[i];
            float sqrMagnitude = (currentVertexPos - targetVertexPos).sqrMagnitude; //3
            if (sqrMagnitude > sqrRadius)
            {
                continue; //4
            }
            float distance = Mathf.Sqrt(sqrMagnitude); //5
            float falloff = GaussFalloff(distance, radius);
            Vector3 translate = (currentVertexPos * force) * falloff; //6
            translate.z = 0f;
            Quaternion rotation = Quaternion.Euler(translate);
            Matrix4x4 m = Matrix4x4.TRS(translate, rotation, Vector3.one);
            modifiedVertices[i] = m.MultiplyPoint3x4(currentVertexPos);
        }
        originalMesh.vertices = modifiedVertices; //7
        originalMesh.RecalculateNormals();
    }

    void ClearSelectedData()
    {
        selectedIndices = new List<int>();
        targetIndex = 0;
        targetVertex = Vector3.zero;
    }

    void CreateNewShape(Vector3 pPos, Vector3 ePos, Vector3[] prevVertices)
    {
        Vector3Int roundpPos = new Vector3Int(Mathf.RoundToInt(pPos.x), Mathf.RoundToInt(pPos.y), Mathf.RoundToInt(pPos.z));
        Vector3Int roundePos = new Vector3Int(Mathf.RoundToInt(ePos.x), Mathf.RoundToInt(ePos.y), Mathf.RoundToInt(ePos.z));

        for (var i = 0; i < prevVertices.Length; i++)
        {
            // Select Vert
            if (prevVertices[i] == new Vector3(roundePos.x, prevVertices[i].y, roundePos.z))
            {
                // Change verts around selected
                ChangeVerts(roundePos, roundpPos, prevVertices);

            }
        }
        CreateTriangles();
    }

    void ChangeVerts(Vector3Int ePos, Vector3Int pPos, Vector3[] prevVertices)
    {
        float dist = Vector3.Distance(ePos, pPos);
        int raiseArea = 1 + Mathf.RoundToInt(dist / 4);
        float raiseHeight = 0.6f + (dist / 10f);

        raiseHeight += Random.Range(-0.2f, 0.4f);

        for (int u = ePos.x - raiseArea; u < ePos.x + raiseArea; u++)
        {
            /*
            if (u == ePos.x - raiseArea || u == ePos.x + raiseArea)
            {
                raiseHeight -= 0.2f;
            }*/

            for (int v = ePos.z - raiseArea; v < ePos.z + raiseArea; v++)
            {
                /*
                if (v == ePos.z - raiseArea || v == ePos.z + raiseArea)
                {
                    raiseHeight -= 0.2f;
                }*/

                for (var j = 0; j < prevVertices.Length; j++)
                {
                    if (prevVertices[j] == new Vector3(u, prevVertices[j].y, v))
                    {
                        prevVertices[j].y += raiseHeight + Random.Range(-0.04f, 0.04f); ;
                    }
                }
            }
        }
    }

    void CreateInitShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * .1f, z * .1f) * 2f;
                //float y = 0;
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        CreateTriangles();
    }

    void CreateTriangles()
    {
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

    void StartDisplacement()
    {
        targetVertex = originalVertices[selectedIndices[currentIndex]];
        startTime = Time.time;
        isAnimate = true;
    }

    void CloneMesh()
    {
        meshFilter = GetComponent<MeshFilter>();
        originalMesh = meshFilter.sharedMesh; //1
        clonedMesh = new Mesh(); //2

        clonedMesh.name = "clone";
        clonedMesh.vertices = originalMesh.vertices;
        clonedMesh.triangles = originalMesh.triangles;
        clonedMesh.normals = originalMesh.normals;
        clonedMesh.uv = originalMesh.uv;
        meshFilter.mesh = clonedMesh;  //3

        vertices = clonedMesh.vertices; //4
        triangles = clonedMesh.triangles;
        isCloned = true; //5
        Debug.Log("Init & Cloned");
    }

    void ResetMesh()
    {
        if (clonedMesh != null && originalMesh != null) //1
        {
            clonedMesh.vertices = originalMesh.vertices; //2
            clonedMesh.triangles = originalMesh.triangles;
            clonedMesh.normals = originalMesh.normals;
            clonedMesh.uv = originalMesh.uv;
            meshFilter.mesh = clonedMesh; //3

            vertices = clonedMesh.vertices; //4
            triangles = clonedMesh.triangles;
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


        triangles = prevTriangles;

        int vert = 0;
        int tris = 0;

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

    #region HELPER FUNCTIONS

    static float LinearFalloff(float dist, float inRadius)
    {
        return Mathf.Clamp01(0.5f + (dist / inRadius) * 0.5f);
    }

    static float GaussFalloff(float dist, float inRadius)
    {
        return Mathf.Clamp01(Mathf.Pow(360, -Mathf.Pow(dist / inRadius, 2.5f) - 0.01f));
    }

    static float NeedleFalloff(float dist, float inRadius)
    {
        return -(dist * dist) / (inRadius * inRadius) + 1.0f;
    }

    #endregion
}