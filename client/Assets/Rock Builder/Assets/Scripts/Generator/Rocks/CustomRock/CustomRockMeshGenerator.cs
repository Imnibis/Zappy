using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace RockBuilder
{
    ///-----------------------------------------------------------------
    ///   Namespace:      RockBuilder
    ///   Class:          CubeRockMeshGenerator
    ///   Description:    The mesh Generator for the custom rock.
    ///   Author:         Stefano Canonico                    
    ///   Date:           04.01.2020
    ///   Version:        1.0
    ///-----------------------------------------------------------------
    enum IterationType
    {
        firstQuarter,
        secondQuarter,
        thirdQuarter,
        fourthQuarter
    }

    public class CustomRockMeshGenerator
    {
        private static CustomRockMeshGenerator instance = null;
        private static readonly object padlock = new object();

        CustomRockMeshGenerator()
        {
        }

        public static CustomRockMeshGenerator Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new CustomRockMeshGenerator();
                    }
                    return instance;
                }
            }
        }

        public Mesh CreateRockMesh(CustomRock customRock)
        {
            List<List<Vector3>> vertexIteratios = CreateIterations(customRock.rockBuildPoints);
            customRock.sortedVertices = SortVerticesClockwise(vertexIteratios);
            customRock.sortedVertices = equalizeVertexCount(customRock.sortedVertices);
            // customRock.sortedVertices = InterpolateBetweenIteration(customRock.sortedVertices);
            return CreateSmoothMesh(customRock);
        }

        private List<List<Vector3>> CreateIterations(List<Vector3> buildPoints)
        {
            List<List<Vector3>> iterationList = new List<List<Vector3>>();

            float highestBuildPoint = buildPoints[0].y;
            float lowestBuildPoint = buildPoints[0].y;

            foreach (Vector3 buildPoint in buildPoints)
            {
                if (buildPoint.y > highestBuildPoint)
                {
                    highestBuildPoint = buildPoint.y;
                }

                if (buildPoint.y < lowestBuildPoint)
                {
                    lowestBuildPoint = buildPoint.y;
                }
            }

            float meshHeight = Mathf.Abs(highestBuildPoint - lowestBuildPoint);
            float iterationScanHeight = meshHeight / 6;


            for (int loopCount = 0; loopCount < 6; loopCount++)
            {
                List<Vector3> iteration = new List<Vector3>();
                float startHieght = lowestBuildPoint + iterationScanHeight * loopCount;
                float endHeight = lowestBuildPoint + iterationScanHeight * (loopCount + 1);

                foreach (Vector3 buildPoint in buildPoints)
                {
                    if (buildPoint.y >= startHieght && buildPoint.y <= endHeight)
                    {
                        iteration.Add(buildPoint);
                    }
                }

                if (iteration.Count != 0)
                {
                    iterationList.Add(iteration);
                }

            }

            return iterationList;
        }

        private List<CustomRockListIteration> SortVerticesClockwise(List<List<Vector3>> unsortedVertexPositions)
        {
            List<CustomRockListIteration> sortedVertexPositions = new List<CustomRockListIteration>();

            foreach (List<Vector3> iteration in unsortedVertexPositions)
            {
                CustomRockListIteration sortedIteration = new CustomRockListIteration(iteration);

                Vector3 centerPoint = sortedIteration.GetCenterPoint();

                foreach (Vector3 vertexPosition in iteration)
                {
                    // from 0 to 3 o'clock
                    if (vertexPosition.x > centerPoint.x && vertexPosition.z > centerPoint.z)
                    {
                        sortedIteration.firstQuarter.Add(vertexPosition);
                    }
                    // from 3 to 6 o'clock
                    if (vertexPosition.x > centerPoint.x && vertexPosition.z < centerPoint.z)
                    {
                        sortedIteration.secondQuarter.Add(vertexPosition);
                    }
                    // from 6 to 9 o'clock
                    if (vertexPosition.x < centerPoint.x && vertexPosition.z < centerPoint.z)
                    {
                        sortedIteration.thirdQuarter.Add(vertexPosition);
                    }
                    // from 9 to 12 o'clock
                    if (vertexPosition.x < centerPoint.x && vertexPosition.z > centerPoint.z)
                    {
                        sortedIteration.fourthQuarter.Add(vertexPosition);
                    }
                }

                sortedIteration.Sort();

                sortedVertexPositions.Add(sortedIteration);
            }

            return sortedVertexPositions;
        }

        private List<CustomRockListIteration> equalizeVertexCount(List<CustomRockListIteration> sortedVertexPositions)
        {
            int highestCount = 0;

            int loopCount = 0;

            List<int> firstQuarterIndexesWithZeroCount = new List<int>();
            List<int> secondQuarterIndexesWithZeroCount = new List<int>();
            List<int> thirdQuarterIndexesWithZeroCount = new List<int>();
            List<int> fourthQuarterIndexesWithZeroCount = new List<int>();

            RemoveEmptyIterations(sortedVertexPositions);

            foreach (CustomRockListIteration iteration in sortedVertexPositions)
            {
                if (iteration.firstQuarter.Count > highestCount)
                {
                    highestCount = iteration.firstQuarter.Count;
                }
                if (iteration.secondQuarter.Count > highestCount)
                {
                    highestCount = iteration.secondQuarter.Count;
                }
                if (iteration.thirdQuarter.Count > highestCount)
                {
                    highestCount = iteration.thirdQuarter.Count;
                }
                if (iteration.fourthQuarter.Count > highestCount)
                {
                    highestCount = iteration.fourthQuarter.Count;
                }


                if (iteration.firstQuarter.Count == 0)
                {
                    firstQuarterIndexesWithZeroCount.Add(loopCount);
                }
                if (iteration.secondQuarter.Count == 0)
                {
                    secondQuarterIndexesWithZeroCount.Add(loopCount);
                }
                if (iteration.thirdQuarter.Count == 0)
                {
                    thirdQuarterIndexesWithZeroCount.Add(loopCount);
                }
                if (iteration.fourthQuarter.Count == 0)
                {
                    fourthQuarterIndexesWithZeroCount.Add(loopCount);
                }

                loopCount++;
            }

            FillEmptyQuarters(firstQuarterIndexesWithZeroCount, sortedVertexPositions, IterationType.firstQuarter);
            FillEmptyQuarters(secondQuarterIndexesWithZeroCount, sortedVertexPositions, IterationType.secondQuarter);
            FillEmptyQuarters(thirdQuarterIndexesWithZeroCount, sortedVertexPositions, IterationType.thirdQuarter);
            FillEmptyQuarters(fourthQuarterIndexesWithZeroCount, sortedVertexPositions, IterationType.fourthQuarter);

            foreach (CustomRockListIteration iteration in sortedVertexPositions)
            {
                if (iteration.firstQuarter.Count == 0)
                {
                    iteration.firstQuarter.Add(iteration.GetCenterPoint());
                }
                if (iteration.secondQuarter.Count == 0)
                {
                    iteration.secondQuarter.Add(iteration.GetCenterPoint());
                }
                if (iteration.thirdQuarter.Count == 0)
                {
                    iteration.thirdQuarter.Add(iteration.GetCenterPoint());
                }
                if (iteration.fourthQuarter.Count == 0)
                {
                    iteration.fourthQuarter.Add(iteration.GetCenterPoint());
                }
            }

            foreach (CustomRockListIteration iteration in sortedVertexPositions)
            {

                iteration.EqualizeVertexCountOnFirstQuarter(highestCount);
                iteration.EqualizeVertexCountOnSecondQuarter(highestCount);
                iteration.EqualizeVertexCountOnThirdQuarter(highestCount);
                iteration.EqualizeVertexCountOnFourthQuarter(highestCount);

                iteration.Sort();
            }

            return sortedVertexPositions;
        }

        private List<CustomRockListIteration> EqualizeToNewVertexCount(List<CustomRockListIteration> sortedList, int newCount)
        {
            foreach (CustomRockListIteration iteration in sortedList)
            {
                iteration.EqualizeVertexCountOnFirstQuarter(newCount);
                iteration.EqualizeVertexCountOnSecondQuarter(newCount);
                iteration.EqualizeVertexCountOnThirdQuarter(newCount);
                iteration.EqualizeVertexCountOnFourthQuarter(newCount);
            }

            return sortedList;
        }

        private void RemoveEmptyIterations(List<CustomRockListIteration> sortedLists)
        {
            List<int> emptyIterationList = new List<int>();
            int loopCount = 0;
            foreach (CustomRockListIteration iteration in sortedLists)
            {
                if (iteration.firstQuarter.Count == 0 && iteration.secondQuarter.Count == 0
                     && iteration.thirdQuarter.Count == 0 && iteration.fourthQuarter.Count == 0)
                {
                    emptyIterationList.Add(loopCount);
                }
                loopCount++;
            }

            int removeCount = 0;
            foreach (int index in emptyIterationList)
            {
                sortedLists.RemoveAt(index - removeCount);
                removeCount++;
            }
        }
        private void FillEmptyQuarters(List<int> quarterIndexes, List<CustomRockListIteration> sortedLists, IterationType type)
        {
            if (quarterIndexes.Count != 0 && quarterIndexes.Count != sortedLists.Count)
            {
                foreach (int index in quarterIndexes)
                {
                    float newValueY = sortedLists[index].GetAverageHeight();
                    bool vertexFound = false;
                    Vector3 vertex;

                    int loopCount = 1;

                    while (vertexFound == false)
                    {
                        int upperIterationIndex = index + loopCount;
                        int lowerIterationIndex = index - loopCount;

                        if (upperIterationIndex <= sortedLists.Count - 1)
                        {
                            if (type == IterationType.firstQuarter && sortedLists[upperIterationIndex].firstQuarter.Count != 0)
                            {
                                vertexFound = true;
                                vertex = sortedLists[upperIterationIndex].firstQuarter.First();
                                vertex = new Vector3(vertex.x, newValueY, vertex.z);
                                sortedLists[index].firstQuarter.Add(vertex);
                            }

                            if (type == IterationType.secondQuarter && sortedLists[upperIterationIndex].secondQuarter.Count != 0)
                            {
                                vertexFound = true;
                                vertex = sortedLists[upperIterationIndex].secondQuarter.First();
                                vertex = new Vector3(vertex.x, newValueY, vertex.z);
                                sortedLists[index].secondQuarter.Add(vertex);
                            }

                            if (type == IterationType.thirdQuarter && sortedLists[upperIterationIndex].thirdQuarter.Count != 0)
                            {
                                vertexFound = true;
                                vertex = sortedLists[upperIterationIndex].thirdQuarter.First();
                                vertex = new Vector3(vertex.x, newValueY, vertex.z);
                                sortedLists[index].thirdQuarter.Add(vertex);
                            }

                            if (type == IterationType.fourthQuarter && sortedLists[upperIterationIndex].fourthQuarter.Count != 0)
                            {
                                vertexFound = true;
                                vertex = sortedLists[upperIterationIndex].fourthQuarter.First();
                                vertex = new Vector3(vertex.x, newValueY, vertex.z);
                                sortedLists[index].fourthQuarter.Add(vertex);
                            }
                        }

                        if (lowerIterationIndex >= 0)
                        {
                            if (type == IterationType.firstQuarter && sortedLists[lowerIterationIndex].firstQuarter.Count != 0)
                            {
                                vertexFound = true;
                                vertex = sortedLists[lowerIterationIndex].firstQuarter.First();
                                vertex = new Vector3(vertex.x, newValueY, vertex.z);
                                sortedLists[index].firstQuarter.Add(vertex);
                            }

                            if (type == IterationType.secondQuarter && sortedLists[lowerIterationIndex].secondQuarter.Count != 0)
                            {
                                vertexFound = true;
                                vertex = sortedLists[lowerIterationIndex].secondQuarter.First();
                                vertex = new Vector3(vertex.x, newValueY, vertex.z);
                                sortedLists[index].secondQuarter.Add(vertex);
                            }

                            if (type == IterationType.thirdQuarter && sortedLists[lowerIterationIndex].thirdQuarter.Count != 0)
                            {
                                vertexFound = true;
                                vertex = sortedLists[lowerIterationIndex].thirdQuarter.First();
                                vertex = new Vector3(vertex.x, newValueY, vertex.z);
                                sortedLists[index].thirdQuarter.Add(vertex);
                            }

                            if (type == IterationType.fourthQuarter && sortedLists[lowerIterationIndex].fourthQuarter.Count != 0)
                            {
                                vertexFound = true;
                                vertex = sortedLists[lowerIterationIndex].fourthQuarter.First();
                                vertex = new Vector3(vertex.x, newValueY, vertex.z);
                                sortedLists[index].fourthQuarter.Add(vertex);
                            }
                        }

                        if (upperIterationIndex > sortedLists.Count && lowerIterationIndex < 0)
                        {
                            Debug.LogWarning("Not Enough Build points. Add some more.");
                            vertexFound = true;
                        }

                        loopCount++;
                    }
                }
            }
        }

        private List<CustomRockListIteration> InterpolateBetweenIteration(List<CustomRockListIteration> sortedList)
        {
            List<CustomRockListIteration> newSortedList = new List<CustomRockListIteration>();

            int nextIterationIndex = 1;

            foreach (CustomRockListIteration iteration in sortedList)
            {
                if (nextIterationIndex != sortedList.Count)
                {
                    CustomRockListIteration nextIteration = sortedList[nextIterationIndex];
                    float lowestHeight = iteration.GetAverageHeight();
                    float highestHeight = nextIteration.GetAverageHeight();
                    float heightDifference = Mathf.Abs(highestHeight - lowestHeight);
                    int interpolationsBetweenIterations = Mathf.RoundToInt(heightDifference);
                    List<CustomRockListIteration> interpolationsBetween = new List<CustomRockListIteration>();

                    for (int interpolationCount = 1; interpolationsBetweenIterations > interpolationCount; interpolationCount++)
                    {
                        float lerpFactor = 1f / (interpolationsBetweenIterations + 1);
                        lerpFactor *= interpolationCount;
                        CustomRockListIteration newIteration = new CustomRockListIteration(new List<Vector3>());
                        float minNoise = -1f;
                        float maxNoise = 1f;

                        for (int loopCount = 0; loopCount < iteration.firstQuarter.Count; loopCount++)
                        {
                            Vector3 firstVertex = iteration.firstQuarter[loopCount];
                            Vector3 lastVertex = nextIteration.firstQuarter[loopCount];
                            Vector3 interpolatedVertex = Vector3.Lerp(firstVertex, lastVertex, lerpFactor);
                            interpolatedVertex = AddNoise(interpolatedVertex, minNoise, maxNoise);
                            newIteration.firstQuarter.Add(interpolatedVertex);
                        }

                        for (int loopCount = 0; loopCount < iteration.secondQuarter.Count; loopCount++)
                        {
                            Vector3 firstVertex = iteration.secondQuarter[loopCount];
                            Vector3 lastVertex = nextIteration.secondQuarter[loopCount];
                            Vector3 interpolatedVertex = Vector3.Lerp(firstVertex, lastVertex, lerpFactor);
                            interpolatedVertex = AddNoise(interpolatedVertex, minNoise, maxNoise);
                            newIteration.secondQuarter.Add(interpolatedVertex);
                        }

                        for (int loopCount = 0; loopCount < iteration.thirdQuarter.Count; loopCount++)
                        {
                            Vector3 firstVertex = iteration.thirdQuarter[loopCount];
                            Vector3 lastVertex = nextIteration.thirdQuarter[loopCount];
                            Vector3 interpolatedVertex = Vector3.Lerp(firstVertex, lastVertex, lerpFactor);
                            interpolatedVertex = AddNoise(interpolatedVertex, minNoise, maxNoise);
                            newIteration.thirdQuarter.Add(interpolatedVertex);
                        }

                        for (int loopCount = 0; loopCount < iteration.fourthQuarter.Count; loopCount++)
                        {
                            Vector3 firstVertex = iteration.fourthQuarter[loopCount];
                            Vector3 lastVertex = nextIteration.fourthQuarter[loopCount];
                            Vector3 interpolatedVertex = Vector3.Lerp(firstVertex, lastVertex, lerpFactor);
                            interpolatedVertex = AddNoise(interpolatedVertex, minNoise, maxNoise);
                            newIteration.fourthQuarter.Add(interpolatedVertex);
                        }
                        interpolationsBetween.Add(newIteration);
                    }

                    newSortedList.Add(iteration);
                    newSortedList.AddRange(interpolationsBetween);
                }
                else
                {
                    newSortedList.Add(iteration);
                }

                nextIterationIndex++;
            }

            return newSortedList;
        }

        private Vector3 AddNoise(Vector3 vertex, float min, float max)
        {
            float x = vertex.x + Random.Range(min, max);
            float z = vertex.z + Random.Range(min, max);
            return new Vector3(x, vertex.y, z);
        }
        private Mesh CreateSmoothMesh(CustomRock rockBuildData)
        {

            //List<Vector3> vertexPositions = rockBuildData.GetVertexCount();
            int vrticesCount = rockBuildData.GetVertexCount() + 2;
            Vector3[] vertices = new Vector3[vrticesCount];
            Vector2[] uv = new Vector2[vrticesCount];
            int vertexLoop = 0;
            int vertexCountPerIteration = rockBuildData.sortedVertices[0].GetVertexCount();
            int uvIterationY = 0;
            foreach (CustomRockListIteration iterationList in rockBuildData.sortedVertices)
            {

                float uvHeightIteration = 1f / rockBuildData.sortedVertices.Count;
                float uvWidthIteration = 1f / vertexCountPerIteration;
                float uvPositionY = uvIterationY * uvHeightIteration;
                int uvIterationX = 0;
                foreach (Vector3 vertex in iterationList.GetSortedVertexList())
                {
                    float uvPositionX = uvIterationX * uvWidthIteration;
                    vertices[vertexLoop] = vertex;
                    uv[vertexLoop] = new Vector2(uvPositionX, uvPositionY);
                    vertexLoop++;
                    uvIterationX++;
                }
                uvIterationY++;
            }

            //    for (int loopCount = 0; rockBuildData.sortedVertices.Count > loopCount; loopCount++)
            //{
            //    foreach(Vector3 vertex in rockBuildData.sortedVertices[loopCount].GetSortedVertexList())
            //    {
            //        vertices[vertexLoop] = vertex;
            //        vertexLoop++;
            //    }
            //}

            int[] triangles = new int[rockBuildData.GetVertexCount() * 9];
            int verticesCount = 0;
            int triangleVerticesCount = 0;


            for (int literationCount = 0; literationCount < rockBuildData.sortedVertices.Count; literationCount++)
            {
                int finalIteration = rockBuildData.sortedVertices.Count - 1;
                if (literationCount != finalIteration)
                {
                    for (int vertexCount = 0; vertexCount < vertexCountPerIteration; vertexCount++)
                    {
                        int finalLoop = vertexCountPerIteration - 1;
                        int nextVertex = verticesCount + 1;
                        int firstVertexFromNextIteration = verticesCount + vertexCountPerIteration;
                        int secondVertexFromNextIteration = verticesCount + vertexCountPerIteration + 1;

                        triangles[triangleVerticesCount] = verticesCount;
                        triangles[triangleVerticesCount + 1] = nextVertex;
                        triangles[triangleVerticesCount + 2] = firstVertexFromNextIteration;
                        triangles[triangleVerticesCount + 3] = firstVertexFromNextIteration;
                        triangles[triangleVerticesCount + 4] = nextVertex;
                        triangles[triangleVerticesCount + 5] = secondVertexFromNextIteration;

                        if (vertexCount == finalLoop)
                        {
                            triangles[triangleVerticesCount + 1] = nextVertex - vertexCountPerIteration;
                            // triangles[triangleVerticesCount + 2] = firstVertexFromNextIteration + 1 - vertexCountPerIteration;
                            triangles[triangleVerticesCount + 4] = nextVertex - vertexCountPerIteration;
                            triangles[triangleVerticesCount + 5] = secondVertexFromNextIteration - vertexCountPerIteration;
                        }

                        verticesCount += 1;
                        triangleVerticesCount += 6;
                    }
                }
            }

            int lastVertexCount = vertexLoop - 1;
            CustomRockListIteration firstIteration = rockBuildData.sortedVertices.First();
            int lowerMiddlePointIndex = vertexLoop;
            vertices[lowerMiddlePointIndex] = firstIteration.GetCenterPoint();

            for (int loopCount = 0; loopCount < vertexCountPerIteration; loopCount++)
            {
                triangles[triangleVerticesCount] = loopCount;
                triangles[triangleVerticesCount + 1] = lowerMiddlePointIndex;
                triangles[triangleVerticesCount + 2] = loopCount + 1;

                if (loopCount == vertexCountPerIteration - 1)
                {
                    triangles[triangleVerticesCount + 2] = 0;
                }

                triangleVerticesCount += 3;
            }

            CustomRockListIteration lastIteration = rockBuildData.sortedVertices.Last();
            int upperMiddlePointIndex = vertexLoop + 1;
            vertices[upperMiddlePointIndex] = lastIteration.GetCenterPoint();

            for (int loopCount = 0; loopCount < vertexCountPerIteration; loopCount++)
            {
                triangles[triangleVerticesCount] = upperMiddlePointIndex;
                triangles[triangleVerticesCount + 1] = lastVertexCount - (loopCount + 1);
                triangles[triangleVerticesCount + 2] = lastVertexCount - loopCount;

                if (loopCount == vertexCountPerIteration - 1)
                {
                    triangles[triangleVerticesCount + 1] = lastVertexCount;
                }

                triangleVerticesCount += 3;
            }

            Mesh mesh = new Mesh();
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.name = "generated custom stone mesh";
            mesh.RecalculateNormals();

            //#region Recalculate some normals manually for smoother shading. 
            //Vector3[] normals = mesh.normals;

            //Vector3 averageNormal1 = (normals[0] + normals[(crystal.edges * 2)]) / 2;
            //normals[0] = averageNormal1;
            //normals[(crystal.edges * 2)] = averageNormal1;

            //Vector3 averageNormal2 = (normals[1] + normals[(crystal.edges * 2) + 1]) / 2;
            //normals[1] = averageNormal2;
            //normals[(crystal.edges * 2) + 1] = averageNormal2;

            //for (int i = 1; i < crystal.edges + 1; i++)
            //{
            //    normals[normals.Length - i] = new Vector3(0f, 1f, 0f);
            //    normals[normals.Length - i - crystal.edges] = new Vector3(0f, -1f, 0f);
            //}

            //mesh.normals = normals;
            //#endregion


            //MeshUtility.Optimize(mesh);
            return mesh;
        }
    }
}