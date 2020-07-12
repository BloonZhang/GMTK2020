using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextManager : MonoBehaviour
{
    //////// Singleton shenanigans ////////
    private static TextManager _instance;
    public static TextManager Instance { get {return _instance;} }
    //////// Singleton shenanigans continue in Awake() ////

    // public variable
    public int maxMessages = 25; // private or public?
    public TMP_Text textLog;

    public GameObject chatPanel;
    public GameObject textObject;
    public TMP_InputField inputField;

    public string[] speakerArray;
    public Color[] colorArray;

    // private variables
    [SerializeField]
    List<Message> messageList = new List<Message>();
    private bool gameOver = false;

    void Awake() {
        // Singleton shenanigans
        if (_instance != null && _instance != this) {Destroy(this.gameObject);} // no duplicates
        else {_instance = this;}

        // Set up color dictionary for Message class
        if (speakerArray.Length != colorArray.Length) { Debug.Log("Error: speakerArray.Length != colorArray.Length"); }
        else {for (int i = 0; i < speakerArray.Length; ++i) {Message.colorDict.Add(speakerArray[i], colorArray[i]);} }
    }
    /*
    public void testButton1()
    {
        ShipManager.Instance.SetShipVert("superslow", 1);
    }
    */

    void Update() {
        // If input field is not empty
        if (inputField.text != "")
        {
            // On Enter
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SendMessageToChat("Player", inputField.text);
                TextParser.Instance.Parse(inputField.text);
                inputField.text = "";
            }
        }

        /*
        // test code
        if (!inputField.isFocused)
        {
            //inputField.ActivateInputField();
            if(Input.GetKeyDown(KeyCode.CapsLock)) {SendMessageToChat("System", "You pressed CapsLock! Good work");}
        }
        */

        // Input field is always active if game not over
        if (!gameOver && !inputField.isFocused) {inputField.ActivateInputField();}
        // Input field is always deactivated if game over
        if (gameOver && inputField.isFocused) {inputField.DeactivateInputField();}
    }

    // Public methods
    public void SendMessageToChat(string speaker, string text)
    {
        // check if too many messages
        if (messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].myTextObject.gameObject);
            messageList.Remove(messageList[0]);
        }
        // Create new message
        Message newMessage = new Message(speaker, text);
        // Instantiate new text object
        newMessage.InstantiateMessage(textObject, chatPanel);

        // Add to message list
        messageList.Add(newMessage);
    }
    public void EndGame()
    {
        gameOver = true;
    }

}

// Message class
[System.Serializable]
public class Message
{

    // public variables
    public string speaker;
    public string myText;
    public Color myColor;
    public TMP_Text myTextObject;
    public static Dictionary<string, Color> colorDict = new Dictionary<string, Color>();

    // Constructors
    public Message(){speaker = ""; myText = "";} // Must be constructed with text. Default is ""
    public Message(string speaker, string text) 
    {
        this.speaker = speaker; 
        myText = speaker + ": " + text;
        SetColor(speaker);
    }

    // public methods
    // Method for changing text, color, etc. of text object prefab
    public void InstantiateMessage(GameObject textObject, GameObject chatPanel)
    {
        GameObject newText = GameObject.Instantiate(textObject, chatPanel.transform);
        // Set up myTextObject
        myTextObject = newText.GetComponent<TMP_Text>();
        // Change the text, colors, etc.
        myTextObject.text = myText;
        myTextObject.color = myColor;
    }

    // Static method for setting up the color dictionary
    public static void AddToColorDict(string player, Color color) {colorDict.Add(player, color);}

    // helper methods
    private void SetColor(string speaker)
    {
        
        // Set color based on speaker
        if (colorDict.ContainsKey(speaker)) {myColor = colorDict[speaker];}
        else {myColor = Color.white;}
        
    }

}
