using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextManager : MonoBehaviour
{

    // public variable
    public int maxMessages = 25; // private or public?
    public TMP_Text textLog;

    public GameObject chatPanel;
    public GameObject textObject;
    public TMP_InputField inputField;

    // private variables
    [SerializeField]
    List<Message> messageList = new List<Message>();


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
                inputField.text = "";
            }
        }

        // test code
        if (!inputField.isFocused)
        {
            //inputField.ActivateInputField();
            if(Input.GetKeyDown(KeyCode.CapsLock)) {SendMessageToChat("System", "You pressed CapsLock! Good work");}
        }

        // Input field is always active
        /*
        if (!inputField.isFocused) {inputField.ActivateInputField();}
        */
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

    // helper methods
    private void SetColor(string speaker)
    {
        // TODO: set color based on speaker. Maybe a Dictionary<string, Color>
        // Or maybe even move it out of the message class?
        // for now, system is blue, otherwise white
        if (speaker.Equals("System")) {myColor = Color.blue;}
        else {myColor = Color.white;}
    }

}
