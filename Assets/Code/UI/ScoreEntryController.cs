using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UIElements;

public class ScoreEntryController
{
    Label nameLabel,pointsLabel;
    public void SetVisualElement(VisualElement visualElement)
    {
        nameLabel = visualElement.Q<Label>("player-name");
        pointsLabel = visualElement.Q<Label>("points");
    }

    //This function receives the character whose name this list 
    //element displays. Since the elements listed 
    //in a `ListView` are pooled and reused, it's necessary to 
    //have a `Set` function to change which character's data to display.

    public void SetScoreInfo(string username,int points)
    {
        nameLabel.text = username;
        pointsLabel.text = points + "";
    }
}
