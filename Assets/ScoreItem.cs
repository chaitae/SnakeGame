using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreItem : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text nameText;
    public Image img;
    public Color subColor;
    public Color primaryColor;
    public void InsertScoreInfo(string username, int score,int rank)
    {
        if (rank%2 == 0) { img.color = primaryColor; }
        else { img.color = subColor; }
        nameText.text = rank + ". " + username;
        scoreText.text = score.ToString();
    }
}
