using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;


public class GameOver : MonoBehaviour
{
    [SerializeField]
    private Text scoreUI;

    // Use this for initializationvoid Start()
    private void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        scoreUI.text = "Score: " + Airbase.Score;
        Debug.Log(Airbase.Score);
    }
}