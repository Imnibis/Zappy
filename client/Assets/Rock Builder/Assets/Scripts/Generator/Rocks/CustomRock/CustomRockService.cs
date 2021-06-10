using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RockBuilder
{
    ///-----------------------------------------------------------------
    ///   Namespace:      RockBuilder
    ///   Class:          CubeRockService
    ///   Description:    This Class handles the initialisation of new custom rocks.
    ///   Author:         Stefano Canonico                    
    ///   Date:           04.01.2020
    ///   Version:        1.0
    ///-----------------------------------------------------------------
    public class CustomRockService
    {
        private static CustomRockService instance = null;
        private static readonly object padlock = new object();

        CustomRockService()
        {
        }

        public static CustomRockService Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new CustomRockService();
                    }
                    return instance;
                }
            }
        }

        public CustomRock CreateEmptyCustomRock()
        {
            CustomRock customRock = new GameObject().AddComponent(typeof(CustomRock)) as CustomRock;
            customRock.smoothFlag = false;
            customRock.lodCount = 0;
            customRock.colliderFlag = true;
            //Undo.RegisterCreatedObjectUndo(diamondGenerator, "Created diamond");
            customRock.transform.position = CalculateCustomRockSpawnPosition();
            SceneView.lastActiveSceneView.camera.transform.LookAt(customRock.transform);
            FocusCustomRock(customRock);
            return customRock;
        }

        public CustomRock CreateEmptyCustomRock(string name)
        {
            CustomRock customRock = CreateEmptyCustomRock();
            customRock.name = name;
            return customRock;
        }

        public CustomRock CreateCustomRock(CustomRock customRock, Material material)
        {
            //Undo.RegisterCreatedObjectUndo(diamondGenerator, "Created diamond");
            SceneView.lastActiveSceneView.camera.transform.LookAt(customRock.transform);
            FocusCustomRock(customRock);

            customRock.mesh = CustomRockMeshGenerator.Instance.CreateRockMesh(customRock);
            customRock.GetComponent<MeshRenderer>().material = material;
            //CreateLods(customRock);
            CreateMeshCollider(customRock);
            return customRock;
        }

        public CustomRock GetCustomRockFromSelection()
        {
            if (Selection.activeGameObject)
            {
                return Selection.activeGameObject.GetComponent<CustomRock>();
            }

            return null;
        }

        private void FocusCustomRock(CustomRock customRock)
        {
            Selection.activeGameObject = customRock.gameObject;
            SceneView.lastActiveSceneView.FrameSelected();
        }

        private Vector3 CalculateCustomRockSpawnPosition()
        {
            Transform cameraTransform = SceneView.lastActiveSceneView.camera.transform;
            return (cameraTransform.forward * (3f * 2f)) + cameraTransform.position;
        }

        private void CreateMeshCollider(CustomRock customRock)
        {
            customRock.RemoveMeshCollider();
            if (customRock.colliderFlag)
            {
                MeshCollider meshCollider = customRock.gameObject.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = customRock.mesh;
                meshCollider.convex = true;
            }
        }

        // public void CreateLods(CustomRock customRock)
        // {
        //     if (customRock.childrens != null)
        //     {
        //         customRock.RemoveLOD();
        //     }

        //     int lodCounter = customRock.lodCount;

        //     if (lodCounter != 0 && 3 <= customRock.edges / customRock.lodCount)
        //     {
        //         // Programmatically create a LOD group and add LOD levels.
        //         // Create a GUI that allows for forcing a specific LOD level.
        //         lodCounter += 1;
        //         LODGroup group = customRock.gameObject.AddComponent<LODGroup>();
        //         Transform[] childrens = new Transform[lodCounter - 1];

        //         // Add 4 LOD levels
        //         LOD[] lods = new LOD[lodCounter];
        //         for (int i = 0; i < lodCounter; i++)
        //         {

        //             Renderer[] renderers;
        //             CustomRock childCustomRock;

        //             if (i != 0)
        //             {
        //                 childCustomRock = new GameObject().AddComponent(typeof(CustomRock)) as CustomRock;
        //                 childCustomRock.edges = customRock.edges / (i + 1);
        //                 childCustomRock.radius = customRock.radius;
        //                 childCustomRock.height = customRock.height;
        //                 childCustomRock.heightPeak = customRock.heightPeak;
        //                 childCustomRock.smoothFlag = customRock.smoothFlag;
        //                 childCustomRock.vertexPositions = CustomRockMeshGenerator.Instance.CreateVertexPositions(childCustomRock);
        //                 childCustomRock.mesh = CustomRockMeshGenerator.Instance.CreateMesh(childCustomRock);
        //                 childCustomRock.name = customRock.name + "_LOD_0" + i;
        //                 childCustomRock.transform.parent = customRock.transform;
        //                 childCustomRock.transform.localPosition = Vector3.zero;
        //                 childCustomRock.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
        //                 childCustomRock.GetComponent<MeshRenderer>().material = customRock.GetComponent<MeshRenderer>().sharedMaterial;
        //                 renderers = new Renderer[1];
        //                 renderers[0] = childCustomRock.GetComponent<Renderer>();
        //                 childrens[i - 1] = childCustomRock.transform;
        //                 childCustomRock.RemoveCustomRockClass();
        //             }
        //             else
        //             {
        //                 renderers = new Renderer[1];
        //                 renderers[0] = customRock.GetComponent<Renderer>();
        //             }

        //             if (i != lodCounter - 1)
        //             {
        //                 lods[i] = new LOD((1f / lodCounter) * (lodCounter - i - 1) / 2, renderers);
        //             }
        //             else
        //             {
        //                 lods[i] = new LOD(0f, renderers);
        //             }

        //         }
        //         customRock.childrens = childrens;
        //         group.SetLODs(lods);
        //         group.RecalculateBounds();
        //     }
        // }
    }
}
