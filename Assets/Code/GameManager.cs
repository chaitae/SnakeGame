using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int deathcount = 0;
    public static Action OnGameStart;
    public static Action OnDeath;
    public static Action<int> OnScoreChanged;
    public int score = 0;
    public static GameManager instance;
    // Start is called before the first frame update
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        OnGameStart?.Invoke();
    }
    public void RestartGame()
    {
        Debug.Log("restart game");
        SceneManager.LoadScene(0);
    }
    public void AddScore(int points)
    {
        score += points;
        OnScoreChanged?.Invoke(score);
    }
    public void Death()
    {
        if(deathcount == 0)
        {
            OnDeath?.Invoke();
        }
        deathcount++;

    }
}
