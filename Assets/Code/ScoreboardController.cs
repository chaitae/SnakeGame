using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using ChaitaesWeb;
using System;

public class ScoreboardController : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text nameText;
    public GameObject scorePrefab;
    public GameObject scoreParent;
    // Start is called before the first frame update
    void Awake()
    {
        LeaderBoardRequests.instance.onGetScores += UpdateScoreboard;
    }
    public void UpdateScoreboard(List<ScoreInfo> scores)
    {
        int rank = 1;
        foreach(var score in scores)
        {
            ScoreItem scoreItem = Instantiate(scorePrefab, scoreParent.transform).GetComponent<ScoreItem>();
            scoreItem.gameObject.name = score.username;
            scoreItem.InsertScoreInfo(score.username, score.score, rank);
            rank++;

        }
        //scores.ForEach(score => { });
        //foreach (var pair in scores)
        //{
        //    ScoreItem scoreItem = Instantiate(scorePrefab, scoreParent.transform).GetComponent<ScoreItem>();
        //    //scoreItem.gameObject.name = pair.Item1;
        //    scoreItem.InsertScoreInfo(pair.Item1, pair.Item2, rank);
        //    rank++;
        //}
    }
}
