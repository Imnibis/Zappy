using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RockBuilder
{
    ///-----------------------------------------------------------------
    ///   Namespace:      RockBuilder
    ///   Class:          CubeRockMeshGenerator
    ///   Description:    The mesh Generator for the gem.
    ///   Author:         Stefano Canonico                    
    ///   Date:           04.01.2020
    ///   Version:        1.0
    ///-----------------------------------------------------------------
    public class GemMeshGenerator
    {
        private static GemMeshGenerator instance = null;
        private static readonly object padlock = new object();

        GemMeshGenerator()
        {
        }

        public static GemMeshGenerator Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new GemMeshGenerator();
                    }
                    return instance;
                }
            }
        }

        public List<Vector3> CreateVertexPositions(Gem gem)
        {
            List<Vector3> spawnPoints = new List<Vector3>();

            float startPositionZ = -gem.depth / 2;
            // Get the vertex for the bottom middlepoint
            spawnPoints.Add(gem.transform.position + (Vector3.forward * startPositionZ));

            for (int loopCountZ = 0; 5 > loopCountZ; loopCountZ++)
            {
                float positionZ = startPositionZ + (gem.depth / 4) * loopCountZ;
                float radiusX;
                float radiusY;
                if (loopCountZ < 3)
                {
                    radiusX = (gem.width / 3) * (loopCountZ + 1);
                    radiusY = (gem.height / 3) * (loopCountZ + 1);
                }
                else
                {
                    radiusX = gem.width - ((gem.width / 3) * (loopCountZ - 2));
                    radiusY = gem.height - ((gem.height / 3) * (loopCountZ - 2));
                }

                int edges;
                if (loopCountZ == 2)
                {
                    edges = gem.edges;
                }
                else
                {
                    edges = gem.edges / 2;
                }

                bool offset;

                if (loopCountZ == 1 || loopCountZ == 3)
                {
                    offset = true;
                }
                else
                {
                    offset = false;
                }

                // Get the vertices for the body
                for (int loopCount = 0; edges > loopCount; loopCount++)
                {
                    Vector3 spawnPoint = DrawCircularVertices(gem, radiusX, radiusY, positionZ, edges, loopCount, offset);
                    spawnPoints.Add(spawnPoint);
                }
            }

            // Get the vertex for the upper middlepoint
            float endPositionZ = gem.depth / 2;
            spawnPoints.Add(gem.transform.position + (Vector3.forward * endPositionZ));

            return spawnPoints;
        }

        private Vector3 DrawCircularVertices(Gem gem, float radiusX, float radiusY, float positionZ, int edges, int loopCount, bool offset)
        {
            Vector3 spawnPoint;
            float degree = (360f / edges) * loopCount;
            if (offset)
            {
                degree += (360f / edges) / 2;
            }
            float radian = degree * Mathf.Deg2Rad;
            float x = Mathf.Cos(radian) * radiusX;
            float y = Mathf.Sin(radian) * radiusY;
            spawnPoint = new Vector3(x, y, 0);
            spawnPoint.z = positionZ;
            spawnPoint += gem.transform.position;
            return spawnPoint;
        }

        public Mesh CreateMesh(Gem gem)
        {
            if (gem.smoothFlag)
            {
                return CreateSmoothMesh(gem);
            }
            else
            {
                return CreateHardMesh(gem);
            }
            //CreateLods(Gem);
        }

        private Mesh CreateHardMesh(Gem gem)
        {
            List<Vector3> vertexPositions = gem.vertexPositions;
            int halfAmountOfEdges = gem.edges / 2;

            // Initialize variables for vertices logic
            int initialVerticesCount = gem.edges * 15 + 2;
            Vector3[] vertices = new Vector3[initialVerticesCount];
            int vertexLoop = 0;

            // Initialize variables for uv logic
            Vector2[] uv = new Vector2[initialVerticesCount];
            float uvOffset = 1f;
            float radiusUvModifierX;
            float radiusUvModifierY;

            if (gem.width > gem.height)
            {
                radiusUvModifierX = 1f;
                radiusUvModifierY = gem.height / gem.width;
            }
            else
            {
                radiusUvModifierX = gem.width / gem.height;
                radiusUvModifierY = 1f;
            }

            // Initialize variables for triangle logic
            int[] triangles = new int[gem.edges * 18];
            int verticesCount = 0;
            int triangleVerticesCount = 0;

            // The start indices of the vertex groups
            int startIndex = 0;
            int innerRingIndex = 1;
            int middleRingIndex = 1 + halfAmountOfEdges;
            int outerRingIndex = 1 + gem.edges;
            int secondMiddleRingIndex = 1 + gem.edges * 2;
            int secondInnerRingIndex = 1 + halfAmountOfEdges + gem.edges * 2;
            int endIndex = 1 + gem.edges * 3;

            vertices[0] = vertexPositions[startIndex] - gem.transform.position;
            vertices[1] = vertexPositions[endIndex] - gem.transform.position;
            uv[0] = new Vector2(.5f, .5f);
            uv[1] = new Vector2(.5f, .5f);
            verticesCount = +2;
            vertexLoop = +2;

            // Calculate vertices, uv and triangles for the first part of the pavillon
            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {
                vertices[vertexLoop] = vertexPositions[innerRingIndex + loopCount] - gem.transform.position;
                vertices[vertexLoop + 1] = vertexPositions[innerRingIndex + loopCount + 1] - gem.transform.position;

                vertices[vertexLoop + 2] = vertexPositions[secondInnerRingIndex + loopCount] - gem.transform.position;
                vertices[vertexLoop + 3] = vertexPositions[secondInnerRingIndex + loopCount + 1] - gem.transform.position;

                if (halfAmountOfEdges - 1 == loopCount)
                {
                    vertices[vertexLoop + 1] = vertexPositions[innerRingIndex] - gem.transform.position;
                    vertices[vertexLoop + 3] = vertexPositions[secondInnerRingIndex] - gem.transform.position;
                }

                uv[vertexLoop] = DrawCircularVerticesForUv(halfAmountOfEdges, .166f, 0f + loopCount - uvOffset, radiusUvModifierX, radiusUvModifierY);
                uv[vertexLoop + 1] = DrawCircularVerticesForUv(halfAmountOfEdges, .166f, 1f + loopCount - uvOffset, radiusUvModifierX, radiusUvModifierY);

                uv[vertexLoop + 2] = DrawCircularVerticesForUv(halfAmountOfEdges, .166f, 1f - loopCount, radiusUvModifierX, radiusUvModifierY);
                uv[vertexLoop + 3] = DrawCircularVerticesForUv(halfAmountOfEdges, .166f, 0f - loopCount, radiusUvModifierX, radiusUvModifierY);

                triangles[triangleVerticesCount] = 0;
                triangles[triangleVerticesCount + 1] = verticesCount + 1;
                triangles[triangleVerticesCount + 2] = verticesCount;

                triangles[triangleVerticesCount + 3] = 1;
                triangles[triangleVerticesCount + 4] = verticesCount + 2;
                triangles[triangleVerticesCount + 5] = verticesCount + 3;

                triangleVerticesCount += 6;
                verticesCount += 4;

                vertexLoop = vertexLoop + 4;
            }

            // Calculate vertices, uv and triangles for the first part of the pavillon
            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {

                vertices[vertexLoop] = vertexPositions[innerRingIndex + loopCount] - gem.transform.position;
                vertices[vertexLoop + 1] = vertexPositions[middleRingIndex + loopCount] - gem.transform.position;
                vertices[vertexLoop + 2] = vertexPositions[innerRingIndex + loopCount + 1] - gem.transform.position;

                vertices[vertexLoop + 3] = vertexPositions[secondInnerRingIndex + loopCount] - gem.transform.position;
                vertices[vertexLoop + 4] = vertexPositions[secondMiddleRingIndex + loopCount] - gem.transform.position;
                vertices[vertexLoop + 5] = vertexPositions[secondInnerRingIndex + loopCount + 1] - gem.transform.position;

                if (halfAmountOfEdges - 1 == loopCount)
                {
                    vertices[vertexLoop + 2] = vertexPositions[innerRingIndex] - gem.transform.position;
                    vertices[vertexLoop + 5] = vertexPositions[secondInnerRingIndex] - gem.transform.position;
                }

                uv[vertexLoop] = DrawCircularVerticesForUv(halfAmountOfEdges, .166f, 0f + loopCount - uvOffset, radiusUvModifierX, radiusUvModifierY);
                uv[vertexLoop + 1] = DrawCircularVerticesForUv(halfAmountOfEdges, .333f, 0.5f + loopCount - uvOffset, radiusUvModifierX, radiusUvModifierY);
                uv[vertexLoop + 2] = DrawCircularVerticesForUv(halfAmountOfEdges, .166f, 1f + loopCount - uvOffset, radiusUvModifierX, radiusUvModifierY);
                uv[vertexLoop + 3] = DrawCircularVerticesForUv(halfAmountOfEdges, .166f, 1f - loopCount, radiusUvModifierX, radiusUvModifierY);
                uv[vertexLoop + 4] = DrawCircularVerticesForUv(halfAmountOfEdges, .333f, 0.5f - loopCount, radiusUvModifierX, radiusUvModifierY);
                uv[vertexLoop + 5] = DrawCircularVerticesForUv(halfAmountOfEdges, .166f, 0f - loopCount, radiusUvModifierX, radiusUvModifierY);

                triangles[triangleVerticesCount] = verticesCount;
                triangles[triangleVerticesCount + 1] = verticesCount + 2;
                triangles[triangleVerticesCount + 2] = verticesCount + 1;
                triangles[triangleVerticesCount + 3] = verticesCount + 3;
                triangles[triangleVerticesCount + 4] = verticesCount + 4;
                triangles[triangleVerticesCount + 5] = verticesCount + 5;

                triangleVerticesCount += 6;
                verticesCount += 6;

                vertexLoop = vertexLoop + 6;
            }

            // Calculate vertices, uv and triangles for the first part of the pavillon
            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {

                vertices[vertexLoop] = vertexPositions[innerRingIndex + loopCount] - gem.transform.position;
                vertices[vertexLoop + 1] = vertexPositions[middleRingIndex + loopCount] - gem.transform.position;
                vertices[vertexLoop + 2] = vertexPositions[outerRingIndex + loopCount * 2] - gem.transform.position;
                vertices[vertexLoop + 3] = vertexPositions[middleRingIndex + loopCount - 1] - gem.transform.position;

                vertices[vertexLoop + 4] = vertexPositions[secondInnerRingIndex + loopCount] - gem.transform.position;
                vertices[vertexLoop + 5] = vertexPositions[secondMiddleRingIndex + loopCount] - gem.transform.position;
                vertices[vertexLoop + 6] = vertexPositions[outerRingIndex + loopCount * 2] - gem.transform.position;
                vertices[vertexLoop + 7] = vertexPositions[secondMiddleRingIndex + loopCount - 1] - gem.transform.position;

                if (loopCount == 0)
                {
                    vertices[vertexLoop + 3] = vertexPositions[middleRingIndex + halfAmountOfEdges - 1] - gem.transform.position;
                    vertices[vertexLoop + 7] = vertexPositions[secondMiddleRingIndex + halfAmountOfEdges - 1] - gem.transform.position;
                }

                uv[vertexLoop] = DrawCircularVerticesForUv(halfAmountOfEdges, .166f, 0f + loopCount - uvOffset, radiusUvModifierX, radiusUvModifierY);
                uv[vertexLoop + 1] = DrawCircularVerticesForUv(halfAmountOfEdges, .333f, 0.5f + loopCount - uvOffset, radiusUvModifierX, radiusUvModifierY);
                uv[vertexLoop + 2] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, 0f + loopCount - uvOffset, radiusUvModifierX, radiusUvModifierY);
                uv[vertexLoop + 3] = DrawCircularVerticesForUv(halfAmountOfEdges, .333f, -0.5f + loopCount - uvOffset, radiusUvModifierX, radiusUvModifierY);

                uv[vertexLoop + 4] = DrawCircularVerticesForUv(halfAmountOfEdges, .166f, 1f - loopCount, radiusUvModifierX, radiusUvModifierY);
                uv[vertexLoop + 5] = DrawCircularVerticesForUv(halfAmountOfEdges, .333f, 0.5f - loopCount, radiusUvModifierX, radiusUvModifierY);
                uv[vertexLoop + 6] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, 1f - loopCount, radiusUvModifierX, radiusUvModifierY);
                uv[vertexLoop + 7] = DrawCircularVerticesForUv(halfAmountOfEdges, .333f, 1.5f - loopCount, radiusUvModifierX, radiusUvModifierY);

                triangles[triangleVerticesCount] = verticesCount;
                triangles[triangleVerticesCount + 1] = verticesCount + 1;
                triangles[triangleVerticesCount + 2] = verticesCount + 2;
                triangles[triangleVerticesCount + 3] = verticesCount + 3;
                triangles[triangleVerticesCount + 4] = verticesCount;
                triangles[triangleVerticesCount + 5] = verticesCount + 2;

                triangles[triangleVerticesCount + 6] = verticesCount + 4;
                triangles[triangleVerticesCount + 7] = verticesCount + 6;
                triangles[triangleVerticesCount + 8] = verticesCount + 5;
                triangles[triangleVerticesCount + 9] = verticesCount + 7;
                triangles[triangleVerticesCount + 10] = verticesCount + 6;
                triangles[triangleVerticesCount + 11] = verticesCount + 4;

                triangleVerticesCount += 12;
                verticesCount += 8;

                vertexLoop = vertexLoop + 8;
            }

            // Calculate vertices, uv and triangles for the first part of the pavillon
            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {

                vertices[vertexLoop] = vertexPositions[middleRingIndex + loopCount] - gem.transform.position;
                vertices[vertexLoop + 1] = vertexPositions[outerRingIndex + loopCount * 2] - gem.transform.position;
                vertices[vertexLoop + 2] = vertexPositions[outerRingIndex + loopCount * 2 + 1] - gem.transform.position;
                vertices[vertexLoop + 3] = vertexPositions[middleRingIndex + loopCount] - gem.transform.position;
                vertices[vertexLoop + 4] = vertexPositions[outerRingIndex + loopCount * 2 + 1] - gem.transform.position;
                vertices[vertexLoop + 5] = vertexPositions[outerRingIndex + loopCount * 2 + 2] - gem.transform.position;

                vertices[vertexLoop + 6] = vertexPositions[secondMiddleRingIndex + loopCount] - gem.transform.position;
                vertices[vertexLoop + 7] = vertexPositions[outerRingIndex + loopCount * 2] - gem.transform.position;
                vertices[vertexLoop + 8] = vertexPositions[outerRingIndex + loopCount * 2 + 1] - gem.transform.position;
                vertices[vertexLoop + 9] = vertexPositions[secondMiddleRingIndex + loopCount] - gem.transform.position;
                vertices[vertexLoop + 10] = vertexPositions[outerRingIndex + loopCount * 2 + 1] - gem.transform.position;
                vertices[vertexLoop + 11] = vertexPositions[outerRingIndex + loopCount * 2 + 2] - gem.transform.position;


                if (halfAmountOfEdges - 1 == loopCount)
                {

                    vertices[vertexLoop + 5] = vertexPositions[outerRingIndex] - gem.transform.position;
                    vertices[vertexLoop + 11] = vertexPositions[outerRingIndex] - gem.transform.position;
                }

                uv[vertexLoop] = DrawCircularVerticesForUv(halfAmountOfEdges, .333f, 0.5f + loopCount - uvOffset, radiusUvModifierX, radiusUvModifierY);
                uv[vertexLoop + 1] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, 0f + loopCount - uvOffset, radiusUvModifierX, radiusUvModifierY);
                uv[vertexLoop + 2] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, 0.5f + loopCount - uvOffset, radiusUvModifierX, radiusUvModifierY);
                uv[vertexLoop + 3] = DrawCircularVerticesForUv(halfAmountOfEdges, .333f, 0.5f + loopCount - uvOffset, radiusUvModifierX, radiusUvModifierY);
                uv[vertexLoop + 4] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, 0.5f + loopCount - uvOffset, radiusUvModifierX, radiusUvModifierY);
                uv[vertexLoop + 5] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, 1f + loopCount - uvOffset, radiusUvModifierX, radiusUvModifierY);

                uv[vertexLoop + 6] = DrawCircularVerticesForUv(halfAmountOfEdges, .333f, 0.5f - loopCount, radiusUvModifierX, radiusUvModifierY);
                uv[vertexLoop + 7] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, 1f - loopCount, radiusUvModifierX, radiusUvModifierY);
                uv[vertexLoop + 8] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, 0.5f - loopCount, radiusUvModifierX, radiusUvModifierY);
                uv[vertexLoop + 9] = DrawCircularVerticesForUv(halfAmountOfEdges, .333f, 0.5f - loopCount, radiusUvModifierX, radiusUvModifierY);
                uv[vertexLoop + 10] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, 0.5f - loopCount, radiusUvModifierX, radiusUvModifierY);
                uv[vertexLoop + 11] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, 0f - loopCount, radiusUvModifierX, radiusUvModifierY);

                triangles[triangleVerticesCount] = verticesCount;
                triangles[triangleVerticesCount + 1] = verticesCount + 2;
                triangles[triangleVerticesCount + 2] = verticesCount + 1;
                triangles[triangleVerticesCount + 3] = verticesCount + 3;
                triangles[triangleVerticesCount + 4] = verticesCount + 5;
                triangles[triangleVerticesCount + 5] = verticesCount + 4;

                triangles[triangleVerticesCount + 6] = verticesCount + 6;
                triangles[triangleVerticesCount + 7] = verticesCount + 7;
                triangles[triangleVerticesCount + 8] = verticesCount + 8;
                triangles[triangleVerticesCount + 9] = verticesCount + 9;
                triangles[triangleVerticesCount + 10] = verticesCount + 10;
                triangles[triangleVerticesCount + 11] = verticesCount + 11;

                triangleVerticesCount += 12;
                verticesCount += 12;

                vertexLoop = vertexLoop + 12;
            }

            Mesh mesh = new Mesh();
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.name = "generated gem mesh";
            MeshUtility.Optimize(mesh);
            mesh.RecalculateNormals();
            return mesh;
        }

        private Mesh CreateSmoothMesh(Gem gem)
        {
            List<Vector3> vertexPositions = gem.vertexPositions;
            int halfAmountOfEdges = gem.edges / 2;

            // Initialize variables for vertices logic
            int initialVerticesCount = gem.edges * 4 + 2;
            Vector3[] vertices = new Vector3[initialVerticesCount];
            int vertexLoop = 0;

            // Initialize variables for uv logic
            Vector2[] uv = new Vector2[initialVerticesCount];
            float uvOffset = 1f;
            float radiusUvModifierX;
            float radiusUvModifierY;

            if (gem.width > gem.height)
            {
                radiusUvModifierX = 1f;
                radiusUvModifierY = gem.height / gem.width;
            }
            else
            {
                radiusUvModifierX = gem.width / gem.height;
                radiusUvModifierY = 1f;
            }

            // Initialize variables for triangle logic
            int[] triangles = new int[gem.edges * 18];
            int verticesCount = 0;
            int triangleVerticesCount = 0;

            // The start indices of the vertex groups
            int startIndex = 0;
            int innerRingIndex = 1;
            int middleRingIndex = 1 + halfAmountOfEdges;
            int outerRingIndex = 1 + gem.edges;
            int secondMiddleRingIndex = 1 + gem.edges * 2;
            int secondInnerRingIndex = 1 + halfAmountOfEdges + gem.edges * 2;
            int endIndex = 1 + gem.edges * 3;

            vertices[0] = vertexPositions[startIndex] - gem.transform.position;
            vertices[1] = vertexPositions[endIndex] - gem.transform.position;
            uv[0] = new Vector2(.5f, .5f);
            uv[1] = new Vector2(.5f, .5f);
            verticesCount = +2;
            vertexLoop = +2;

            // Calculate vertices, uv and triangles for the first part of the pavillon
            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {
                vertices[vertexLoop] = vertexPositions[innerRingIndex + loopCount] - gem.transform.position;
                vertices[vertexLoop + 1] = vertexPositions[secondInnerRingIndex + loopCount] - gem.transform.position;

                uv[vertexLoop] = DrawCircularVerticesForUv(halfAmountOfEdges, .166f, 0f + loopCount - uvOffset, radiusUvModifierX, radiusUvModifierY);
                uv[vertexLoop + 1] = DrawCircularVerticesForUv(halfAmountOfEdges, .166f, 1f - loopCount, radiusUvModifierX, radiusUvModifierY);

                vertexLoop = vertexLoop + 2;
            }

            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {
                triangles[triangleVerticesCount] = 0;
                triangles[triangleVerticesCount + 1] = verticesCount + 2;
                triangles[triangleVerticesCount + 2] = verticesCount;

                triangles[triangleVerticesCount + 3] = 1;
                triangles[triangleVerticesCount + 4] = verticesCount + 1;
                triangles[triangleVerticesCount + 5] = verticesCount + 3;

                if (halfAmountOfEdges - 1 == loopCount)
                {
                    triangles[triangleVerticesCount + 1] = 2;
                    triangles[triangleVerticesCount + 5] = 3;
                }

                triangleVerticesCount += 6;
                verticesCount += 2;
            }

            // Calculate vertices, uv and triangles for the first part of the pavillon
            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {

                vertices[vertexLoop] = vertexPositions[middleRingIndex + loopCount] - gem.transform.position;
                vertices[vertexLoop + 1] = vertexPositions[secondMiddleRingIndex + loopCount] - gem.transform.position;

                uv[vertexLoop] = DrawCircularVerticesForUv(halfAmountOfEdges, .333f, 0.5f + loopCount - uvOffset, radiusUvModifierX, radiusUvModifierY);
                uv[vertexLoop + 1] = DrawCircularVerticesForUv(halfAmountOfEdges, .333f, 0.5f - loopCount, radiusUvModifierX, radiusUvModifierY);

                triangles[triangleVerticesCount] = verticesCount - gem.edges;
                triangles[triangleVerticesCount + 1] = verticesCount - gem.edges + 2;
                triangles[triangleVerticesCount + 2] = verticesCount;
                triangles[triangleVerticesCount + 3] = verticesCount - gem.edges + 1;
                triangles[triangleVerticesCount + 4] = verticesCount + 1;
                triangles[triangleVerticesCount + 5] = verticesCount - gem.edges + 3;

                if (halfAmountOfEdges - 1 == loopCount)
                {
                    triangles[triangleVerticesCount + 1] = 2;
                    triangles[triangleVerticesCount + 5] = 3;
                }

                triangleVerticesCount += 6;
                verticesCount += 2;

                vertexLoop = vertexLoop + 2;
            }

            // Calculate vertices, uv and triangles for the first part of the pavillon
            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {

                vertices[vertexLoop] = vertexPositions[outerRingIndex + loopCount * 2] - gem.transform.position;
                vertices[vertexLoop + 1] = vertexPositions[outerRingIndex + loopCount * 2] - gem.transform.position;

                uv[vertexLoop] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, 0f + loopCount - uvOffset, radiusUvModifierX, radiusUvModifierY);
                uv[vertexLoop + 1] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, 1f - loopCount, radiusUvModifierX, radiusUvModifierY);

                vertexLoop = vertexLoop + 2;
            }

            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {
                triangles[triangleVerticesCount] = verticesCount;
                triangles[triangleVerticesCount + 1] = verticesCount - (gem.edges * 2);
                triangles[triangleVerticesCount + 2] = verticesCount - gem.edges;
                triangles[triangleVerticesCount + 3] = verticesCount - gem.edges;
                triangles[triangleVerticesCount + 4] = verticesCount - (gem.edges * 2) + 2;
                triangles[triangleVerticesCount + 5] = verticesCount + 2;

                triangles[triangleVerticesCount + 6] = verticesCount + 1;
                triangles[triangleVerticesCount + 7] = verticesCount - gem.edges + 1;
                triangles[triangleVerticesCount + 8] = verticesCount - (gem.edges * 2) + 1;
                triangles[triangleVerticesCount + 9] = verticesCount - gem.edges + 1;
                triangles[triangleVerticesCount + 10] = verticesCount + 3;
                triangles[triangleVerticesCount + 11] = verticesCount - (gem.edges * 2) + 3;

                if (halfAmountOfEdges - 1 == loopCount)
                {
                    triangles[triangleVerticesCount + 4] = 2;
                    triangles[triangleVerticesCount + 5] = verticesCount - gem.edges + 2;
                    triangles[triangleVerticesCount + 11] = 3;
                    triangles[triangleVerticesCount + 10] = verticesCount - gem.edges + 3;
                }

                triangleVerticesCount += 12;
                verticesCount += 2;
            }

            // Calculate vertices, uv and triangles for the first part of the pavillon
            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {

                vertices[vertexLoop] = vertexPositions[outerRingIndex + loopCount * 2 + 1] - gem.transform.position;
                vertices[vertexLoop + 1] = vertexPositions[outerRingIndex + loopCount * 2 + 1] - gem.transform.position;

                uv[vertexLoop] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, 0.5f + loopCount - uvOffset, radiusUvModifierX, radiusUvModifierY);
                uv[vertexLoop + 1] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, 0.5f - loopCount, radiusUvModifierX, radiusUvModifierY);

                vertexLoop = vertexLoop + 2;
            }

            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {
                triangles[triangleVerticesCount] = verticesCount;
                triangles[triangleVerticesCount + 1] = verticesCount - gem.edges;
                triangles[triangleVerticesCount + 2] = verticesCount - (gem.edges * 2);
                triangles[triangleVerticesCount + 3] = verticesCount - (gem.edges * 2);
                triangles[triangleVerticesCount + 4] = verticesCount - gem.edges + 2;
                triangles[triangleVerticesCount + 5] = verticesCount;

                triangles[triangleVerticesCount + 6] = verticesCount + 1;
                triangles[triangleVerticesCount + 7] = verticesCount - (gem.edges * 2) + 1;
                triangles[triangleVerticesCount + 8] = verticesCount - gem.edges + 1;
                triangles[triangleVerticesCount + 9] = verticesCount - gem.edges + 3;
                triangles[triangleVerticesCount + 10] = verticesCount - (gem.edges * 2) + 1;
                triangles[triangleVerticesCount + 11] = verticesCount + 1;

                if (halfAmountOfEdges - 1 == loopCount)
                {
                    triangles[triangleVerticesCount + 4] = verticesCount - gem.edges * 2 + 2;
                    triangles[triangleVerticesCount + 9] = verticesCount - gem.edges * 2 + 3;
                }

                triangleVerticesCount += 12;
                verticesCount += 2;
            }

            Mesh mesh = new Mesh();
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.name = "generated gem mesh";
            mesh.RecalculateNormals();

            #region Recalculate some normals manually for smoother shading. 
            Vector3[] normals = mesh.normals;
            verticesCount--;

            for (int i = 0; i < gem.edges; i++)
            {
                int firstIndex = verticesCount - i * 2;
                int secondIndex = verticesCount - i * 2 - 1;
                Vector3 averageNormal = (normals[firstIndex] + normals[secondIndex]) / 2;
                normals[firstIndex] = averageNormal;
                normals[secondIndex] = averageNormal;
            }

            mesh.normals = normals;
            #endregion

            MeshUtility.Optimize(mesh);
            return mesh;
        }

        private Vector3 DrawCircularVerticesForUv(int edges, float radius, float offset, float modifierX, float modifierY)
        {
            Vector2 uvPosition;
            float degree = (360f / edges);
            degree += (360f / edges) * offset;
            float radian = degree * Mathf.Deg2Rad;
            float x = Mathf.Cos(radian) * (radius * modifierX);
            float y = Mathf.Sin(radian) * (radius * modifierY);
            uvPosition = new Vector2(.5f, .5f);
            uvPosition = uvPosition + new Vector2(x, y);
            return uvPosition;
        }
    }
}
