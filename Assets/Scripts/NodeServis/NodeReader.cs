using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XNode;
using static DialogueNode;


public class NodeReader : MonoBehaviour
{
    [SerializeField]
    private TextOption textPrefab;
    [SerializeField]
    private Transform optionsContainer;
    [SerializeField]
    private DialogueGraf graf;
    [SerializeField]
    private TextMeshProUGUI speaker;
    [SerializeField]
    private TextMeshProUGUI dialogue;
    [SerializeField]
    private Image speakerImage;

    private Coroutine _parser;
    private bool buttonPresed = false;
    private string port;
    private List<TextOption> optionsListToDestroy = new List<TextOption>();

    private void Start()
    {
        foreach (BaseNode b in graf.nodes)
        {
            if (b.GetString() == "Start")
            {
                graf.current = b;
                break;
            }
        }

        _parser = StartCoroutine(ParseNode());
    }

    private IEnumerator ParseNode()
    {
        BaseNode b = graf.current;
        string data = b.GetString();
        string[] dataParts = data.Split('/');
        buttonPresed = false;
        if (dataParts[0] == "Start")
        {
            NextNode("exit");
        }

        if (dataParts[0] == "Stop")
        {
            dialogue.text = "The End";
        }

        if (dataParts[0] == "DialogNode")
        {
            speaker.text = dataParts[1];
            dialogue.text = dataParts[2];
            speakerImage.sprite = b.GetSprite();

            List<DialogueOption> optionsList = b.GetOptions();

            foreach (var o in optionsList)
            {
                TextOption textOption = Instantiate(textPrefab, optionsContainer);
                textOption.InitializeOptin(o.portName, OptionSelected);
                optionsListToDestroy.Add(textOption);
            }

            yield return new WaitUntil(() => buttonPresed);

            foreach (var textButton in optionsListToDestroy)
            {
                textButton.Destroy();
            }
            optionsListToDestroy.Clear();
            NextNode(port);
        }
    }

    public void NextNode(string fieldName)
    {
        if (_parser != null)
        {
            StopCoroutine(_parser);
            _parser = null;
        }
        foreach (NodePort p in graf.current.Ports)
        {
            if (p.fieldName == fieldName)
            {
                graf.current = p.Connection.node as BaseNode;
                break;
            }
        }
        _parser = StartCoroutine(ParseNode());
    }

    private void OptionSelected(string option)
    {
        port = option;
        buttonPresed = true;
    }
}
