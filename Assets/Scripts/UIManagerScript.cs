using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager.LoadScene()

public class UIManagerScript : MonoBehaviour
{

    //////// Singleton shenanigans ////////
    private static UIManagerScript _instance;
    public static UIManagerScript Instance { get {return _instance;} }
    //////// Singleton shenanigans continue in Awake() ////

    void Awake()
    {
        // Singleton shenanigans
        if (_instance != null && _instance != this) {Destroy(this.gameObject);} // no duplicates
        else {_instance = this;}
        //DontDestroyOnLoad(this.gameObject);
    }

    public void startGame()
    {
        SceneManager.LoadScene("IntroScreen");
    }

    public void restartGame()
    {
        startGame();
    }

    public void finishIntro()
    {
        SceneManager.LoadScene("GameScene");
    }
}
