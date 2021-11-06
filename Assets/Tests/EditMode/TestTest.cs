using System.Collections;
using Gameplay;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.EditMode
{
    public class TestTest
    {
        [Test]
        public void TestTestSimplePasses()
        {
            Assert.AreEqual(true, true);
        }
        
        [UnityTest]
        public IEnumerator PlayerCanTakeDamage()
        {
            yield return null;
            
            var gameObject = new GameObject();
            var player = gameObject.AddComponent<Player>();
            
            yield return null;

            Assert.IsNotNull(player);

            var hitPointsBefore = player.hitPoints;
            
            player.TakeDamage(10f);

            Assert.IsTrue(hitPointsBefore > player.hitPoints);
        }
    }
}
