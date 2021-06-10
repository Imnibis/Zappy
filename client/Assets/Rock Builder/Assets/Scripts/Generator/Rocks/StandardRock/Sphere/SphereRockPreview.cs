using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RockBuilder
{
    ///-----------------------------------------------------------------
    ///   Namespace:      RockBuilder
    ///   Class:          SphereRockPreview
    ///   Description:    Draws the preview for the sphere rock.
    ///   Author:         Stefano Canonico                    
    ///   Date:           04.01.2020
    ///   Version:        1.0
    ///-----------------------------------------------------------------
    public class SphereRockPreview
    {
        private static SphereRockPreview instance = null;
        private static readonly object padlock = new object();

        SphereRockPreview()
        {
        }

        public static SphereRockPreview Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new SphereRockPreview();
                    }
                    return instance;
                }
            }
        }

        public void DrawLines(SphereRock sphereRock)
        {
            Gizmos.color = Color.blue;

            Vector3 vertexFrom = Vector3.zero;

            List<List<Vector3>> orderedList = sphereRock.GetOrderedVertexList();

            foreach (List<Vector3> iteration in orderedList)
            {
                int vertexCount = 1;
                bool skipFirstVertex = true;
                Vector3 firstVertex = iteration[0];
                foreach (Vector3 vertex in iteration)
                {
                    if (!skipFirstVertex)
                    {
                        Gizmos.DrawLine(vertexFrom, vertex);
                        if (vertexCount == iteration.Count)
                        {
                            Gizmos.DrawLine(vertex, firstVertex);
                        }
                    }

                    vertexFrom = vertex;
                    skipFirstVertex = false;
                    vertexCount++;
                }
            }

            for (int loopCount = 0; loopCount < orderedList.Count; loopCount++)
            {

                for (int innerLoopCount = 0; innerLoopCount < orderedList[loopCount].Count; innerLoopCount++)
                {
                    Vector3 verticalVertexFrom = orderedList[innerLoopCount][loopCount];
                    Vector3 verticalVertexTo;

                    if (orderedList.Count - 1 != innerLoopCount)
                    {
                        verticalVertexTo = orderedList[innerLoopCount + 1][loopCount];
                    }
                    else
                    {
                        verticalVertexTo = orderedList[0][loopCount];
                    }

                    Gizmos.DrawLine(verticalVertexFrom, verticalVertexTo);

                }
            }
        }

        public void DrawGizmo(SphereRock sphereRock)
        {
            Gizmos.matrix = sphereRock.transform.localToWorldMatrix;

            DrawLines(sphereRock);

            // Draw black cubes on every vertex position of the gem
            foreach (List<Vector3> iteration in sphereRock.GetOrderedVertexList())
            {
                foreach (Vector3 spawnPosition in iteration)
                {
                    Gizmos.color = Color.black;
                    float scaleModeModifier = 1f / (sphereRock.width / 2);
                    float cubeSize = Mathf.Clamp(0.05f / scaleModeModifier, 0.05f, 0.3f);
                    Gizmos.DrawCube(spawnPosition, new Vector3(cubeSize, cubeSize, cubeSize));
                    Gizmos.color = Color.blue;
                }
            }

        }
    }
}
