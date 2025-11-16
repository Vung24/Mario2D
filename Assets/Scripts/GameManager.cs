using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject gameOverUi;
    [SerializeField] private GameObject gameWinUi;
    private bool isGameOver = false;
    private bool isGameWin = false;

    // Start is called before the first frame update
    void Start()
    {
        UpdateScore();
        gameOverUi.SetActive(false);
        gameWinUi.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void AddScore(int points)
    {
        if (!isGameOver && !isGameWin)
        {
            score += points;
            UpdateScore();           
        }

    }
    private void UpdateScore()
    {
        scoreText.text = score.ToString();
    }
    public void GameOver()
    {
        isGameOver = true;
        score = 0;
        Time.timeScale = 0;
        gameOverUi.SetActive(true);
    }
    public void GameWin()
    {
        isGameWin = true;
        Time.timeScale = 0;
        gameWinUi.SetActive(true);
    }
    public void RestartGame()
    {
        isGameOver = false;
        score = 0;
        UpdateScore();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);    }
    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }
    public bool IsGameOver()
    {
        return isGameOver;
    }
    public bool IsGameWin()
    {
        return isGameWin;
    }
}

