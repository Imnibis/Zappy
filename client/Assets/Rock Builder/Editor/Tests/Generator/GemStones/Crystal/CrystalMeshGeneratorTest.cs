using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using RockBuilder;

namespace RockBuilder.Tests
{
    public class CrystalMeshGeneratorTest
    {
        [Test]
        public void CreateVertexPositions_Test()
        {
            //ARRANGE
            TestUtilities util = TestUtilities.Instance;
            Crystal crystal = CrystalService.Instance.CreateEmptyCrystal();
            crystal.edges = 6;

            //ACT
            List<Vector3> vertexPositions = CrystalMeshGenerator.Instance.CreateVertexPositions(crystal);

            //ASSERT
            Assert.IsNotNull(crystal.gameObject.GetComponent<MeshRenderer>(),
            util.PrintMessageObjectIsNull("Vertex list"));

            int expectedCount = 14;
            Assert.AreEqual(expectedCount, vertexPositions.Count, util.PrintMessageDoesNotHaveCount("Vertex list", expectedCount));
        }

        [Test]
        public void CreateSmoothMesh_Test()
        {
            //ARRANGE
            TestUtilities util = TestUtilities.Instance;
            Crystal crystal = CrystalService.Instance.CreateEmptyCrystal();
            crystal.edges = 6;
            crystal.height = 1f;
            crystal.heightPeak = 1f;
            crystal.smoothFlag = true;
            crystal.vertexPositions = CrystalMeshGenerator.Instance.CreateVertexPositions(crystal);

            //ACT
            Mesh mesh = CrystalMeshGenerator.Instance.CreateMesh(crystal);

            //ASSERT
            Assert.IsNotNull(mesh,
            util.PrintMessageObjectIsNull("Sphere Rock Mesh"));

            int expectedVerticesCount = 26;
            int expectedUvCount = 26;
            int expectedTrianglesCount = 78;
            Assert.AreEqual(expectedVerticesCount, mesh.vertices.Length, util.PrintMessageDoesNotHaveCount("Vertex list", expectedVerticesCount));
            Assert.AreEqual(expectedUvCount, mesh.uv.Length, util.PrintMessageDoesNotHaveCount("UV list", expectedUvCount));
            Assert.AreEqual(expectedTrianglesCount, mesh.triangles.Length, util.PrintMessageDoesNotHaveCount("Triangle list", expectedTrianglesCount));
        }

        [Test]
        public void CreateHardMesh_Test()
        {
            //ARRANGE
            TestUtilities util = TestUtilities.Instance;
            Crystal crystal = CrystalService.Instance.CreateEmptyCrystal();
            crystal.edges = 6;
            crystal.height = 1f;
            crystal.heightPeak = 1f;
            crystal.vertexPositions = CrystalMeshGenerator.Instance.CreateVertexPositions(crystal);

            //ACT
            Mesh mesh = CrystalMeshGenerator.Instance.CreateMesh(crystal);

            //ASSERT
            Assert.IsNotNull(mesh,
            util.PrintMessageObjectIsNull("Sphere Rock Mesh"));

            int expectedVerticesCount = 36;
            int expectedUvCount = 36;
            int expectedTrianglesCount = 72;
            Assert.AreEqual(expectedVerticesCount, mesh.vertices.Length, util.PrintMessageDoesNotHaveCount("Vertex list", expectedVerticesCount));
            Assert.AreEqual(expectedUvCount, mesh.uv.Length, util.PrintMessageDoesNotHaveCount("UV list", expectedUvCount));
            Assert.AreEqual(expectedTrianglesCount, mesh.triangles.Length, util.PrintMessageDoesNotHaveCount("Triangle list", expectedTrianglesCount));
        }
    }
}
