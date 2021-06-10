using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using RockBuilder;

namespace RockBuilder.Tests
{
    public class DiamondMeshGeneratorTest
    {
        [Test]
        public void CreateVertexPositions_Test()
        {
            //ARRANGE
            TestUtilities util = TestUtilities.Instance;
            Diamond diamond = DiamondService.Instance.CreateEmptyDiamond();
            diamond.edges = 6;

            //ACT
            List<Vector3> vertexPositions = DiamondMeshGenerator.Instance.CreateVertexPositions(diamond);

            //ASSERT
            Assert.IsNotNull(diamond.gameObject.GetComponent<MeshRenderer>(),
            util.PrintMessageObjectIsNull("Vertex list"));

            int expectedCount = 17;
            Assert.AreEqual(expectedCount, vertexPositions.Count, util.PrintMessageDoesNotHaveCount("Vertex list", expectedCount));
        }

        [Test]
        public void CreateSmoothMesh_Test()
        {
            //ARRANGE
            TestUtilities util = TestUtilities.Instance;
            Diamond diamond = DiamondService.Instance.CreateEmptyDiamond();
            diamond.edges = 6;
            diamond.crownHeight = 1f;
            diamond.pavillonHeight = 1f;
            diamond.vertexPositions = DiamondMeshGenerator.Instance.CreateVertexPositions(diamond);
            diamond.smoothFlag = true;

            //ACT
            Mesh mesh = DiamondMeshGenerator.Instance.CreateMesh(diamond);

            //ASSERT
            Assert.IsNotNull(mesh,
            util.PrintMessageObjectIsNull("Sphere Rock Mesh"));

            int expectedVerticesCount = 20;
            int expectedUvCount = 20;
            int expectedTrianglesCount = 72;
            Assert.AreEqual(expectedVerticesCount, mesh.vertices.Length, util.PrintMessageDoesNotHaveCount("Vertex list", expectedVerticesCount));
            Assert.AreEqual(expectedUvCount, mesh.uv.Length, util.PrintMessageDoesNotHaveCount("UV list", expectedUvCount));
            Assert.AreEqual(expectedTrianglesCount, mesh.triangles.Length, util.PrintMessageDoesNotHaveCount("Triangle list", expectedTrianglesCount));
        }

        [Test]
        public void CreateHardMesh_Test()
        {
            //ARRANGE
            TestUtilities util = TestUtilities.Instance;
            Diamond diamond = DiamondService.Instance.CreateEmptyDiamond();
            diamond.edges = 6;
            diamond.crownHeight = 1f;
            diamond.pavillonHeight = 1f;
            diamond.vertexPositions = DiamondMeshGenerator.Instance.CreateVertexPositions(diamond);

            //ACT
            Mesh mesh = DiamondMeshGenerator.Instance.CreateMesh(diamond);

            //ASSERT
            Assert.IsNotNull(mesh,
            util.PrintMessageObjectIsNull("Sphere Rock Mesh"));

            int expectedVerticesCount = 78;
            int expectedUvCount = 78;
            int expectedTrianglesCount = 90;
            Assert.AreEqual(expectedVerticesCount, mesh.vertices.Length, util.PrintMessageDoesNotHaveCount("Vertex list", expectedVerticesCount));
            Assert.AreEqual(expectedUvCount, mesh.uv.Length, util.PrintMessageDoesNotHaveCount("UV list", expectedUvCount));
            Assert.AreEqual(expectedTrianglesCount, mesh.triangles.Length, util.PrintMessageDoesNotHaveCount("Triangle list", expectedTrianglesCount));
        }
    }
}
