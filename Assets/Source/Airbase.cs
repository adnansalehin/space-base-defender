using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Airbase : MonoBehaviour {

    [SerializeField]
    private int HP = 9000;

    [SerializeField]
    private Slider hpUI;

    [SerializeField]
    private Text scoreUI;

    [SerializeField]
    private Text highScoreUI;

    [SerializeField]
    private Text finalScoreUI;

    [SerializeField]
    private Text gameWonScoreUI;

    [SerializeField]
    private GameObject pausePanelUI;

    [SerializeField]
    private GameObject finalPanelUI;

    [SerializeField]
    private GameObject gameWonPanelUI;

    [SerializeField]
    private GameObject spawner;

    [SerializeField]
    private List<Material> skyboxes;

    private bool gameOver;
    private bool gameWon;
    private bool isPaused;

    public String username;
    public static int Score { get; set; }
    public static int AllTimeOverallScore { get; set; }
    public static int AllTimeHighScore { get; set; } 

    private void Start() {
        Time.timeScale = 1;
        hpUI.maxValue = HP;
        String gameMode = PlayerPrefs.GetString("gameMode");
        PlayerData pd = new PlayerData();
        username = PlayerPrefs.GetString("username");
        if (gameMode == "LOAD")
            pd = LoadGame();
        else
        {
            pd = pd = new PlayerData
            {
                username = username,
                allTimeHighScore = LoadGame().allTimeHighScore,
                currentScore = 0,
                baseHealth = 0
            };
        }
        hpUI.value = pd.baseHealth;
        Score = pd.currentScore;
        AllTimeHighScore = pd.allTimeHighScore;
        highScoreUI.text = "High Score: " + AllTimeHighScore;

        RenderSettings.skybox = skyboxes[UnityEngine.Random.Range(0, skyboxes.Count)];

        StartCoroutine(ScoreCounter());
    }

    public void Update()
    {
        StartCoroutine(CheckPause());

    }

    public void ApplyDamage(int damage) {
        HP -= damage;
        hpUI.value = HP;

        if(HP <= 0) {
            Destroy(spawner);
            GetComponent<BoxCollider>().enabled = false;

            gameObject.AddComponent<Rigidbody>();
            foreach (Transform ch in transform) {
                var rb = ch.gameObject.AddComponent<Rigidbody>();
                rb.AddTorque(new Vector3(UnityEngine.Random.Range(-20, 20), 0, UnityEngine.Random.Range(-20, 20)));
            }

            finalScoreUI.text = Score.ToString();
            gameOver = true;
            StartCoroutine(EndGame());
        }
    }

    private IEnumerator ScoreCounter() {
        while (!gameOver && !gameWon) {
            scoreUI.text = "Score: " + Score;
            if (Score >= 1000) {
                gameWonScoreUI.text = Score.ToString();
                Debug.Log("Hit Winning score");
                gameWon = true;
                StartCoroutine(EndGame());
            }
            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator EndGame() {
        StopCoroutine(ScoreCounter());
        yield return new WaitForSeconds(2);
        SaveGame();
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (gameWon)
        {
            gameWonPanelUI.SetActive(true);
        }
        else if(gameOver)
        {
            finalPanelUI.SetActive(true);
        }
    }


    public IEnumerator CheckPause() {
        if(Input.GetKeyDown(KeyCode.P) && !isPaused) {
            Pause();
        }
        else if(Input.GetKeyDown(KeyCode.P) && isPaused) {
            UnPause();
        }
        yield return new WaitForSeconds(0.1f);
    }
    public void Pause() {
        Time.timeScale = 0;
        isPaused = true;
        pausePanelUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void UnPause() {
        Time.timeScale = 1;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pausePanelUI.SetActive(false);
    }
    public void Restart() {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
        Score = 0;
    }
    public void BackToMain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
        Score = 0;
    }

    public void Quit() {
        Application.Quit();
    }


    //----------------------------------------------------------------
    //Persistence
    //----------------------------------------------------------------
    public void SaveGame()
    {
        string filename = Application.persistentDataPath + "/" + username + ".dat";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(filename, FileMode.OpenOrCreate);

        int highScore;
        if (Score > AllTimeHighScore)
            highScore = Score;
        else
            highScore = AllTimeHighScore;
        PlayerData pd = new PlayerData
        {
            username = username,
            //allTimeTotalScore = Score + LoadGame().allTimeTotalScore,
            allTimeHighScore = highScore,
            currentScore = Score,
            baseHealth = HP
        };
        bf.Serialize(file, pd);
        file.Close();
    }

    public PlayerData LoadGame()
    {
        Debug.Log("Scene 2: Loading game for " + "/" + username + ".dat");

        PlayerData pd = new PlayerData
        {
            username = username,
            //allTimeTotalScore = 0,
            allTimeHighScore = 0,
            currentScore = 0
        };
        string filename = Application.persistentDataPath + "/" + username + ".dat";
        try
        {
            Debug.Log("Trying to load for user: " + username);
            FileStream file = File.Open(filename, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            pd = (PlayerData)bf.Deserialize(file);
            file.Close();
            //Debug.Log(pd.allTimeTotalScore);
            Debug.Log(pd.allTimeHighScore);
        }
        catch (FileNotFoundException)
        {
            Debug.Log("New player created");
        }
        return pd;
    }

    [Serializable]
    public class PlayerData
    {
        public string username;
        //public int allTimeTotalScore;
        public int allTimeHighScore;
        public int currentScore;
        public int baseHealth;
    }
}