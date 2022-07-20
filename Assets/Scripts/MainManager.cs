using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    [SerializeField] DataPersistanceManager dataPersistanceManager;
    private bool isPersistanceManagerExist;
    [SerializeField] Text highscoreText;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Points = 0;

        isPersistanceManagerExist = (GameObject.Find("Data Persistance Manager") != null);
        if (isPersistanceManagerExist)
        {
            dataPersistanceManager = GameObject.Find("Data Persistance Manager").GetComponent<DataPersistanceManager>();
            ScoreText.text = $"{dataPersistanceManager.PlayerName} score : {m_Points}";
            highscoreText.text = $"Best score: {dataPersistanceManager.HighscoreName}: {dataPersistanceManager.Highscore}";
        }
            

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
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
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        if (isPersistanceManagerExist)
        {
            m_Points += point;
            ScoreText.text = $"{dataPersistanceManager.PlayerName} score : {m_Points}";
        } else {
            m_Points += point;
            ScoreText.text = $"Score : {m_Points}";
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
        SaveHighScore();
        GameOverText.SetActive(true);
    }

    private void SaveHighScore()
    {
        if (!isPersistanceManagerExist) return; // For testing purpouses, because Data Persistance can be unable.
        if (m_Points > dataPersistanceManager.Highscore)
        {
            dataPersistanceManager.Highscore = m_Points;
            dataPersistanceManager.HighscoreName = dataPersistanceManager.PlayerName;
            dataPersistanceManager.SaveHighScore();

            highscoreText.text = $"Best score: {dataPersistanceManager.HighscoreName}: {dataPersistanceManager.Highscore}";
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }

    public void RestartGameButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void MainMenuGameButton()
    {
        SceneManager.LoadScene(0);
    }
}
