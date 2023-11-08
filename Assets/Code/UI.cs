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
    public UIDocument uiDocument;
    VisualElement root;
    List<score> scores = new List<score>();
    [SerializeField]
    VisualTreeAsset ListEntryTemplate;
    [SerializeField]
    VisualTreeAsset submitScore,endScreen;
    ListView ScoreList;
    Label userScore;
    private void OnDisable()
    {
        LeaderBoardRequests.onGetScores -= SetScores;
        GameManager.OnDeath -= ShowEndScreen;
    }
    void SwitchSubmitScore()
    {
        uiDocument.visualTreeAsset = submitScore;
        VisualElement rootB = uiDocument.rootVisualElement;
        Button submitScoreB = rootB.Q<Button>("SubmitScore");
        TextField userField = rootB.Q<TextField>("textField"); 
        //TextInput userField = root.Q<TextInput>("userTextField");
        Debug.Log(userField.text);
        Debug.Log(GameManager.instance.score);
        //ChaitaesWeb.LeaderBoardRequests.instance.UpdateUsername(userField.text);
        //ChaitesWeb.LeaderBoardRequests.userField.text;
        submitScoreB.clicked += () => {
            ChaitaesWeb.LeaderBoardRequests.instance.UpdateUsername(userField.text);
            ChaitaesWeb.LeaderBoardRequests.instance.SendScore(GameManager.instance.score);
            ShowEndScreen();
        };
    }
    void ShowEndScreen()
    {
        uiDocument.enabled = true;
        root = uiDocument.rootVisualElement;
        userScore = root.Q<Label>("userScoreLabel");
        userScore.text ="Your Score: \n" + GameManager.instance.score + "";
        //uiDocument.visualTreeAsset = endScreen;
        //FillLeaderboard();
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
        ScoreList = root.Q<ListView>("ScoreList");
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
        LeaderBoardRequests.onGetScores += SetScores;
        LeaderBoardRequests.instance.GetScore();
    }
    void SetScores(List<Tuple<string, int>> _scores)
    {
        scores = ConvertTupleIntoScore(_scores);

        ScoreList.itemsSource = scores;
    }
    void SetEndScreenBehaviors()
    {
        
        //endScreenRoot = uiDocument.rootVisualElement;
        //Button submit = endScreenRoot.Q<Button>("submit");
        //Button retry = endScreenRoot.Q<Button>("retry");
        //userScore = endScreenRoot.Q<Label>("userScoreLabel");
        ////retry.RegisterCallback<>(Debug.Log("noo"));
        ////submit.onClick += GameM
        //submit.clicked += SwitchSubmitScore;
        //retry.clicked += GameManager.instance.RestartGame;
        //ChaitaesWeb.LeaderBoardRequests.instance.UpdateUsername(submit);
    }
    // Start is called before the first frame update
    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        //SetEndScreenBehaviors();
        //FillLeaderboard();
        //endScreenRoot = uiDocument.rootVisualElement;

        //SetButtons
        GameManager.OnDeath += ShowEndScreen;
    }

}
