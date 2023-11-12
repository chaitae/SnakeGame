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
    public static Action OnRestart;
    public static Action<bool> OnPause;
    public static Action<int> OnScoreChanged;
    
    public int score = 0;
    public static GameManager instance;
    bool isPaused = false;
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
    private void Update()
    {
        if(Input.GetButtonUp("Start"))
        {
            TogglePause();
        }
    }
    private void TogglePause()
    {
        if(!isPaused)
        {
            Time.timeScale = 0;
            OnPause?.Invoke(isPaused);
        }
        else
        {
            Time.timeScale = 1;
            OnPause?.Invoke(isPaused);

        }
        isPaused = !isPaused; 
    }
    private void Start()
    {
        OnGameStart?.Invoke();
    }
    public void RestartGame()
    {
        OnRestart?.Invoke();
        Debug.Log("restart game");
        StartCoroutine(LoadLevel());
    }
    private IEnumerator LoadLevel()
    {
        var asyncLoadLevel = SceneManager.LoadSceneAsync(1);
        while (!asyncLoadLevel.isDone)
        {
            Debug.Log("Loading the Scene");
            yield return null;
        }
        deathcount = 0;
        OnGameStart?.Invoke();
        //LoadScene?.Invoke(newSceneName);
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
