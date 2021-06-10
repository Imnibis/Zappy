using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RockBuilder
{
    ///-----------------------------------------------------------------
    ///   Namespace:      RockBuilder
    ///   Class:          CubeRockPreview
    ///   Description:    Draws the preview for the diamond.
    ///   Author:         Stefano Canonico                    
    ///   Date:           04.01.2020
    ///   Version:        1.0
    ///-----------------------------------------------------------------
    public class DiamondPreview
    {
        private static DiamondPreview instance = null;
        private static readonly object padlock = new object();

        DiamondPreview()
        {
        }

        public static DiamondPreview Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new DiamondPreview();
                    }
                    return instance;
                }
            }
        }

        public void DrawLines(List<Vector3> spawnPoints, int edges, Diamond diamond)
        {
            Gizmos.color = Color.blue;
            Gizmos.matrix = diamond.transform.localToWorldMatrix;

            for (int loopCount = 0; spawnPoints.Count > loopCount; loopCount++)
            {
                spawnPoints[loopCount] = spawnPoints[loopCount] - diamond.transform.position;
            }

            for (int loopCount = 0; edges > loopCount; loopCount++)
            {

                if (loopCount == edges - 1)
                {
                    //Draw line from the bottom peak to the upper pavillon vertices 
                    if (loopCount % 2 == 0)
                    {
                        Gizmos.DrawLine(spawnPoints[0], spawnPoints[1 + loopCount + edges / 2]);
                    }

                    // Draw the upper pavillon circumference 
                    Gizmos.DrawLine(spawnPoints[(edges / 2) + 1 + loopCount], spawnPoints[(edges / 2) + 1]);
                }
                else
                {
                    //Draw line from the bottom peak to the upper pavillon vertices     
                    if (loopCount % 2 == 0)
                    {
                        Gizmos.DrawLine(spawnPoints[0], spawnPoints[1 + loopCount + edges / 2]);
                    }
                    // Draw the upper pavillon circumference 
                    Gizmos.DrawLine(spawnPoints[(edges / 2) + 1 + loopCount], spawnPoints[(edges / 2) + 2 + loopCount]);
                }


                if (loopCount < edges / 2)
                {
                    int index;
                    if (loopCount == 0)
                    {
                        index = (edges / 2) + edges;
                        // Draw from the lower pavillon vertices to the upper vertices
                        Gizmos.DrawLine(spawnPoints[(edges / 2) + 2], spawnPoints[1 + loopCount]);
                        Gizmos.DrawLine(spawnPoints[index], spawnPoints[1 + loopCount]);

                        // Draw from the pavillon vertices to the lower crown vertices
                        Gizmos.DrawLine(spawnPoints[index + 1], spawnPoints[(edges / 2) + edges]);
                        Gizmos.DrawLine(spawnPoints[index + 1], spawnPoints[(edges / 2) + 1]);
                        Gizmos.DrawLine(spawnPoints[index + 1], spawnPoints[(edges / 2) + 2]);

                        // Draw from the lower crown vertices to the upper crown vertices
                        Gizmos.DrawLine(spawnPoints[index + 1], spawnPoints[(edges / 2) + index + 1]);
                        Gizmos.DrawLine(spawnPoints[index + 1], spawnPoints[(edges) + index]);

                        // Draw the first line of the top plane of the diamond
                        Gizmos.DrawLine(spawnPoints[(edges / 2) + index + 1], spawnPoints[(edges) + index]);
                    }
                    else
                    {
                        index = (edges / 2) + (loopCount * 2);
                        // Draw from the lower pavillon vertices to the upper vertices
                        Gizmos.DrawLine(spawnPoints[index], spawnPoints[1 + loopCount]);
                        Gizmos.DrawLine(spawnPoints[index + 2], spawnPoints[1 + loopCount]);

                        index = (edges / 2) + edges + loopCount + 1;
                        // Draw from the pavillon vertices to the lower crown vertices
                        // Always connect one crown vertex to three pavillon vertices
                        Gizmos.DrawLine(spawnPoints[index], spawnPoints[(edges / 2) + loopCount * 2]);
                        Gizmos.DrawLine(spawnPoints[index], spawnPoints[(edges / 2) + 1 + loopCount * 2]);
                        Gizmos.DrawLine(spawnPoints[index], spawnPoints[(edges / 2) + 2 + loopCount * 2]);

                        // Draw from the lower crown vertices to the upper crown vertices
                        // Always connect one lower crown vertex to two upper crown vertices
                        Gizmos.DrawLine(spawnPoints[index], spawnPoints[(edges / 2) + index]);
                        Gizmos.DrawLine(spawnPoints[index], spawnPoints[(edges / 2) + index - 1]);

                        index = edges * 2 + loopCount;
                        // Draw the top plane of the diamond
                        Gizmos.DrawLine(spawnPoints[index], spawnPoints[index + 1]);
                    }
                }
            }
        }

        public void DrawGizmo(Diamond diamond)
        {

            DrawLines(diamond.vertexPositions, diamond.edges, diamond);

            // Draw black cubes on every vertex position of the diamond
            foreach (Vector3 spawnPosition in diamond.vertexPositions)
            {
                Gizmos.color = Color.black;
                float scaleModeModifier = 1f / (diamond.radius);
                float cubeSize = Mathf.Clamp(0.05f / scaleModeModifier, 0.05f, 0.3f);
                Gizmos.DrawCube(spawnPosition, new Vector3(cubeSize, cubeSize, cubeSize));
                Gizmos.color = Color.blue;
            }

        }
    }
}