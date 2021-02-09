using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;



public class TestScript
{
    [UnityTest]
    public IEnumerator TestScriptWithEnumeratorPasses()
    {
        GameObject gamePrefab = MonoBehaviour.Instantiate(Resources.Load<GameObject>("game"));

        

        yield return null;
    }
}
