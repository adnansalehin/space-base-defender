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

    // Start is called before the first frame update
    void Start()
    {
        var se = new InputField.SubmitEvent();
        se.AddListener(SubmitName);
        username.onEndEdit = se;

        //StartCoroutine(CheckForUsername());
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
        //overwrite existing file. 
        //Airbase.PlayerData pd = new Airbase.PlayerData
        //{
        //    username = usernameText,
        //    allTimeTotalScore = 0,
        //    allTimeHighScore = 0
        //};
        //string filename = Application.persistentDataPath + "/" + usernameText + ".dat";
        //FileStream file = File.Open(filename, FileMode.Create);
        //BinaryFormatter bf = new BinaryFormatter();
        //bf.Serialize(file, pd);
        //file.Close();
        PlayerPrefs.SetString("username", usernameText);
        PlayerPrefs.SetString("gameMode", "START");
        SceneManager.LoadScene("Game");
    }

    public void LoadGame()
    {
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
