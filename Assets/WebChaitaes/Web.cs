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
    public class Web : MonoBehaviour
    {
        string url;
        string username, password,email;
        public UnityEvent onUsernameExistsRegister;
        public UnityEvent onEmailExistsRegister;
        public static Action<Dictionary<string, int>> onGetScores;
        public Dictionary<string, int> scores = new Dictionary<string, int>();
        [SerializeField]
        string classicScoreURL = "http://localhost/HighscoreTemplate/SnakeSetScore.php";
        [SerializeField]
        string secureScoreURL = "http://localhost/HighscoreTemplate/SetScore.php";
        string classicGetScoreURL = "http://localhost/HighscoreTemplate/GetScores.php";


        [Tooltip("No need for logging in")]
        public bool isClassicScoreboard = true;

        public string GetCurrentUserName()
        {
            return username;
        }

        public void UpdateUserName(string str)
        {
            username = str;
        }
        public void UpdatePassword(string str)
        {
            password = str;
        }
        public void UpdateEmail(string str)
        {
            email = str;
        }
        public void Login()
        {
            Login(username, password);
        }
        public void Register()
        {
            string salt = Encryption.CreateSalt(5);
            string hashedPass = Encryption.GenerateSHA256Hash(password, salt);
            StartCoroutine(RegisterHelper(username, hashedPass,salt, email));
        }
        public void SendClassicScore(int score)
        {
            StartCoroutine(SendScoreHelper(score, classicScoreURL));
        }
        public void SendScore(int score)
        {
            if(isClassicScoreboard)
            {
                StartCoroutine(SendScoreHelper(score, classicScoreURL));
            }
            else
            {
                StartCoroutine(SendScoreHelper(score, secureScoreURL));

            }
        }
        public void GetScore()
        {
            StartCoroutine(GetScoresHelper(classicGetScoreURL));

        }
        void Login(string username,string password)
        {
            StartCoroutine(LoginHelper(username, password));
        }
        IEnumerator RegisterHelper(string username,string password,string salt, string email)
        {

            WWWForm form = new WWWForm();
            form.AddField("loginUser", username); //this needs to be a field reflected in the php file
            form.AddField("loginPass", password);
            form.AddField("loginSalt", salt);
            form.AddField("loginEmail", email);
            string url = "http://localhost/HighscoreTemplate/RegisterUser.php";
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
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                        if(webRequest.downloadHandler.text == "Email is already taken.")
                        {
                            onEmailExistsRegister.Invoke();
                        }
                        if (webRequest.downloadHandler.text == "Username is already taken.")
                        {
                            onUsernameExistsRegister.Invoke();

                        }
                        //webRequest.downloadHandler.data this gets the bytes
                        break;
                }
            }
        }

        IEnumerator LoginHelper(string username, string password)
        {
            WWWForm form = new WWWForm();
            form.AddField("loginUser", username); //this needs to be a field reflected in the php file
            url = "http://localhost/HighscoreTemplate/Login.php";
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url,form))
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
                        String[] saltPass = webRequest.downloadHandler.text.Trim().Split(' ');
                        string hashedPass = Encryption.GenerateSHA256Hash(password, saltPass[0]);
                        if (saltPass[1] == hashedPass)
                        {
                            Debug.Log("Login Successful!");
                        }
                        else
                        {
                            Debug.Log("Wrong credentials.");
                        }
                        break;
                }
            }
        }
        IEnumerator SendScoreHelper(int score, string urlTemp)
        {
            WWWForm form = new WWWForm();
            form.AddField("loginUser", username); //this needs to be a field reflected in the php file
            form.AddField("sentScore", score); //this needs to be a field reflected in the php file
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
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);

                        Debug.Log("score updated");
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
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                        string input = webRequest.downloadHandler.text;

                        string[] lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
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
                                    // Add the username and score to the dictionary
                                    scores[username] = score;
                                }
                                //concat it
                            }
                        }
                        onGetScores?.Invoke(scores);
                        // Print the usernames and scores
                        foreach (var pair in scores)
                        {
                            Debug.Log($"Username: {pair.Key}, Score: {pair.Value}");
                        }
                        break;

                    }
                }
            }
        

    }
}