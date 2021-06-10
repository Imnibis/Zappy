using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RockBuilder
{
    ///-----------------------------------------------------------------
    ///   Namespace:      RockBuilder
    ///   Class:          CubeRockService
    ///   Description:    This Class handles the initialisation of new cube rocks.
    ///   Author:         Stefano Canonico                    
    ///   Date:           04.01.2020
    ///   Version:        1.0
    ///-----------------------------------------------------------------
    public class CubeRockService
    {
        private static CubeRockService instance = null;
        private static readonly object padlock = new object();

        CubeRockService()
        {
        }

        public static CubeRockService Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new CubeRockService();
                    }
                    return instance;
                }
            }
        }

        public CubeRock CreateEmptyCubeRock()
        {
            CubeRock cubeRock = new GameObject().AddComponent(typeof(CubeRock)) as CubeRock;
            cubeRock.smoothFlag = false;
            cubeRock.lodCount = 0;
            cubeRock.colliderFlag = true;
            //Undo.RegisterCreatedObjectUndo(diamondGenerator, "Created diamond");
            cubeRock = CubeRockMeshGenerator.Instance.CreateVertexPositions(cubeRock);
            cubeRock.transform.position = CalculateCubeRockSpawnPosition();
            SceneView.lastActiveSceneView.camera.transform.LookAt(cubeRock.transform);
            FocusCubeRock(cubeRock);
            return cubeRock;
        }

        public CubeRock CreateEmptyCubeRock(string name)
        {
            CubeRock cubeRock = CreateEmptyCubeRock();
            cubeRock.name = name;
            return cubeRock;
        }

        public CubeRock CreateCubeRock(CubeRock cubeRock, Material material)
        {
            //Undo.RegisterCreatedObjectUndo(diamondGenerator, "Created diamond");
            SceneView.lastActiveSceneView.camera.transform.LookAt(cubeRock.transform);
            FocusCubeRock(cubeRock);
            cubeRock.mesh = CubeRockMeshGenerator.Instance.CreateRockMesh(cubeRock);
            cubeRock.GetComponent<MeshRenderer>().material = material;
            CreateLods(cubeRock);
            CreateMeshCollider(cubeRock);
            return cubeRock;
        }

        public CubeRock GetCubeRockFromSelection()
        {
            if (Selection.activeGameObject)
            {
                return Selection.activeGameObject.GetComponent<CubeRock>();
            }

            return null;
        }

        private void FocusCubeRock(CubeRock cubeRock)
        {
            Selection.activeGameObject = cubeRock.gameObject;
            SceneView.lastActiveSceneView.FrameSelected();
        }

        private Vector3 CalculateCubeRockSpawnPosition()
        {
            Transform cameraTransform = SceneView.lastActiveSceneView.camera.transform;
            return (cameraTransform.forward * (3f * 2f)) + cameraTransform.position;
        }

        private void CreateMeshCollider(CubeRock cubeRock)
        {
            cubeRock.RemoveMeshCollider();
            if (cubeRock.colliderFlag)
            {
                MeshCollider meshCollider = cubeRock.gameObject.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = cubeRock.mesh;
                meshCollider.convex = true;
            }
        }

        public void CreateLods(CubeRock cubeRock)
        {
            if (cubeRock.childrens != null)
            {
                cubeRock.RemoveLOD();
            }

            int lodCounter = cubeRock.lodCount;

            if (lodCounter != 0 && 2 <= cubeRock.divider / cubeRock.lodCount)
            {
                // Programmatically create a LOD group and add LOD levels.
                // Create a GUI that allows for forcing a specific LOD level.
                lodCounter += 1;
                LODGroup group = cubeRock.gameObject.AddComponent<LODGroup>();
                Transform[] childrens = new Transform[lodCounter - 1];

                // Add 4 LOD levels
                LOD[] lods = new LOD[lodCounter];
                for (int i = 0; i < lodCounter; i++)
                {

                    Renderer[] renderers;
                    CubeRock childCubeRock;

                    if (i != 0)
                    {
                        childCubeRock = new GameObject().AddComponent(typeof(CubeRock)) as CubeRock;
                        childCubeRock.divider = cubeRock.divider / (i + 1);
                        childCubeRock.width = cubeRock.width;
                        childCubeRock.height = cubeRock.height;
                        childCubeRock.depth = cubeRock.depth;
                        childCubeRock.noise = cubeRock.noise;
                        childCubeRock.smoothFlag = cubeRock.smoothFlag;
                        childCubeRock = CubeRockMeshGenerator.Instance.CreateVertexPositions(childCubeRock);
                        childCubeRock.mesh = CubeRockMeshGenerator.Instance.CreateRockMesh(childCubeRock);
                        childCubeRock.name = cubeRock.name + "_LOD_0" + i;
                        childCubeRock.transform.parent = cubeRock.transform;
                        childCubeRock.transform.localPosition = Vector3.zero;
                        childCubeRock.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
                        childCubeRock.GetComponent<MeshRenderer>().material = cubeRock.GetComponent<MeshRenderer>().sharedMaterial;
                        renderers = new Renderer[1];
                        renderers[0] = childCubeRock.GetComponent<Renderer>();
                        childrens[i - 1] = childCubeRock.transform;
                        childCubeRock.RemoveCubeRockClass();
                    }
                    else
                    {
                        renderers = new Renderer[1];
                        renderers[0] = cubeRock.GetComponent<Renderer>();
                    }

                    if (i != lodCounter - 1)
                    {
                        lods[i] = new LOD((1f / lodCounter) * (lodCounter - i - 1) / 2, renderers);
                    }
                    else
                    {
                        lods[i] = new LOD(0f, renderers);
                    }

                }
                cubeRock.childrens = childrens;
                group.SetLODs(lods);
                group.RecalculateBounds();
            }
        }
    }
}
