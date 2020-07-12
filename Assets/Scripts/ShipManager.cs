using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    
    //////// Singleton shenanigans ////////
    private static ShipManager _instance;
    public static ShipManager Instance { get {return _instance;} }
    //////// Singleton shenanigans continue in Awake() ////



    /*
    public bool myBool = false; // private?
 
    // and expose static members through properties
    // this way, you have a lot more control over what is actually being sent out.
    public static bool MyBool { get { return fetch ? fetch.myBool : false; } }
    */


    // public variables
    public GameObject bulletPrefab;

    // Enums and dicts
    // For adjusting ship movement speed
    private Dictionary<string, float> shipSpeedDict = new Dictionary<string, float>();



    // ship stats
    private float shipBaseSpeedHorz = 0.04f;
    private float shipBaseSpeedVert = 0.02f;
    private float shipSpeedHorz = 0f; // Set by SetShipHorz()
    private float shipSpeedVert = 0f; // Set by SetShipVert()

    // Awake
    void Awake() 
    {
        // Singleton shenanigans
        if (_instance != null && _instance != this) {Destroy(this.gameObject);} // no duplicates
        else {_instance = this;}

        // shipSpeedDict
        shipSpeedDict.Add("stop", 0f);
        shipSpeedDict.Add("superslow", 0.25f);
        shipSpeedDict.Add("slow", 0.5f);
        shipSpeedDict.Add("normal", 1f);
        shipSpeedDict.Add("fast", 1.25f);
        shipSpeedDict.Add("superfast", 1.5f);
    }

    // Update
    void FixedUpdate()
    {
        // move horz and vert
        transform.position = new Vector2(
            transform.position.x + (shipBaseSpeedHorz * shipSpeedHorz), 
            transform.position.y + (shipBaseSpeedVert * shipSpeedVert)
            );
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        // touches the side
        if (col.gameObject.tag == "Boundary") {
            SetShipStop();
            switch (col.gameObject.name)
            {
                case "RightBoundary":
                    transform.position = new Vector2(
                    transform.position.x - 0.05f, 
                    transform.position.y);
                    break;
                case "LeftBoundary":
                    transform.position = new Vector2(
                    transform.position.x + 0.05f, 
                    transform.position.y);
                    break;
                case "TopBoundary":
                    transform.position = new Vector2(
                    transform.position.x, 
                    transform.position.y - 0.05f);
                    break;
                case "BottomBoundary":
                    transform.position = new Vector2(
                    transform.position.x, 
                    transform.position.y + 0.05f);
                    break;
                default:
                    Debug.Log("Error in ShipManager switch");
                    break;
            }
        }
    }

    // Methods to control ship
    public void SetShipHorz(string speed, float direction) { shipSpeedHorz = shipSpeedDict[speed] * direction; }
    public void SetShipVert(string speed, float direction) { shipSpeedVert = shipSpeedDict[speed] * direction; }
    //public void testMethod(string speed, float direction) {return;}
    public void SetShipStop() { SetShipHorz("stop", 0f); SetShipVert("stop", 0f); }
    public void SetShipShoot(string speed)
    {
        // TODO: shooting
        Instantiate(bulletPrefab, this.gameObject.transform.position, Quaternion.identity);
    }
    public void SetShipRepair(string speed)
    {
        // TODO: repairing
    }

    // Helper Methods
    private float stringToShipSpeed(string speed) {
        if (shipSpeedDict.ContainsKey(speed)) {return shipSpeedDict[speed];} 
        else {Debug.Log("Error in GetShipSpeed"); return 0f;}
    }
}
