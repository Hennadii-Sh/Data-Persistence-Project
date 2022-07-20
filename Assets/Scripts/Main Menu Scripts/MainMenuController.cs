using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] DataPersistanceManager dataPersistanceManager;
    [SerializeField] private TextMeshProUGUI highscoreText;
    [SerializeField] private TMP_InputField playerNameInputField;

    private void Start()
    {
        dataPersistanceManager = GameObject.Find("Data Persistance Manager").GetComponent<DataPersistanceManager>();

        dataPersistanceManager.LoadHighScore();
        SetHighscoreText();
    }
    public void StartGame()
    {
        SavePlayerName();
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }

    private void SavePlayerName()
    {
        dataPersistanceManager.PlayerName = playerNameInputField.text;
    }

    private void SetHighscoreText()
    {
        highscoreText.text = dataPersistanceManager.HighscoreName + ": " + dataPersistanceManager.Highscore;
    }

    // TESTING METHOD!
    public void ResetHighscore()
    {
        dataPersistanceManager.Highscore = 0;
        dataPersistanceManager.HighscoreName = "Player";
        dataPersistanceManager.SaveHighScore();
        highscoreText.text = $"{dataPersistanceManager.HighscoreName}: {dataPersistanceManager.Highscore}";
    }
}
