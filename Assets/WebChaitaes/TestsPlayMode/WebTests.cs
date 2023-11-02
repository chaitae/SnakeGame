using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using ChaitaesWeb;

public class WebTests
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
        //ChaitaesWeb.Web cWeb = new ChaitaesWeb.Web();
        GameObject blah = new GameObject();
        blah.AddComponent<ChaitaesWeb.Web>();
        ChaitaesWeb.Web cWeb = blah.GetComponent<ChaitaesWeb.Web>();
        cWeb.UpdateUserName("hero");
        cWeb.SendClassicScore(5);
        yield return new WaitForSeconds(2);
        cWeb.GetScore();
        yield return new WaitForSeconds(5);
        Assert.AreEqual(5, cWeb.scores["hero"]);
    }
}
