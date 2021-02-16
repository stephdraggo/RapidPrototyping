using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Crashteroids
{
    public class GameManager : MonoBehaviour
    {
        public static int score;
        public Text scoreText;
        public GameObject startButton;
        

        void Start()
        {
            Time.timeScale = 1;
            score = 0;
            startButton.SetActive(false);
        }

        void Update()
        {
            scoreText.text = score.ToString();
        }

        public void EndGame(float _timeAlive)
        {
            Time.timeScale = 0;
            score += (int)_timeAlive;
            startButton.SetActive(true);
            
        }

        public void StartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}