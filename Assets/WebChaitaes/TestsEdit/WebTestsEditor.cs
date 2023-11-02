using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using ChaitaesWeb;

public class WebTestsEditor
{
    // A Test behaves as an ordinary method
    [Test]
    public void WebTestsSimplePasses()
    {
        //do login and see if it works?
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator WebTestsWithEnumeratorPasses()
    {
        GameObject blah = new GameObject();
        blah.AddComponent<ChaitaesWeb.Web>();
        ChaitaesWeb.Web cWeb = blah.GetComponent<ChaitaesWeb.Web>();
        //ChaitaesWeb.Web cWeb = new MonoBehaviour<ChaitaesWeb.Web>();
        cWeb.UpdateUserName("hero");
        Assert.AreEqual(cWeb.GetCurrentUserName(), "hero");
        cWeb.SendClassicScore(5);
        yield return null;
        cWeb.GetScore();
        yield return null;
        Assert.AreEqual(cWeb.scores["hero"], null);
    }
}
