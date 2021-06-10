using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using RockBuilder;

namespace RockBuilder.Tests
{
    public class CubeRockMeshGeneratorTest
    {
        [Test]
        public void CreateVertexPositions_Test()
        {
            //ARRANGE
            TestUtilities util = TestUtilities.Instance;
            CubeRock cubeRock = CubeRockService.Instance.CreateEmptyCubeRock();
            cubeRock.divider = 3;

            //ACT
            cubeRock = CubeRockMeshGenerator.Instance.CreateVertexPositions(cubeRock);

            //ASSERT
            Assert.IsNotNull(cubeRock.gameObject.GetComponent<MeshRenderer>(),
            util.PrintMessageObjectIsNull("Vertex list"));

            int expectedCount = 4;
            int expectedVerticalCount = 8;
            Assert.AreEqual(expectedCount, cubeRock.bottomBezelsVertices.Count, util.PrintMessageDoesNotHaveCount("BottomBezelsVertices list", expectedCount));
            Assert.AreEqual(expectedCount, cubeRock.bottomPlaneVertices.Count, util.PrintMessageDoesNotHaveCount("BottomPlaneVertices list", expectedCount));
            Assert.AreEqual(expectedVerticalCount, cubeRock.bottomVerticalBezelsVertices.Count, util.PrintMessageDoesNotHaveCount("BottomVerticalBezelsVertices list", expectedCount));
            Assert.AreEqual(expectedCount, cubeRock.upperBezelsVertices.Count, util.PrintMessageDoesNotHaveCount("UpperBezelsVertices list", expectedCount));
            Assert.AreEqual(expectedCount, cubeRock.upperPlaneVertices.Count, util.PrintMessageDoesNotHaveCount("UpperPlaneVertices list", expectedCount));
            Assert.AreEqual(expectedVerticalCount, cubeRock.upperVerticalBezelsVertices.Count, util.PrintMessageDoesNotHaveCount("UpperVerticalBezelsVertices list", expectedCount));
        }

        [Test]
        public void CreateSmoothMesh_Test()
        {
            //ARRANGE
            TestUtilities util = TestUtilities.Instance;
            CubeRock cubeRock = CubeRockService.Instance.CreateEmptyCubeRock();
            cubeRock.divider = 3;
            cubeRock.width = 1f;
            cubeRock.height = 1f;
            cubeRock.depth = 1f;
            cubeRock.smoothFlag = true;
            cubeRock = CubeRockMeshGenerator.Instance.CreateVertexPositions(cubeRock);

            //ACT
            Mesh mesh = CubeRockMeshGenerator.Instance.CreateRockMesh(cubeRock);

            //ASSERT
            Assert.IsNotNull(mesh,
            util.PrintMessageObjectIsNull("Cube Rock Mesh"));

            int expectedVerticesCount = 168;
            int expectedUvCount = 168;
            int expectedTrianglesCount = 420;
            Assert.AreEqual(expectedVerticesCount, mesh.vertices.Length, util.PrintMessageDoesNotHaveCount("Vertex list", expectedVerticesCount));
            Assert.AreEqual(expectedUvCount, mesh.uv.Length, util.PrintMessageDoesNotHaveCount("UV list", expectedUvCount));
            Assert.AreEqual(expectedTrianglesCount, mesh.triangles.Length, util.PrintMessageDoesNotHaveCount("Triangle list", expectedTrianglesCount));
        }

        [Test]
        public void CreateHardMesh_Test()
        {
            //ARRANGE
            TestUtilities util = TestUtilities.Instance;
            CubeRock cubeRock = CubeRockService.Instance.CreateEmptyCubeRock();
            cubeRock.divider = 3;
            cubeRock.width = 1f;
            cubeRock.height = 1f;
            cubeRock.depth = 1f;
            cubeRock = CubeRockMeshGenerator.Instance.CreateVertexPositions(cubeRock);

            //ACT
            Mesh mesh = CubeRockMeshGenerator.Instance.CreateRockMesh(cubeRock);

            //ASSERT
            Assert.IsNotNull(mesh,
            util.PrintMessageObjectIsNull("Cube Rock Mesh"));

            int expectedVerticesCount = 288;
            int expectedUvCount = 288;
            int expectedTrianglesCount = 420;
            Assert.AreEqual(expectedVerticesCount, mesh.vertices.Length, util.PrintMessageDoesNotHaveCount("Vertex list", expectedVerticesCount));
            Assert.AreEqual(expectedUvCount, mesh.uv.Length, util.PrintMessageDoesNotHaveCount("UV list", expectedUvCount));
            Assert.AreEqual(expectedTrianglesCount, mesh.triangles.Length, util.PrintMessageDoesNotHaveCount("Triangle list", expectedTrianglesCount));
        }
    }
}
