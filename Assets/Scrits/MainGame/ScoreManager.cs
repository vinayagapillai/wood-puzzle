using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int Score;
    public int BestScore;

    public TMP_Text PlayingScoreUI;
    public TMP_Text PlayingBestUI;

    public TMP_Text GameoverScoreUI;
    public TMP_Text GameoverBestUI;

    private void OnEnable()
    {
        GameEvents.AddScore += AddScore;
        GameEvents.ShowGameOverScreen += UpdateScoreUIGameOver;
    }

    private void OnDisable()
    {
        GameEvents.AddScore -= AddScore;
        GameEvents.ShowGameOverScreen -= UpdateScoreUIGameOver;
    }


    private void Start()
    {
        Score = 0;
        RetriveScoreFromDisk();
        UpdateScoreUIGamePlaying();
    }

    public void AddScore(int addScore)
    {
        Score += addScore;
        if(Score > BestScore)
        {
            BestScore = Score;
            SaveScoreToDisk();
        }
        UpdateScoreUIGamePlaying();
    }

    private void UpdateScoreUIGamePlaying()
    {
        PlayingScoreUI.SetText(Score.ToString());
        PlayingBestUI.SetText(BestScore.ToString());
    }

    private void UpdateScoreUIGameOver()
    {
        GameoverScoreUI.SetText("Score: " + Score);
        GameoverBestUI.SetText("Best: " + BestScore);
    }

    private void RetriveScoreFromDisk()
    {
        BestScore = PlayerPrefs.GetInt("Best");
    }

    private void SaveScoreToDisk()
    {
        PlayerPrefs.SetInt("Best", BestScore);
        PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        SaveScoreToDisk();
    }

}
