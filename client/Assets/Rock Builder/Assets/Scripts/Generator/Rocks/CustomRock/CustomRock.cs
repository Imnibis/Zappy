using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RockBuilder
{

    ///-----------------------------------------------------------------
    ///   Namespace:      RockBuilder
    ///   Class:          CubeRock
    ///   Description:    Model for the custom rock.
    ///   Author:         Stefano Canonico                    
    ///   Date:           04.01.2020
    ///   Version:        1.0
    ///-----------------------------------------------------------------
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class CustomRock : MonoBehaviour
    {
        [HideInInspector]
        public List<CustomRockListIteration> sortedVertices; 
        public List<Vector3> rockBuildPoints = new List<Vector3>();
        private int verticalIterations;
        private int verticesPerIteration;
        [HideInInspector]
        public int lodCount;
        [HideInInspector]
        public bool smoothFlag;
        [HideInInspector]
        public bool colliderFlag;
        [HideInInspector]
        public Transform[] childrens;
        public Mesh mesh
        {
            get
            {
                //Some other code
                return GetComponent<MeshFilter>().sharedMesh;
            }
            set
            {
                //Some other code
                GetComponent<MeshFilter>().sharedMesh = value;
            }
        }

        public void SetLODGroup(LODGroup lodGroup)
        {
            if (GetComponent<LODGroup>() == null)
            {
                LODGroup newLodGroup = gameObject.AddComponent(typeof(LODGroup)) as LODGroup;
                newLodGroup = lodGroup;
            }
            else
            {
                LODGroup newLodGroup = GetComponent<LODGroup>();
                newLodGroup = lodGroup;
            }
        }

        public void RemoveLOD()
        {
            if (GetComponent<LODGroup>() != null)
            {
                DestroyImmediate(GetComponent<LODGroup>());
            }

            foreach (Transform child in childrens)
            {
                if (child != null)
                {
                    DestroyImmediate(child.gameObject);
                }
            }
        }

        public void AddNewBuildPoint()
        {
            AddNewBuildPoint(Vector3.up);
        }

        public void AddNewBuildPoint(Vector3 position)
        {
            rockBuildPoints.Add(position);
        }

        public int GetVertexCount()
        {
            int vertexCount = 0;
            foreach (CustomRockListIteration iteration in sortedVertices)
            {
                vertexCount += iteration.GetVertexCount();
            }
            return vertexCount;
        }

        public void RemoveCustomRockClass()
        {
            DestroyImmediate(this.GetComponent<CustomRock>());
        }

        public void RemoveMeshCollider()
        {
            if (this.GetComponent<MeshCollider>())
            {
                DestroyImmediate(this.GetComponent<MeshCollider>());
            }
        }

        public void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;

            Gizmos.DrawSphere(transform.position, 0.5f);
        }
    }
}
