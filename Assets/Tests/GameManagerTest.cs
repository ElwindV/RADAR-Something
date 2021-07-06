using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    [TestFixture]
    public class GameManagerTest : MonoBehaviourBaseTest
    {
        [UnityTest]
        public IEnumerator GameManagerScoreTestPasses()
        {
            var gameManager = subject.AddComponent<GameManager>();

            yield return new WaitForEndOfFrame(); // Awake
            yield return new WaitForEndOfFrame(); // Start

            Assert.AreEqual(0, gameManager.score);

            gameManager.addScore(1);
            Assert.AreEqual(1, gameManager.score);

            gameManager.addScore(1);
            Assert.AreEqual(3, gameManager.score);

            gameManager.addScore(1);
            Assert.AreEqual(6, gameManager.score);

            gameManager.resetMultiplier();
            gameManager.addScore(1);
            Assert.AreEqual(7, gameManager.score);
        }

        [UnityTest]
        public IEnumerator GameManagerMultiplierResetTestPasses()
        {
            var gameManager = subject.AddComponent<GameManager>();

            yield return new WaitForEndOfFrame(); // Awake
            yield return new WaitForEndOfFrame(); // Start

            Assert.AreEqual(1, gameManager.multiplier);

            gameManager.addScore(1);
            Assert.AreEqual(2, gameManager.multiplier);

            gameManager.addScore(1);
            Assert.AreEqual(3, gameManager.multiplier);

            gameManager.addScore(1);
            Assert.AreEqual(4, gameManager.multiplier);

            gameManager.resetMultiplier();
            Assert.AreEqual(1, gameManager.multiplier);

            gameManager.addScore(1);
            Assert.AreEqual(2, gameManager.multiplier);
        }
    }
}

