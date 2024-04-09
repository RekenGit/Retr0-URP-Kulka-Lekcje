using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HelpSign : MonoBehaviour, IInteractibleObiects
{
    [SerializeField] private string text;
    [SerializeField] private string secondText;
    [SerializeField] private int fontSize;
    [SerializeField] private TextMeshProUGUI middleTextField;
    [SerializeField] private TextMeshProUGUI firstTextField;
    [SerializeField] private TextMeshProUGUI secondTextField;
    private Transform playerCamera;
    
    void Start()
    {
        playerCamera = GameObject.Find("Main Camera").transform;
        if (string.IsNullOrEmpty(secondText))
        {
            firstTextField.enabled = secondTextField.enabled = false;
            middleTextField.text = text;
            middleTextField.fontSize = fontSize;
        }
        else
        {
            middleTextField.enabled = false; 
            firstTextField.text = text;
            secondTextField.text = secondText;
            firstTextField.fontSize = secondTextField.fontSize = fontSize;
        }
    }

    void Update()
    {
        transform.LookAt(playerCamera);
    }

    public void ObiectInteract() => gameObject.SetActive(false);
    public void ObiectToggle() => gameObject.SetActive(!gameObject.active);
}
