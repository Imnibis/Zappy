using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using RockBuilder;

namespace RockBuilder
{

    [CustomEditor(typeof(SphereRock))]
    public class SphereRockHandles : Editor
    {

        SphereRock sphereRock;

        void OnEnable()
        {
            sphereRock = (SphereRock)target;
            //RockBuilderWindow.ShowWindow();
            //Debug.Log("Current Pipeline: " + RenderPipelineManager.currentPipeline);
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }

        void OnSceneGUI()
        {
            if (sphereRock.GetComponent<MeshFilter>().sharedMesh != null)
            {
                Handles.Label(sphereRock.transform.TransformPoint(new Vector3(0f, sphereRock.height + 2, 0f)), "Vertices: " + sphereRock.GetComponent<MeshFilter>().sharedMesh.vertices.Length);
            }

            Handles.BeginGUI();

            GUILayout.BeginHorizontal();

            GUILayout.Space(50);

            if (GUILayout.Button("Edit sphere rock", GUILayout.Width(150)))
            {
                RockBuilderWindow.ShowWindow();
            }

            GUILayout.EndHorizontal();

            Handles.EndGUI();
        }
    }

    [CustomEditor(typeof(CubeRock))]
    public class CubeRockandles : Editor
    {

        CubeRock cubeRock;

        void OnEnable()
        {
            cubeRock = (CubeRock)target;
            //RockBuilderWindow.ShowWindow();
            //Debug.Log("Current Pipeline: " + RenderPipelineManager.currentPipeline);
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }

        void OnSceneGUI()
        {
            if (cubeRock.GetComponent<MeshFilter>().sharedMesh != null)
            {
                Handles.Label(cubeRock.transform.TransformPoint(new Vector3(0f, cubeRock.height + 2, 0f)), "Vertices: " + cubeRock.GetComponent<MeshFilter>().sharedMesh.vertices.Length);
            }

            Handles.BeginGUI();

            GUILayout.BeginHorizontal();

            GUILayout.Space(50);

            if (GUILayout.Button("Edit squared rock", GUILayout.Width(150)))
            {
                RockBuilderWindow.ShowWindow();
            }

            GUILayout.EndHorizontal();

            Handles.EndGUI();
        }
    }
}
