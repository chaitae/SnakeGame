using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using System.Threading;
using UnityEngine.Events;
using static System.Net.WebRequestMethods;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms.Impl;

namespace ChaitaesWeb
{
    public class LeaderBoardRequests : MonoBehaviour
    {
        string url;
        string username;
        public UnityEvent onUsernameExistsRegister;
        public UnityEvent onEmailExistsRegister;
        public Action<List<Tuple<string, int>>> onGetScores;
        public List<Tuple<string, int>> scores = new List<Tuple<string, int>>();
        string sendScoreURL = "http://3.89.209.183/SnakeSetScore.php";
        string getScoreURL = "http://3.89.209.183/GetScores.php";
        public static LeaderBoardRequests instance;

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


        [Tooltip("No need for logging in")]
        public bool isClassicScoreboard = true;

        public void UpdateUsername(string username)
        {
            this.username = username;
        }
        public string GetCurrentUserName()
        {
            return username;
        }

        public void SendScore(int score)
        {
            StartCoroutine(SendScoreHelper(score, sendScoreURL));

        }
        public List<Tuple<string, int>> GetScore()
        {
            StartCoroutine(GetScoresHelper(getScoreURL));
            return scores;

        }
        IEnumerator SendScoreHelper(int score, string urlTemp)
        {
            WWWForm form = new WWWForm();
            form.AddField("loginUser", username); //this needs to be a field reflected in the php file
            form.AddField("sentScore", score); //this needs to be a field reflected in the php file
            url = sendScoreURL;
            using (UnityWebRequest webRequest = UnityWebRequest.Post(sendScoreURL, form))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();
                string[] pages = url.Split('/');
                int page = pages.Length - 1;
                Debug.Log(pages[3]);

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                        break;

                }
            }
        }
        IEnumerator GetScoresHelper(string urlTemp)
        {
            WWWForm form = new WWWForm();

            url = urlTemp;
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();
                string[] pages = url.Split('/');
                int page = pages.Length - 1;

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                        //Do local high score
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                        string input = webRequest.downloadHandler.text;

                        string[] lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                        scores.Clear();
                        foreach (string line in lines)
                        {
                            // Split each line into username and score
                            string[] parts = line.Split(' ');

                            if (parts.Length == 2)
                            {
                                string username = parts[0];
                                int score;
                                if (int.TryParse(parts[1], out score))
                                {

                                    scores.Add(new Tuple<string, int>(username, score));
                                }
                                //concat it
                            }
                        }
                        onGetScores?.Invoke(scores);
                        break;

                }
            }

        }
        

    }
}