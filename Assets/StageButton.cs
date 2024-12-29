using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageButton : MonoBehaviour
{
    public TMP_Text stageNameTxt;
    public TMP_Text stageScore;

    public string stageName;
    int score;

    void Start()
    {
        stageNameTxt.text = stageName;
        LoadStageScore();
    }

    void LoadStageScore()
    {
        score = PlayerPrefs.GetInt(stageName + "highScore", 0);
        stageScore.text = "High : " + score;
    }
}
