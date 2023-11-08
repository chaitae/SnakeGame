using ChaitaesWeb;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Player;
using UnityEngine;
using UnityEngine.UIElements;
[Serializable]
public struct score
{
    public string username;
    public int points;
    public score(string username, int points)
    {
        this.username = username;
        this.points = points;
    }
}
public class UI : MonoBehaviour
{
    public UIDocument uiLeaderBoard,uiSubmitScore;
    VisualElement rootLeaderBoard,rootSubmitScore;
    List<score> scores = new List<score>();
    [SerializeField]
    VisualTreeAsset ListEntryTemplate;
    [SerializeField]
    VisualTreeAsset submitScore,endScreen;
    ListView ScoreList;
    Label userScore;
    private void OnDisable()
    {
        LeaderBoardRequests.instance.onGetScores -= SetScores;
        GameManager.OnDeath -= ShowLeaderBoard;
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
            ShowLeaderBoard();
        };
    }
    void ShowLeaderBoard()
    {
        uiLeaderBoard.enabled = true;
        uiSubmitScore.enabled = false;
        uiLeaderBoard.visualTreeAsset = endScreen;

        rootLeaderBoard = uiLeaderBoard.rootVisualElement;
        userScore = rootLeaderBoard.Q<Label>("userScoreLabel");
        userScore.text ="Your Score: \n" + GameManager.instance.score + "";
        Button submit = rootLeaderBoard.Q<Button>("submit");
        Button retry = rootLeaderBoard.Q<Button>("retry");

        submit.clicked += ShowSubmitScore;
        retry.clicked += GameManager.instance.RestartGame;
        FillLeaderboard();
    }
    List<score> ConvertTupleIntoScore(List<Tuple<string,int>> _scores)
    {
        List<score> tempScores = new List<score>();
        _scores.ForEach(n => tempScores.Add(new score(n.Item1,n.Item2)));
        return tempScores;
    }
    void FillLeaderboard()
    {
        userScore.text = "Your Score:\n" + GameManager.instance.score + "";
        ScoreList = rootLeaderBoard.Q<ListView>("ScoreList");
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
            (item.userData as ScoreEntryController).SetScoreInfo(scores[index].username, scores[index].points);
        };
        LeaderBoardRequests.instance.GetScore();
    }
    void SetScores(List<Tuple<string, int>> _scores)
    {
        scores = ConvertTupleIntoScore(_scores);
        ScoreList.itemsSource = scores;
    }
    // Start is called before the first frame update
    void Start()
    {
        LeaderBoardRequests.instance.onGetScores += SetScores;
        GameManager.OnDeath += ShowLeaderBoard;
    }

}
