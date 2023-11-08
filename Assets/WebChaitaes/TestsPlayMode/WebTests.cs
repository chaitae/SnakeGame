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
    public IEnumerator InsertSallyHighScoreCheckRegistered()
    {
        GameObject blah = new GameObject();
        ChaitaesWeb.LeaderBoardRequests cWeb = blah.AddComponent<ChaitaesWeb.LeaderBoardRequests>();
        cWeb.UpdateUsername("sally");
        Assert.AreEqual(cWeb.GetCurrentUserName(), "sally");
        cWeb.SendScore(200);
        yield return null;
        cWeb.GetScore();
        yield return null;
    }

    [UnityTest]
    public IEnumerator InsertHighScoreCheckRegistered()
    {
        GameObject blah = new GameObject();
        ChaitaesWeb.LeaderBoardRequests cWeb = blah.AddComponent<ChaitaesWeb.LeaderBoardRequests>();
        cWeb.UpdateUsername("hero");
        Assert.AreEqual(cWeb.GetCurrentUserName(), "hero");
        cWeb.SendScore(5);
        yield return null;
        cWeb.GetScore();
        yield return null;
        //Assert.AreEqual(cWeb.scores[cWeb.scores.Count - 1].Item1, "hero");
    }
}
