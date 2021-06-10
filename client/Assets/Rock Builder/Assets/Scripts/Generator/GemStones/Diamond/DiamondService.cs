using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RockBuilder
{
    ///-----------------------------------------------------------------
    ///   Namespace:      RockBuilder
    ///   Class:          CubeRockService
    ///   Description:    This Class handles the initialisation of new diamonds.
    ///   Author:         Stefano Canonico                    
    ///   Date:           04.01.2020
    ///   Version:        1.0
    ///-----------------------------------------------------------------
    public class DiamondService
    {
        private static DiamondService instance = null;
        private static readonly object padlock = new object();

        DiamondService()
        {
        }

        public static DiamondService Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new DiamondService();
                    }
                    return instance;
                }
            }
        }

        public Diamond CreateEmptyDiamond()
        {
            Diamond diamond = new GameObject().AddComponent(typeof(Diamond)) as Diamond;
            diamond.radius = 1f;
            diamond.pavillonHeight = 1f;
            diamond.crownHeight = 0.33f;
            diamond.upperRadius = 1f;
            diamond.bottomRadiusPosition = 0.5f;
            diamond.edges = 8;
            diamond.smoothFlag = false;
            diamond.lodCount = 0;
            diamond.colliderFlag = true;
            //Undo.RegisterCreatedObjectUndo(diamondGenerator, "Created diamond");
            diamond.vertexPositions = DiamondMeshGenerator.Instance.CreateVertexPositions(diamond);
            diamond.transform.position = CalculateDiamondSpawnPosition(diamond);
            SceneView.lastActiveSceneView.camera.transform.LookAt(diamond.transform);
            FocusDiamond(diamond);
            return diamond;
        }

        public Diamond CreateEmptyDiamond(string name)
        {
            Diamond diamond = CreateEmptyDiamond();
            diamond.name = name;
            return diamond;
        }

        public Diamond CreateDiamond(Diamond diamond, Material material)
        {
            //Undo.RegisterCreatedObjectUndo(diamondGenerator, "Created diamond");
            SceneView.lastActiveSceneView.camera.transform.LookAt(diamond.transform);
            FocusDiamond(diamond);
            diamond.vertexPositions = DiamondMeshGenerator.Instance.CreateVertexPositions(diamond);
            diamond.mesh = DiamondMeshGenerator.Instance.CreateMesh(diamond);
            diamond.GetComponent<MeshRenderer>().material = material;
            CreateLods(diamond);
            CreateMeshCollider(diamond);
            return diamond;
        }

        public Diamond GetDiamondFromSelection()
        {
            if (Selection.activeGameObject)
            {
                return Selection.activeGameObject.GetComponent<Diamond>();
            }

            return null;
        }

        private void FocusDiamond(Diamond diamond)
        {
            Selection.activeGameObject = diamond.gameObject;
            SceneView.lastActiveSceneView.FrameSelected();
        }

        private Vector3 CalculateDiamondSpawnPosition(Diamond diamond)
        {
            Transform cameraTransform = SceneView.lastActiveSceneView.camera.transform;
            return (cameraTransform.forward * (diamond.radius * 3f + diamond.pavillonHeight * 2f)) + cameraTransform.position;
        }

        private void CreateMeshCollider(Diamond diamond)
        {
            diamond.RemoveMeshCollider();
            if (diamond.colliderFlag)
            {
                MeshCollider meshCollider = diamond.gameObject.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = diamond.mesh;
                meshCollider.convex = true;
            }
        }

        public void CreateLods(Diamond diamond)
        {
            if (diamond.childrens != null)
            {
                diamond.RemoveLOD();
            }

            int lodCounter = diamond.lodCount;

            if (lodCounter != 0 && 6 <= diamond.edges / diamond.lodCount)
            {
                // Programmatically create a LOD group and add LOD levels.
                // Create a GUI that allows for forcing a specific LOD level.
                lodCounter += 1;
                LODGroup group = diamond.gameObject.AddComponent<LODGroup>();
                Transform[] childrens = new Transform[lodCounter - 1];

                // Add 4 LOD levels
                LOD[] lods = new LOD[lodCounter];
                for (int i = 0; i < lodCounter; i++)
                {

                    Renderer[] renderers;
                    Diamond childDiamond;

                    if (i != 0)
                    {
                        int edges = diamond.edges / (i + 1);
                        if (edges % 2 != 0)
                        {
                            edges += 1;
                        }

                        childDiamond = new GameObject().AddComponent(typeof(Diamond)) as Diamond;
                        childDiamond.edges = edges;
                        childDiamond.radius = diamond.radius;
                        childDiamond.bottomRadiusPosition = diamond.bottomRadiusPosition;
                        childDiamond.pavillonHeight = diamond.pavillonHeight;
                        childDiamond.crownHeight = diamond.crownHeight;
                        childDiamond.smoothFlag = diamond.smoothFlag;
                        childDiamond.vertexPositions = DiamondMeshGenerator.Instance.CreateVertexPositions(childDiamond);
                        childDiamond.mesh = DiamondMeshGenerator.Instance.CreateMesh(childDiamond);
                        childDiamond.name = diamond.name + "_LOD_0" + i;
                        childDiamond.transform.parent = diamond.transform;
                        childDiamond.transform.localPosition = Vector3.zero;
                        childDiamond.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
                        childDiamond.GetComponent<MeshRenderer>().material = diamond.GetComponent<MeshRenderer>().sharedMaterial;
                        renderers = new Renderer[1];
                        renderers[0] = childDiamond.GetComponent<Renderer>();
                        childrens[i - 1] = childDiamond.transform;
                        childDiamond.RemoveDiamondClass();
                    }
                    else
                    {
                        renderers = new Renderer[1];
                        renderers[0] = diamond.GetComponent<Renderer>();
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
                diamond.childrens = childrens;
                group.SetLODs(lods);
                group.RecalculateBounds();
            }
        }
    }
}
