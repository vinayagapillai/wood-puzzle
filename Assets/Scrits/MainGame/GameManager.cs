using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject GameOverPanel;
    private void OnEnable()
    {
        GameEvents.ShowGameOverScreen += ShowGameOverScreen;
    }

    private void OnDisable()
    {
        GameEvents.ShowGameOverScreen -= ShowGameOverScreen;
    }


    public void ShowGameOverScreen()
    {
        GameOverPanel.SetActive(true);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("MainGame");
    }

}
