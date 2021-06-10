using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace RockBuilder
{
    ///-----------------------------------------------------------------
    ///   Namespace:      RockBuilder
    ///   Class:          CubeRockPreview
    ///   Description:    Draws the preview for the cube rock.
    ///   Author:         Stefano Canonico                    
    ///   Date:           04.01.2020
    ///   Version:        1.0
    ///-----------------------------------------------------------------
    public class CubeRockPreview
    {
        private static CubeRockPreview instance = null;
        private static readonly object padlock = new object();

        CubeRockPreview()
        {
        }

        public static CubeRockPreview Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new CubeRockPreview();
                    }
                    return instance;
                }
            }
        }

        public void DrawLines(CubeRock cubeRock)
        {
            Gizmos.color = Color.blue;
            Gizmos.matrix = cubeRock.transform.localToWorldMatrix;

            List<Vector3> bottomPlaneVertices = cubeRock.bottomPlaneVertices;
            List<Vector3> upperPlaneVertices = cubeRock.upperPlaneVertices;
            List<Vector3> bottomBezelsVertices = cubeRock.bottomVerticalBezelsVertices;
            List<Vector3> upperBezelsVertices = cubeRock.upperVerticalBezelsVertices;

            // List<Vector3> frontPlaneList = new List<Vector3> { bottomBezelsVertices[0], bottomBezelsVertices[1], upperBezelsVertices[0], upperBezelsVertices[1] };
            List<Vector3> frontPlaneList = new List<Vector3> { upperBezelsVertices[0], upperBezelsVertices[1], bottomBezelsVertices[1], bottomBezelsVertices[0] };
            List<Vector3> leftPlaneList = new List<Vector3> { upperBezelsVertices[2], upperBezelsVertices[3], bottomBezelsVertices[3], bottomBezelsVertices[2] };
            List<Vector3> backPlaneList = new List<Vector3> { upperBezelsVertices[4], upperBezelsVertices[5], bottomBezelsVertices[5], bottomBezelsVertices[4] };
            List<Vector3> rightPlaneList = new List<Vector3> { upperBezelsVertices[6], upperBezelsVertices[7], bottomBezelsVertices[7], bottomBezelsVertices[6] };

            DrawDividedLines(frontPlaneList, cubeRock.divider);
            DrawDividedLines(leftPlaneList, cubeRock.divider);
            DrawDividedLines(backPlaneList, cubeRock.divider);
            DrawDividedLines(rightPlaneList, cubeRock.divider);
            DrawDividedLines(cubeRock.bottomPlaneVertices, cubeRock.divider);
            DrawDividedLines(cubeRock.upperPlaneVertices, cubeRock.divider);
            // DrawDividedLines(cubeRock.upperPlaneVertices, cubeRock.divider);
            // DrawDividedLines(cubeRock.upperPlaneVertices, cubeRock.divider);
            // DrawDividedLines(cubeRock.upperPlaneVertices, cubeRock.divider);
            // DrawDividedLines(cubeRock.upperPlaneVertices, cubeRock.divider);

            int verticalBezelLoopCount = 0;
            int bottomLoopCount = 1;
            int linkLoopCount = 0;

            foreach (var vertex in cubeRock.bottomPlaneVertices)
            {
                Vector3 vertexFrom = vertex;
                Vector3 vertexTo;
                Vector3 firstVerticalBezelTo = cubeRock.bottomVerticalBezelsVertices[verticalBezelLoopCount];
                Vector3 secondVerticalBezelTo;

                if (verticalBezelLoopCount == 0)
                {
                    secondVerticalBezelTo = cubeRock.bottomVerticalBezelsVertices[7];
                }
                else
                {
                    secondVerticalBezelTo = cubeRock.bottomVerticalBezelsVertices[verticalBezelLoopCount - 1];
                }

                if (bottomLoopCount == 4)
                {
                    vertexTo = cubeRock.bottomPlaneVertices[0];
                }
                else
                {
                    vertexTo = cubeRock.bottomPlaneVertices[bottomLoopCount];
                }

                Gizmos.DrawLine(vertexFrom, vertexTo);
                Gizmos.DrawLine(vertexFrom, firstVerticalBezelTo);
                Gizmos.DrawLine(vertexFrom, secondVerticalBezelTo);

                bottomLoopCount++;
                verticalBezelLoopCount += 2;
            }

            verticalBezelLoopCount = 0;
            int upperLoopCount = 1;
            foreach (var vertex in cubeRock.upperPlaneVertices)
            {
                Vector3 vertexFrom = vertex;
                Vector3 vertexTo;
                Vector3 firstVerticalBezelTo = cubeRock.upperVerticalBezelsVertices[verticalBezelLoopCount];
                Vector3 secondVerticalBezelTo;

                if (verticalBezelLoopCount == 0)
                {
                    secondVerticalBezelTo = cubeRock.upperVerticalBezelsVertices[7];
                }
                else
                {
                    secondVerticalBezelTo = cubeRock.upperVerticalBezelsVertices[verticalBezelLoopCount - 1];
                }

                if (upperLoopCount == 4)
                {
                    vertexTo = cubeRock.upperPlaneVertices[0];
                }
                else
                {
                    vertexTo = cubeRock.upperPlaneVertices[upperLoopCount];
                }

                Gizmos.DrawLine(vertexFrom, vertexTo);
                Gizmos.DrawLine(vertexFrom, firstVerticalBezelTo);
                Gizmos.DrawLine(vertexFrom, secondVerticalBezelTo);

                upperLoopCount++;
                verticalBezelLoopCount += 2;
            }


            if (cubeRock.bevelSize == 0f)
            {
                bottomLoopCount = 1;
                linkLoopCount = 0;
                foreach (var vertex in cubeRock.bottomBezelsVertices)
                {
                    Vector3 vertexFrom = vertex;

                    Vector3 upperVertexTo = cubeRock.upperBezelsVertices[linkLoopCount];

                    Gizmos.DrawLine(vertexFrom, upperVertexTo);

                    linkLoopCount++;
                    bottomLoopCount++;
                }
            }

            verticalBezelLoopCount = 0;
            foreach (var vertex in cubeRock.upperVerticalBezelsVertices)
            {
                Vector3 vertexFrom = vertex;
                Vector3 vertexTo = cubeRock.bottomVerticalBezelsVertices[verticalBezelLoopCount];

                Gizmos.DrawLine(vertexFrom, vertexTo);

                verticalBezelLoopCount++;
            }

            verticalBezelLoopCount = 1;
            foreach (var vertex in cubeRock.upperVerticalBezelsVertices)
            {
                Vector3 vertexFrom = vertex;
                Vector3 vertexTo;

                if (verticalBezelLoopCount == 8)
                {
                    vertexTo = cubeRock.upperVerticalBezelsVertices[0];
                }
                else
                {
                    vertexTo = cubeRock.upperVerticalBezelsVertices[verticalBezelLoopCount];
                }


                Gizmos.DrawLine(vertexFrom, vertexTo);

                verticalBezelLoopCount++;
            }

            verticalBezelLoopCount = 1;
            foreach (var vertex in cubeRock.bottomVerticalBezelsVertices)
            {
                Vector3 vertexFrom = vertex;
                Vector3 vertexTo;

                if (verticalBezelLoopCount == 8)
                {
                    vertexTo = cubeRock.bottomVerticalBezelsVertices[0];
                }
                else
                {
                    vertexTo = cubeRock.bottomVerticalBezelsVertices[verticalBezelLoopCount];
                }

                Gizmos.DrawLine(vertexFrom, vertexTo);

                verticalBezelLoopCount++;
            }
        }

        public void DrawGizmo(CubeRock cubeRock)
        {

            DrawLines(cubeRock);

            // Draw black cubes on every vertex position of the cubeRock
            foreach (Vector3 spawnPosition in cubeRock.bottomPlaneVertices)
            {
                VisualizeVertex(spawnPosition, cubeRock);
            }

            foreach (Vector3 spawnPosition in cubeRock.bottomVerticalBezelsVertices)
            {
                VisualizeVertex(spawnPosition, cubeRock);
            }

            foreach (Vector3 spawnPosition in cubeRock.upperPlaneVertices)
            {
                VisualizeVertex(spawnPosition, cubeRock);
            }

            foreach (Vector3 spawnPosition in cubeRock.upperVerticalBezelsVertices)
            {
                VisualizeVertex(spawnPosition, cubeRock);
            }

            if (cubeRock.bevelSize == 0f)
            {
                foreach (Vector3 spawnPosition in cubeRock.bottomBezelsVertices)
                {
                    VisualizeVertex(spawnPosition, cubeRock);
                }

                foreach (Vector3 spawnPosition in cubeRock.upperBezelsVertices)
                {
                    VisualizeVertex(spawnPosition, cubeRock);
                }
            }
        }

        private void VisualizeVertex(Vector3 vertex, CubeRock cubeRock)
        {
            Gizmos.color = Color.black;
            float scaleModeModifier = 10f;
            float cubeSize = Mathf.Clamp(0.05f / scaleModeModifier, 0.025f, 0.3f);
            Gizmos.DrawCube(vertex, new Vector3(cubeSize, cubeSize, cubeSize));
            Gizmos.color = Color.blue;
        }

        private void DrawDividedLines(List<Vector3> vertexList, int divider)
        {
            float lerpFactor = 1f / divider;

            Vector3 lowerRightCorner = vertexList[0];
            Vector3 upperRightCorner = vertexList[1];
            Vector3 upperLeftCorner = vertexList[2];
            Vector3 lowerLeftCorner = vertexList[3];


            for (int loopCount = 1; loopCount < divider; loopCount++)
            {
                float finalLerpFactor = lerpFactor * loopCount;
                Vector3 verticalFrom = Vector3.Lerp(lowerLeftCorner, upperLeftCorner, finalLerpFactor);
                Vector3 verticalTo = Vector3.Lerp(lowerRightCorner, upperRightCorner, finalLerpFactor);
                Vector3 horizontalFrom = Vector3.Lerp(lowerLeftCorner, lowerRightCorner, finalLerpFactor);
                Vector3 horizontalTo = Vector3.Lerp(upperLeftCorner, upperRightCorner, finalLerpFactor);

                Gizmos.DrawLine(verticalFrom, verticalTo);
                Gizmos.DrawLine(horizontalFrom, horizontalTo);
            }
        }

    }
}
