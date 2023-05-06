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
        }
    }
}
