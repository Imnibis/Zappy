using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using RockBuilder;

namespace RockBuilder
{

    [CustomEditor(typeof(Crystal))]
    public class GemHandles : Editor
    {

        Crystal crystal;

        void OnEnable()
        {
            crystal = (Crystal)target;
            //RockBuilderWindow.ShowWindow();
            //Debug.Log("Current Pipeline: " + RenderPipelineManager.currentPipeline);
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }

        void OnSceneGUI()
        {
            if (crystal.GetComponent<MeshFilter>().sharedMesh != null)
            {
                Handles.Label(crystal.transform.TransformPoint(new Vector3(0f, crystal.height + crystal.heightPeak, 0f)), "Vertices: " + crystal.GetComponent<MeshFilter>().sharedMesh.vertices.Length);
            }

            Handles.BeginGUI();

            GUILayout.BeginHorizontal();

            GUILayout.Space(50);

            if (GUILayout.Button("Edit crystal", GUILayout.Width(150)))
            {
                RockBuilderWindow.ShowWindow();
            }

            GUILayout.EndHorizontal();

            Handles.EndGUI();
        }
    }

    [CustomEditor(typeof(Diamond))]
    public class DiamondHandles : Editor
    {

        Diamond diamond;

        void OnEnable()
        {
            diamond = (Diamond)target;
            //RockBuilderWindow.ShowWindow();
            //Debug.Log("Current Pipeline: " + RenderPipelineManager.currentPipeline);
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }

        void OnSceneGUI()
        {
            if (diamond.GetComponent<MeshFilter>().sharedMesh != null)
            {
                Handles.Label(diamond.transform.TransformPoint(new Vector3(0f, diamond.pavillonHeight + diamond.crownHeight, 0f)), "Vertices: " + diamond.GetComponent<MeshFilter>().sharedMesh.vertices.Length);
            }

            Handles.BeginGUI();

            GUILayout.BeginHorizontal();

            GUILayout.Space(50);

            if (GUILayout.Button("Edit diamond", GUILayout.Width(150)))
            {
                RockBuilderWindow.ShowWindow();
            }

            GUILayout.EndHorizontal();

            Handles.EndGUI();
        }

        [CustomEditor(typeof(Gem))]
        public class GemHandles : Editor
        {

            Gem gem;

            void OnEnable()
            {
                gem = (Gem)target;
                //RockBuilderWindow.ShowWindow();
                //Debug.Log("Current Pipeline: " + RenderPipelineManager.currentPipeline);
            }

            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();
            }

            void OnSceneGUI()
            {
                if (gem.GetComponent<MeshFilter>().sharedMesh != null)
                {
                    Handles.Label(gem.transform.TransformPoint(new Vector3(0f, gem.height + gem.height, 0f)), "Vertices: " + gem.GetComponent<MeshFilter>().sharedMesh.vertices.Length);
                }

                Handles.BeginGUI();

                GUILayout.BeginHorizontal();

                GUILayout.Space(50);

                if (GUILayout.Button("Edit gem", GUILayout.Width(150)))
                {
                    RockBuilderWindow.ShowWindow();
                }

                GUILayout.EndHorizontal();

                Handles.EndGUI();
            }
        }
    }
}