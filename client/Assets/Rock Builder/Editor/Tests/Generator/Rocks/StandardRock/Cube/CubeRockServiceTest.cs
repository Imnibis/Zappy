using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using RockBuilder;
using UnityEngine;

namespace RockBuilder.Tests
{
    public class CubeRockServiceTest
    {
        [Test]
        public void CreateEmptyCubeRock_Test()
        {
            //ARRANGE
            TestUtilities util = TestUtilities.Instance;

            //ACT
            CubeRock cubeRock = CubeRockService.Instance.CreateEmptyCubeRock();

            //ASSERT
            Assert.IsNotNull(cubeRock.gameObject.GetComponent<MeshFilter>(),
            util.PrintMessageOnMissingComponents("Cube Rock", "MeshFilter"));
            Assert.IsNotNull(cubeRock.gameObject.GetComponent<MeshRenderer>(),
            util.PrintMessageOnMissingComponents("Cube Rock", "MeshRenderer"));

            Assert.IsTrue(cubeRock.colliderFlag, util.PrintMessageOnWrongDefaultValue("Cube Rock", "ColliderFlag"));
            Assert.IsFalse(cubeRock.smoothFlag, util.PrintMessageOnWrongDefaultValue("Cube Rock", "SmoothFlag"));
            Assert.AreEqual(cubeRock.lodCount, 0, util.PrintMessageOnWrongDefaultValue("Cube Rock", "LodCount"));
            Assert.AreEqual(cubeRock.name, "New Game Object", util.PrintMessageOnWrongDefaultValue("Cube Rock", "Name"));
        }

        [Test]
        public void CreateEmptyCubeRockWithName_Test()
        {
            //ARRANGE
            TestUtilities util = TestUtilities.Instance;
            string objectName = "new cube rock";

            //ACT
            CubeRock cubeRock = CubeRockService.Instance.CreateEmptyCubeRock(objectName);

            //ASSERT
            Assert.IsNotNull(cubeRock.gameObject.GetComponent<MeshFilter>(),
            util.PrintMessageOnMissingComponents("Cube Rock", "MeshFilter"));
            Assert.IsNotNull(cubeRock.gameObject.GetComponent<MeshRenderer>(),
            util.PrintMessageOnMissingComponents("Cube Rock", "MeshRenderer"));

            Assert.IsTrue(cubeRock.colliderFlag, util.PrintMessageOnWrongDefaultValue("Cube Rock", "ColliderFlag"));
            Assert.IsFalse(cubeRock.smoothFlag, util.PrintMessageOnWrongDefaultValue("Cube Rock", "SmoothFlag"));
            Assert.AreEqual(cubeRock.lodCount, 0, util.PrintMessageOnWrongDefaultValue("Cube Rock", "LodCount"));
            Assert.AreEqual(cubeRock.name, objectName, util.PrintMessageOnWrongDefaultValue("Cube Rock", "Name"));
        }
    }
}
