using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using RockBuilder;

namespace RockBuilder.Tests
{
    public class GemMeshGeneratorTest
    {
        [Test]
        public void CreateVertexPositions_Test()
        {
            //ARRANGE
            TestUtilities util = TestUtilities.Instance;
            Gem gem = GemService.Instance.CreateEmptyGem();
            gem.edges = 6;

            //ACT
            List<Vector3> vertexPositions = GemMeshGenerator.Instance.CreateVertexPositions(gem);

            //ASSERT
            Assert.IsNotNull(gem.gameObject.GetComponent<MeshRenderer>(),
            util.PrintMessageObjectIsNull("Vertex list"));

            int expectedCount = 20;
            Assert.AreEqual(expectedCount, vertexPositions.Count, util.PrintMessageDoesNotHaveCount("Vertex list", expectedCount));
        }

        [Test]
        public void CreateSmoothMesh_Test()
        {
            //ARRANGE
            TestUtilities util = TestUtilities.Instance;
            Gem gem = GemService.Instance.CreateEmptyGem();
            gem.edges = 6;
            gem.width = 1f;
            gem.height = 1f;
            gem.depth = 1f;
            gem.smoothFlag = true;
            gem.vertexPositions = GemMeshGenerator.Instance.CreateVertexPositions(gem);

            //ACT
            Mesh mesh = GemMeshGenerator.Instance.CreateMesh(gem);

            //ASSERT
            Assert.IsNotNull(mesh,
            util.PrintMessageObjectIsNull("Sphere Rock Mesh"));

            int expectedVerticesCount = 26;
            int expectedUvCount = 26;
            int expectedTrianglesCount = 108;
            Assert.AreEqual(expectedVerticesCount, mesh.vertices.Length, util.PrintMessageDoesNotHaveCount("Vertex list", expectedVerticesCount));
            Assert.AreEqual(expectedUvCount, mesh.uv.Length, util.PrintMessageDoesNotHaveCount("UV list", expectedUvCount));
            Assert.AreEqual(expectedTrianglesCount, mesh.triangles.Length, util.PrintMessageDoesNotHaveCount("Triangle list", expectedTrianglesCount));
        }

        [Test]
        public void CreateHardMesh_Test()
        {
            //ARRANGE
            TestUtilities util = TestUtilities.Instance;
            Gem gem = GemService.Instance.CreateEmptyGem();
            gem.edges = 6;
            gem.width = 1f;
            gem.height = 1f;
            gem.depth = 1f;
            gem.vertexPositions = GemMeshGenerator.Instance.CreateVertexPositions(gem);

            //ACT
            Mesh mesh = GemMeshGenerator.Instance.CreateMesh(gem);

            //ASSERT
            Assert.IsNotNull(mesh,
            util.PrintMessageObjectIsNull("Sphere Rock Mesh"));

            int expectedVerticesCount = 92;
            int expectedUvCount = 92;
            int expectedTrianglesCount = 108;
            Assert.AreEqual(expectedVerticesCount, mesh.vertices.Length, util.PrintMessageDoesNotHaveCount("Vertex list", expectedVerticesCount));
            Assert.AreEqual(expectedUvCount, mesh.uv.Length, util.PrintMessageDoesNotHaveCount("UV list", expectedUvCount));
            Assert.AreEqual(expectedTrianglesCount, mesh.triangles.Length, util.PrintMessageDoesNotHaveCount("Triangle list", expectedTrianglesCount));
        }
    }
}
