using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using RockBuilder;
using UnityEngine;

namespace RockBuilder.Tests
{
    public class SphereRockServiceTest
    {
        [Test]
        public void CreateEmptySphereRock_Test()
        {
            //ARRANGE
            TestUtilities util = TestUtilities.Instance;
            SphereRock sphereRock = SphereRockService.Instance.CreateEmptySphereRock();

            //ACT
            // Do nothing in this test case

            //ASSERT
            Assert.IsNotNull(sphereRock.gameObject.GetComponent<MeshFilter>(),
            util.PrintMessageOnMissingComponents("Sphere Rock", "MeshFilter"));
            Assert.IsNotNull(sphereRock.gameObject.GetComponent<MeshRenderer>(),
            util.PrintMessageOnMissingComponents("Sphere Rock", "MeshRenderer"));

            Assert.IsTrue(sphereRock.colliderFlag, util.PrintMessageOnWrongDefaultValue("Sphere Rock", "ColliderFlag"));
            Assert.IsTrue(sphereRock.smoothFlag, util.PrintMessageOnWrongDefaultValue("Sphere Rock", "SmoothFlag"));
            Assert.AreEqual(sphereRock.lodCount, 0, util.PrintMessageOnWrongDefaultValue("Sphere Rock", "LodCount"));
            Assert.AreEqual(sphereRock.name, "New Game Object", util.PrintMessageOnWrongDefaultValue("Sphere Rock", "Name"));
        }

        [Test]
        public void CreateEmptySphereRockWithName_Test()
        {
            //ARRANGE
            TestUtilities util = TestUtilities.Instance;
            string objectName = "new sphere rock";
            SphereRock sphereRock = SphereRockService.Instance.CreateEmptySphereRock(objectName);

            //ACT
            // Do nothing in this test case

            //ASSERT
            Assert.IsNotNull(sphereRock.gameObject.GetComponent<MeshFilter>(),
            util.PrintMessageOnMissingComponents("Sphere Rock", "MeshFilter"));
            Assert.IsNotNull(sphereRock.gameObject.GetComponent<MeshRenderer>(),
            util.PrintMessageOnMissingComponents("Sphere Rock", "MeshRenderer"));

            Assert.IsTrue(sphereRock.colliderFlag, util.PrintMessageOnWrongDefaultValue("Sphere Rock", "ColliderFlag"));
            Assert.IsTrue(sphereRock.smoothFlag, util.PrintMessageOnWrongDefaultValue("Sphere Rock", "SmoothFlag"));
            Assert.AreEqual(sphereRock.lodCount, 0, util.PrintMessageOnWrongDefaultValue("Sphere Rock", "LodCount"));
            Assert.AreEqual(sphereRock.name, objectName, util.PrintMessageOnWrongDefaultValue("Sphere Rock", "Name"));
        }
    }
}
