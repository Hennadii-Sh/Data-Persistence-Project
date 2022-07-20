using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class DataPersistanceManager : MonoBehaviour
{
    public static DataPersistanceManager Instance;

    [SerializeField] private int score;
    private int maxPlayerNameLength = 10;

    [SerializeField] private string playerName;
    public string PlayerName
    {
        get => playerName;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) { playerName = "Player"; }
            else if (value.Length <= maxPlayerNameLength) playerName = value;
            else playerName = value.Remove(maxPlayerNameLength);
        }
    }
   
    
    public int Highscore { get; set; } = 0;
    public string HighscoreName { get; set; } = "Player";

    private void Awake()
    {
        //Making SINGLETONE:
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist singletone between scenes.
    }



    [System.Serializable]
    class SaveData
    {
        public int highscore;
        public string highscoreName;
    }

    public void SaveHighScore()
    {
        SaveData data = new();
        data.highscore = Highscore;
        data.highscoreName = HighscoreName;

        string json = JsonUtility.ToJson(data);
        string path = Application.persistentDataPath + "/savefile.json";
        File.WriteAllText(path, json);
    }
    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Highscore = data.highscore;
            HighscoreName = data.highscoreName;
        }
    }
}
