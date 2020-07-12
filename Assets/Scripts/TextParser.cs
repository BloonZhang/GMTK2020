using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextParser : MonoBehaviour
{
    //////// Singleton shenanigans ////////
    private static TextParser _instance;
    public static TextParser Instance { get {return _instance;} }
    //////// Singleton shenanigans continue in Awake() ////

    // Word dictionaries
    private List<string> slowDictionary = new List<string>()
    {"slow", "slowly"};
    private List<string> fastDictionary = new List<string>()
    {"fast", "fastly", "quick", "quickly", "hurry"};
    private List<string> adverbsDictionary = new List<string>()
    {"very", "super", "extremely", "really"};
    private List<string> stopDictionary = new List<string>()
    {"stop", "quit", "break", "pause", "freeze"};
    private List<string> moveDictionary = new List<string>()
    {"move","moving", "drive", "driving", "fly", "flying", "dodge","dodging,", "go", "going"};
    private List<string> leftDictionary = new List<string>()
    {"left", "port", "leftward", "portbound"};
    private List<string> rightDictionary = new List<string>()
    {"right", "starboard", "rightward"};
    private List<string> upDictionary = new List<string>()
    {"up", "forward", "forwards", "front", "top", "upward", "upwards", "bow", "proceed"};
    private List<string> downDictionary = new List<string>()
    {"down", "backward", "backwards", "back", "backup", "bottom", "downward", "downwards", "stern", "retreat"};
    private List<string> shootDictionary = new List<string>()
    {"shoot", "shooting", "fire", "firing"};
    private List<string> repairDictionary = new List<string>()
    {"repair", "repairing", "fix", "fixing", "maintenance"};
    private List<string> andDictionary = new List<string>()
    {"and", "adn", "also", "aslo", "alos"};
    /*
    private List<string> fillerWords = new List<string>()
    {"captain", "pilot"};
    */

    // public variables
    public string pilotName = "Pilot";
    public float minDelayTime = 0.3f;
    public float maxDelayTime = 0.5f;
    public float slightlyConfusedTime = 0.4f;

    //Awake
    void Awake()
    {
        // Singleton shenanigans
        if (_instance != null && _instance != this) {Destroy(this.gameObject);} // no duplicates
        else {_instance = this;}
    }

    // Start is called before the first frame update
    void Start()
    {
        // Test code
        //ShipManager.Instance.SetShipHorz("fast", 1);
    }

    // Main Parse method
    public void Parse(string input)
    {
        // How long it takes for the message to arrive
        float delay = Random.Range(minDelayTime, maxDelayTime);
        //Debug.Log(delay);

        // Find individual words
        //string[] words = input.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
        string[] words = input.Split(' ');
        //foreach(string str in words) {Debug.Log(str);}

        // Empty Lists to hold what we need to do after we Parse, and how quickly we need to do it
        // Probably going to be very hardcoded
        /*
        valid strings for commands:
        MovePlaceholder
        MoveLeft, MoveRight
        MoveUp, MoveDown
        Stop
        Shoot
        Repair
        */
        List<string> commands = new List<string>();
        /*
        valid strings for speeds
        SuperPlaceholder
        superslow, slow, normal, fast, superfast
        */
        List<string> speeds = new List<string>();
        // bool for if the driver is slightly confused
        bool slightlyConfused = false;
        // Command number. will increment on "and", "also", "adn", "aslo", "alos"
        int i = 0;
        commands.Add(""); speeds.Add("normal");

        // Go through the words and see what needs to be done
        // if an order or speed overrides an existing order or speed, pilot will be confused
        foreach(string parsedWord in words)
        {
            // Remove punctuation and make all lowercase
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach(char c in parsedWord) {if (!char.IsPunctuation(c)){sb.Append(c);}}
            string word = sb.ToString();
            word = word.ToLower();
            // Movement command
            if (moveDictionary.Contains(word)) 
            {
                if (commands[i] != "") {StartCoroutine(confusedCommand(commands[i], "MovePlaceholder", delay)); break;}
                commands[i] = "MovePlaceholder";
                //speeds[i] = (speeds[i] == "") ? "normal" : speeds[i];
            }

            // Direction command
            // Up
            else if (upDictionary.Contains(word)) 
            {
                if (!(commands[i] == "MovePlaceholder"||commands[i] == "")) {StartCoroutine(confusedCommand(commands[i], "MoveUp", delay)); break;}
                commands[i] = "MoveUp"; //speeds[i] = (speeds[i] == "") ? "normal" : speeds[i];
            }
            // Down
            else if (downDictionary.Contains(word)) 
            {
                if (!(commands[i] == "MovePlaceholder"||commands[i] == "")) {StartCoroutine(confusedCommand(commands[i], "MoveDown", delay)); break;}
                commands[i] = "MoveDown"; //speeds[i] = (speeds[i] == "") ? "normal" : speeds[i];
            }
            // Right
            else if (rightDictionary.Contains(word)) 
            {
                if (!(commands[i] == "MovePlaceholder"||commands[i] == "")) {StartCoroutine(confusedCommand(commands[i], "MoveRight", delay)); break;}
                commands[i] = "MoveRight"; //speeds[i] = (speeds[i] == "") ? "normal" : speeds[i];
            }
            // Left
            else if (leftDictionary.Contains(word)) 
            {
                if (!(commands[i] == "MovePlaceholder"||commands[i] == "")) {StartCoroutine(confusedCommand(commands[i], "MoveLeft", delay)); break;}
                commands[i] = "MoveLeft"; //speeds[i] = (speeds[i] == "") ? "normal" : speeds[i];
            }

            // super
            else if (adverbsDictionary.Contains(word))
            {
                // Empty
                if (speeds[i] == "") {speeds[i] = "SuperPlaceholder";}
                // Not empty
                else if (speeds[i] == "slow") {speeds[i] = "superslow";}
                else if (speeds[i] == "fast") {speeds[i] = "superfast";}
                // no match
                else {slightlyConfused = true;}
            }

            // Speed command
            else if (slowDictionary.Contains(word))
            {
                // Multiple speeds
                if (speeds[i]=="superfast"||speeds[i]=="fast") {StartCoroutine(confusedCommand("fast", "slow", delay)); break;}
                // Super placeholder
                if (speeds[i] == "SuperPlaceholder") {speeds[i] = "superslow";}
                else {speeds[i] = "slow";}
            }
            else if (fastDictionary.Contains(word))
            {
                if (speeds[i]=="superslow"||speeds[i]=="slow") {StartCoroutine(confusedCommand("slow", "fast", delay)); break;}
                if (speeds[i] == "SuperPlaceholder") {speeds[i] = "superfast";}
                else {speeds[i] = "fast";}
            }
            // Stop command
            // TODO: "stop moving down" "stop moving right" instead of just full stop
            else if (stopDictionary.Contains(word))
            {
                if (commands[i] != "") {StartCoroutine(confusedCommand(commands[i], "Stop", delay)); break;}
                commands[i] = "Stop";
            }

            // Shoot command
            else if (shootDictionary.Contains(word))
            {
                if (commands[i] != "") {StartCoroutine(confusedCommand(commands[i], "Shoot", delay)); break;}
                commands[i] = "Shoot";
            }

            // Repair command
            else if (repairDictionary.Contains(word))
            {
                if (commands[i] != "") {StartCoroutine(confusedCommand(commands[i], "Repair", delay)); break;}
                commands[i] = "Repair";
            }

            // and command
            else if (andDictionary.Contains(word)) {++i; commands.Add(""); speeds.Add("normal");}

            // No matches
            else {slightlyConfused = true;}
        }

        // We have our commands and speeds. Time to make them work
        /*
        // test code
        foreach(string str in commands){Debug.Log(str);}
        foreach(string str in speeds){Debug.Log(str);}
        */
        // No valid command was input
        if (commands.Count == 1 && commands[0] == "") {StartCoroutine(cantUnderstandCommand(delay));}
        // At least one of the commands was invalid
        else if (commands.Contains("") || speeds.Contains("SuperPlaceholder")) {StartCoroutine(cantUnderstandCommand(delay));}
        // Ambiguous movement command
        else if (commands.Contains("MovePlaceholder")) {StartCoroutine(noDirection(delay));}
        // Everything has been sorted out
        else 
        {
            // Slightly confused, but still carries out instructions
            if (slightlyConfused) {StartCoroutine(slightlyConfusedCommand(delay)); delay += slightlyConfusedTime;}
            // Execute all commands
            for (int commandNo = 0; commandNo < commands.Count; ++commandNo)
            {
                /*
                valid strings for commands:
                MovePlaceholder
                MoveLeft, MoveRight
                MoveUp, MoveDown
                Stop
                Shoot
                Repair
                valid strings for speeds
                SuperPlaceholder
                stop, superslow, slow, normal, fast, superfast
                */
                switch (commands[commandNo])
                {
                    case "MoveLeft":
                        StartCoroutine(moveHorzCommand(speeds[commandNo], -1, delay));
                        break;
                    case "MoveRight":
                        StartCoroutine(moveHorzCommand(speeds[commandNo], 1, delay));
                        break;
                    case "MoveUp":
                        StartCoroutine(moveVertCommand(speeds[commandNo], 1, delay));
                        break;
                    case "MoveDown":
                        StartCoroutine(moveVertCommand(speeds[commandNo], -1, delay));
                        break;
                    case "Stop":
                        StartCoroutine(stopCommand(delay));
                        break;
                    case "Shoot":
                        StartCoroutine(shootCommand(speeds[commandNo], delay));
                        break;
                    case "Repair":
                        StartCoroutine(repairCommand(speeds[commandNo], delay));
                        break;
                    default:
                        Debug.Log("Error in Parse switch statement");
                        cantUnderstandCommand(delay);
                        break;
                }
            }
        }

    }


    // Command methods
    // TODO: response delays
    private IEnumerator moveHorzCommand(string speed, float direction, float delay)
    {
        yield return new WaitForSeconds(delay);
        // TODO: print to log better
        TextManager.Instance.SendMessageToChat(pilotName, "Got it. Flying " + ((direction == 1) ? "right" : "left") + ".");
        ShipManager.Instance.SetShipHorz(speed, direction);
        yield return null;
    }
    private IEnumerator moveVertCommand(string speed, float direction, float delay)
    {
        yield return new WaitForSeconds(delay);
        // TODO: print to log
        TextManager.Instance.SendMessageToChat(pilotName, "Got it. Flying " + ((direction == 1) ? "up" : "down") + ".");
        ShipManager.Instance.SetShipVert(speed, direction);
        yield return null;
    }
    private IEnumerator stopCommand(float delay)
    {
        yield return new WaitForSeconds(delay);
        // TODO: print to log better
        TextManager.Instance.SendMessageToChat(pilotName, "Stopping.");
        ShipManager.Instance.SetShipStop();
    }
    private IEnumerator shootCommand(string speed, float delay)
    {
        yield return new WaitForSeconds(delay);
        // TODO: print to log
        TextManager.Instance.SendMessageToChat(pilotName, "Shooting.");
        ShipManager.Instance.SetShipShoot(speed);
        yield return null;
    }
    private IEnumerator repairCommand(string speed, float delay)
    {
        yield return new WaitForSeconds(delay);
        // TODO: print to log
        TextManager.Instance.SendMessageToChat(pilotName, "Reparing.");
        ShipManager.Instance.SetShipRepair(speed);
        yield return null;
    }
    // Can't understand command
    private IEnumerator cantUnderstandCommand(float delay)
    {
        yield return new WaitForSeconds(delay);
        // TODO: print to log
        TextManager.Instance.SendMessageToChat(pilotName, "Not sure what you mean.");
        yield return null;
    }
    // Multiple commands in one phrase
    private IEnumerator confusedCommand(string command1, string command2, float delay)
    {
        yield return new WaitForSeconds(delay);
        // TODO: print to log
        TextManager.Instance.SendMessageToChat(pilotName, "Could you be more clear?");
        yield return null;
    }
    // Movement command with no direction
    private IEnumerator noDirection(float delay)
    {
        yield return new WaitForSeconds(delay);
        // TODO: print to log
        TextManager.Instance.SendMessageToChat(pilotName, "Which way shoud I fly?");
        yield return null;
    }
    // Pilot is slightly confused. Still performs the other command
    private IEnumerator slightlyConfusedCommand(float delay)
    {
        yield return new WaitForSeconds(delay);
        TextManager.Instance.SendMessageToChat(pilotName, "Uh...");
        yield return null;
    }

}
