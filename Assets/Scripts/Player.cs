using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float SpeedMod => (1 - coins / 100);
    public int damageCost = 50;
    public int coins = 0;

    [SerializeField]
    private GameObject winScreen, loseScreen, menuScreen;

    private void Start() {
        Time.timeScale = 0;
        menuScreen.SetActive(true);
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }

    public void TakeDamage() {
        Debug.Log("take damage");
        if (coins >= damageCost) {
            coins -= damageCost;
            transform.position += new Vector3(0, 10, 0);
        }
        else {
            Lose();
        }
    }

    public void CollectCoin() {
        Debug.Log("collect coin");
        coins++;
    }

    public void Win() {
        Time.timeScale = 0;
        Debug.Log("won");
        winScreen.SetActive(true);
    }

    public void Lose() {
        Time.timeScale = 0;
        Debug.Log("lost");
        loseScreen.SetActive(true);
    }

    public void StartGame() {
        Time.timeScale = 1;
        menuScreen.SetActive(false);
    }
    

    public void Reload() {
        SceneManager.LoadScene(0);
        Time.timeScale = 0;
    }

    public void Quit() {
        Application.Quit();
    }
}