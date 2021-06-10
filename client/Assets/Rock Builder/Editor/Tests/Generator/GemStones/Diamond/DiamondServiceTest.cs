using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using RockBuilder;
using UnityEngine;

namespace RockBuilder.Tests
{
    public class DiamondServiceTest
    {
        [Test]
        public void CreateEmptyDiamond_Test()
        {
            //ARRANGE
            TestUtilities util = TestUtilities.Instance;

            //ACT
            Diamond diamond = DiamondService.Instance.CreateEmptyDiamond();

            //ASSERT
            Assert.IsNotNull(diamond.gameObject.GetComponent<MeshFilter>(),
            util.PrintMessageOnMissingComponents("Diamond", "MeshFilter"));
            Assert.IsNotNull(diamond.gameObject.GetComponent<MeshRenderer>(),
            util.PrintMessageOnMissingComponents("Diamond", "MeshRenderer"));

            Assert.IsTrue(diamond.colliderFlag, util.PrintMessageOnWrongDefaultValue("Diamond", "ColliderFlag"));
            Assert.IsFalse(diamond.smoothFlag, util.PrintMessageOnWrongDefaultValue("Diamond", "SmoothFlag"));
            Assert.AreEqual(diamond.lodCount, 0, util.PrintMessageOnWrongDefaultValue("Diamond", "LodCount"));
            Assert.AreEqual(diamond.name, "New Game Object", util.PrintMessageOnWrongDefaultValue("Diamond", "Name"));
        }

        [Test]
        public void CreateEmptyDiamondWithName_Test()
        {
            //ARRANGE
            TestUtilities util = TestUtilities.Instance;
            string objectName = "new diamond";

            //ACT
            Diamond diamond = DiamondService.Instance.CreateEmptyDiamond(objectName);

            //ASSERT
            Assert.IsNotNull(diamond.gameObject.GetComponent<MeshFilter>(),
            util.PrintMessageOnMissingComponents("Diamond", "MeshFilter"));
            Assert.IsNotNull(diamond.gameObject.GetComponent<MeshRenderer>(),
            util.PrintMessageOnMissingComponents("Diamond", "MeshRenderer"));

            Assert.IsTrue(diamond.colliderFlag, util.PrintMessageOnWrongDefaultValue("Diamond", "ColliderFlag"));
            Assert.IsFalse(diamond.smoothFlag, util.PrintMessageOnWrongDefaultValue("Diamond", "SmoothFlag"));
            Assert.AreEqual(diamond.lodCount, 0, util.PrintMessageOnWrongDefaultValue("Diamond", "LodCount"));
            Assert.AreEqual(diamond.name, objectName, util.PrintMessageOnWrongDefaultValue("Diamond", "Name"));
        }
    }
}
