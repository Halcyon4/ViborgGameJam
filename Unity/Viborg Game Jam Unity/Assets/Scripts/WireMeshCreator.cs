using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Utility;


namespace PathCreation.Examples
{
    public class WireMeshCreator : PathSceneTool
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
        public Material stormMaterial;
        //public Material undersideMaterial;
        public float textureTiling = 1;

        [SerializeField, HideInInspector]

        GameObject meshHolder;

        MeshFilter meshFilter;
        MeshRenderer meshRenderer;
        Mesh mesh;

        protected override void PathUpdated()
        {
            if (pathCreator != null)
            {
                AssignMeshComponents();
                AssignMaterials();
                CreateWireMesh();
            }
        }

        void CreateWireMesh()
        {
            Vector3[] verts = new Vector3[path.NumPoints * wireSegments];
            Vector2[] uvs = new Vector2[verts.Length];
            Vector3[] normals = new Vector3[verts.Length];

            int lStorm = (stormSegments > wireSegments) ? wireSegments : stormSegments;
            int numTris = 2 * wireSegments * ((path.NumPoints - 1) + ((path.isClosedLoop) ? 1 : 0 ));
            int[] stormTriangles = new int[numTris / wireSegments * stormSegments * 3];
            int[] wireTriangles = new int[numTris * 3 - stormTriangles.Length];
            //int[] underRoadTriangles = new int[numTris * 3];
            //int[] sideOfRoadTriangles = new int[numTris * 2 * 3];

            int vertIndex = 0;
            int triIndex = 0;
            int stormIndex = 0;

            // Vertices for a wire segment are layed out:
            // 0  1
            // 2  3
            // and so on... So the triangle map 0,8,1 for example, defines a triangle from top left to bottom left to bottom right.
            int[] triangleMap = { 0, 2, 1, 1, 2, 3 };
            //int[] sidesTriangleMap = { 4, 6, 14, 12, 4, 14, 5, 15, 7, 13, 15, 5 };

            bool usePathNormals = !(path.space == PathSpace.xyz && flattenSurface);

            for (int i = 0; i < path.NumPoints; i++)
            {
                for (int j = 0; j < wireSegments; j++)
                {
                    int iNext = (i == (path.NumPoints - 1)) ? 0 : i;

                    // Calcualte directions
                    Vector3 localUp = (usePathNormals) ? Vector3.Cross(path.GetTangent(i), path.GetNormal(i)) : path.up;
                    Vector3 nextUp = (usePathNormals) ? Vector3.Cross(path.GetTangent(iNext), path.GetNormal(iNext)) : path.up;
                    Vector3 localRight = (usePathNormals) ? path.GetNormal(i) : Vector3.Cross(localUp, path.GetTangent(i));
                    Vector3 nextRight = (usePathNormals) ? path.GetNormal(iNext) : Vector3.Cross(localUp, path.GetTangent(iNext));
                     
                    Vector3 currentAngleRight = Quaternion.AngleAxis((360 / wireSegments * j), path.GetTangent(i)) * localUp;
                    Vector3 currentAngleLeft = Quaternion.AngleAxis((360 / wireSegments * (j + 1)), path.GetTangent(i)) * localUp;
                    Vector3 nextAngleRight = Quaternion.AngleAxis((360 / wireSegments * j), path.GetTangent(iNext)) * nextUp;
                    Vector3 nextAngleLeft = Quaternion.AngleAxis((360 / wireSegments * (j + 1)), path.GetTangent(iNext)) * nextUp;

                    // Find positions
                    Vector3 vert0 = path.GetPoint(i) + currentAngleRight * Mathf.Abs(wireRadius);
                    //Vector3 vert1 = path.GetPoint(i) + currentAngleLeft * Mathf.Abs(wireRadius);
                    //Vector3 vert2 = path.GetPoint(iNext) + nextAngleRight * Mathf.Abs(wireRadius);
                    //Vector3 vert3 = path.GetPoint(iNext) + nextAngleLeft * Mathf.Abs(wireRadius);

                    int vert2Index = vertIndex + j;
                    int vert1Index = vertIndex + ((j + 1) % wireSegments);
                    int vert4Index = ( vertIndex + wireSegments + j ) % verts.Length;
                    int vert3Index = ( vertIndex + wireSegments + ((j + 1) % wireSegments) ) % verts.Length;

                    //Add current vert
                    verts[vertIndex + j] = vert0;

                    //Set current normal
                    normals[vertIndex + j] = currentAngleRight;

                    //Add triangles
                    if (j < stormSegments)
                    {
                        if (stormIndex + 6 < stormTriangles.Length)
                        {
                            stormTriangles[stormIndex] = vert1Index;
                            stormTriangles[stormIndex + 1] = vert3Index;
                            stormTriangles[stormIndex + 2] = vert2Index;
                            stormTriangles[stormIndex + 3] = vert2Index;
                            stormTriangles[stormIndex + 4] = vert3Index;
                            stormTriangles[stormIndex + 5] = vert4Index;

                            stormIndex += 6;
                        }
                    
                    }
                    else
                    {
                        if (triIndex + 6 < wireTriangles.Length)
                        {
                            wireTriangles[triIndex] = vert1Index;
                            wireTriangles[triIndex + 1] = vert3Index;
                            wireTriangles[triIndex + 2] = vert2Index;
                            wireTriangles[triIndex + 3] = vert2Index;
                            wireTriangles[triIndex + 4] = vert3Index;
                            wireTriangles[triIndex + 5] = vert4Index;

                            triIndex += 6;
                        }
                    }


                    //// Set uv on y axis to path time (0 at start of path, up to 1 at end of path)
                    //uvs[vertIndex + 0] = new Vector2(0, path.times[i]);
                    //uvs[vertIndex + 1] = new Vector2(1, path.times[i]);
                    
                    // Ignore UV for now
                    uvs[vertIndex + j] = new Vector2(0, 0);
                }

                vertIndex += wireSegments;


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
                meshRenderer.sharedMaterials = new Material[] { wireMaterial, stormMaterial };
                meshRenderer.sharedMaterials[0].mainTextureScale = new Vector3(1, textureTiling);
            }
        }

    }
}