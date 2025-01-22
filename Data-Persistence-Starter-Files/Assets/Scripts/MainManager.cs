using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public TextMeshProUGUI ScoreText;
    public GameObject GameOverCanvas;
    public TextMeshProUGUI FinalScoreText;
    public TextMeshProUGUI NewHighScoreText;
    public Button RestartButton;
    public Button MainMenuButton;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;

    private string m_PlayerName;
    private int m_HighScore;
    private string m_HighScorePlayer;

    private const string HighScoreKey = "HighScore";
    private const string HighScorePlayerKey = "HighScorePlayer";

    void Start()
    {

        m_PlayerName = PlayerPrefs.GetString("PlayerName", "Player");
        LoadHighScore();

        GameOverCanvas.SetActive(false);
        NewHighScoreText.gameObject.SetActive(false);

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RestartGame();
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score  {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverCanvas.SetActive(true);

        // Hide the score text
        ScoreText.gameObject.SetActive(false);

        FinalScoreText.text = $"Your Score: {m_Points}";

        if (m_Points > m_HighScore)
        {
            m_HighScore = m_Points;
            m_HighScorePlayer = m_PlayerName;
            SaveHighScore();
            NewHighScoreText.gameObject.SetActive(true);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetInt(HighScoreKey, m_HighScore);
        PlayerPrefs.SetString(HighScorePlayerKey, m_HighScorePlayer);
        PlayerPrefs.Save();
    }

    private void LoadHighScore()
    {
        m_HighScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        m_HighScorePlayer = PlayerPrefs.GetString(HighScorePlayerKey, "None");
    }

}
