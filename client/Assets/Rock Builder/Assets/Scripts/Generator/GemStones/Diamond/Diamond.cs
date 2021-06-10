using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RockBuilder
{
    ///-----------------------------------------------------------------
    ///   Namespace:      RockBuilder
    ///   Class:          CubeRock
    ///   Description:    Model for the diamond.
    ///   Author:         Stefano Canonico                    
    ///   Date:           04.01.2020
    ///   Version:        1.0
    ///-----------------------------------------------------------------
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class Diamond : MonoBehaviour
    {
        [HideInInspector]
        public float radius;
        [HideInInspector]
        public float upperRadius;
        [HideInInspector]
        public float bottomRadiusPosition;
        [HideInInspector]
        public float pavillonHeight;
        [HideInInspector]
        public float crownHeight;
        [HideInInspector]
        public int edges;
        [HideInInspector]
        public bool smoothFlag;
        [HideInInspector]
        public bool colliderFlag;
        [HideInInspector]
        public int lodCount;
        [HideInInspector]
        public Transform[] childrens;
        [HideInInspector]
        public List<Vector3> vertexPositions;

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
                vertexPositions = DiamondMeshGenerator.Instance.CreateVertexPositions(this);
                DiamondPreview.Instance.DrawGizmo(this);
            }
        }

        public void RemoveDiamondClass()
        {
            DestroyImmediate(this.GetComponent<Diamond>());
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
