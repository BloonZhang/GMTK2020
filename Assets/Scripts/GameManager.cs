using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //////// Singleton shenanigans ////////
    private static GameManager _instance;
    public static GameManager Instance { get {return _instance;} }
    //////// Singleton shenanigans continue in Awake() ////


    // public variables
    public GameObject astroidPrefab;
    public GameObject astroidSpawnZone;
    public float astroidStartingSpawnRate; // set in editor
    public float astroidFinalSpawnRate;
    public float levelTime;
    public GameObject youWinScreen;
    public GameObject gameOverScreen;

    // private variables
    private float timer;
    private float timeSinceLastAstroid;
    private bool levelOver = false;
    private bool gameOver = false;

    void Awake() {
        // Singleton shenanigans
        if (_instance != null && _instance != this) {Destroy(this.gameObject);} // no duplicates
        else {_instance = this;}
    }

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        timeSinceLastAstroid = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // update timer if game still going
        if (!gameOver) {timer += Time.deltaTime;}
        // Check if level is over
        if (timer > levelTime) {levelOver = true;}

        // update time since last astroid
        timeSinceLastAstroid += Time.deltaTime;
        // Check if it's time to spawn a new astroid
        if (!levelOver && !gameOver && timeSinceLastAstroid > (astroidFinalSpawnRate - astroidStartingSpawnRate) * (timer/levelTime) + astroidStartingSpawnRate)
        {
            timeSinceLastAstroid = 0f;
            // Choose a random area in astroid spawn zone
            Instantiate(astroidPrefab, new Vector3(Random.Range(-5.5f, 2f), astroidSpawnZone.transform.position.y, 0), Quaternion.identity);
        }

        if (timer > levelTime + 5.0f) {EndGame(); levelTime = 50000f; } //to prevent multiple EndGame() calls
    }

    // Public methods
    public void shipIsDead()
    {
        GameOver();
    }

    // Helper methods
    private void EndGame()
    {
        Debug.Log("you win");
        TextManager.Instance.EndGame();
        ShipManager.Instance.SetShipStop();
        youWinScreen.SetActive(true);
    }
    private void GameOver()
    {
        gameOver = true;
        Debug.Log("you lose");
        TextManager.Instance.EndGame();
        gameOverScreen.SetActive(true);
    }
    /*
    public void RestartGame()
    {
        UIManagerScript.Instance.restartGame();
    }
    */
}
