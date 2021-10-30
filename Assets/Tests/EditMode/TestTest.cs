using System.Collections;
using NUnit.Framework;
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
        public IEnumerator TestTestWithEnumeratorPasses()
        {
            yield return null;
        }
    }
}
