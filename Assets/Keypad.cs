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

    private void _incorrectCodeEnteredStart()
    {
        SetButtonsInteractable(false);
        display.transform.parent.GetComponent<Renderer>().material.color = Color.red;

        Invoke("_incorrectCodeEnteredEnd", 3);
    }

    private void _incorrectCodeEnteredEnd()
    {
        SetButtonsInteractable(true);
        display.transform.parent.GetComponent<Renderer>().material.color = Color.white;
        text = string.Empty;
    }

    public void AppendString(string c)
    {
        if (text.Length + c.Length > MAX_DIGITS)
        {
            return;
        }

        text += c;
        UpdateDisplay();
    }

    public void ClearText()
    {
        text = string.Empty;
        UpdateDisplay();
    }

    public void Enter()
    {
        if (text == code)
        {
            correctCodeEntered.Invoke();
        } else
        {
            incorrectCodeEntered.Invoke();
            _incorrectCodeEnteredStart();
        }
    }
}
