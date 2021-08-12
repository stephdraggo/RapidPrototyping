using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFlowManager : MonoBehaviour
{
    #region Variables

    public static SceneFlowManager Instance;

    public int points, winThreshold;

    [SerializeField]
    private GameObject winScreen, loseScreen, menuScreen;

    [SerializeField]
    private Image fill;

    #endregion

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        Time.timeScale = 0;
        menuScreen.SetActive(true);
        loseScreen.SetActive(false);
        winScreen.SetActive(false);
        
        UpdateFill();
    }

    public void UpdateFill() {
        fill.fillAmount = (float) points / (float) winThreshold;
    }

    public void CheckEnd() {
        UpdateFill();
        if (points >= winThreshold) {
            Win();
        }
        else if (points <= 0) {
            Lose();
        }
    }

    public void Win() {
        Time.timeScale = 0;
        winScreen.SetActive(true);
    }

    public void Lose() {
        Time.timeScale = 0;
        loseScreen.SetActive(true);
    }

    public void Play() {
        Time.timeScale = 1;
        menuScreen.SetActive(false);
    }

    public void ReloadScene() {
        SceneManager.LoadScene(0);
    }

    public void Quit() {
        Application.Quit();
    }
}