using System;
using BeesNuts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFlowManager : MonoBehaviour
{
    #region Variables

    public static SceneFlowManager Instance;

    [SerializeField]
    private GameObject winScreen, loseScreen, menuScreen;
    [SerializeField]
    private Text score, hudHoney, hudScore, hudHealth;

    #endregion

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        hudHoney.text = Player.Instance.honeyComb + " Honey on Hand";
        hudScore.text = DepositBox.Instance.score + " Honey Delivered";
        hudHealth.text = Player.Instance.health + " Immunity";
    }

    private void Start() {
        Time.timeScale = 0;
        menuScreen.SetActive(true);
        loseScreen.SetActive(false);
        winScreen.SetActive(false);
    }

    public void Win() {
        Time.timeScale = 0;
        winScreen.SetActive(true);
    }

    public void Lose() {
        Time.timeScale = 0;
        loseScreen.SetActive(true);
        score.text = "Score: " + DepositBox.Instance.score;
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