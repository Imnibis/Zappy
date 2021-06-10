using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RockBuilder
{
    ///-----------------------------------------------------------------
    ///   Namespace:      RockBuilder
    ///   Class:          CubeRockPreview
    ///   Description:    Draws the preview for the gem.
    ///   Author:         Stefano Canonico                    
    ///   Date:           04.01.2020
    ///   Version:        1.0
    ///-----------------------------------------------------------------
    public class GemPreview
    {
        private static GemPreview instance = null;
        private static readonly object padlock = new object();

        GemPreview()
        {
        }

        public static GemPreview Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new GemPreview();
                    }
                    return instance;
                }
            }
        }

        public void DrawLines(List<Vector3> spawnPoints, int edges, Gem gem)
        {
            Gizmos.color = Color.blue;
            Gizmos.matrix = gem.transform.localToWorldMatrix;

            for (int loopCount = 0; spawnPoints.Count > loopCount; loopCount++)
            {
                spawnPoints[loopCount] = spawnPoints[loopCount] - gem.transform.position;
            }

            int halfAmountOfEdges = edges / 2;
            int middleRingIndex = 1 + halfAmountOfEdges;
            int outerRingIndex = 1 + edges;
            int secondMiddleRingIndex = 1 + edges * 2;
            int secondInnerRingIndex = 1 + halfAmountOfEdges + edges * 2;

            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {
                int index = loopCount + 1;
                int nextIndex;
                if (halfAmountOfEdges - 1 == loopCount)
                {
                    nextIndex = 1;

                }
                else
                {
                    nextIndex = index + 1;
                }

                if (0 == loopCount)
                {
                    Gizmos.DrawLine(spawnPoints[index], spawnPoints[index + edges - 1]);
                }
                else
                {
                    Gizmos.DrawLine(spawnPoints[index], spawnPoints[index + halfAmountOfEdges - 1]);
                }

                Gizmos.DrawLine(spawnPoints[index], spawnPoints[nextIndex]);
                Gizmos.DrawLine(spawnPoints[index], spawnPoints[index + halfAmountOfEdges]);
            }

            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {
                int index = middleRingIndex + loopCount;
                int nextIndex = (outerRingIndex + loopCount * 2) + 1;

                Gizmos.DrawLine(spawnPoints[index], spawnPoints[nextIndex]);
                Gizmos.DrawLine(spawnPoints[index], spawnPoints[nextIndex - 1]);

                if (halfAmountOfEdges - 1 == loopCount)
                {
                    Gizmos.DrawLine(spawnPoints[index], spawnPoints[outerRingIndex]);
                }
                else
                {
                    Gizmos.DrawLine(spawnPoints[index], spawnPoints[nextIndex + 1]);
                }
            }

            for (int loopCount = 0; edges > loopCount; loopCount++)
            {
                int index = outerRingIndex + loopCount;
                int nextIndex = index + 1;

                if (edges - 1 == loopCount)
                {
                    Gizmos.DrawLine(spawnPoints[index], spawnPoints[outerRingIndex]);
                }
                else
                {
                    Gizmos.DrawLine(spawnPoints[index], spawnPoints[nextIndex]);
                }
            }

            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {
                int index = secondMiddleRingIndex + loopCount;
                int nextIndex = (outerRingIndex + loopCount * 2) + 1;

                Gizmos.DrawLine(spawnPoints[index], spawnPoints[nextIndex]);
                Gizmos.DrawLine(spawnPoints[index], spawnPoints[nextIndex - 1]);

                if (halfAmountOfEdges - 1 == loopCount)
                {
                    Gizmos.DrawLine(spawnPoints[index], spawnPoints[outerRingIndex]);
                }
                else
                {
                    Gizmos.DrawLine(spawnPoints[index], spawnPoints[nextIndex + 1]);
                }
            }

            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {
                int index = loopCount + secondInnerRingIndex;
                int nextIndex;
                if (halfAmountOfEdges - 1 == loopCount)
                {
                    nextIndex = secondInnerRingIndex;
                }
                else
                {
                    nextIndex = index + 1;
                }

                if (0 == loopCount)
                {
                    Gizmos.DrawLine(spawnPoints[index], spawnPoints[index - 1]);
                }
                else
                {
                    Gizmos.DrawLine(spawnPoints[index], spawnPoints[index - halfAmountOfEdges - 1]);
                }

                Gizmos.DrawLine(spawnPoints[index], spawnPoints[nextIndex]);
                Gizmos.DrawLine(spawnPoints[index], spawnPoints[index - halfAmountOfEdges]);
            }

        }

        public void DrawGizmo(Gem gem)
        {

            DrawLines(gem.vertexPositions, gem.edges, gem);

            // Draw black cubes on every vertex position of the gem
            foreach (Vector3 spawnPosition in gem.vertexPositions)
            {
                Gizmos.color = Color.black;
                float scaleModeModifier = 1f / (gem.width / 2);
                float cubeSize = Mathf.Clamp(0.05f / scaleModeModifier, 0.05f, 0.3f);
                Gizmos.DrawCube(spawnPosition, new Vector3(cubeSize, cubeSize, cubeSize));
                Gizmos.color = Color.blue;
            }

        }
    }
}
