using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // public variables
    public GameObject astroidPrefab;
    public GameObject astroidSpawnZone;
    public float astroidStartingSpawnRate; // set in editor
    public float astroidFinalSpawnRate;
    public float levelTime;

    // private variables
    private float timer;
    private float timeSinceLastAstroid;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        timeSinceLastAstroid = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // update timer
        timer += Time.deltaTime;

        // update time since last astroid
        timeSinceLastAstroid += Time.deltaTime;
        // Check if it's time to spawn a new astroid
        if (timeSinceLastAstroid > (astroidFinalSpawnRate - astroidStartingSpawnRate) * (timer/levelTime) + astroidStartingSpawnRate)
        {
            Debug.Log("launched after " + timeSinceLastAstroid + " secs");
            timeSinceLastAstroid = 0f;
            // Choose a random area in astroid spawn zone
            Instantiate(astroidPrefab, new Vector3(Random.Range(-5.5f, 2f), astroidSpawnZone.transform.position.y, 0), Quaternion.identity);
        }
    }
}
