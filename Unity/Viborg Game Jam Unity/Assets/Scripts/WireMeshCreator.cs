﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Utility;


namespace PathCreation.Examples
{
    public class WireMeshCreator : PathSceneTool
    {
        [Header("Wire settings")]
        public float wireWidth = .4f;
        public int wireSegments = 6;
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

            int numTris = 2 * ((path.NumPoints - 1) + ((path.isClosedLoop) ? 1 : 0 ));
            int[] wireTriangles = new int[numTris * wireSegments];
            //int[] underRoadTriangles = new int[numTris * 3];
            //int[] sideOfRoadTriangles = new int[numTris * 2 * 3];

            int vertIndex = 0;
            int triIndex = 0;

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
                    Vector3 vert0 = 
                }

                // Find position to left and right of current path vertex
                Vector3 vertSideA = path.GetPoint(i) - localRight * Mathf.Abs(roadWidth);
                Vector3 vertSideB = path.GetPoint(i) + localRight * Mathf.Abs(roadWidth);

                // Add top of road vertices
                verts[vertIndex + 0] = vertSideA;
                verts[vertIndex + 1] = vertSideB;
                // Add bottom of road vertices
                verts[vertIndex + 2] = vertSideA - localUp * thickness;
                verts[vertIndex + 3] = vertSideB - localUp * thickness;

                // Duplicate vertices to get flat shading for sides of road
                verts[vertIndex + 4] = verts[vertIndex + 0];
                verts[vertIndex + 5] = verts[vertIndex + 1];
                verts[vertIndex + 6] = verts[vertIndex + 2];
                verts[vertIndex + 7] = verts[vertIndex + 3];

                // Set uv on y axis to path time (0 at start of path, up to 1 at end of path)
                uvs[vertIndex + 0] = new Vector2(0, path.times[i]);
                uvs[vertIndex + 1] = new Vector2(1, path.times[i]);

                // Top of road normals
                normals[vertIndex + 0] = localUp;
                normals[vertIndex + 1] = localUp;
                // Bottom of road normals
                normals[vertIndex + 2] = -localUp;
                normals[vertIndex + 3] = -localUp;
                // Sides of road normals
                normals[vertIndex + 4] = -localRight;
                normals[vertIndex + 5] = localRight;
                normals[vertIndex + 6] = -localRight;
                normals[vertIndex + 7] = localRight;

                // Set triangle indices
                if (i < path.NumPoints - 1 || path.isClosedLoop)
                {
                    for (int j = 0; j < triangleMap.Length; j++)
                    {
                        roadTriangles[triIndex + j] = (vertIndex + triangleMap[j]) % verts.Length;
                        // reverse triangle map for under road so that triangles wind the other way and are visible from underneath
                        underRoadTriangles[triIndex + j] = (vertIndex + triangleMap[triangleMap.Length - 1 - j] + 2) % verts.Length;
                    }
                    for (int j = 0; j < sidesTriangleMap.Length; j++)
                    {
                        sideOfRoadTriangles[triIndex * 2 + j] = (vertIndex + sidesTriangleMap[j]) % verts.Length;
                    }

                }

                vertIndex += 8;
                triIndex += 6;
            }

            mesh.Clear();
            mesh.vertices = verts;
            mesh.uv = uvs;
            mesh.normals = normals;
            mesh.subMeshCount = 3;
            mesh.SetTriangles(roadTriangles, 0);
            mesh.SetTriangles(underRoadTriangles, 1);
            mesh.SetTriangles(sideOfRoadTriangles, 2);
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
            if (roadMaterial != null && undersideMaterial != null)
            {
                meshRenderer.sharedMaterials = new Material[] { roadMaterial, undersideMaterial, undersideMaterial };
                meshRenderer.sharedMaterials[0].mainTextureScale = new Vector3(1, textureTiling);
            }
        }

    }
}