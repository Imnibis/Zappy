using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RockBuilder
{
    ///-----------------------------------------------------------------
    ///   Namespace:      RockBuilder
    ///   Class:          CubeRockMeshGenerator
    ///   Description:    The mesh Generator for the diamond.
    ///   Author:         Stefano Canonico                    
    ///   Date:           04.01.2020
    ///   Version:        1.0
    ///-----------------------------------------------------------------
    public class DiamondMeshGenerator
    {
        private static DiamondMeshGenerator instance = null;
        private static readonly object padlock = new object();

        DiamondMeshGenerator()
        {
        }

        public static DiamondMeshGenerator Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new DiamondMeshGenerator();
                    }
                    return instance;
                }
            }
        }

        public List<Vector3> CreateVertexPositions(Diamond diamond)
        {
            List<Vector3> spawnPoints = new List<Vector3>();

            // Get the point for the bottom peak
            spawnPoints.Add(diamond.transform.position - (Vector3.up * (diamond.pavillonHeight)));

            // Get the points of the middle pavillon circle
            for (int loopCount = 0; diamond.edges / 2 > loopCount; loopCount++)
            {
                float height = diamond.pavillonHeight * diamond.bottomRadiusPosition;
                float radius = diamond.radius * diamond.bottomRadiusPosition;
                Vector3 spawnPoint = DrawCircularVertices(diamond, radius, -height, diamond.edges / 2, loopCount, false);
                spawnPoints.Add(spawnPoint);
            }

            // Get the points of the upper pavillon circle
            for (int loopCount = 0; diamond.edges > loopCount; loopCount++)
            {
                Vector3 spawnPoint = DrawCircularVertices(diamond, diamond.radius, 0f, diamond.edges, loopCount, false);
                spawnPoints.Add(spawnPoint);
            }

            // Get the points for the upper circle
            for (int loopCount = 0; diamond.edges / 2 > loopCount; loopCount++)
            {
                Vector3 spawnPoint = DrawCircularVertices(diamond, diamond.radius * 0.75f, diamond.crownHeight / 2, diamond.edges / 2, loopCount, false);
                spawnPoints.Add(spawnPoint);
            }

            // Get the points for the upper plane
            for (int loopCount = 0; diamond.edges / 2 > loopCount; loopCount++)
            {
                Vector3 spawnPoint = DrawCircularVertices(diamond, diamond.radius / 2, diamond.crownHeight, diamond.edges / 2, loopCount, true);
                spawnPoints.Add(spawnPoint);
            }

            // Get the vertex position in the middle from the upper plane
            spawnPoints.Add(diamond.transform.position + (Vector3.up * (diamond.crownHeight)));

            return spawnPoints;
        }

        private Vector3 DrawCircularVertices(Diamond diamond, float radius, float height, int edges, int loopCount, bool offset)
        {
            Vector3 spawnPoint;
            float degree = (360f / edges) * loopCount;
            if (offset)
            {
                degree += (360f / edges) / 2;
            }
            float radian = degree * Mathf.Deg2Rad;
            float x = Mathf.Cos(radian);
            float z = Mathf.Sin(radian);
            spawnPoint = new Vector3(x, 0, z) * radius;
            spawnPoint.y = height;
            spawnPoint += diamond.transform.position;
            return spawnPoint;
        }

        public Mesh CreateMesh(Diamond diamond)
        {
            if (diamond.smoothFlag)
            {
                return CreateSmoothMesh(diamond);
            }
            else
            {
                return CreateHardMesh(diamond);
            }
            //CreateLods(Diamond);
        }

        private Mesh CreateHardMesh(Diamond diamond)
        {
            List<Vector3> vertexPositions = diamond.vertexPositions;
            int halfAmountOfEdges = diamond.edges / 2;

            // Initialize variables for vertices logic
            int initialVerticesCount = diamond.edges * 13;
            Vector3[] vertices = new Vector3[initialVerticesCount];
            int vertexLoop = 0;

            // Initialize variables for uv logic
            Vector2[] uv = new Vector2[initialVerticesCount];

            // Initialize variables for triangle logic
            int[] triangles = new int[diamond.edges * 15];
            int verticesCount = 0;
            int triangleVerticesCount = 0;

            // Calculate vertices, uv and triangles for the first part of the pavillon
            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {
                int bootomPeakStartIndex = 0;
                int pavillonStartIndex = 1;
                int upperPavillonStartIndex = halfAmountOfEdges + 2;

                vertices[vertexLoop] = vertexPositions[bootomPeakStartIndex] - diamond.transform.position;
                vertices[vertexLoop + 1] = vertexPositions[pavillonStartIndex + loopCount] - diamond.transform.position;
                vertices[vertexLoop + 2] = vertexPositions[upperPavillonStartIndex + loopCount * 2] - diamond.transform.position;

                if (halfAmountOfEdges - 1 != loopCount)
                {
                    vertices[vertexLoop + 3] = vertexPositions[pavillonStartIndex + loopCount + 1] - diamond.transform.position;
                }
                else
                {
                    vertices[vertexLoop + 3] = vertexPositions[pavillonStartIndex] - diamond.transform.position;
                }

                uv[vertexLoop] = new Vector2(.5f, .5f);
                uv[vertexLoop + 1] = DrawCircularVerticesForUv(halfAmountOfEdges, .25f, .5f + loopCount * -1f);
                uv[vertexLoop + 2] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, 0f + loopCount * -1f);
                uv[vertexLoop + 3] = DrawCircularVerticesForUv(halfAmountOfEdges, .25f, -.5f + loopCount * -1f);

                triangles[triangleVerticesCount] = verticesCount;
                triangles[triangleVerticesCount + 1] = verticesCount + 1;
                triangles[triangleVerticesCount + 2] = verticesCount + 2;
                triangles[triangleVerticesCount + 3] = verticesCount;
                triangles[triangleVerticesCount + 4] = verticesCount + 2;
                triangles[triangleVerticesCount + 5] = verticesCount + 3;

                triangleVerticesCount += 6;
                verticesCount += 4;

                vertexLoop = vertexLoop + 4;
            }

            // Calculate vertices, uv and triangles for the second part of the pavillon
            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {

                int pavillonStartIndex = 1;
                int upperPavillonStartIndex = halfAmountOfEdges + 1 + (loopCount * 2);

                vertices[vertexLoop] = vertexPositions[pavillonStartIndex + loopCount] - diamond.transform.position;
                vertices[vertexLoop + 1] = vertexPositions[upperPavillonStartIndex] - diamond.transform.position;
                vertices[vertexLoop + 2] = vertexPositions[upperPavillonStartIndex + 1] - diamond.transform.position;
                vertices[vertexLoop + 4] = vertexPositions[upperPavillonStartIndex + 1] - diamond.transform.position;

                if (halfAmountOfEdges - 1 != loopCount)
                {
                    vertices[vertexLoop + 3] = vertexPositions[pavillonStartIndex + loopCount + 1] - diamond.transform.position;
                    vertices[vertexLoop + 5] = vertexPositions[upperPavillonStartIndex + 2] - diamond.transform.position;
                }
                else
                {
                    vertices[vertexLoop + 3] = vertexPositions[pavillonStartIndex] - diamond.transform.position;
                    vertices[vertexLoop + 5] = vertexPositions[halfAmountOfEdges + 1] - diamond.transform.position;
                }

                uv[vertexLoop] = DrawCircularVerticesForUv(halfAmountOfEdges, .25f, -.5f - loopCount + 1);
                uv[vertexLoop + 1] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, -.5f - loopCount + 1);
                uv[vertexLoop + 2] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, -1f - loopCount + 1);
                uv[vertexLoop + 3] = DrawCircularVerticesForUv(halfAmountOfEdges, .25f, -1.5f - loopCount + 1);
                uv[vertexLoop + 4] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, -1f - loopCount + 1);
                uv[vertexLoop + 5] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, -1.5f - loopCount + 1);


                triangles[triangleVerticesCount] = verticesCount;
                triangles[triangleVerticesCount + 1] = verticesCount + 1;
                triangles[triangleVerticesCount + 2] = verticesCount + 2;
                triangles[triangleVerticesCount + 3] = verticesCount + 3;
                triangles[triangleVerticesCount + 4] = verticesCount + 4;
                triangles[triangleVerticesCount + 5] = verticesCount + 5;

                triangleVerticesCount += 6;
                verticesCount += 6;

                vertexLoop = vertexLoop + 6;
            }

            // Calculate vertices, uv and triangles for the first part of the crown
            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {

                int crownStartIndex = (halfAmountOfEdges * 3) + 1;
                int upperPavillonStartIndex = halfAmountOfEdges + 1 + (loopCount * 2);

                vertices[vertexLoop] = vertexPositions[upperPavillonStartIndex] - diamond.transform.position;
                vertices[vertexLoop + 1] = vertexPositions[crownStartIndex + loopCount] - diamond.transform.position;
                vertices[vertexLoop + 2] = vertexPositions[upperPavillonStartIndex + 1] - diamond.transform.position;
                vertices[vertexLoop + 5] = vertexPositions[upperPavillonStartIndex + 1] - diamond.transform.position;

                if (halfAmountOfEdges - 1 != loopCount)
                {
                    vertices[vertexLoop + 3] = vertexPositions[crownStartIndex + loopCount + 1] - diamond.transform.position;
                    vertices[vertexLoop + 4] = vertexPositions[upperPavillonStartIndex + 2] - diamond.transform.position;
                }
                else
                {
                    vertices[vertexLoop + 3] = vertexPositions[crownStartIndex] - diamond.transform.position;
                    vertices[vertexLoop + 4] = vertexPositions[halfAmountOfEdges + 1] - diamond.transform.position;
                }

                uv[vertexLoop] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, 1f + loopCount + 1);
                uv[vertexLoop + 1] = DrawCircularVerticesForUv(halfAmountOfEdges, .375f, 1f + loopCount + 1);
                uv[vertexLoop + 2] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, 1.5f + loopCount + 1);
                uv[vertexLoop + 3] = DrawCircularVerticesForUv(halfAmountOfEdges, .375f, 2f + loopCount + 1);
                uv[vertexLoop + 4] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, 2f + loopCount + 1);
                uv[vertexLoop + 5] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, 1.5f + loopCount + 1);

                triangles[triangleVerticesCount] = verticesCount;
                triangles[triangleVerticesCount + 1] = verticesCount + 1;
                triangles[triangleVerticesCount + 2] = verticesCount + 2;
                triangles[triangleVerticesCount + 3] = verticesCount + 3;
                triangles[triangleVerticesCount + 4] = verticesCount + 4;
                triangles[triangleVerticesCount + 5] = verticesCount + 5;

                triangleVerticesCount += 6;
                verticesCount += 6;

                vertexLoop = vertexLoop + 6;
            }

            // Calculate vertices, uv and triangles for the second part of the crown
            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {

                int crownStartIndex = (halfAmountOfEdges * 3) + 1;
                int upperCrownStartIndex = (halfAmountOfEdges * 4) + 1;
                int upperPavillonStartIndex = halfAmountOfEdges + (2 + (loopCount * 2));

                vertices[vertexLoop] = vertexPositions[upperPavillonStartIndex] - diamond.transform.position;
                vertices[vertexLoop + 1] = vertexPositions[crownStartIndex + loopCount] - diamond.transform.position;
                vertices[vertexLoop + 2] = vertexPositions[upperCrownStartIndex + loopCount] - diamond.transform.position;

                if (halfAmountOfEdges - 1 != loopCount)
                {
                    vertices[vertexLoop + 3] = vertexPositions[crownStartIndex + loopCount + 1] - diamond.transform.position;
                }
                else
                {
                    vertices[vertexLoop + 3] = vertexPositions[crownStartIndex] - diamond.transform.position;
                }

                uv[vertexLoop] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, 1.5f + loopCount + 1);
                uv[vertexLoop + 1] = DrawCircularVerticesForUv(halfAmountOfEdges, .375f, 1f + loopCount + 1);
                uv[vertexLoop + 2] = DrawCircularVerticesForUv(halfAmountOfEdges, .25f, 1.5f + loopCount + 1);
                uv[vertexLoop + 3] = DrawCircularVerticesForUv(halfAmountOfEdges, .375f, 2f + loopCount + 1);

                triangles[triangleVerticesCount] = verticesCount;
                triangles[triangleVerticesCount + 1] = verticesCount + 1;
                triangles[triangleVerticesCount + 2] = verticesCount + 2;
                triangles[triangleVerticesCount + 3] = verticesCount;
                triangles[triangleVerticesCount + 4] = verticesCount + 2;
                triangles[triangleVerticesCount + 5] = verticesCount + 3;

                triangleVerticesCount += 6;
                verticesCount += 4;

                vertexLoop = vertexLoop + 4;
            }

            // Calculate vertices, uv and triangles for the third part of the crown
            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {

                int crownStartIndex = (halfAmountOfEdges * 3) + 1 + loopCount;
                int upperCrownStartIndex = (halfAmountOfEdges * 4) + 1 + loopCount;

                vertices[vertexLoop + 1] = vertexPositions[upperCrownStartIndex] - diamond.transform.position;

                if (halfAmountOfEdges - 1 != loopCount)
                {
                    vertices[vertexLoop] = vertexPositions[crownStartIndex + 1] - diamond.transform.position;
                    vertices[vertexLoop + 2] = vertexPositions[upperCrownStartIndex + 1] - diamond.transform.position;
                }
                else
                {
                    vertices[vertexLoop] = vertexPositions[(halfAmountOfEdges * 3) + 1] - diamond.transform.position;
                    vertices[vertexLoop + 2] = vertexPositions[(halfAmountOfEdges * 4) + 1] - diamond.transform.position;
                }

                uv[vertexLoop] = DrawCircularVerticesForUv(halfAmountOfEdges, .375f, 2f + loopCount + 1);
                uv[vertexLoop + 1] = DrawCircularVerticesForUv(halfAmountOfEdges, .25f, 1.5f + loopCount + 1);
                uv[vertexLoop + 2] = DrawCircularVerticesForUv(halfAmountOfEdges, .25f, 2.5f + loopCount + 1);

                triangles[triangleVerticesCount] = verticesCount;
                triangles[triangleVerticesCount + 1] = verticesCount + 1;
                triangles[triangleVerticesCount + 2] = verticesCount + 2;

                triangleVerticesCount += 3;
                verticesCount += 3;

                vertexLoop = vertexLoop + 3;
            }

            // Calculate vertices, uv and triangles for the upper plane
            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {
                int upperMiddle = (halfAmountOfEdges * 5) + 1;
                int upperCrownStartIndex = (halfAmountOfEdges * 4) + 1 + loopCount;

                vertices[vertexLoop] = vertexPositions[upperCrownStartIndex] - diamond.transform.position;
                vertices[vertexLoop + 1] = vertexPositions[upperMiddle] - diamond.transform.position;

                if (halfAmountOfEdges - 1 != loopCount)
                {
                    vertices[vertexLoop + 2] = vertexPositions[upperCrownStartIndex + 1] - diamond.transform.position;
                }
                else
                {
                    vertices[vertexLoop + 2] = vertexPositions[(halfAmountOfEdges * 4) + 1] - diamond.transform.position;
                }

                uv[vertexLoop] = DrawCircularVerticesForUv(halfAmountOfEdges, .25f, 1.5f + loopCount + 1);
                uv[vertexLoop + 1] = new Vector2(0.5f, 0.5f);
                uv[vertexLoop + 2] = DrawCircularVerticesForUv(halfAmountOfEdges, .25f, 2.5f + loopCount + 1);

                triangles[triangleVerticesCount] = verticesCount;
                triangles[triangleVerticesCount + 1] = verticesCount + 1;
                triangles[triangleVerticesCount + 2] = verticesCount + 2;

                triangleVerticesCount += 3;
                verticesCount += 3;

                vertexLoop = vertexLoop + 3;
            }

            Mesh mesh = new Mesh();
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.name = "generated diamond mesh";
            MeshUtility.Optimize(mesh);
            mesh.RecalculateNormals();
            return mesh;
        }

        private Mesh CreateSmoothMesh(Diamond diamond)
        {
            List<Vector3> vertexPositions = diamond.vertexPositions;
            int halfAmountOfEdges = diamond.edges / 2;

            // Initialize variables for vertices logic
            int initialVerticesCount = diamond.edges * 3 + 2;
            Vector3[] vertices = new Vector3[initialVerticesCount];
            int vertexLoop = 0;

            // Initialize variables for uv logic
            Vector2[] uv = new Vector2[initialVerticesCount];

            // Initialize variables for triangle logic
            int[] triangles = new int[diamond.edges * 12];
            int verticesCount = 0;
            int triangleVerticesCount = 0;

            // Set the vertex of the bottom peak
            vertices[vertexLoop] = vertexPositions[0] - diamond.transform.position;
            vertexLoop++;

            // Calculate vertices, uv and triangles for the first part of the pavillon
            for (int loopCount = 0; diamond.edges > loopCount; loopCount++)
            {
                int bootomPeakStartIndex = 0;
                int upperPavillonStartIndex = halfAmountOfEdges + 1;

                if (loopCount < halfAmountOfEdges)
                {
                    vertices[vertexLoop + 1] = vertexPositions[upperPavillonStartIndex + loopCount * 2 + 1] - diamond.transform.position;
                    vertices[vertexLoop] = vertexPositions[upperPavillonStartIndex + loopCount * 2] - diamond.transform.position;


                    uv[bootomPeakStartIndex] = new Vector2(.5f, .5f);
                    uv[vertexLoop] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, .5f + loopCount * -1f);
                    uv[vertexLoop + 1] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, 0f + loopCount * -1f);

                    vertexLoop = vertexLoop + 2;
                }

                triangles[triangleVerticesCount] = bootomPeakStartIndex;
                triangles[triangleVerticesCount + 1] = verticesCount + 1;
                triangles[triangleVerticesCount + 2] = verticesCount + 2;
                if (diamond.edges - 1 == loopCount)
                {
                    triangles[triangleVerticesCount + 2] = bootomPeakStartIndex + 1;
                }

                triangleVerticesCount += 3;
                verticesCount += 1;
            }

            // Calculate vertices, uv and triangles for the first part of the pavillon
            for (int loopCount = 0; diamond.edges > loopCount; loopCount++)
            {
                int crownStartIndex = (halfAmountOfEdges * 3) + 1;
                int upperPavillonStartIndex = halfAmountOfEdges + 1;

                if (loopCount < halfAmountOfEdges)
                {
                    vertices[vertexLoop] = vertexPositions[upperPavillonStartIndex + loopCount * 2] - diamond.transform.position;
                    vertices[vertexLoop + 1] = vertexPositions[upperPavillonStartIndex + loopCount * 2 + 1] - diamond.transform.position;
                    vertices[diamond.edges * 2 + 1 + loopCount] = vertexPositions[crownStartIndex + loopCount] - diamond.transform.position;

                    uv[diamond.edges * 2 + 1 + loopCount] = DrawCircularVerticesForUv(halfAmountOfEdges, 0.375f, 0.5f + loopCount);
                    uv[vertexLoop] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, 0.5f + loopCount);
                    uv[vertexLoop + 1] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, 1f + loopCount);

                    vertexLoop = vertexLoop + 2;
                }

                triangles[triangleVerticesCount] = (diamond.edges * 2) + 1 + loopCount / 2;
                triangles[triangleVerticesCount + 2] = verticesCount;
                triangles[triangleVerticesCount + 1] = verticesCount + 1;

                triangleVerticesCount += 3;
                verticesCount += 1;
            }

            vertexLoop = vertexLoop + halfAmountOfEdges;
            verticesCount = vertexLoop;


            // Calculate vertices, uv and triangles for the first part of the pavillon
            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {
                int upperCrownStartIndex = (halfAmountOfEdges * 4) + 1;
                int crownStartIndex = (halfAmountOfEdges * 3) + 1;
                int upperPavillonStartIndex = halfAmountOfEdges + 1 + loopCount * 2;

                vertices[vertexLoop] = vertexPositions[upperCrownStartIndex + loopCount] - diamond.transform.position;
                uv[vertexLoop] = DrawCircularVerticesForUv(halfAmountOfEdges, .25f, 1f + loopCount);
                vertexLoop = vertexLoop + 1;

                triangles[triangleVerticesCount] = diamond.edges + 2 + loopCount * 2;
                triangles[triangleVerticesCount + 1] = (diamond.edges * 2) + 1 + loopCount;
                triangles[triangleVerticesCount + 2] = verticesCount;
                triangles[triangleVerticesCount + 3] = diamond.edges + 2 + loopCount * 2;
                triangles[triangleVerticesCount + 4] = verticesCount;
                triangles[triangleVerticesCount + 5] = (diamond.edges * 2) + 2 + loopCount;

                if (halfAmountOfEdges - 1 == loopCount)
                {
                    triangles[triangleVerticesCount + 5] = (diamond.edges * 2) + 1;
                }

                triangleVerticesCount += 6;
                verticesCount += 1;
            }

            // Calculate vertices, uv and triangles for the first part of the pavillon
            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {
                int upperCrownStartIndex = (halfAmountOfEdges * 5) + 1;
                int crownStartIndex = (diamond.edges * 2) + 1;

                triangles[triangleVerticesCount] = crownStartIndex + loopCount + 1;
                triangles[triangleVerticesCount + 1] = upperCrownStartIndex + loopCount;
                triangles[triangleVerticesCount + 2] = upperCrownStartIndex + loopCount + 1;


                if (halfAmountOfEdges - 1 == loopCount)
                {
                    triangles[triangleVerticesCount] = crownStartIndex;
                    triangles[triangleVerticesCount + 2] = upperCrownStartIndex;
                }

                triangleVerticesCount += 3;
            }


            // Get the vertex in the middle of the upper plane
            int upperMiddlePlaneStartIndex = (halfAmountOfEdges * 5 + 1);
            vertices[vertexLoop] = vertexPositions[upperMiddlePlaneStartIndex] - diamond.transform.position;
            uv[vertexLoop] = DrawCircularVerticesForUv(halfAmountOfEdges, 0f, 0f);

            // Calculate vertices, uv and triangles for the first part of the pavillon
            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {

                int upperCrownStartIndex = (halfAmountOfEdges * 5) + 1;

                triangles[triangleVerticesCount] = upperCrownStartIndex + loopCount;
                triangles[triangleVerticesCount + 1] = verticesCount;
                triangles[triangleVerticesCount + 2] = upperCrownStartIndex + loopCount + 1;


                if (halfAmountOfEdges - 1 == loopCount)
                {
                    triangles[triangleVerticesCount + 2] = upperCrownStartIndex;
                }

                triangleVerticesCount += 3;
            }



            Mesh mesh = new Mesh();
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.name = "generated diamond mesh";
            mesh.RecalculateNormals();

            #region Recalculate some normals manually for smoother shading. 
            Vector3[] normals = mesh.normals;

            for (int i = 1; i < diamond.edges + 1; i++)
            {
                Vector3 averageNormal = (normals[i] + normals[i + diamond.edges]) / 2;
                normals[i] = averageNormal;
                normals[i + diamond.edges] = averageNormal;
            }

            mesh.normals = normals;
            #endregion

            MeshUtility.Optimize(mesh);
            return mesh;
        }

        private Vector3 DrawCircularVerticesForUv(int edges, float radius, float offset)
        {
            Vector2 uvPosition;
            float degree = (360f / edges);
            degree += (360f / edges) * offset;
            float radian = degree * Mathf.Deg2Rad;
            float x = Mathf.Cos(radian);
            float y = Mathf.Sin(radian);
            uvPosition = new Vector2(.5f, .5f);
            uvPosition = uvPosition + new Vector2(x, y) * radius;
            return uvPosition;
        }
    }
}
