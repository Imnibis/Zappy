using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

namespace RockBuilder
{
    ///-----------------------------------------------------------------
    ///   Namespace:      RockBuilder
    ///   Class:          SphereRockMeshGenerator
    ///   Description:    The mesh generator for the sphere rock.
    ///   Author:         Stefano Canonico                    
    ///   Date:           04.01.2020
    ///   Version:        1.0
    ///-----------------------------------------------------------------
    public class SphereRockMeshGenerator
    {
        private static SphereRockMeshGenerator instance = null;
        private static readonly object padlock = new object();
        private int triangleVerticesCount;
        private int vertexLoop;
        Vector3[] vertices;
        Vector2[] uv;
        int[] triangles;
        float noiseFactor;
        SphereRockMeshGenerator()
        {
        }

        public static SphereRockMeshGenerator Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new SphereRockMeshGenerator();
                    }
                    return instance;
                }
            }
        }

        public List<Vector3> CreateVertexPositions(SphereRock sphereRock)
        {
            // List<List<Vector3>> circularIerationList = new List<List<Vector3>>();
            List<Vector3> finalList = new List<Vector3>();

            float startPositionY = -sphereRock.height / 2;
            int edges = sphereRock.edges;

            float radiusX = sphereRock.width;
            float radiusY = sphereRock.height;
            float radiusZ = sphereRock.depth;

            float positionZ = radiusZ / 2;

            List<Vector3> circularIeration = new List<Vector3>();
            // Get the vertices for the body
            for (int loopCount = 0; edges > loopCount; loopCount++)
            {
                int halfCircleFactor = (edges * 2) - 2;
                Vector3 spawnPoint = DrawCircularVerticesXY(sphereRock, radiusX, radiusY, positionZ, halfCircleFactor, loopCount);
                circularIeration.Add(spawnPoint);
            }

            foreach (Vector3 vertex in circularIeration)
            {
                // List<Vector3> circularIerationY = new List<Vector3>();
                for (int loopCount = 0; edges > loopCount; loopCount++)
                {
                    float multiplyFactor = vertex.x / radiusX;
                    float newRadiusX = radiusX * multiplyFactor;
                    float newRadiusZ = radiusZ * multiplyFactor;
                    float positionY = vertex.y;
                    Vector3 spawnPoint = DrawCircularVerticesXZ(sphereRock, newRadiusX, newRadiusZ, positionY, edges, loopCount);
                    finalList.Add(spawnPoint);
                }
                // circularIerationList.Add(circularIerationY);
            }

            return finalList;
        }

        private Vector3 DrawCircularVerticesXZ(SphereRock sphereRock, float radiusX, float radiusZ, float positionY, int edges, int loopCount)
        {
            Vector3 spawnPoint;
            float degree = (360f / edges) * loopCount;
            float radian = degree * Mathf.Deg2Rad;
            float x = Mathf.Cos(radian) * radiusX;
            float z = Mathf.Sin(radian) * radiusZ;
            spawnPoint = new Vector3(x, 0, z);
            spawnPoint.y = positionY;
            return spawnPoint;
        }

        private Vector3 DrawCircularVerticesXY(SphereRock sphereRock, float radiusX, float radiusY, float positionZ, int edges, int loopCount)
        {
            Vector3 spawnPoint;
            float degree = (360f / edges) * loopCount + 270f;
            float radian = degree * Mathf.Deg2Rad;
            float x = Mathf.Cos(radian) * radiusX;
            float y = Mathf.Sin(radian) * radiusY;
            spawnPoint = new Vector3(x, y, 0);
            spawnPoint.z = positionZ;

            return spawnPoint;
        }


        public Mesh CreateRockMesh(SphereRock sphereRock)
        {
            // return CreateSmoothMesh(sphereRock);
            if (sphereRock.smoothFlag)
            {
                return CreateSmoothMesh(sphereRock);
            }
            else
            {
                return CreateHardMesh(sphereRock);
            }
        }


        private Mesh CreateHardMesh(SphereRock sphereRock)
        {
            List<List<Vector3>> circularIterations = sphereRock.GetOrderedVertexList();
            int verticesPerIteration = circularIterations.Count;
            vertexLoop = 0;
            triangleVerticesCount = 0;
            int baseVerticesCount = (verticesPerIteration * verticesPerIteration);
            int closingVerticesSize = (verticesPerIteration * 2) * 3;
            int bodyVerticesSize = (verticesPerIteration * 4) * (verticesPerIteration - 3);
            int verticesCount = closingVerticesSize + bodyVerticesSize;
            Vector3[] noiseVertices = new Vector3[baseVerticesCount];
            vertices = new Vector3[verticesCount];
            uv = new Vector2[verticesCount];
            triangles = new int[(verticesPerIteration * (verticesPerIteration - 2)) * 6];
            noiseFactor = sphereRock.noise;

            int iterationCount = 0;
            int vertexCount = 1;

            Vector3 bottomClosingVertex = AddNoise(circularIterations.First()[0]);
            Vector3 upperClosingVertex = AddNoise(circularIterations.Last()[0]);

            foreach (List<Vector3> iteration in circularIterations)
            {
                iterationCount++;
                float uvHeightIteration = (1f / circularIterations.Count) * iterationCount;
                vertexCount = 1;

                foreach (Vector3 vertex in iteration)
                {

                    float uvWidthIteration = (1f / circularIterations.Count) * vertexCount / 1;
                    noiseVertices[vertexLoop] = AddNoise(vertex);

                    if (iterationCount == 1)
                    {
                        noiseVertices[vertexLoop] = bottomClosingVertex;
                    }

                    if (iterationCount == verticesPerIteration)
                    {
                        noiseVertices[vertexLoop] = upperClosingVertex;
                    }

                    vertexLoop++;
                    vertexCount++;
                }
            }

            int closingVerticesCount = verticesPerIteration * 2;
            int bodyVerticesCount = baseVerticesCount - (closingVerticesCount);

            int lowerClosingStartIndex = 0;
            int upperClosingStartIndex = baseVerticesCount - closingVerticesCount;
            int bodyStartIndex = verticesPerIteration;

            List<Vector3> noiseVerticesList = noiseVertices.ToList();
            List<Vector3> lowerClosingVertices = noiseVerticesList.GetRange(lowerClosingStartIndex, closingVerticesCount);
            List<Vector3> bodyVertices = noiseVerticesList.GetRange(bodyStartIndex, bodyVerticesCount);
            List<Vector3> upperClosingVertices = noiseVerticesList.GetRange(upperClosingStartIndex, closingVerticesCount);

            vertexLoop = 0;

            int uvIterationCount = verticesPerIteration - 1;
            for (int loopCount = 0; loopCount < verticesPerIteration; loopCount++)
            {
                float firstUvWidthIteration = (1f / uvIterationCount) * loopCount;
                float secondUvWidthIteration = (1f / uvIterationCount) * (loopCount + 1);
                float firstUvHeightIteration = (1f / uvIterationCount) * 0;
                float secondUvHeightIteration = (1f / uvIterationCount) * 1;

                int lowerMiddlePointIndex = loopCount;
                int firstIndex = loopCount + verticesPerIteration;
                int secondIndex = loopCount + verticesPerIteration + 1;

                if (loopCount == verticesPerIteration - 1)
                {
                    secondIndex = iterationCount;
                }

                vertices[vertexLoop] = lowerClosingVertices[lowerMiddlePointIndex];
                vertices[vertexLoop + 1] = lowerClosingVertices[firstIndex];
                vertices[vertexLoop + 2] = lowerClosingVertices[secondIndex];

                Vector2 firstUvVertex = new Vector2(secondUvWidthIteration, secondUvHeightIteration);
                Vector2 secondUvVertex = new Vector2(firstUvWidthIteration, secondUvHeightIteration);
                float middlePointUvWidth = Mathf.Lerp(firstUvWidthIteration, secondUvWidthIteration, 0.5f);
                Vector2 middlePointVertex = new Vector2(middlePointUvWidth, firstUvHeightIteration);

                uv[vertexLoop] = middlePointVertex;
                uv[vertexLoop + 1] = firstUvVertex;
                uv[vertexLoop + 2] = secondUvVertex;

                triangles[triangleVerticesCount] = vertexLoop;
                triangles[triangleVerticesCount + 1] = vertexLoop + 1;
                triangles[triangleVerticesCount + 2] = vertexLoop + 2;

                triangleVerticesCount += 3;
                vertexLoop += 3;
            }

            int IterationCompensation = 3;

            for (int outerLoopCount = 0; outerLoopCount < verticesPerIteration - IterationCompensation; outerLoopCount++)
            {
                int rowCount = outerLoopCount + 1;
                for (int loopCount = 0; loopCount < verticesPerIteration; loopCount++)
                {
                    float firstUvWidthIteration = (1f / uvIterationCount) * loopCount;
                    float secondUvWidthIteration = (1f / uvIterationCount) * (loopCount + 1);
                    float firstUvHeightIteration = (1f / uvIterationCount) * rowCount;
                    float secondUvHeightIteration = (1f / uvIterationCount) * (rowCount + 1);

                    int bodyVertexCount = (verticesPerIteration * outerLoopCount) + loopCount;

                    int firstLowerIndex = bodyVertexCount;
                    int secondLowerIndex = bodyVertexCount + 1;
                    int firstUpperIndex = bodyVertexCount + verticesPerIteration;
                    int secondUpperIndex = bodyVertexCount + verticesPerIteration + 1;

                    if (loopCount == verticesPerIteration - 1)
                    {
                        secondLowerIndex = secondLowerIndex - verticesPerIteration;
                        secondUpperIndex = secondUpperIndex - verticesPerIteration;
                    }

                    vertices[vertexLoop] = bodyVertices[firstLowerIndex];
                    vertices[vertexLoop + 1] = bodyVertices[secondLowerIndex];
                    vertices[vertexLoop + 2] = bodyVertices[firstUpperIndex];
                    vertices[vertexLoop + 3] = bodyVertices[secondUpperIndex];

                    uv[vertexLoop] = new Vector2(firstUvWidthIteration, firstUvHeightIteration);
                    uv[vertexLoop + 1] = new Vector2(secondUvWidthIteration, firstUvHeightIteration);
                    uv[vertexLoop + 2] = new Vector2(firstUvWidthIteration, secondUvHeightIteration);
                    uv[vertexLoop + 3] = new Vector2(secondUvWidthIteration, secondUvHeightIteration);

                    triangles[triangleVerticesCount] = vertexLoop;
                    triangles[triangleVerticesCount + 1] = vertexLoop + 2;
                    triangles[triangleVerticesCount + 2] = vertexLoop + 1;
                    triangles[triangleVerticesCount + 3] = vertexLoop + 3;
                    triangles[triangleVerticesCount + 4] = vertexLoop + 1;
                    triangles[triangleVerticesCount + 5] = vertexLoop + 2;

                    triangleVerticesCount += 6;
                    vertexLoop += 4;
                }
            }

            for (int loopCount = 0; loopCount < iterationCount; loopCount++)
            {
                int upperMiddlePointIndex = loopCount + iterationCount;
                int firstIndex = loopCount;
                int secondIndex = loopCount + 1;

                float firstUvWidthIteration = (1f / uvIterationCount) * loopCount;
                float secondUvWidthIteration = (1f / uvIterationCount) * (loopCount + 1);
                float firstUvHeightIteration = (1f / uvIterationCount) * (uvIterationCount - 1);
                float secondUvHeightIteration = (1f / uvIterationCount) * uvIterationCount;

                if (loopCount == iterationCount - 1)
                {
                    secondIndex = 0;
                }

                vertices[vertexLoop] = upperClosingVertices[upperMiddlePointIndex];
                vertices[vertexLoop + 1] = upperClosingVertices[firstIndex];
                vertices[vertexLoop + 2] = upperClosingVertices[secondIndex];

                Vector2 firstUvVertex = new Vector2(secondUvWidthIteration, firstUvHeightIteration);
                Vector2 secondUvVertex = new Vector2(firstUvWidthIteration, firstUvHeightIteration);
                float middlePointUvWidth = Mathf.Lerp(firstUvWidthIteration, secondUvWidthIteration, 0.5f);
                Vector2 middlePointVertex = new Vector2(middlePointUvWidth, secondUvHeightIteration);

                uv[vertexLoop] = middlePointVertex;
                uv[vertexLoop + 1] = firstUvVertex;
                uv[vertexLoop + 2] = secondUvVertex;

                triangles[triangleVerticesCount] = vertexLoop;
                triangles[triangleVerticesCount + 1] = vertexLoop + 2;
                triangles[triangleVerticesCount + 2] = vertexLoop + 1;

                triangleVerticesCount += 3;
                vertexLoop += 3;
            }

            Mesh mesh = new Mesh();
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.name = "generated sphere mesh";
            mesh.RecalculateNormals();

            MeshUtility.Optimize(mesh);
            return mesh;
        }

        private Mesh CreateSmoothMesh(SphereRock sphereRock)
        {
            List<List<Vector3>> circularIterations = sphereRock.GetOrderedVertexList();
            int verticesPerIteration = circularIterations.Count;
            vertexLoop = 0;
            triangleVerticesCount = 0;
            int verticesCount = (verticesPerIteration + 1) * (verticesPerIteration - 1) + 2;
            vertices = new Vector3[verticesCount];
            uv = new Vector2[verticesCount];
            triangles = new int[verticesCount * 6];
            noiseFactor = sphereRock.noise;

            int iterationCount = 0;

            Vector3 firstVertex = circularIterations.First().First();
            Vector3 lastVertex = circularIterations.Last().Last();

            circularIterations.RemoveAt(sphereRock.edges - 1);
            circularIterations.RemoveAt(0);

            vertices[vertexLoop] = AddNoise(firstVertex);
            uv[vertexLoop] = new Vector2(0.5f, 0f);

            for (int loopCount = 0; loopCount < sphereRock.edges; loopCount++)
            {
                triangles[triangleVerticesCount] = vertexLoop;
                triangles[triangleVerticesCount + 1] = vertexLoop + loopCount + 1;
                triangles[triangleVerticesCount + 2] = vertexLoop + loopCount + 2;

                triangleVerticesCount += 3;
            }

            vertexLoop++;

            foreach (List<Vector3> iteration in circularIterations)
            {
                iterationCount++;
                float uvHeightIteration = (1f / (circularIterations.Count + 1)) * iterationCount;
                int vertexCount = 1;
                Vector3 firstVertexOfIteration = AddNoise(iteration.First());
                foreach (Vector3 vertex in iteration)
                {
                    float uvWidthIteration = (1f / (circularIterations.Count + 2)) * (vertexCount - 1);
                    if (vertexCount == 1)
                    {
                        vertices[vertexLoop] = firstVertexOfIteration;
                    }
                    else
                    {
                        vertices[vertexLoop] = AddNoise(vertex);
                    }

                    uv[vertexLoop] = new Vector2(uvWidthIteration, uvHeightIteration);

                    if (iterationCount != circularIterations.Count)
                    {
                        triangles[triangleVerticesCount] = vertexLoop;
                        triangles[triangleVerticesCount + 1] = vertexLoop + verticesPerIteration + 1;
                        triangles[triangleVerticesCount + 2] = vertexLoop + 1;
                        triangles[triangleVerticesCount + 3] = vertexLoop + verticesPerIteration + 2;
                        triangles[triangleVerticesCount + 4] = vertexLoop + 1;
                        triangles[triangleVerticesCount + 5] = vertexLoop + verticesPerIteration + 1;

                        triangleVerticesCount += 6;

                    }

                    if (vertexCount == verticesPerIteration)
                    {
                        vertexLoop++;
                        vertices[vertexLoop] = firstVertexOfIteration;
                        uv[vertexLoop] = new Vector2(1, uvHeightIteration);
                    }

                    vertexLoop++;
                    vertexCount++;
                }
            }

            vertices[vertexLoop] = AddNoise(lastVertex);
            uv[vertexLoop] = new Vector2(0.5f, 1f);

            for (int loopCount = 0; loopCount < sphereRock.edges; loopCount++)
            {
                triangles[triangleVerticesCount] = vertexLoop;
                triangles[triangleVerticesCount + 1] = vertexLoop - loopCount - 1;
                triangles[triangleVerticesCount + 2] = vertexLoop - loopCount - 2;

                if (loopCount == sphereRock.edges - 1)
                {
                    triangles[triangleVerticesCount + 2] = vertexLoop - 1;
                }

                triangleVerticesCount += 3;
            }

            Mesh mesh = new Mesh();
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.name = "generated sphere mesh";
            mesh.RecalculateNormals();

            #region Recalculate some normals manually for smoother shading. 
            Vector3[] normals = mesh.normals;

            for (int loopCount = 0; loopCount < sphereRock.edges - 1; loopCount++)
            {
                int firstIndex = loopCount * (sphereRock.edges + 1) + 1;
                int secondIndex = firstIndex + sphereRock.edges;
                Vector3 firstNormal = normals[firstIndex];
                Vector3 secondNormal = normals[secondIndex];
                Vector3 averageNormal = (firstNormal + secondNormal) / 2;
                normals[firstIndex] = averageNormal;
                normals[secondIndex] = averageNormal;
            }

            mesh.normals = normals;
            #endregion

            MeshUtility.Optimize(mesh);
            return mesh;
        }

        private Vector3 AddNoise(Vector3 vertex)
        {
            float halfNoiseFactor = noiseFactor / 2;
            float noiseX = Random.Range(-halfNoiseFactor, halfNoiseFactor);
            float noiseY = Random.Range(-halfNoiseFactor, halfNoiseFactor);
            float noiseZ = Random.Range(-halfNoiseFactor, halfNoiseFactor);

            Vector3 noiseVector = new Vector3(noiseX, noiseY, noiseZ);
            return vertex + noiseVector;
        }
    }
}