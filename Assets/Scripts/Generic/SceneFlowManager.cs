using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFlowManager : MonoBehaviour
{
    #region Variables

    public static SceneFlowManager Instance;

    [SerializeField]
    private GameObject winScreen, loseScreen, menuScreen;

    #endregion

    private void Awake() {
        Instance = this;
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
    }

    public void Play() {
        Time.timeScale = 1;
        menuScreen.SetActive(false);
    }

    public void ReloadScene() {
        SceneManager.LoadScene(0);
    }
}