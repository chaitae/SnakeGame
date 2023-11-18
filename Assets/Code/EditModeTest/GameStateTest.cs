using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateTest
{
    [Test]
    public void TestOnDeathPlayerSetDead()
    {
        GameObject go = new GameObject();
        PlayerController playerController = go.AddComponent<PlayerController>();
        playerController.Awake();
        GameManager gameManager = go.AddComponent<GameManager>();
        gameManager.Death();
        Assert.AreEqual(playerController.IsAlive, false);
    }
    [Test]
    public void TestOnDeathIncrementCount()
    {
        GameObject go = new GameObject();
        PlayerController playerController = go.AddComponent<PlayerController>();
        Vector3 initPosition = playerController.transform.position;
        GameManager gameManager = go.AddComponent<GameManager>();
        gameManager.Death();
        Assert.AreEqual(gameManager.DeathCount, 1);

    }
}
