using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using ChaitaesWeb;

public class ScoreboardController : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text nameText;
    public GameObject scorePrefab;
    public GameObject scoreParent;
    // Start is called before the first frame update
    void Awake()
    {
        Web.onGetScores += UpdateScoreboard;
    }
    public void UpdateScoreboard(Dictionary<string,int> scores)
    {
        int rank = 1;
        foreach (var pair in scores)
        {
            ScoreItem scoreItem = Instantiate(scorePrefab, scoreParent.transform).GetComponent<ScoreItem>();
            scoreItem.InsertScoreInfo(pair.Key, pair.Value, rank);
            rank++;
        }
    }
}
