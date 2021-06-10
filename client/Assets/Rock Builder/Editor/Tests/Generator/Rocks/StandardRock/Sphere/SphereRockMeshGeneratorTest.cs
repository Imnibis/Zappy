using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using RockBuilder;

namespace RockBuilder.Tests
{
    public class SphereRockMeshGeneratorTest
    {
        [Test]
        public void CreateVertexPositions_Test()
        {
            //ARRANGE
            TestUtilities util = TestUtilities.Instance;
            SphereRock sphereRock = SphereRockService.Instance.CreateEmptySphereRock();
            sphereRock.edges = 6;

            //ACT
            List<Vector3> vertexPositions = SphereRockMeshGenerator.Instance.CreateVertexPositions(sphereRock);

            //ASSERT
            Assert.IsNotNull(sphereRock.gameObject.GetComponent<MeshRenderer>(),
            util.PrintMessageObjectIsNull("Vertex list"));

            int expectedCount = 36;
            Assert.AreEqual(expectedCount, vertexPositions.Count, util.PrintMessageDoesNotHaveCount("Vertex list", expectedCount));
        }

        [Test]
        public void CreateSmoothMesh_Test()
        {
            //ARRANGE
            TestUtilities util = TestUtilities.Instance;
            SphereRock sphereRock = SphereRockService.Instance.CreateEmptySphereRock();
            sphereRock.edges = 6;
            sphereRock.width = 1f;
            sphereRock.height = 1f;
            sphereRock.depth = 1f;
            sphereRock.vertexPositions = SphereRockMeshGenerator.Instance.CreateVertexPositions(sphereRock);

            //ACT
            Mesh mesh = SphereRockMeshGenerator.Instance.CreateRockMesh(sphereRock);

            //ASSERT
            Assert.IsNotNull(mesh,
            util.PrintMessageObjectIsNull("Sphere Rock Mesh"));

            int expectedVerticesCount = 30;
            int expectedUvCount = 30;
            int expectedTrianglesCount = 222;
            Assert.AreEqual(expectedVerticesCount, mesh.vertices.Length, util.PrintMessageDoesNotHaveCount("Vertex list", expectedVerticesCount));
            Assert.AreEqual(expectedUvCount, mesh.uv.Length, util.PrintMessageDoesNotHaveCount("UV list", expectedUvCount));
            Assert.AreEqual(expectedTrianglesCount, mesh.triangles.Length, util.PrintMessageDoesNotHaveCount("Triangle list", expectedTrianglesCount));
        }

        [Test]
        public void CreateHardMesh_Test()
        {
            //ARRANGE
            TestUtilities util = TestUtilities.Instance;
            SphereRock sphereRock = SphereRockService.Instance.CreateEmptySphereRock();
            sphereRock.edges = 6;
            sphereRock.width = 1f;
            sphereRock.height = 1f;
            sphereRock.depth = 1f;
            sphereRock.smoothFlag = false;
            sphereRock.vertexPositions = SphereRockMeshGenerator.Instance.CreateVertexPositions(sphereRock);

            //ACT
            Mesh mesh = SphereRockMeshGenerator.Instance.CreateRockMesh(sphereRock);

            //ASSERT
            Assert.IsNotNull(mesh,
            util.PrintMessageObjectIsNull("Sphere Rock Mesh"));

            int expectedVerticesCount = 108;
            int expectedUvCount = 108;
            int expectedTrianglesCount = 144;
            Assert.AreEqual(expectedVerticesCount, mesh.vertices.Length, util.PrintMessageDoesNotHaveCount("Vertex list", expectedVerticesCount));
            Assert.AreEqual(expectedUvCount, mesh.uv.Length, util.PrintMessageDoesNotHaveCount("UV list", expectedUvCount));
            Assert.AreEqual(expectedTrianglesCount, mesh.triangles.Length, util.PrintMessageDoesNotHaveCount("Triangle list", expectedTrianglesCount));
        }
    }
}
