using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextOption : MonoBehaviour
{
    [SerializeField] private Button textButton;
    [SerializeField] private TextMeshProUGUI text;
    private string _option;
    public event Action<string> _optionSelected;

    private void Awake()
    {
        textButton.onClick.AddListener(() =>
        {
            _optionSelected?.Invoke(_option);
        });
    }

    public void InitializeOptin(string option, Action<string> optionSelected)
    {
        _optionSelected = optionSelected;
        _option = option;
        text.text = _option;
    }

    public string GetOptinText()
    {
        return _option;
    }


    public void Destroy()
    {
        Destroy(gameObject);
    }
}
