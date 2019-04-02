using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private InputField username;

    [SerializeField]
    private Button start;

    [SerializeField]
    private Button load;

    private string usernameText;

    [SerializeField]
    private AudioSource buttonAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        var se = new InputField.SubmitEvent();
        se.AddListener(SubmitName);
        username.onEndEdit = se;

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void SubmitName(string nameText)
    {
        if (nameText.Length > 0)
        {
            usernameText = nameText;
            Debug.Log(nameText);
            start.interactable = true;
            load.interactable |= SaveFileExists();
        }
    }

    public void StartGame()
    {
        buttonAudioSource.PlayOneShot(buttonAudioSource.clip);
        PlayerPrefs.SetString("username", usernameText);
        PlayerPrefs.SetString("gameMode", "START");
        SceneManager.LoadScene("Game");
    }

    public void LoadGame()
    {
        buttonAudioSource.PlayOneShot(buttonAudioSource.clip);
        PlayerPrefs.SetString("username", usernameText);
        PlayerPrefs.SetString("gameMode", "LOAD");
        SceneManager.LoadScene("Game");
    }

    public bool SaveFileExists()
    {
        string filename = Application.persistentDataPath + "/" + usernameText + ".dat";

        return File.Exists(filename);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
