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
    private GameObject finalPanelUI;

    [SerializeField]
    private GameObject spawner;

    [SerializeField]
    private List<Material> skyboxes;

    private bool gameover;
    private int score;

    public static int Score { get; set; }

    private void Start() {
        hpUI.maxValue = HP;
        hpUI.value = HP;

        RenderSettings.skybox = skyboxes[Random.Range(0, skyboxes.Count)];

        StartCoroutine(ScoreCounter());
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

            finalScoreUI.text = score.ToString();
            gameover = true;
            StartCoroutine(EndGame());
        }
    }

    private IEnumerator ScoreCounter() {
        while (!gameover) {
            scoreUI.text = "Score: " + Score;
            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator EndGame() {
        yield return new WaitForSeconds(10);
        Time.timeScale = .2f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        finalPanelUI.SetActive(true);
    }

    public void Restart() {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }

    public void Quit() {
        Application.Quit();
    }
}
