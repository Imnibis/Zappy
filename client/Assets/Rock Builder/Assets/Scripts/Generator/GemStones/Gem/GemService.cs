using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RockBuilder
{
    ///-----------------------------------------------------------------
    ///   Namespace:      RockBuilder
    ///   Class:          CubeRockService
    ///   Description:    This Class handles the initialisation of gems.
    ///   Author:         Stefano Canonico                    
    ///   Date:           04.01.2020
    ///   Version:        1.0
    ///-----------------------------------------------------------------
    public class GemService
    {
        private static GemService instance = null;
        private static readonly object padlock = new object();

        GemService()
        {
        }

        public static GemService Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new GemService();
                    }
                    return instance;
                }
            }
        }

        public Gem CreateEmptyGem()
        {
            Gem gem = new GameObject().AddComponent(typeof(Gem)) as Gem;
            gem.width = 0.5f;
            gem.height = 1f;
            gem.depth = 0.5f;
            gem.edges = 8;
            gem.smoothFlag = false;
            gem.lodCount = 0;
            gem.colliderFlag = true;
            //Undo.RegisterCreatedObjectUndo(gemGenerator, "Created gem");
            gem.vertexPositions = GemMeshGenerator.Instance.CreateVertexPositions(gem);
            gem.transform.position = CalculateGemSpawnPosition(gem);
            SceneView.lastActiveSceneView.camera.transform.LookAt(gem.transform);
            FocusGem(gem);
            return gem;
        }

        public Gem CreateEmptyGem(string name)
        {
            Gem gem = CreateEmptyGem();
            gem.name = name;
            return gem;
        }

        public Gem CreateGem(Gem gem, Material material)
        {
            //Undo.RegisterCreatedObjectUndo(gemGenerator, "Created gem");
            SceneView.lastActiveSceneView.camera.transform.LookAt(gem.transform);
            FocusGem(gem);
            gem.vertexPositions = GemMeshGenerator.Instance.CreateVertexPositions(gem);
            gem.mesh = GemMeshGenerator.Instance.CreateMesh(gem);
            gem.GetComponent<MeshRenderer>().material = material;
            CreateLods(gem);
            CreateMeshCollider(gem);
            return gem;
        }

        public Gem GetGemFromSelection()
        {
            if (Selection.activeGameObject)
            {
                return Selection.activeGameObject.GetComponent<Gem>();
            }

            return null;
        }

        private void FocusGem(Gem gem)
        {
            Selection.activeGameObject = gem.gameObject;
            SceneView.lastActiveSceneView.FrameSelected();
        }

        private Vector3 CalculateGemSpawnPosition(Gem gem)
        {
            Transform cameraTransform = SceneView.lastActiveSceneView.camera.transform;
            return (cameraTransform.forward * (gem.width * 3f + gem.height * 2f)) + cameraTransform.position;
        }

        private void CreateMeshCollider(Gem gem)
        {
            gem.RemoveMeshCollider();
            if (gem.colliderFlag)
            {
                MeshCollider meshCollider = gem.gameObject.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = gem.mesh;
                meshCollider.convex = true;
            }
        }

        public void CreateLods(Gem gem)
        {
            if (gem.childrens != null)
            {
                gem.RemoveLOD();
            }

            int lodCounter = gem.lodCount;

            if (lodCounter != 0 && 6 <= gem.edges / gem.lodCount)
            {
                // Programmatically create a LOD group and add LOD levels.
                // Create a GUI that allows for forcing a specific LOD level.
                lodCounter += 1;
                LODGroup group = gem.gameObject.AddComponent<LODGroup>();
                Transform[] childrens = new Transform[lodCounter - 1];

                // Add 4 LOD levels
                LOD[] lods = new LOD[lodCounter];
                for (int i = 0; i < lodCounter; i++)
                {

                    Renderer[] renderers;
                    Gem childGem;

                    if (i != 0)
                    {
                        int edges = gem.edges / (i + 1);
                        if (edges % 2 != 0)
                        {
                            edges += 1;
                        }
                        childGem = new GameObject().AddComponent(typeof(Gem)) as Gem;
                        childGem.edges = edges;
                        childGem.height = gem.height;
                        childGem.width = gem.width;
                        childGem.depth = gem.depth;
                        childGem.smoothFlag = gem.smoothFlag;
                        childGem.vertexPositions = GemMeshGenerator.Instance.CreateVertexPositions(childGem);
                        childGem.mesh = GemMeshGenerator.Instance.CreateMesh(childGem);
                        childGem.name = gem.name + "_LOD_0" + i;
                        childGem.transform.parent = gem.transform;
                        childGem.transform.localPosition = Vector3.zero;
                        childGem.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
                        childGem.GetComponent<MeshRenderer>().material = gem.GetComponent<MeshRenderer>().sharedMaterial;
                        renderers = new Renderer[1];
                        renderers[0] = childGem.GetComponent<Renderer>();
                        childrens[i - 1] = childGem.transform;
                        childGem.RemoveGemClass();
                    }
                    else
                    {
                        renderers = new Renderer[1];
                        renderers[0] = gem.GetComponent<Renderer>();
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
                gem.childrens = childrens;
                group.SetLODs(lods);
                group.RecalculateBounds();
            }
        }
    }
}
