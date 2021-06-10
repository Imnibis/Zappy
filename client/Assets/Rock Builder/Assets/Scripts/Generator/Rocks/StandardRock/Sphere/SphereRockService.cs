using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RockBuilder
{
    ///-----------------------------------------------------------------
    ///   Namespace:      RockBuilder
    ///   Class:          CubeRockService
    ///   Description:    This Class handles the initialisation of new sphere rocks.
    ///   Author:         Stefano Canonico                    
    ///   Date:           04.01.2020
    ///   Version:        1.0
    ///-----------------------------------------------------------------
    public class SphereRockService
    {
        private static SphereRockService instance = null;
        private static readonly object padlock = new object();

        SphereRockService()
        {
        }

        public static SphereRockService Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new SphereRockService();
                    }
                    return instance;
                }
            }
        }

        public SphereRock CreateEmptySphereRock()
        {
            SphereRock sphereRock = new GameObject().AddComponent(typeof(SphereRock)) as SphereRock;
            sphereRock.smoothFlag = true;
            sphereRock.lodCount = 0;
            sphereRock.colliderFlag = true;
            //Undo.RegisterCreatedObjectUndo(diamondGenerator, "Created diamond");
            sphereRock.vertexPositions = SphereRockMeshGenerator.Instance.CreateVertexPositions(sphereRock);
            sphereRock.transform.position = CalculateSphereRockSpawnPosition();
            SceneView.lastActiveSceneView.camera.transform.LookAt(sphereRock.transform);
            FocusSphereRock(sphereRock);
            return sphereRock;
        }

        public SphereRock CreateEmptySphereRock(string name)
        {
            SphereRock sphereRock = CreateEmptySphereRock();
            sphereRock.name = name;
            return sphereRock;
        }

        public SphereRock CreateSphereRock(SphereRock sphereRock, Material material)
        {
            //Undo.RegisterCreatedObjectUndo(diamondGenerator, "Created diamond");
            SceneView.lastActiveSceneView.camera.transform.LookAt(sphereRock.transform);
            FocusSphereRock(sphereRock);

            sphereRock.mesh = SphereRockMeshGenerator.Instance.CreateRockMesh(sphereRock);
            sphereRock.GetComponent<MeshRenderer>().material = material;
            CreateLods(sphereRock);
            CreateMeshCollider(sphereRock);
            return sphereRock;
        }

        public SphereRock GetSphereRockFromSelection()
        {
            if (Selection.activeGameObject)
            {
                return Selection.activeGameObject.GetComponent<SphereRock>();
            }

            return null;
        }

        private void FocusSphereRock(SphereRock sphereRock)
        {
            Selection.activeGameObject = sphereRock.gameObject;
            SceneView.lastActiveSceneView.FrameSelected();
        }

        private Vector3 CalculateSphereRockSpawnPosition()
        {
            Transform cameraTransform = SceneView.lastActiveSceneView.camera.transform;
            return (cameraTransform.forward * (3f * 2f)) + cameraTransform.position;
        }

        private void CreateMeshCollider(SphereRock sphereRock)
        {
            sphereRock.RemoveMeshCollider();
            if (sphereRock.colliderFlag)
            {
                MeshCollider meshCollider = sphereRock.gameObject.AddComponent<MeshCollider>();
                Mesh meshData = sphereRock.mesh;
                List<Vector3> vertexIterations = sphereRock.vertexPositions;
                int edges = sphereRock.edges;
                if (sphereRock.edges > 12)
                {
                    sphereRock.edges = 12;
                    sphereRock.vertexPositions = SphereRockMeshGenerator.Instance.CreateVertexPositions(sphereRock);
                    meshData = SphereRockMeshGenerator.Instance.CreateRockMesh(sphereRock);
                }
                sphereRock.edges = edges;
                sphereRock.vertexPositions = vertexIterations;
                meshCollider.sharedMesh = meshData;
                meshCollider.convex = true;
            }
        }

        public void CreateLods(SphereRock sphereRock)
        {
            if (sphereRock.childrens != null)
            {
                sphereRock.RemoveLOD();
            }

            int lodCounter = sphereRock.lodCount;

            if (lodCounter != 0 && 6 <= sphereRock.edges / sphereRock.lodCount)
            {
                // Programmatically create a LOD group and add LOD levels.
                // Create a GUI that allows for forcing a specific LOD level.
                lodCounter += 1;
                LODGroup group = sphereRock.gameObject.AddComponent<LODGroup>();
                Transform[] childrens = new Transform[lodCounter - 1];

                // Add 4 LOD levels
                LOD[] lods = new LOD[lodCounter];
                for (int i = 0; i < lodCounter; i++)
                {

                    Renderer[] renderers;
                    SphereRock childSphereRock;

                    if (i != 0)
                    {
                        childSphereRock = new GameObject().AddComponent(typeof(SphereRock)) as SphereRock;
                        childSphereRock.edges = sphereRock.edges / (i + 1);
                        childSphereRock.width = sphereRock.width;
                        childSphereRock.height = sphereRock.height;
                        childSphereRock.depth = sphereRock.depth;
                        childSphereRock.smoothFlag = sphereRock.smoothFlag;
                        childSphereRock.vertexPositions = SphereRockMeshGenerator.Instance.CreateVertexPositions(childSphereRock);
                        childSphereRock.mesh = SphereRockMeshGenerator.Instance.CreateRockMesh(childSphereRock);
                        childSphereRock.name = sphereRock.name + "_LOD_0" + i;
                        childSphereRock.transform.parent = sphereRock.transform;
                        childSphereRock.transform.localPosition = Vector3.zero;
                        childSphereRock.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
                        childSphereRock.GetComponent<MeshRenderer>().material = sphereRock.GetComponent<MeshRenderer>().sharedMaterial;
                        renderers = new Renderer[1];
                        renderers[0] = childSphereRock.GetComponent<Renderer>();
                        childrens[i - 1] = childSphereRock.transform;
                        childSphereRock.RemoveSphereRockClass();
                    }
                    else
                    {
                        renderers = new Renderer[1];
                        renderers[0] = sphereRock.GetComponent<Renderer>();
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
                sphereRock.childrens = childrens;
                group.SetLODs(lods);
                group.RecalculateBounds();
            }
        }
    }
}
