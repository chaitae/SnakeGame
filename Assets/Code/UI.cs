using ChaitaesWeb;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
[Serializable]

public class UI : MonoBehaviour
{
    public static UI instance;
    public UIDocument uiLeaderBoard, uiSubmitScore, uiPauseScreen;
    VisualElement rootLeaderBoard,rootSubmitScore;
    List<ScoreInfo> scores = new List<ScoreInfo>();
    [SerializeField]
    VisualTreeAsset ListEntryTemplate;
    [SerializeField]
    VisualTreeAsset submitScore,endScreen;
    ListView ScoreList;
    Label userScore;
    Action showLeaderBoardHideSubmit;
    Action showLeaderBoardShowSubmit;
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
        showLeaderBoardShowSubmit = () => ShowLeaderBoard(true);
        
    }
    void Start()
    {
        LeaderBoardRequests.instance.onGetScores += SetScores;
        GameManager.OnDeath += showLeaderBoardShowSubmit;
        GameManager.OnPause += Pause;
        DontDestroyOnLoad(gameObject);
    }
    private void OnDisable()
    {
        LeaderBoardRequests.instance.onGetScores -= SetScores;
        GameManager.OnDeath -= showLeaderBoardShowSubmit;
        GameManager.OnPause -= Pause;
    }

    private void Pause(bool isPaused)
    {
        uiPauseScreen.enabled = !isPaused;
    }

    void ShowSubmitScore()
    {
        uiLeaderBoard.enabled = false;
        uiSubmitScore.enabled = true;
        rootSubmitScore = uiSubmitScore.rootVisualElement;
        Button submitScoreB = rootSubmitScore.Q<Button>("SubmitScore");
        TextField userField = rootSubmitScore.Q<TextField>("textField");
        submitScoreB.clicked += () =>
        {
            ChaitaesWeb.LeaderBoardRequests.instance.UpdateUsername(userField.text);
            ChaitaesWeb.LeaderBoardRequests.instance.SendScore(GameManager.instance.score);
            ShowLeaderBoard(false);
        };
    }
    void ShowLeaderBoard(bool showSubmit = true)
    {
        uiLeaderBoard.enabled = true;
        uiSubmitScore.enabled = false;
        uiLeaderBoard.visualTreeAsset = endScreen;

        rootLeaderBoard = uiLeaderBoard.rootVisualElement;
        userScore = rootLeaderBoard.Q<Label>("userScoreLabel");
        userScore.text ="Your Score: \n" + GameManager.instance.score + "";
        Button submit = rootLeaderBoard.Q<Button>("submit");
        if (showSubmit)
        {
            submit.style.display = DisplayStyle.Flex;
        }
        else
        {
            submit.style.display = DisplayStyle.None;

        }
        FillLeaderboard();
    }
    void FillLeaderboard()
    {
        userScore.text = "Your Score:\n" + GameManager.instance.score + "";
        ScoreList = rootLeaderBoard.Q<ListView>("ScoreList");
        rootLeaderBoard = uiLeaderBoard.rootVisualElement;
        Button retry = rootLeaderBoard.Q<Button>("retry");
        Button submit = rootLeaderBoard.Q<Button>("submit");
        submit.clicked += ShowSubmitScore;
        retry.clicked += () =>
        {
            GameManager.instance.RestartGame();
            uiLeaderBoard.enabled = false;
        };
        // Set up a make item function for a list entry
        ScoreList.makeItem = () =>
        {
            var newListEntry = ListEntryTemplate.Instantiate();
            var newListEntryLogic = new ScoreEntryController();
            newListEntry.userData = newListEntryLogic;
            newListEntryLogic.SetVisualElement(newListEntry);
            return newListEntry;
        };

        // Set up bind function for a specific list entry
        ScoreList.bindItem = (item, index) =>
        {
            (item.userData as ScoreEntryController).SetScoreInfo(scores[index].username, scores[index].score);
        };
        LeaderBoardRequests.instance.GetScore();
    }
    void SetScores(List<ScoreInfo> scores)
    {
        this.scores = scores;
        ScoreList.itemsSource = scores;
    }

}
