using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using RockBuilder;
using UnityEngine;

namespace RockBuilder.Tests
{
    public class CrystalServiceTest
    {
        [Test]
        public void CreateEmptyCrystal_Test()
        {
            //ARRANGE
            TestUtilities util = TestUtilities.Instance;

            //ACT
            Crystal sphereRock = CrystalService.Instance.CreateEmptyCrystal();

            //ASSERT
            Assert.IsNotNull(sphereRock.gameObject.GetComponent<MeshFilter>(),
            util.PrintMessageOnMissingComponents("Crystal", "MeshFilter"));
            Assert.IsNotNull(sphereRock.gameObject.GetComponent<MeshRenderer>(),
            util.PrintMessageOnMissingComponents("Crystal", "MeshRenderer"));

            Assert.IsTrue(sphereRock.colliderFlag, util.PrintMessageOnWrongDefaultValue("Crystal", "ColliderFlag"));
            Assert.IsFalse(sphereRock.smoothFlag, util.PrintMessageOnWrongDefaultValue("Crystal", "SmoothFlag"));
            Assert.AreEqual(sphereRock.lodCount, 0, util.PrintMessageOnWrongDefaultValue("Crystal", "LodCount"));
            Assert.AreEqual(sphereRock.name, "New Game Object", util.PrintMessageOnWrongDefaultValue("Crystal", "Name"));
        }

        [Test]
        public void CreateEmptyCrystalWithName_Test()
        {
            //ARRANGE
            TestUtilities util = TestUtilities.Instance;
            string objectName = "new crystal";

            //ACT
            Crystal sphereRock = CrystalService.Instance.CreateEmptyCrystal(objectName);

            //ASSERT
            Assert.IsNotNull(sphereRock.gameObject.GetComponent<MeshFilter>(),
            util.PrintMessageOnMissingComponents("Crystal", "MeshFilter"));
            Assert.IsNotNull(sphereRock.gameObject.GetComponent<MeshRenderer>(),
            util.PrintMessageOnMissingComponents("Crystal", "MeshRenderer"));

            Assert.IsTrue(sphereRock.colliderFlag, util.PrintMessageOnWrongDefaultValue("Crystal", "ColliderFlag"));
            Assert.IsFalse(sphereRock.smoothFlag, util.PrintMessageOnWrongDefaultValue("Crystal", "SmoothFlag"));
            Assert.AreEqual(sphereRock.lodCount, 0, util.PrintMessageOnWrongDefaultValue("Crystal", "LodCount"));
            Assert.AreEqual(sphereRock.name, objectName, util.PrintMessageOnWrongDefaultValue("Crystal", "Name"));
        }
    }
}
