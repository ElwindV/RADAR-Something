using NUnit.Framework;
using UnityEngine;
using System.Linq;


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
            .ForEach(go => Object.DestroyImmediate(go));
    }
}

