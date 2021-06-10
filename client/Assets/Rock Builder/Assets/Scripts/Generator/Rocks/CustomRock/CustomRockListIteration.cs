using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace RockBuilder
{
    ///-----------------------------------------------------------------
    ///   Namespace:      RockBuilder
    ///   Class:          CubeRockService
    ///   Description:    Model to which contains data of an custom rock iteration.
    ///   Author:         Stefano Canonico                    
    ///   Date:           04.01.2020
    ///   Version:        1.0
    ///-----------------------------------------------------------------
    public class CustomRockListIteration
    {
        public List<Vector3> unsortedVertexList;

        public List<Vector3> firstQuarter = new List<Vector3>();
        public List<Vector3> secondQuarter = new List<Vector3>();
        public List<Vector3> thirdQuarter = new List<Vector3>();
        public List<Vector3> fourthQuarter = new List<Vector3>();

        public CustomRockListIteration(List<Vector3> vertexPositions)
        {
            unsortedVertexList = vertexPositions;
        }

        public void Sort()
        {
            firstQuarter.OrderBy(vector => vector.x);
            firstQuarter.OrderByDescending(vector => vector.z);

            secondQuarter.OrderByDescending(vector => vector.x);
            secondQuarter.OrderBy(vector => vector.z);

            thirdQuarter.OrderByDescending(vector => vector.x);
            thirdQuarter.OrderByDescending(vector => vector.z);

            fourthQuarter.OrderBy(vector => vector.x);
            fourthQuarter.OrderBy(vector => vector.z);
        }

        public int GetHighestCount()
        {
            int highestCount = firstQuarter.Count;

            if (secondQuarter.Count > highestCount)
            {
                highestCount = secondQuarter.Count;
            }

            if (thirdQuarter.Count > highestCount)
            {
                highestCount = thirdQuarter.Count;
            }

            if (fourthQuarter.Count > highestCount)
            {
                highestCount = fourthQuarter.Count;
            }

            return highestCount;
        }

        public Vector3 GetCenterPoint()
        {
            List<Vector3> vertexList = unsortedVertexList.ToList();
            if (vertexList != null || vertexList.Count != 0)
            {
                float maxCoordinateX = vertexList.OrderByDescending(vector => vector.x).First().x;
                float maxCoordinateZ = vertexList.OrderByDescending(vector => vector.z).First().z;
                float minCoordinateX = vertexList.OrderBy(vector => vector.x).First().x;
                float minCoordinateZ = vertexList.OrderBy(vector => vector.z).First().z;

                float differenceX = (maxCoordinateX - minCoordinateX);
                float differenceZ = (maxCoordinateZ - minCoordinateZ);

                float middlePointX = minCoordinateX + differenceX / 2;
                float middlePointZ = minCoordinateZ + differenceZ / 2;

                return new Vector3(middlePointX, GetAverageHeight(), middlePointZ);
            }
            else
            {
                return Vector3.zero;
            }
        }

        public float GetAverageHeight()
        {
            List<Vector3> vertexList = unsortedVertexList.ToList();
            int loopCount = 0;
            float totalHeight = 0f;

            foreach (Vector3 vertex in vertexList)
            {
                loopCount++;
                totalHeight += vertex.y;
            }

            return totalHeight / loopCount;
        }

        public void EqualizeVertexCountOnFirstQuarter(int vertexCount)
        {
            if (firstQuarter.Count != vertexCount)
            {
                InterpolateVertices(firstQuarter, fourthQuarter, secondQuarter, vertexCount);
            }
        }

        public void EqualizeVertexCountOnSecondQuarter(int vertexCount)
        {
            if (secondQuarter.Count != vertexCount)
            {
                InterpolateVertices(secondQuarter, firstQuarter, thirdQuarter, vertexCount);
            }
        }

        public void EqualizeVertexCountOnThirdQuarter(int vertexCount)
        {
            if (thirdQuarter.Count != vertexCount)
            {
                InterpolateVertices(thirdQuarter, secondQuarter, fourthQuarter, vertexCount);
            }
        }

        public void EqualizeVertexCountOnFourthQuarter(int vertexCount)
        {
            if (fourthQuarter.Count != vertexCount)
            {
                InterpolateVertices(fourthQuarter, thirdQuarter, firstQuarter, vertexCount);
            }
        }

        private void InterpolateVertices(List<Vector3> listToInterpolate, List<Vector3> listBefore, List<Vector3> listAfter, int vertexCount)
        {
            int interpolationCountBefore;
            int interpolationCountAfter;
            int interpolationCount = vertexCount - listToInterpolate.Count;
            float interpolationFactorBefore;
            float interpolationFactorAfter;

            if (interpolationCount % 2 == 0)
            {
                interpolationCountBefore = interpolationCount / 2;
                interpolationCountAfter = interpolationCount / 2;
                interpolationFactorBefore = 1f / (interpolationCountBefore + 1);
                interpolationFactorAfter = 1f / (interpolationCountAfter + 1);
            }
            else
            {
                float newValue = (float)interpolationCount / 2 + 0.5f;
                interpolationCountBefore = (int)newValue;
                interpolationCountAfter = interpolationCountBefore - 1;
                interpolationFactorBefore = 1f / (interpolationCountBefore + 1);
                interpolationFactorAfter = 1f / (interpolationCountAfter + 1);
            }

            Vector3 firstVertex = listToInterpolate.First();

            for (int loopCount = 0; interpolationCountBefore > loopCount; loopCount++)
            {
                Vector3 interpolatedVertex = Vector3.Lerp(firstVertex, listBefore.Last(), interpolationFactorBefore * (loopCount + 1));
                listToInterpolate.Add(interpolatedVertex);
            }

            Vector3 lastVertex = listToInterpolate.Last();

            for (int loopCount = 0; interpolationCountBefore > loopCount; loopCount++)
            {
                if (interpolationCount != 1)
                {
                    Vector3 interpolatedVertex = Vector3.Lerp(lastVertex, listAfter.First(), interpolationFactorAfter * (loopCount + 1));
                    listToInterpolate.Add(interpolatedVertex);
                }
            }

            Sort();
        }

        public int GetVertexCount()
        {
            int vertexCount = 0;
            vertexCount += firstQuarter.Count;
            vertexCount += secondQuarter.Count;
            vertexCount += thirdQuarter.Count;
            vertexCount += fourthQuarter.Count;
            return vertexCount;
        }

        public List<Vector3> GetSortedVertexList()
        {
            List<Vector3> sortedVertices = new List<Vector3>();
            sortedVertices.AddRange(firstQuarter);
            sortedVertices.AddRange(secondQuarter);
            sortedVertices.AddRange(thirdQuarter);
            sortedVertices.AddRange(fourthQuarter);

            return sortedVertices;
        }
    }
}