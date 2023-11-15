using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using System.Threading;
using UnityEngine.Events;
using static System.Net.WebRequestMethods;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class ScoreInfo
{
    public string username;
    public int score;
    public ScoreInfo(string username, int score)
    {
        this.username = username;
        this.score = score;
    }
}
namespace ChaitaesWeb
{
    public class LeaderBoardRequests : MonoBehaviour
    {
        string url;
        string username;
        public Action<List<ScoreInfo>> onGetScores;
        public List<ScoreInfo> scores = new List<ScoreInfo>();
        //below needs to be updated when starting and stopping the server due to not buying an elastic ip address
        string sendScoreURL = "http://54.221.140.194/leaderboard/public/scores/add";
        string getScoreURL = "http://54.221.140.194/leaderboard/public/scores";
        public static LeaderBoardRequests instance;
        public bool isLocal = true;

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
            if (!isLocal)
            {
                StartCoroutine(SendScoreHelper(score, sendScoreURL));

            }
            else
            {
                scores.Add(new ScoreInfo(username, score));
            }
        }
        public List<ScoreInfo> GetScore()
        {
            if(!isLocal)
            {
                StartCoroutine(GetScoresHelper(getScoreURL));
            }
            else
            {
                onGetScores?.Invoke(scores);
            }
            return scores;

        }
        IEnumerator SendScoreHelper(int score, string urlTemp)
        {
            WWWForm form = new WWWForm();
            form.AddField("username", username); //this needs to be a field reflected in the php file
            form.AddField("score", score); //this needs to be a field reflected in the php file
            url = sendScoreURL;
            using (UnityWebRequest webRequest = UnityWebRequest.Post(sendScoreURL, form))
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

            url = urlTemp;
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
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
                        ScoreInfo[] converted = JsonHelper.FromRawJson<ScoreInfo>(input);
                        scores = converted.ToList();
                        onGetScores?.Invoke(scores);
                        break;

                }
            }

        }
        

    }
}