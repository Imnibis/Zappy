using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using RockBuilder;
using UnityEngine;

namespace RockBuilder.Tests
{
    public class GemServiceTest
    {
        [Test]
        public void CreateEmptyGem_Test()
        {
            //ARRANGE
            TestUtilities util = TestUtilities.Instance;
            Gem gem = GemService.Instance.CreateEmptyGem();

            //ACT
            // Do nothing in this test case

            //ASSERT
            Assert.IsNotNull(gem.gameObject.GetComponent<MeshFilter>(),
            util.PrintMessageOnMissingComponents("Gem", "MeshFilter"));
            Assert.IsNotNull(gem.gameObject.GetComponent<MeshRenderer>(),
            util.PrintMessageOnMissingComponents("Gem", "MeshRenderer"));

            Assert.IsTrue(gem.colliderFlag, util.PrintMessageOnWrongDefaultValue("Gem", "ColliderFlag"));
            Assert.IsFalse(gem.smoothFlag, util.PrintMessageOnWrongDefaultValue("Gem", "SmoothFlag"));
            Assert.AreEqual(gem.lodCount, 0, util.PrintMessageOnWrongDefaultValue("Gem", "LodCount"));
            Assert.AreEqual(gem.name, "New Game Object", util.PrintMessageOnWrongDefaultValue("Gem", "Name"));
        }

        [Test]
        public void CreateEmptyGemWithName_Test()
        {
            //ARRANGE
            TestUtilities util = TestUtilities.Instance;
            string objectName = "new gem rock";

            //ACT
            Gem gem = GemService.Instance.CreateEmptyGem(objectName);

            //ASSERT
            Assert.IsNotNull(gem.gameObject.GetComponent<MeshFilter>(),
            util.PrintMessageOnMissingComponents("Gem", "MeshFilter"));
            Assert.IsNotNull(gem.gameObject.GetComponent<MeshRenderer>(),
            util.PrintMessageOnMissingComponents("Gem", "MeshRenderer"));

            Assert.IsTrue(gem.colliderFlag, util.PrintMessageOnWrongDefaultValue("Gem", "ColliderFlag"));
            Assert.IsFalse(gem.smoothFlag, util.PrintMessageOnWrongDefaultValue("Gem", "SmoothFlag"));
            Assert.AreEqual(gem.lodCount, 0, util.PrintMessageOnWrongDefaultValue("Gem", "LodCount"));
            Assert.AreEqual(gem.name, objectName, util.PrintMessageOnWrongDefaultValue("Gem", "Name"));
        }
    }
}
