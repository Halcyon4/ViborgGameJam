using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayMeshGenerator : MonoBehaviour
{

    [Header("Wire settings")]
    public float wireRadius = .4f;
    public int wireSegments = 6;
    public int stormSegments = 2;
    //[Range(0, .5f)]
    //public float thickness = .15f;
    public bool flattenSurface;

    [Header("Material settings")]
    public Material wireMaterial;
    //public Material undersideMaterial;
    public float textureTiling = 1;

    [SerializeField, HideInInspector]

    GameObject meshHolder;

    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    Mesh mesh;

    [SerializeField]
    private List<Vector3> rayPoints = new List<Vector3>();

    

    // Start is called before the first frame update
    void Start()
    {
        UpdateMesh();
    }

    public List<Vector3> GetRayPoints()
    {
        return rayPoints;
    }
    public void addPoint(Vector3 newPoint)
    {
        rayPoints.Add(newPoint);
        //UpdateMesh();
    }
    public void setPoints(List<Vector3> points)
    {
        rayPoints = points;
        //UpdateMesh();
    }
    public void setPoints(Vector3[] points)
    {
        rayPoints = new List<Vector3>();
        for(int i = 0; i < points.Length; i++)
        {
            rayPoints.Add(points[i]);
        }
        //UpdateMesh();
    }
    public void clearPoints()
    {
        rayPoints.RemoveAll(allTrue);
    }
    private static bool allTrue(Vector3 v)
    {
        return true;
    }
    public void UpdateMesh()
    {
        if (rayPoints.Count > 1)
        {
            AssignMeshComponents();
            AssignMaterials();
            CreateWireMesh();
        }
        else
        {
            //Clear mesh
        }
    }

    void CreateWireMesh()
    {
        Vector3[] verts = new Vector3[(rayPoints.Count - 1) * wireSegments * 2];
        Vector2[] uvs = new Vector2[verts.Length];
        Vector3[] normals = new Vector3[verts.Length];

        int lStorm = (stormSegments > wireSegments) ? wireSegments : stormSegments;
        int numTris = 2 * (wireSegments* 2) * (rayPoints.Count - 1);
        int[] stormTriangles = new int[numTris / wireSegments * stormSegments * 3];
        int[] wireTriangles = new int[numTris * 3];
        //int[] underRoadTriangles = new int[numTris * 3];
        //int[] sideOfRoadTriangles = new int[numTris * 2 * 3];

        int vertIndex = 0;
        int triIndex = 0;

        // Vertices for a wire segment are layed out:
        // 0  1
        // 2  3
        // and so on... So the triangle map 0,8,1 for example, defines a triangle from top left to bottom left to bottom right.
        int[] triangleMap = { 0, 1, 2, 1, 3, 2};
        //int[] sidesTriangleMap = { 4, 6, 14, 12, 4, 14, 5, 15, 7, 13, 15, 5 };

        for (int i = 0; i < rayPoints.Count - 1; i++)
        {
            int iNext = i + 1;

            Vector3 rayVector = (rayPoints[i + 1] - rayPoints[i]).normalized;

            for (int j = 0; j < wireSegments; j++)
            {

                //Get vector orthagonal to rayVector
                Vector3 localUp;
                if (rayVector.x != 0 || rayVector.z != 0)
                {
                     localUp = Vector3.Cross(Vector3.up, rayVector);
                }
                else if (rayVector.x != 0 || rayVector.y != 0){
                     localUp = Vector3.Cross(Vector3.forward, rayVector);
                }
                else
                {
                     localUp = Vector3.Cross(Vector3.right, rayVector);
                }

                // Calcualte directions
                Vector3 nextUp = Vector3.up;
                Vector3 localRight = Vector3.Cross(localUp, rayVector);
                Vector3 nextRight = localRight;

                Vector3 currentAngleRight = Quaternion.AngleAxis((360 / wireSegments * j), rayVector) * localUp;
                Vector3 currentAngleLeft = Quaternion.AngleAxis((360 / wireSegments * (j + 1)), rayVector) * localUp;
                Vector3 nextAngleRight = currentAngleRight; // Quaternion.AngleAxis((360 / wireSegments * j), path.GetTangent(iNext)) * nextUp;
                Vector3 nextAngleLeft = currentAngleLeft; // Quaternion.AngleAxis((360 / wireSegments * (j + 1)), path.GetTangent(iNext)) * nextUp;

                // Find positions
                Vector3 vert0 = rayPoints[i] + currentAngleRight * Mathf.Abs(wireRadius);
                //Vector3 vert1 = path.GetPoint(i) + currentAngleLeft * Mathf.Abs(wireRadius);
                Vector3 vert2 = rayPoints[iNext] + nextAngleRight * Mathf.Abs(wireRadius);
                //Vector3 vert3 = path.GetPoint(iNext) + nextAngleLeft * Mathf.Abs(wireRadius);

                int[] vertIndexes = new int[4];
                vertIndexes[0] = vertIndex + j * 2;
                vertIndexes[1] = vertIndex + (((j + 1)* 2) % (wireSegments * 2));
                vertIndexes[2] = (vertIndex + j * 2 + 1) % verts.Length;
                vertIndexes[3] = (vertIndex + (((j + 1) * 2) % (wireSegments * 2) + 1)) % verts.Length;

                //Add current vert
                verts[vertIndex + j * 2] = vert0;
                verts[vertIndex + j * 2 + 1] = vert2;

                //Set current normal
                normals[vertIndex + j] = currentAngleRight;

                //Add triangles
                //if (j < stormSegments)
                //{
                //    if (stormIndex + 6 < stormTriangles.Length)
                //    {
                //        stormTriangles[stormIndex] = vert1Index;
                //        stormTriangles[stormIndex + 1] = vert3Index;
                //        stormTriangles[stormIndex + 2] = vert2Index;
                //        stormTriangles[stormIndex + 3] = vert2Index;
                //        stormTriangles[stormIndex + 4] = vert3Index;
                //        stormTriangles[stormIndex + 5] = vert4Index;

                //        stormIndex += 6;
                //    }

                //}
                //else
                //{
                if( j == wireSegments - 1)
                {
                    int abc = 1;
                }
                if (triIndex + 6 < wireTriangles.Length)
                {
                    wireTriangles[triIndex] = vertIndexes[triangleMap[0]];
                    wireTriangles[triIndex + 1] = vertIndexes[triangleMap[1]];
                    wireTriangles[triIndex + 2] = vertIndexes[triangleMap[2]];
                    wireTriangles[triIndex + 3] = vertIndexes[triangleMap[3]];
                    wireTriangles[triIndex + 4] = vertIndexes[triangleMap[4]];
                    wireTriangles[triIndex + 5] = vertIndexes[triangleMap[5]];

                    triIndex += 6;
                }
                //}


                //// Set uv on y axis to path time (0 at start of path, up to 1 at end of path)
                //uvs[vertIndex + 0] = new Vector2(0, path.times[i]);
                //uvs[vertIndex + 1] = new Vector2(1, path.times[i]);

                // Ignore UV for now
                uvs[vertIndex + j] = new Vector2(0, 0);
            }

            vertIndex += wireSegments * 2;


        }

        mesh.Clear();
        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.normals = normals;
        mesh.subMeshCount = 2;
        mesh.SetTriangles(wireTriangles, 0);
        mesh.SetTriangles(stormTriangles, 1);
        //mesh.SetTriangles(sideOfRoadTriangles, 2);
        mesh.RecalculateBounds();
    }

    // Add MeshRenderer and MeshFilter components to this gameobject if not already attached
    void AssignMeshComponents()
    {

        if (meshHolder == null)
        {
            meshHolder = new GameObject("Road Mesh Holder");
        }

        meshHolder.transform.rotation = Quaternion.identity;
        meshHolder.transform.position = Vector3.zero;
        meshHolder.transform.localScale = Vector3.one;

        // Ensure mesh renderer and filter components are assigned
        if (!meshHolder.gameObject.GetComponent<MeshFilter>())
        {
            meshHolder.gameObject.AddComponent<MeshFilter>();
        }
        if (!meshHolder.GetComponent<MeshRenderer>())
        {
            meshHolder.gameObject.AddComponent<MeshRenderer>();
        }

        meshRenderer = meshHolder.GetComponent<MeshRenderer>();
        meshFilter = meshHolder.GetComponent<MeshFilter>();
        if (mesh == null)
        {
            mesh = new Mesh();
        }
        meshFilter.sharedMesh = mesh;
    }

    void AssignMaterials()
    {
        if (wireMaterial != null)
        {
            meshRenderer.sharedMaterials = new Material[] { wireMaterial, wireMaterial };
            meshRenderer.sharedMaterials[0].mainTextureScale = new Vector3(1, textureTiling);
        }
    }

}
