using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening; // DOTween for animation

public class StartMenuManager : MonoBehaviour
{
    public TMP_InputField NameInput;
    public TMP_Text HighScoreText; // TextMeshPro for high score display

    private void Start()
    {
        // Load and display the high score
        string highScorePlayer = PlayerPrefs.GetString("HighScorePlayer", "None");
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        HighScoreText.text = $"High Score {highScorePlayer} : {highScore}";
    }

    public void StartGame()
    {
        string playerName = NameInput.text;
        if (!string.IsNullOrEmpty(playerName))
        {
            PlayerPrefs.SetString("PlayerName", playerName); // Save player name
            SceneManager.LoadScene("main");
        }
        else
        {
            Debug.Log("Please enter a name to start the game.");
            ShakeNameInput();
        }
    }

    private void ShakeNameInput()
    {
        Vector3 originalPosition = NameInput.transform.localPosition;
        NameInput.transform.DOShakePosition(0.5f, new Vector3(10f, 0, 0), 10, 90, false, true)
            .OnComplete(() => NameInput.transform.localPosition = originalPosition);
    }
}