using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RockBuilder
{
    ///-----------------------------------------------------------------
    ///   Namespace:      RockBuilder
    ///   Class:          SphereRock
    ///   Description:    Model for the sphere rock.
    ///   Author:         Stefano Canonico                    
    ///   Date:           04.01.2020
    ///   Version:        1.0
    ///-----------------------------------------------------------------
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class SphereRock : MonoBehaviour
    {
        [HideInInspector]
        public float height;
        [HideInInspector]
        public float width;
        [HideInInspector]
        public float depth;
        [HideInInspector]
        public float noise;
        [HideInInspector]
        public int edges;
        [HideInInspector]
        public int lodCount;
        [HideInInspector]
        public bool smoothFlag;
        [HideInInspector]
        public bool colliderFlag;
        [HideInInspector]
        public List<Vector3> vertexPositions;
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

        private void OnDrawGizmosSelected()
        {
            if (vertexPositions != null)
            {
                // update vertex positions
                vertexPositions = SphereRockMeshGenerator.Instance.CreateVertexPositions(this);
                SphereRockPreview.Instance.DrawGizmo(this);
            }
        }

        public List<List<Vector3>> GetOrderedVertexList()
        {

            List<List<Vector3>> orderedVertexList = new List<List<Vector3>>();
            for (int rowCount = 0; rowCount < edges; rowCount++)
            {
                List<Vector3> iteration = new List<Vector3>();
                for (int vertexCount = 0; vertexCount < edges; vertexCount++)
                {
                    int index = (edges * rowCount) + vertexCount;
                    iteration.Add(vertexPositions[index]);
                }
                orderedVertexList.Add(iteration);
            }
            return orderedVertexList;
        }

        public void RemoveSphereRockClass()
        {
            DestroyImmediate(this.GetComponent<SphereRock>());
        }

        public void RemoveMeshCollider()
        {
            if (this.GetComponent<MeshCollider>())
            {
                DestroyImmediate(this.GetComponent<MeshCollider>());
            }
        }
    }
}
