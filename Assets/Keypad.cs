using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Keypad : MonoBehaviour
{
    private static readonly int MAX_DIGITS = 4;

    public TMPro.TextMeshPro display;
    public string code;

    public UnityEvent correctCodeEntered;
    public UnityEvent incorrectCodeEntered;

    private string text = string.Empty;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateDisplay()
    {
        display.text = text;
    }

    private void SetButtonsInteractable(bool interactable)
    {
        var buttonContainer = transform.GetChild(0);
        for (int i = 0; i < buttonContainer.childCount; i++)
        {
            var child = buttonContainer.GetChild(i);

            var xrGrabInteractable = child.gameObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.XRGrabInteractable>();
            if (xrGrabInteractable != null)
            {
                xrGrabInteractable.enabled = interactable;
            }
        }
    }

    /**
     * Helper method that disables all buttons, turns the display red and invokes a timed execution of _incorrectCodeEnteredEnd.
     **/
    private void _incorrectCodeEnteredStart()
    {
        SetButtonsInteractable(false);
        display.transform.parent.GetComponent<Renderer>().material.color = Color.red;

        Invoke("_incorrectCodeEnteredEnd", 3);
    }

    /**
     * Helper method that enables all buttons, resets the display color and the text.
     **/
    private void _incorrectCodeEnteredEnd()
    {
        SetButtonsInteractable(true);
        display.transform.parent.GetComponent<Renderer>().material.color = Color.white;
        text = string.Empty;
        UpdateDisplay();
    }

    /**
     * Appends a string to the keypad text and updates the display.
     * If the string is too long, aborts.
     **/
    public void AppendString(string c)
    {
        if (text.Length + c.Length > MAX_DIGITS)
        {
            return;
        }

        text += c;
        UpdateDisplay();
    }

    /**
     * Clears the content of the keypad and updates the display.
     **/
    public void ClearText()
    {
        text = string.Empty;
        UpdateDisplay();
    }

    /**
     * Checks the currently entered code.
     * If correct, invokes the correctCodeEntered event, makes the display background green and disables all buttons.
     * If incorrect, invokes the incorrectCodeEntered event and temporarily makes the display background red and disables all buttons. Also clears the text.
     **/
    public void Enter()
    {
        if (text == code)
        {
            correctCodeEntered.Invoke();
            display.transform.parent.GetComponent<Renderer>().material.color = Color.green;
            SetButtonsInteractable(false);
        } else
        {
            incorrectCodeEntered.Invoke();
            _incorrectCodeEnteredStart();
        }
    }
}
