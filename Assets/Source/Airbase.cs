using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Airbase : MonoBehaviour {

    [SerializeField]
    private int HP = 9000;

    [SerializeField]
    private Slider hpUI;

    [SerializeField]
    private Text scoreUI;

    [SerializeField]
    private Text finalScoreUI;

    [SerializeField]
    private Text gameWonScoreUI;

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

    public static int Score { get; set; }

    private void Start() {
        Time.timeScale = 1;
        hpUI.maxValue = HP;
        hpUI.value = HP;

        RenderSettings.skybox = skyboxes[Random.Range(0, skyboxes.Count)];

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
                rb.AddTorque(new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20)));
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
        Debug.Log("Pausing");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void UnPause() {
        Time.timeScale = 1;
        isPaused = false;
        Debug.Log("Unpausing from paused");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void Restart() {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
        Score = 0;
    }

    public void Quit() {
        Application.Quit();
    }
}
