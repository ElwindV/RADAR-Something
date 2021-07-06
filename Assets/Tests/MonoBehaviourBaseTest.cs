using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public abstract class MonoBehaviourBaseTest
    {
        protected GameObject subject;

        [SetUp]
        public virtual void SetUp()
        {
            subject = new GameObject("Subject");
        }

        [TearDown]
        public virtual void TearDown() 
        {
            Object.FindObjectsOfType<GameObject>()
                .ToList()
                .ForEach(Object.DestroyImmediate);
        }
    }
}

